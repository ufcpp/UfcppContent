<#
.SYNOPSIS
  Reconciles the CSS classes used by the migrated content against the generated
  site's stylesheet and against the original ufcpp.net stylesheet.

.DESCRIPTION
  Produces three sets and their differences:
    used      - class tokens appearing in class="..." in content/ and Templates/
                (fenced code blocks, inline code spans and entity-escaped markup
                 are stripped first, so classes that only appear inside code
                 samples or quoted HTML are not counted)
    site      - class selectors defined in wwwroot/css/site.css
    original  - class selectors defined in ufcpp.net's bundle.min.css

  Writes a TSV report next to the reference stylesheet.
#>
[CmdletBinding()]
param(
    [string] $RepoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path,
    [string] $ReferenceCss = (Join-Path $env:TEMP 'ufcpp-css-ref\bundle.min.css'),
    [string] $OutputDirectory = (Join-Path $env:TEMP 'ufcpp-css-ref')
)

$ErrorActionPreference = 'Stop'

function Get-CssClassSelector {
    param([string] $Path)
    if (-not (Test-Path $Path)) { return @() }
    $text = Get-Content -LiteralPath $Path -Raw
    # Drop comments and declaration blocks so only selector text remains.
    $text = [regex]::Replace($text, '/\*.*?\*/', ' ', 'Singleline')
    $selectorText = [regex]::Replace($text, '\{[^{}]*\}', ' ', 'Singleline')
    [regex]::Matches($selectorText, '\.(-?[_a-zA-Z][_a-zA-Z0-9-]*)') |
        ForEach-Object { $_.Groups[1].Value } |
        Sort-Object -Unique
}

function Remove-NonMarkupText {
    param([string] $Text)
    # Fenced code blocks (``` or ~~~), then inline code spans, then HTML that is
    # shown as text via entities (e.g. `&lt;p class="footnote"&gt;`).
    $t = [regex]::Replace($Text, '(?ms)^[ \t]*(`{3,}|~{3,}).*?^[ \t]*\1[ \t]*$', "`n")
    $t = [regex]::Replace($t, '(?s)(`+)(?!`).*?\1', ' ')
    $t = [regex]::Replace($t, '(?s)&lt;.*?&gt;', ' ')
    return $t
}

function Get-UsedClass {
    param([string[]] $Path, [switch] $StripCode)
    $result = @{}
    foreach ($file in $Path) {
        $text = Get-Content -LiteralPath $file -Raw
        if (-not $text) { continue }
        if ($StripCode) { $text = Remove-NonMarkupText $text }
        foreach ($m in [regex]::Matches($text, '(?i)\bclass\s*=\s*"([^"]*)"')) {
            foreach ($token in ($m.Groups[1].Value -split '\s+')) {
                if ($token -and $token -notmatch '[@{}()]') {
                    if (-not $result.ContainsKey($token)) { $result[$token] = @() }
                    $result[$token] += $file
                }
            }
        }
    }
    return $result
}

$contentFiles  = Get-ChildItem -Path (Join-Path $RepoRoot 'content')  -Filter *.md    -Recurse -File | ForEach-Object FullName
$templateFiles = Get-ChildItem -Path (Join-Path $RepoRoot 'tools\Ufcpp.SiteGenerator\Templates') -Filter *.razor -Recurse -File | ForEach-Object FullName

$contentUsage  = Get-UsedClass -Path $contentFiles  -StripCode
$templateUsage = Get-UsedClass -Path $templateFiles

$siteCssPath = Join-Path $RepoRoot 'tools\Ufcpp.SiteGenerator\wwwroot\css\site.css'
$siteClasses     = Get-CssClassSelector -Path $siteCssPath
$originalClasses = Get-CssClassSelector -Path $ReferenceCss

$usedClasses = @($contentUsage.Keys) + @($templateUsage.Keys) | Sort-Object -Unique

New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null
$reportPath = Join-Path $OutputDirectory 'class-reconciliation.tsv'

$rows = foreach ($cls in $usedClasses) {
    $files = @()
    if ($contentUsage.ContainsKey($cls))  { $files += $contentUsage[$cls] }
    if ($templateUsage.ContainsKey($cls)) { $files += $templateUsage[$cls] }
    [pscustomobject]@{
        Class        = $cls
        Uses         = $files.Count
        Files        = ($files | Sort-Object -Unique).Count
        InSiteCss    = $siteClasses -contains $cls
        InOriginal   = $originalClasses -contains $cls
        SampleFile   = (($files | Sort-Object -Unique | Select-Object -First 1) -replace [regex]::Escape($RepoRoot + '\'), '')
    }
}

$rows | Sort-Object InSiteCss, @{Expression='Uses';Descending=$true} |
    Export-Csv -Path $reportPath -Delimiter "`t" -NoTypeInformation -Encoding UTF8

$missing = $rows | Where-Object { -not $_.InSiteCss } | Sort-Object -Property @{Expression='Uses';Descending=$true}
$unused  = $siteClasses | Where-Object { $usedClasses -notcontains $_ }

Write-Output "report: $reportPath"
Write-Output ""
Write-Output "=== used but NOT defined in site.css ($($missing.Count)) ==="
$missing | Format-Table Class, Uses, Files, InOriginal, SampleFile -AutoSize | Out-String -Width 200 | Write-Output
Write-Output "=== defined in site.css but not found in content/templates ($($unused.Count)) — informational ==="
Write-Output ($unused -join ', ')
