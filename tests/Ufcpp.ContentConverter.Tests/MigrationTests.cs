using System.Text;
using Ufcpp.ContentConverter;

namespace Ufcpp.ContentConverter.Tests;

public sealed class MigrationTests
{
    [Fact]
    public void MarkdownHeadingSpacingIsNormalizedOutsideProtectedBlocks()
    {
        var input = """
            ---
            title: Test
            # Front matter comment
            ---
            # Heading
            Body
            ##<a id="legacy"></a>Legacy heading
            Body
            ## Already spaced

            Body
            ```text
            # Fenced code
            Body
            ```
            <pre>
            # HTML code
            Body
            </pre>
            <!--
            # Commented heading
            Body
            -->
            """;
        var expected = """
            ---
            title: Test
            # Front matter comment
            ---
            # Heading

            Body
            ## <a id="legacy"></a>Legacy heading

            Body
            ## Already spaced

            Body
            ```text
            # Fenced code
            Body
            ```
            <pre>
            # HTML code
            Body
            </pre>
            <!--
            # Commented heading
            Body
            -->
            """;

        Assert.Equal(expected, TextUtilities.NormalizeMarkdownHeadingSpacing(input));
        Assert.Equal(expected, TextUtilities.NormalizeMarkdownHeadingSpacing(expected));
    }

    [Fact]
    public void ParserIgnoresDtdWithoutResolvingIt()
    {
        using var workspace = new TestWorkspace();
        var snapshot = workspace.Write(
            "published.xml",
            """
            <?xml version="1.0"?>
            <!DOCTYPE root SYSTEM "file:///this-file-must-not-be-opened.dtd">
            <root id="-1">
              <Home id="1" parentID="-1" level="1" sortOrder="0" nodeName="Home" urlName="home"
                    createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                    nodeTypeAlias="Home">
                <description>safe</description>
              </Home>
            </root>
            """);

        var parsed = PublishedContentParser.Load(snapshot);

        Assert.Single(parsed.Nodes);
        Assert.Equal("safe", parsed.Home.Get("description"));
    }

    [Fact]
    public void ParserRejectsInvalidHierarchy()
    {
        using var workspace = new TestWorkspace();
        var snapshot = workspace.Write(
            "invalid.xml",
            SnapshotXml(
                """
                <Article id="2" parentID="99" level="2" sortOrder="0" nodeName="bad" urlName="bad"
                         createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                         nodeTypeAlias="Article"><title>Bad</title><bodyText /></Article>
                """));

        var error = Assert.Throws<InvalidDataException>(() => PublishedContentParser.Load(snapshot));
        Assert.Contains("missing parent", error.Message);
    }

    [Fact]
    public void ParserRejectsMissingRequiredBodyProperty()
    {
        using var workspace = new TestWorkspace();
        var snapshot = workspace.Write(
            "missing-body.xml",
            SnapshotXml(
                """
                <Article id="2" parentID="1" level="2" sortOrder="0" nodeName="missing" urlName="missing"
                         createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                         nodeTypeAlias="Article"><title>Missing</title></Article>
                """));

        var error = Assert.Throws<InvalidDataException>(() => PublishedContentParser.Load(snapshot));

        Assert.Contains("missing required property 'bodyText'", error.Message);
    }

    [Fact]
    public void ParserRecordsKnownMissingLegacyBody()
    {
        using var workspace = new TestWorkspace();
        var snapshotPath = workspace.Write(
            "known-missing-body.xml",
            SnapshotXml(
                """
                <Chapter id="1408" parentID="1" level="2" sortOrder="0" nodeName="legacy" urlName="legacy"
                         createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                         nodeTypeAlias="Chapter">
                  <Article id="1410" parentID="1408" level="3" sortOrder="0"
                           nodeName="keywords" urlName="keywords"
                           createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                           nodeTypeAlias="Article"><title>重要語句一覧</title></Article>
                </Chapter>
                """));

        var snapshot = PublishedContentParser.Load(snapshotPath);

        Assert.Equal(["bodyText"], snapshot.ById[1410].KnownMissingProperties);
    }

    [Fact]
    public void InvalidInputDoesNotDeleteExistingOutput()
    {
        using var workspace = new TestWorkspace();
        var output = workspace.Directory("output");
        var existing = workspace.WriteAt(output, "content/existing.md", "keep");
        var snapshot = workspace.Write(
            "invalid-migration.xml",
            SnapshotXml(
                """
                <Article id="2" parentID="1" level="2" sortOrder="0" nodeName="missing" urlName="missing"
                         createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                         nodeTypeAlias="Article"><title>Missing</title></Article>
                """));

        Assert.Throws<InvalidDataException>(
            () => new ContentMigration(
                new MigrationOptions(
                    snapshot,
                    workspace.Directory("media"),
                    workspace.Write("sitemap.xml", "<urlset />"),
                    workspace.Write("maps.config", RewriteMapsXml(string.Empty, string.Empty)),
                    workspace.Directory("legacy"),
                    output,
                    false)).Run());

        Assert.Equal("keep", File.ReadAllText(existing));
    }

    [Fact]
    public void CanonicalUrlEscapesJapaneseSegmentsExactly()
    {
        using var workspace = new TestWorkspace();
        var snapshot = workspace.Write(
            "japanese.xml",
            SnapshotXml(
                """
                <Article id="2" parentID="1" level="2" sortOrder="0" nodeName="情報工学" urlName="情報工学"
                         createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                         nodeTypeAlias="Article"><title>情報工学</title><bodyText /></Article>
                """));
        var article = PublishedContentParser.Load(snapshot).ById[2];

        Assert.Equal("/%E6%83%85%E5%A0%B1%E5%B7%A5%E5%AD%A6/", ContentPaths.CanonicalUrl(article));
        Assert.Equal("content/情報工学.md", ContentPaths.OutputPath(article));
    }

    [Fact]
    public void RewriteMapsAttachLegacyHtmlAliases()
    {
        using var workspace = new TestWorkspace();
        var snapshotPath = workspace.Write(
            "aliases.xml",
            SnapshotXml(
                """
                <StudyTop id="2" parentID="1" level="2" sortOrder="0" nodeName="study" urlName="study"
                          createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                          nodeTypeAlias="StudyTop">
                  <Subject id="3" parentID="2" level="3" sortOrder="0" nodeName="csharp" urlName="csharp"
                           createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                           nodeTypeAlias="Subject">
                    <Chapter id="4" parentID="3" level="4" sortOrder="0" nodeName="start" urlName="start"
                             createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                             nodeTypeAlias="Chapter">
                      <Article id="5" parentID="4" level="5" sortOrder="0" nodeName="foo" urlName="foo"
                               createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                               nodeTypeAlias="Article"><title>Foo</title><bodyText /></Article>
                    </Chapter>
                  </Subject>
                </StudyTop>
                """));
        var mapsPath = workspace.Write(
            "maps.config",
            RewriteMapsXml(
                """<add key="study/csharp/start/foo" value="/study/csharp/foo.html" />""",
                """<add key="/study/csharp/foo.html" value="study/csharp/start/foo" />"""));
        var snapshot = PublishedContentParser.Load(snapshotPath);
        var urls = snapshot.Nodes.ToDictionary(node => node.Id, ContentPaths.CanonicalUrl);

        var aliases = RewriteMapCatalog.Load(mapsPath).BuildAliases(snapshot.Nodes, urls);

        Assert.Contains("/study/csharp/foo.html", aliases[5]);
        Assert.Contains("/study/csharp/foo", aliases[5]);
        Assert.Contains("/csharp/foo.html", aliases[5]);
    }

    [Theory]
    [InlineData("?key=alpha", "#alpha")]
    [InlineData("?sec=section", "#section")]
    [InlineData("?p=3#continued", "#continued")]
    [InlineData("?exercise=q1", "#exercise-q1")]
    [InlineData("?list", "#list")]
    [InlineData("?bc=category", "#blog-category-test")]
    public void RuntimeQueriesBecomeFragments(string input, string expected)
    {
        using var fixture = LinkFixture.Create();

        Assert.Equal(expected, fixture.Rewriter.RewriteUrl(input, fixture.Current));
    }

    [Fact]
    public void SamePageLegacyCorrectionCanTargetSiblingPage()
    {
        using var workspace = new TestWorkspace();
        var snapshotPath = workspace.Write(
            "legacy-fragment.xml",
            SnapshotXml(
                """
                <BlogTop id="2" parentID="1" level="2" sortOrder="0" nodeName="blog" urlName="blog"
                         createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                         nodeTypeAlias="BlogTop">
                  <BlogYear id="3" parentID="2" level="3" sortOrder="0" nodeName="2021" urlName="2021"
                            createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                            nodeTypeAlias="BlogYear">
                    <BlogMonth id="4" parentID="3" level="4" sortOrder="0" nodeName="12" urlName="12"
                               createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                               nodeTypeAlias="BlogMonth">
                      <BlogEntry id="5" parentID="4" level="5" sortOrder="0"
                                 nodeName="notorious-compat-char" urlName="notorious-compat-char"
                                 createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                                 nodeTypeAlias="BlogEntry"><bodyText /></BlogEntry>
                      <BlogEntry id="6" parentID="4" level="5" sortOrder="1"
                                 nodeName="ninjacatdies" urlName="ninjacatdies"
                                 createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                                 nodeTypeAlias="BlogEntry"><bodyText /></BlogEntry>
                    </BlogMonth>
                  </BlogYear>
                </BlogTop>
                """));
        var snapshot = PublishedContentParser.Load(snapshotPath);
        var urls = snapshot.Nodes.ToDictionary(node => node.Id, ContentPaths.CanonicalUrl);
        var outputs = snapshot.Nodes
            .Where(node => ContentPaths.OutputPath(node) is not null)
            .ToDictionary(node => node.Id, node => ContentPaths.OutputPath(node)!);
        var aliases = snapshot.Nodes.ToDictionary(
            node => node.Id,
            _ => (IReadOnlyList<string>)Array.Empty<string>());
        var rewriter = new LinkRewriter(
            snapshot.Nodes,
            urls,
            outputs,
            aliases,
            new Dictionary<string, string>(),
            new AssetManager(
                workspace.Directory("media"),
                workspace.Directory("legacy"),
                workspace.Directory("output")));

        var rewritten = rewriter.RewriteUrl("#apple-log", snapshot.ById[5]);

        Assert.Equal("../ninjacatdies/index.md#apple-log", rewritten);
    }

    [Fact]
    public void ExternalHostContainingMediaIsNeverTreatedAsInternal()
    {
        using var fixture = LinkFixture.Create();
        const string url = "https://pbs.twimg.com/media/example.jpg:large";

        Assert.Equal(url, fixture.Rewriter.RewriteUrl(url, fixture.Current));
        Assert.Empty(fixture.Assets.Records);
    }

    [Fact]
    public void NestedImageLinksAndInternalAutolinksAreRewritten()
    {
        using var fixture = LinkFixture.Create();
        var input =
            "[![image](/media/image.png)](/target/)\n" +
            "<https://ufcpp.net/target/>";
        Directory.CreateDirectory(Path.Combine(fixture.MediaRoot));
        File.WriteAllText(Path.Combine(fixture.MediaRoot, "image.png"), "png");

        var output = fixture.Rewriter.Rewrite(input, fixture.Current);

        Assert.Equal(
            "[![image](../assets/media/image.png)](target.md)\n" +
            "<target.md>",
            output);
    }

    [Fact]
    public void MalformedDoubleOpenLinksAreNormalized()
    {
        using var fixture = LinkFixture.Create();

        var output = fixture.Rewriter.Rewrite(
            "[external]((https://example.com/)) [internal]((https://ufcpp.net/target/))",
            fixture.Current);

        Assert.Equal("[external](https://example.com/) [internal](target.md)", output);
    }

    [Fact]
    public void WindowsPathRulesRejectUnsafeNamesAndCollisions()
    {
        Assert.Throws<InvalidDataException>(() => ContentPaths.ValidateSegment("CON"));
        Assert.Throws<InvalidDataException>(() => ContentPaths.ValidateSegment("name."));
        Assert.Throws<InvalidDataException>(() => ContentPaths.ValidateSegment("e\u0301"));
        Assert.Throws<InvalidDataException>(
            () => ContentPaths.ValidateNoCollisions(["content/Foo.md", "content/foo.md"]));
    }

    [Fact]
    public void KnownMacrosExpandAndUnknownMacrosFail()
    {
        using var workspace = new TestWorkspace();
        var snapshotPath = workspace.Write(
            "macro.xml",
            SnapshotXml(
                """
                <Article id="2" parentID="1" level="2" sortOrder="0" nodeName="version" urlName="version"
                         createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                         nodeTypeAlias="Article">
                  <title>Version</title><tags>Ver. 2.0</tags><bodyText />
                </Article>
                """));
        var snapshot = PublishedContentParser.Load(snapshotPath);
        var urls = snapshot.Nodes.ToDictionary(node => node.Id, ContentPaths.CanonicalUrl);
        var expander = new MacroExpander(snapshot.Nodes, urls);

        var release = expander.Expand(
            """<?UMBRACO_MACRO macroAlias="CsharpIndexVersionRelease" />""",
            snapshot.ById[2]);
        var byVersion = expander.Expand(
            """<?UMBRACO_MACRO macroAlias="CsharpIndexByVersion" />""",
            snapshot.ById[2]);

        Assert.Contains("C# 7", release);
        Assert.Contains("[Version](/version/)", byVersion);
        Assert.Throws<InvalidDataException>(
            () => expander.Expand(
                """<?UMBRACO_MACRO macroAlias="Unknown" />""",
                snapshot.ById[2]));
        Assert.Throws<InvalidDataException>(
            () => expander.Expand(
                """<?UMBRACO_MACRO macroAlias = 'Unknown' parameter="x" />""",
                snapshot.ById[2]));
    }

    [Fact]
    public void KeywordSummaryMacroBuildsSubjectArticleKeywordTable()
    {
        using var workspace = new TestWorkspace();
        var snapshotPath = workspace.Write(
            "keywords.xml",
            SnapshotXml(
                """
                <StudyTop id="2" parentID="1" level="2" sortOrder="0" nodeName="study" urlName="study"
                          createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                          nodeTypeAlias="StudyTop">
                  <Subject id="3" parentID="2" level="3" sortOrder="0" nodeName="subject" urlName="subject"
                           createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                           nodeTypeAlias="Subject">
                    <Chapter id="4" parentID="3" level="4" sortOrder="0" nodeName="chapter" urlName="chapter"
                             createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                             nodeTypeAlias="Chapter"><title>Chapter</title>
                      <Article id="5" parentID="4" level="5" sortOrder="0" nodeName="article" urlName="article"
                               createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                               nodeTypeAlias="Article"><title>Article</title>
                        <bodyText><![CDATA[<strong id="alias">Keyword</strong>]]></bodyText>
                      </Article>
                    </Chapter>
                  </Subject>
                </StudyTop>
                """));
        var snapshot = PublishedContentParser.Load(snapshotPath);
        var urls = snapshot.Nodes.ToDictionary(node => node.Id, ContentPaths.CanonicalUrl);
        var expander = new MacroExpander(snapshot.Nodes, urls);

        var output = expander.Expand(
            """<?UMBRACO_MACRO macroAlias="KeywordSummary" />""",
            snapshot.ById[5]);

        Assert.Contains("<table>", output);
        Assert.Contains("/study/subject/chapter/article/?key=alias", output);
        Assert.Contains("Keyword", output);
    }

    [Fact]
    public void ExerciseAnswersRequireJsonArrayObjectsWithStringValues()
    {
        using var workspace = new TestWorkspace();
        var validPath = workspace.Write(
            "exercise.xml",
            SnapshotXml(
                """
                <Exercise id="2" parentID="1" level="2" sortOrder="0" nodeName="q1" urlName="q1"
                          createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                          nodeTypeAlias="Exercise">
                   <questionText>question</questionText><answerText>[{"value":"answer"}]</answerText>
                 </Exercise>
                """));
        var malformedPath = workspace.Write(
            "malformed.xml",
            SnapshotXml(
                """
                <Exercise id="2" parentID="1" level="2" sortOrder="0" nodeName="q1" urlName="q1"
                          createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                          nodeTypeAlias="Exercise">
                   <questionText>question</questionText><answerText>{broken</answerText>
                 </Exercise>
                """));

        Assert.Equal(["answer"], ExerciseRenderer.ParseAnswers(PublishedContentParser.Load(validPath).ById[2]));
        Assert.Throws<InvalidDataException>(
            () => ExerciseRenderer.ParseAnswers(PublishedContentParser.Load(malformedPath).ById[2]));
    }

    [Fact]
    public void AssetManagerCopiesMediaAndLegacyStaticFiles()
    {
        using var workspace = new TestWorkspace();
        var media = workspace.Directory("media");
        var legacy = workspace.Directory("legacy");
        var output = workspace.Directory("output");
        workspace.WriteAt(media, "100/image.png", "png");
        workspace.WriteAt(legacy, "Liszt/Liszt.Umbraco/images/logo.jpg", "jpg");
        workspace.WriteAt(
            legacy,
            "元/ufcpp.net/study/testxsl/nomenu.xsl",
            "xsl");
        var manager = new AssetManager(media, legacy, output);

        Assert.Equal("assets/media/100/image.png", manager.ResolveAndCopy("/media/100/image.png"));
        Assert.Equal("assets/images/logo.jpg", manager.ResolveAndCopy("/images/logo.jpg"));
        Assert.Equal(
            "assets/media/ufcpp2000/xml/xslfiles/nomenu.xsl",
            manager.ResolveAndCopy("/media/ufcpp2000/xml/xslfiles/nomenu.xsl"));
        Assert.True(File.Exists(Path.Combine(output, "assets", "media", "100", "image.png")));
        Assert.True(File.Exists(Path.Combine(output, "assets", "images", "logo.jpg")));
        Assert.Throws<FileNotFoundException>(() => manager.ResolveAndCopy("/media/missing.bin"));
    }

    [Fact]
    public void GenerationIsByteDeterministic()
    {
        using var workspace = new TestWorkspace();
        var snapshot = workspace.Write("published.xml", SnapshotXml(string.Empty));
        var sitemap = workspace.Write(
            "sitemap.xml",
            """<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9"><url><loc>https://ufcpp.net/</loc></url></urlset>""");
        var maps = workspace.Write("maps.config", RewriteMapsXml(string.Empty, string.Empty));
        var media = workspace.Directory("media");
        var legacy = workspace.Directory("legacy");
        var first = workspace.Directory("first");
        var second = workspace.Directory("second");

        var firstReport = new ContentMigration(
            new MigrationOptions(snapshot, media, sitemap, maps, legacy, first, false)).Run();
        var secondReport = new ContentMigration(
            new MigrationOptions(snapshot, media, sitemap, maps, legacy, second, false)).Run();

        Assert.Equal(1, firstReport.MarkdownOutputs);
        Assert.Equal(firstReport, secondReport);
        var firstFiles = RelativeFiles(first);
        var secondFiles = RelativeFiles(second);
        Assert.Equal(firstFiles, secondFiles);
        foreach (var file in firstFiles)
        {
            Assert.Equal(
                File.ReadAllBytes(Path.Combine(first, file)),
                File.ReadAllBytes(Path.Combine(second, file)));
        }
    }

    [Fact]
    public void GenerationCreatesAbsoluteSourceUrlsAndResolvableLegacyHeadingAnchors()
    {
        using var workspace = new TestWorkspace();
        var snapshot = workspace.Write(
            "published.xml",
            SnapshotXml(
                """
                <StudyTop id="2" parentID="1" level="2" sortOrder="0" nodeName="study" urlName="study"
                          createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                          nodeTypeAlias="StudyTop">
                  <Subject id="3" parentID="2" level="3" sortOrder="0" nodeName="subject" urlName="subject"
                           createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                           nodeTypeAlias="Subject"><title>Subject</title>
                    <Chapter id="4" parentID="3" level="4" sortOrder="0" nodeName="chapter" urlName="chapter"
                             createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                             nodeTypeAlias="Chapter"><title>Chapter</title>
                      <Article id="5" parentID="4" level="5" sortOrder="0" nodeName="target" urlName="target"
                               createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                               nodeTypeAlias="Article"><title>Target</title><bodyText>##Section&#10;Section body</bodyText></Article>
                      <Article id="6" parentID="4" level="5" sortOrder="1" nodeName="source" urlName="source"
                               createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                               nodeTypeAlias="Article"><title>Source</title>
                        <bodyText>[target](/study/subject/chapter/target/?sec=sec-generated-title-1)</bodyText>
                      </Article>
                    </Chapter>
                  </Subject>
                </StudyTop>
                """));
        var sitemap = workspace.Write(
            "sitemap.xml",
            """
            <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
              <url><loc>https://ufcpp.net/</loc></url>
              <url><loc>https://ufcpp.net/study/subject/</loc></url>
              <url><loc>https://ufcpp.net/study/subject/chapter/target/</loc></url>
              <url><loc>https://ufcpp.net/study/subject/chapter/source/</loc></url>
            </urlset>
            """);
        var output = workspace.Directory("output");

        new ContentMigration(
            new MigrationOptions(
                snapshot,
                workspace.Directory("media"),
                sitemap,
                workspace.Write("maps.config", RewriteMapsXml(string.Empty, string.Empty)),
                workspace.Directory("legacy"),
                output,
                false)).Run();

        var target = File.ReadAllText(
            Path.Combine(output, "content", "study", "subject", "chapter", "target.md"));
        var source = File.ReadAllText(
            Path.Combine(output, "content", "study", "subject", "chapter", "source.md"));
        var subject = File.ReadAllText(
            Path.Combine(output, "content", "study", "subject", "index.md"));
        Assert.Contains("source_url: \"https://ufcpp.net/study/subject/chapter/target/\"", target);
        Assert.Contains("## <a id=\"sec-generated-title-1\"></a>Section\n\nSection body", target);
        Assert.Contains("""[target](target.md#sec-generated-title-1)""", source);
        Assert.Contains("""<a id="chapter"></a>""", subject);
    }

    [Fact]
    public void GenerationRejectsMissingAndCaseMismatchedFragments()
    {
        using var workspace = new TestWorkspace();
        var maps = workspace.Write("maps.config", RewriteMapsXml(string.Empty, string.Empty));
        var media = workspace.Directory("media");
        var legacy = workspace.Directory("legacy");

        AssertInvalidFragment(
            "same-page",
            """
            <Article id="2" parentID="1" level="2" sortOrder="0" nodeName="source" urlName="source"
                     createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                     nodeTypeAlias="Article">
              <title>Source</title><bodyText><![CDATA[[bad](#missing)]]></bodyText>
            </Article>
            """,
            "https://ufcpp.net/",
            "https://ufcpp.net/source/");
        AssertInvalidFragment(
            "case-mismatch",
            """
            <Article id="2" parentID="1" level="2" sortOrder="0" nodeName="target" urlName="target"
                     createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                     nodeTypeAlias="Article">
              <title>Target</title><bodyText><![CDATA[<a id="com"></a>]]></bodyText>
            </Article>
            <Article id="3" parentID="1" level="2" sortOrder="1" nodeName="source" urlName="source"
                     createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                     nodeTypeAlias="Article">
              <title>Source</title><bodyText><![CDATA[[COM](/target/#COM)]]></bodyText>
            </Article>
            """,
            "https://ufcpp.net/",
            "https://ufcpp.net/target/",
            "https://ufcpp.net/source/");

        void AssertInvalidFragment(string name, string children, params string[] urls)
        {
            var snapshot = workspace.Write($"{name}.xml", SnapshotXml(children));
            var sitemapEntries = string.Concat(urls.Select(url => $"<url><loc>{url}</loc></url>"));
            var sitemap = workspace.Write(
                $"{name}-sitemap.xml",
                $"<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">{sitemapEntries}</urlset>");

            var error = Assert.Throws<InvalidDataException>(
                () => new ContentMigration(
                    new MigrationOptions(
                        snapshot,
                        media,
                        sitemap,
                        maps,
                        legacy,
                        workspace.Directory(name + "-output"),
                        false)).Run());

            Assert.Contains("targets missing fragment", error.Message);
        }
    }

    private static string SnapshotXml(string children) =>
        $$"""
        <?xml version="1.0"?>
        <root id="-1">
          <Home id="1" parentID="-1" level="1" sortOrder="0" nodeName="Home" urlName="home"
                createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                nodeTypeAlias="Home">
            <description>Home</description>
            {{children}}
          </Home>
        </root>
        """;

    private static string RewriteMapsXml(string redirects, string rewrites) =>
        $$"""
        <rewriteMaps>
          <rewriteMap name="SubjectRedirectsNewSubject" />
          <rewriteMap name="SubjectRedirects" />
          <rewriteMap name="SubjectRewrites" />
          <rewriteMap name="ArticleRedirects">{{redirects}}</rewriteMap>
          <rewriteMap name="ArticleRewrites">{{rewrites}}</rewriteMap>
        </rewriteMaps>
        """;

    private static string[] RelativeFiles(string root) =>
        Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories)
            .Select(path => Path.GetRelativePath(root, path))
            .Order(StringComparer.Ordinal)
            .ToArray();

    private sealed class LinkFixture : IDisposable
    {
        private readonly TestWorkspace _workspace;

        private LinkFixture(
            TestWorkspace workspace,
            ContentNode current,
            LinkRewriter rewriter,
            AssetManager assets,
            string mediaRoot)
        {
            _workspace = workspace;
            Current = current;
            Rewriter = rewriter;
            Assets = assets;
            MediaRoot = mediaRoot;
        }

        public ContentNode Current { get; }

        public LinkRewriter Rewriter { get; }

        public AssetManager Assets { get; }

        public string MediaRoot { get; }

        public static LinkFixture Create()
        {
            var workspace = new TestWorkspace();
            var snapshotPath = workspace.Write(
                "links.xml",
                SnapshotXml(
                    """
                    <Article id="2" parentID="1" level="2" sortOrder="0" nodeName="current" urlName="current"
                             createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                             nodeTypeAlias="Article"><title>Current</title><bodyText /></Article>
                    <Article id="3" parentID="1" level="2" sortOrder="1" nodeName="target" urlName="target"
                             createDate="2020-01-01T00:00:00" updateDate="2020-01-02T00:00:00"
                             nodeTypeAlias="Article"><title>Target</title><bodyText /></Article>
                    """));
            var snapshot = PublishedContentParser.Load(snapshotPath);
            var urls = snapshot.Nodes.ToDictionary(node => node.Id, ContentPaths.CanonicalUrl);
            var outputs = snapshot.Nodes
                .Where(node => ContentPaths.OutputPath(node) is not null)
                .ToDictionary(node => node.Id, node => ContentPaths.OutputPath(node)!);
            var aliases = snapshot.Nodes.ToDictionary(
                node => node.Id,
                _ => (IReadOnlyList<string>)Array.Empty<string>());
            var mediaRoot = workspace.Directory("media");
            var assets = new AssetManager(
                mediaRoot,
                workspace.Directory("legacy"),
                workspace.Directory("output"));
            var rewriter = new LinkRewriter(
                snapshot.Nodes,
                urls,
                outputs,
                aliases,
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["category"] = "blog-category-test",
                },
                assets);
            return new LinkFixture(workspace, snapshot.ById[2], rewriter, assets, mediaRoot);
        }

        public void Dispose() => _workspace.Dispose();
    }

    private sealed class TestWorkspace : IDisposable
    {
        public TestWorkspace()
        {
            Root = Path.Combine(
                AppContext.BaseDirectory,
                "test-work",
                Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(Root);
        }

        public string Root { get; }

        public string Directory(string relative)
        {
            var path = Path.Combine(Root, relative.Replace('/', Path.DirectorySeparatorChar));
            System.IO.Directory.CreateDirectory(path);
            return path;
        }

        public string Write(string relative, string value) => WriteAt(Root, relative, value);

        public string WriteAt(string root, string relative, string value)
        {
            var path = Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar));
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, value, new UTF8Encoding(false));
            return path;
        }

        public void Dispose()
        {
            if (System.IO.Directory.Exists(Root))
            {
                System.IO.Directory.Delete(Root, true);
            }
        }
    }
}
