---
title: "ローカル関数"
source_url: "https://ufcpp.net/blog/2016/7/cs7localfunc/"
content_type: "BlogEntry"
published_at: "2016-07-17T12:47:00"
updated_at: "2016-07-17T12:47:00"
tags:
  - "C# 7思い出話"
umbraco_id: 1930
parent_id: 1927
sort_order: 1
aliases: []
---

# ローカル関数

ちょっと間が空いたんで「C# 7思い出話」タグでなんか書いてたことを忘れかかっていたりはしますが。

[C# 7のページにローカル関数の話を足しました](../../../../study/csharp/cheatsheet/ap_ver7.md)。

これで、現状で仕様が結構安定してるやつは全部なんですよねぇ。type switch辺りはそろそろ書いてもよさそうなくらい作業進んでは来てますが。タプル型とかはもうちょっと先かなぁ(自分の執筆ペースを考えるとたぶんちょうどいいくらいに安定しそうな気も)。

## ローカル関数

これは[本文にも書いちゃってる](../../../../study/csharp/functional/fun_localfunctions.md#background-local-function)んですけど、要望として多いのは、ローカル関数がほしいというよりも、匿名関数の改良だったんですよね。

- [匿名イテレーター書かせて](https://github.com/dotnet/roslyn/issues/24)
- [`Func<int, int> f = x => x * x;` みたいなのを `var f = x => x * x;` と書かせて](https://github.com/dotnet/roslyn/issues/14)
- 再帰呼び出しさせて
- ジェネリック(polymorphic lambda)にさせて

などなど。

そこに来て、[ローカル関数ほしい？](https://github.com/dotnet/roslyn/issues/259)って話を出したところ、この手の匿名関数に対する問題が、ローカル関数なら結構解決するんじゃないか的な話にどんどんなっていって。数か月後には(正式に機能追加の提案に至る](https://github.com/dotnet/roslyn/issues/2930)。

まあ、元々、[C# Scripting/Interactive](http://www.buildinsider.net/language/csharpscript/01)由来っぽい感じはあったんですが。
スクリプト環境だと、常にメソッド内部にいるかのような文法になる(トップレベルにステートメントを書ける)んで、
そこにメソッドを書くのは、実際のところローカル関数を作るのと大差ない感じになります。
たぶん、今まで全然入れてくれなかった機能が急に出てきたのはこれのせいじゃないかなぁとか思います。
