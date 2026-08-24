---
title: "「マクロ欲しい」→「Source Generator 使え」→「むずい」→「ほら、作ったよ」"
source_url: "https://ufcpp.net/blog/2026/3/sourcegeneratordemo/"
content_type: "BlogEntry"
published_at: "2026-03-07T22:46:21"
updated_at: "2026-03-08T15:52:44"
tags: []
umbraco_id: 2521
parent_id: 2520
sort_order: 0
aliases: []
---

# 「マクロ欲しい」→「Source Generator 使え」→「むずい」→「ほら、作ったよ」

少し前に C# 言語仕様のディスカッションで、こんな提案が立ちました。

- [Compile-Time Inline Macros](https://github.com/dotnet/csharplang/discussions/10010)

この手の提案は、ほぼ同様のものがたびたび出てきました。
そして毎回かなり高い確率で、「Source Generator を使ってくれ」が結論になります。

それでも同様の提案が繰り返し出るのは、つまりそれだけ Source Generator に心理的ハードルがある、というです。

## い　つ　も　の

毎度おおむね、話の流れはこうです:

1. 「マクロ欲しい」
2. 「Source Generator 使えば？」
3. 「いや、あれ大変。プロジェクト分かれるのがつらい。同じプロジェクトにテンプレートを書きたい」
4. 「属性ベースの Source Generator を最初に 1 個だけ作れば、あとは使い回せるよ」
5. 「簡単って言うなら、実物あるんですか？」

で、だいたいこうなった辺りで横やりを入れたくなります。

「**じゃあ、今作ったわ**。」

実際、過去にもそういう流れで作ったものがあります。
確か以下の2つとかもそういう出自。

- [BitFieldGenerator](https://github.com/ufcpp/BitFieldGenerator)
- [StringLiteralGenerator](https://github.com/ufcpp/StringLiteralGenerator)

## そして再び。今回は気合入れ気味

まあ久しぶりに恒例行事参加いたしまして。
今回、気が付いたらちょっと気合入り気味。
その結果がこれです:

- [SourceGeneratorDemo](https://github.com/ufcpp/SourceGeneratorDemo)

目的が目的なのでこんな名前。特定の目的の Source Generator を作りたいとか広めたいということではなく、また同様の提案が出たときに「あるよ」と即出しするためのリポジトリ。

当然ながら英語でないと目的を果たせず、これまでだったらソースコードだけ書いてそこから説明文を書くのが面倒で一切何も書かなかったりするんですが。
最近の Copilot の進化に感謝しつつの readme 付き。

「雑に書いて雑に使える」を目標にしているのでほんとに最低限実装を目指しまして、
NuGet パッケージなし(パッケージ化する設定一切入れず、ProjectReference での利用を想定)。
Visual Studio のプロジェクト テンプレートの「Analyzer with Code Fix (.NET Standard)」が作るようなごてごてとしたコードは一切使わず、[数ファイルのテンプレ](https://github.com/ufcpp/SourceGeneratorDemo/tree/main/Starter)をコピペする想定。

## マクロ的 Source Generator

そして今回は特に、
「属性ベースの Source Generator を最初に 1 個だけ作って、あとは使い回し」を実装しまして。

* [AttributeTemplateGenerator](https://github.com/ufcpp/SourceGeneratorDemo/blob/main/AttributeTemplate/README.md)

(ちなみに、今回のこの[ディスカッションに同じく居合わせて](https://github.com/dotnet/csharplang/discussions/10010#discussioncomment-15924116)、同じくこれをモチベーションにしたっぽい方がいらっしゃったり: 「[C# でマクロを使う](https://zenn.dev/sator_imaging/articles/0ac6bf76bafe2a)」)

こちらのやつは、属性中に[文字列補間](../../../../study/csharp/start/st_string.md#string-interpolation)を書くことでマクロとして機能する Source Generator です。
例えばよくある `INotifyPropertyChanged` 実装用のマクロを書くなら以下のような感じ。

まず、クラス自体に `INotifyPropertyChanged` インターフェイス実装を用意するマクロ:

```csharp {title="クラス用マクロ属性"}
[AttributeUsage(AttributeTargets.Class)]
class NotifyClassAttribute() : TemplateAttribute(
    Global("using System.ComponentModel;"),
    Parent($"partial class {Name} : INotifyPropertyChanged;"),
"""
    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }
    
    protected bool SetProperty<T>(ref T storage, T value, PropertyChangedEventArgs e)
    {
        if (global::System.Collections.Generic.EqualityComparer<T>.Default.Equals(storage, value))
        {
            return false;
        }
        storage = value;
        OnPropertyChanged(e);
        return true;
    }
"""
    );
```

次にプロパティ用:

```csharp {title="プロパティ用マクロ属性"}
[AttributeUsage(AttributeTargets.Property)]
class NotifyPropertyAttribute() : TemplateAttribute(
$"""
    get => field;
    set => SetProperty(ref field, value, {Name}PropertyChangedEventArgs);
""",
Parent($"""
    private static readonly System.ComponentModel.PropertyChangedEventArgs {Name}PropertyChangedEventArgs = new(nameof({Name}));
""")
    );
```

で、これの利用側コード:

```csharp {title="マクロ利用者コードの例"}
using AttributeTemplateGenerator;

namespace Examples;

[NotifyClass]
partial class NotifyPropertyChangedExample
{
    [NotifyProperty]
    public partial int Integer { get; set; }

    [NotifyProperty]
    public partial string? Str { get; set; }

    [NotifyProperty]
    public partial TimeOnly Time { get; set; }

    [NotifyProperty]
    public partial DateOnly? Date { get; set; }

    [NotifyProperty]
    public partial (int, string?, DateTimeOffset) Tuple { get; set; }

    [NotifyProperty]
    public partial List<int> List { get; set; }
}
```

「デモ用だし雑に」とか思いながら作り始めたんですけど、
その先 Copilot に向かって「これもやろう」とか言ってみたら思った以上にしっかり作ってくれたというのもあり。

## やった感想

Source Generator を雑に書いて雑に使うの、さすがに昔よりもだいぶ楽になってますねぇ。
昔は Source Generator 側を書き換えたあとの、利用側プロジェクトへの反映性とかかなり悪かったんですけども、
今はもうビルド1回するだけで即時反映になってたりで。

AI コーディング大流行の昨今、(Source Generator みたいに) 学習ソースが少なそうなものはきついのかなぁとかうっすら思いながら書き始めたんですが、
試してみたら全然余裕でちゃんとしたコードが出てきてびっくりしたりも。
ソリューション内に他の例がある状態からとはいえ、[このコード](https://github.com/ufcpp/SourceGeneratorDemo/blob/main/DependencyProperty/Generators/NotifyPropertyChangedGenerator.cs)とか 100% Copilot (Claude Sonnet 4.5)製。
「DependencyProperty に INotifyPropertyChanged 実装の例も足しましょう」とかです。

あと、おそらく割かし最近の話なんですが、 Visual Studio (for Windows)の Analyzer 実行プロセス(Source Generator もこれの中で動いてる)が .NET 8 になっていまして、もう .NET Standard 2.0 (.NET Framework 相当)にとらわれる必要もないかも。

* [.NET 8 Source Generators](https://github.com/ufcpp/SourceGeneratorDemo/tree/main/Net8.0)

まあそれでも .NET 8 なんですけども(ちなみに、 VS Code のやつは普通に .NET 10 で動いてる)。

AttributeTemplateGenerator だけ高機能、かつ、多少の実用性あるのでこいつだけ独立させた方がいいのかなぁとか NuGet パッケージ公開した方がいいのかなぁとかも思いつつ。
