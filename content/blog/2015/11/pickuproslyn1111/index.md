---
title: "ピックアップRoslyn 11/19"
source_url: "https://ufcpp.net/blog/2015/11/pickuproslyn1111/"
content_type: "BlogEntry"
published_at: "2015-11-19T15:14:52"
updated_at: "2015-11-19T15:14:52"
tags: []
umbraco_id: 1815
parent_id: 1810
sort_order: 2
aliases: []
---

# ピックアップRoslyn 11/19

## どこにでも書ける属性

[Proposal: Attributes everywhere #6671](https://github.com/dotnet/roslyn/issues/6671)

属性をどこにでもつけれるようにしたいという話。用途は「コンパイル時限定属性」。

.NETの型システムで規定されてる実行時属性(.NET ILの制限的に付けれる場所が限られてる)よりも、だいぶフレキシブルにどこにでも付けれて、主に analyzer/fixer/injectorで解析・修正・コード生成したい場所につける目印として使いたい。

で、通常の属性と少し構文変えるみたい。今の提案では`[]`の代わりに`[[]]`。

## JSON風のdictionary/arrayリテラル

[JSON-like syntax for dictionary/array initialization and operation #6673](https://github.com/dotnet/roslyn/issues/6673)

最初はタイトルが「Support "json literals" in source」になってて、「VBのXMLリテラルと何が違うんだ。C#にはそんなリテラルは入れない」とか文句を言われてたり。そういうのではなく、要するに、`{ "key": value }`でDictionaryを、`[a, b, c]`で配列を作りたいという話。

C#チームからの提案 issue ページではないものの、元々C# 7のテーマの1つにそういう機能が含まれてたし、実のところ近い実装がすでにあるみたい。

多少、JSONにそろえるべきか、JSONの嫌なとこまで引きずらない方がいいんじゃないかみたいな議論とか、現状のC#の構文と混ぜた時に大丈夫？みたいな心配はある。

## 式ツリー強化

[https://github.com/dotnet/roslyn/issues/2134#issuecomment-156205248](https://github.com/dotnet/roslyn/issues/2134#issuecomment-156205248)

ずいぶん昔のissueページに、以下のようなコメントがついてた。

> In the meantime, I've taken an early stab at providing a runtime library with C#-specific expression nodes that are implemented as reducible expressions. More information at [https://github.com/bartdesmet/ExpressionFutures/tree/master/CSharpExpressions](https://github.com/bartdesmet/ExpressionFutures/tree/master/CSharpExpressions). I'll discuss this with the team in the weeks to come.

> ところで、初期試作として、C#固有の式ツリー ノードに関するランタイム ライブラリ提供試みてる。式の簡約化もできる実装。詳しくは[https://github.com/bartdesmet/ExpressionFutures/tree/master/CSharpExpressions](https://github.com/bartdesmet/ExpressionFutures/tree/master/CSharpExpressions)にて。数週内に、C#チームとディスカッションする。

ですって。

.NET 4の時に導入された式ツリーは、ILレベルの機能しかなくて、C#と比べるとだいぶ貧相で。動的コード生成とかする人からすると、C#の全機能を式ツリー化できないのは結構不便な状況。今だと、Roslynを使ってコード生成という手もなくはないんだけど、ラムダ式からの式ツリー化ができたりするわけではなく、ちょっと用途が違ったりするので。
