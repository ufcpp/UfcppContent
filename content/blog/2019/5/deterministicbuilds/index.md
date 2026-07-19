---
title: "C# で、同じソースコードから常に同じバイナリを生成する"
source_url: "https://ufcpp.net/blog/2019/5/deterministicbuilds/"
content_type: "BlogEntry"
published_at: "2019-05-24T23:01:00"
updated_at: "2019-05-24T23:01:00"
tags: []
umbraco_id: 2245
parent_id: 2241
sort_order: 3
aliases: []
---

# C# で、同じソースコードから常に同じバイナリを生成する

昔、[gist にだけ置いてて、そういえばブログに書いてなかったもの](https://gist.github.com/ufcpp/f3cc89e8c266997063eb2c633114668a)を思い出したので書いておくことに。

(一応、部分的には言及したことがあるんですけど、ちゃんとした話はしたことがなかったはず。)

## 決定論的ビルド

3年くらい前まで、C# コードをコンパイルすると、ソースコードを一切書き換えていなくても、生成結果の exe/dll や [pdb](https://docs.microsoft.com/ja-jp/visualstudio/debugger/specify-symbol-dot-pdb-and-source-files-in-the-visual-studio-debugger?view=vs-2019) のバイナリが変化していました(決定性(deteminism)がない)。

原因は以下の2つです。

- バイナリ中に埋め込まれる GUID にタイムスタンプと乱数から生成される値を使っていた
- デバッグ用のファイル情報がフルパスで埋め込まれていた

GUID の方はタイムスタンプと乱数なので本当に致命的で、ローカルで再コンパイルしても毎回バイナリが変化していました。

フルパスの方は基本的には pdb (デバッグ用シンボル情報)だけの問題なんですが、
exe/dll でも、[`CallerFilePath` 属性](../../../../study/csharp/cheatsheet/ap_ver5.md#CallerInfo)を使ったりすると文字列定数でフルパスが埋め込まれます。
ローカルで再コンパイルする分には変化しないんですが、`C:\users\ユーザー名\Documents\` みたいなのが pdb 中に残ってしまってみっともないです。
また、クラウド ビルド環境では常に同じパスでコンパイルされるとは限らないので、やっぱりバイナリの一意性を確保できなくなります。

バイナリが決定論的でないというのは単に不格好という問題ではなく、
CI/CD 環境でキャッシュが効かなくてなってサーバー リソースを食いつぶすことにつながったりします。

これらに対処するための C# コンパイラー(csc)オプションは、2016年頃から提供されるようになりました(参考: [Deterministic builds in Roslyn](https://blog.paranoidcoding.com/2016/04/05/deterministic-builds-in-roslyn.html))。

- `/deteministic`: GUID の算出にタイムスタンプと乱数を使わなくする
  - ソースコードなどのハッシュ値から計算するので、ソースコードに変化がない限り一意
- `/pathmap`: デバッグ情報などで使うファイルのパスを所定のルールで置き換え可能にする
  - `置き換え元=置き換え後` という形式で、単純な置換をするだけ
  - 置き換え元の方に `$(MSBuildProjectDirectory)` とかを指定して、置き換え先の方を`/`とかあたりさわりのない固定の文字列にしておけば、どの環境でも同じ結果が得られる

## csproj での指定

C# コンパイラー オプションとしては3年前に実装されていても、
dotnet コマンドでのビルド(csproj ファイル中にオプション記述)できるようになったのはもうちょっと後です。
それでも、Visual Studio 2017 (2年前)から、以下のオプションが指定できるようになりました。

- `<Deterministic>`: csc の `/deteministic` に相当
- `<PathMap>`: csc の `/pathmap` に相当

ちなみに、[SDK-based なプロジェクト](../../../2017/5/newcsproj/index.md)の場合、
`<Deterministic>` がデフォルトで true になっています。
タイムスタンプ問題は何もしなくても解消済み。

問題は `<PathMap>` の方で、こちらは明示的に指定しないと今でもフルパスが入ります。

## Directory.Build

ちなみに、`<PathMap>` は [`Directory.Build.props`](../../../2018/12/directorybuild/index.md) に書いておいても動作します。
プロジェクト1個1個に設定を入れるのも面倒なので、自分はリポジトリのルートに1個、以下のような `Directory.Build.props` を入れています。

<pre class="xsource" title="">
<code><span class="attvalue">&lt;</span><span class="element">Project</span><span class="attvalue">&gt;</span>
 
<span class="attvalue">  &lt;</span><span class="element">PropertyGroup</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">LangVersion</span><span class="attvalue">&gt;</span>latest<span class="attvalue">&lt;/</span><span class="element">LangVersion</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">RepoRoot</span><span class="attvalue">&gt;</span>$([System.IO.Path]::GetFullPath(&#39;$(MSBuildThisFileDirectory)..\&#39;))<span class="attvalue">&lt;/</span><span class="element">RepoRoot</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">PathMap</span><span class="attvalue">&gt;</span>$(RepoRoot)=.<span class="attvalue">&lt;/</span><span class="element">PathMap</span><span class="attvalue">&gt;</span>
<span class="attvalue">  &lt;/</span><span class="element">PropertyGroup</span><span class="attvalue">&gt;</span>
 
<span class="attvalue">&lt;/</span><span class="element">Project</span><span class="attvalue">&gt;</span>
</code></pre>

`$(MSBuildProjectDirectory)` (csproj があるフォルダー)からではなく、
`$(MSBuildThisFileDirectory)` (props があるフォルダー)を基準にして、
リポジトリ内の相対パスは残すようにしています。

単に `$(MSBuildThisFileDirectory)` ではなく、その1つ上のフォルダーまで(git clone してきてるリポジトリ名まで)残すようにしています。

## PathMap の例

`<Deterministic>` の方は一応過去に少し触れたことがあるのと、
SDK-based な csproj を使う限りもう今はデフォルトで true なので、
改めて例を挙げるほどでもないかと思います。
ということで、`<PathMap>` の方。

- [`<PathMap>` の例](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2019/PathMap/PathMap)

実行すると、`CallerFilePath` 属性で取ったファイル名と、例外のスタックトレースが書き換わってる(短くなってる)ことがわかります。

#### 余談

2年くらい前の機能をなんで今更ブログに書くことになったかというと、
今日、ちょうど「ログ出力で `CallerFilePath` 使うか」という話になったからだったり。
「フルパス入るの嫌か」「あっ、でも、確か `PathMap` 導入してたはず」みたいな。

で、その `PathMap` 導入がいつかというと、このとき:

- [今年3月15日の投稿](https://twitter.com/ufcpp/status/1106533083224788997)

違う人がコンパイルするたびに差分が出て困ったところで、ネットで検索してみたら自分の gist が出てきたというよくあるやつ。
