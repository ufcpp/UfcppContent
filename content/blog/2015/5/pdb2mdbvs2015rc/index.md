---
title: "Visual Studio 2015 RC と pdb2mdb"
source_url: "https://ufcpp.net/blog/2015/5/pdb2mdbvs2015rc/"
content_type: "BlogEntry"
published_at: "2015-05-22T11:05:39"
updated_at: "2015-05-22T11:06:29"
tags: []
umbraco_id: 1743
parent_id: 1700
sort_order: 3
aliases: []
---

# Visual Studio 2015 RC と pdb2mdb

Unity (というか、Visual Studio Tools for Unity)とVisual Studio 2015 RCとの間での問題で、PDBからMDBへの変換がうまく動いてなくて困っている(一時的な対処方はなんとかできてる)という話。

## PDB, MDB

まず、PDBって何かの簡単な紹介から。

PDBはProgram Databaseの略です。

Program Databaseは、実行ファイルのどの辺りが、ソースコードの何行目でどのメソッド通ってるかとかの情報が入ったファイルです。Visual Studio上でブレイク ポイント仕掛けて止めたり、スタック トレース追えたりするのはこの情報ありき。

とはいえ、この手のProgram Databaseには規格とか標準がなくて、Visual Studioが使ってる形式がPDB(拡張子も .pdb)で、Monoが使ってる形式がMDB。別形式です。

一応、Monoはpdb2mdb っていうツールを提供していて、PDBからMDB形式に変換できたりはします。

[Xamarin.Android, Xamarin.iOS](http://xamarin.com/platform), [Unity](http://japan.unity3d.com/)とかに対してVisual Studioを使った開発がしたい場合、PDBからMDB変換して実行することになります。だいたいは裏で勝手にやってくれるんですが、それが最近うまく動いていない状態だったりします。

## PDB の形式変わったみたい

何のせいで問題が出ているかというと、PDBの形式が変わったみたい。

Unityについてくるバージョンのpdb2mdb(Unityインストール フォルダー以下、 Editor\Data\MonoBleedingEdge\lib\mono\4.0\pdb2mdb.exe にある)とか、[NuGet Gallaryから取れるバージョン](https://www.nuget.org/packages/Mono.pdb2mdb)だと、

<pre>
Unknown custom metadata item kind: 6
</pre>

とかいうエラーを吐いて停止。

monoのGitHubリポジトリを見てみたところ、先月に

- [pdb2mdb fix decimal values reading](https://github.com/mono/mono/commit/facbf81e0589bd1a854bec2336caa0d4c061b801)

っていうコミットがあったんで、たぶんこれですかね。PDB中のdecimalの値がちゃんと読めていなかったらしい。

ちなみに、Visual Studio 2015 RC付属(インストーラーでチェックを入れれば一緒にインストールされる)Xamarin.Androidだとこのコミットとりこみ済みみたいで、正しく実行できます。

## 最新のpdb2mdb

が、[Visual Studio 2015 Preview Tools for Unity](https://visualstudiogallery.msdn.microsoft.com/8d26236e-4a64-4d64-8486-7df95156aba9) がまだこれに対応していないくさくて、Visual Studio 2015でビルドしたDLLをUnityに持ってきても、MDBファイルができなくなっておりまして。

(最終更新日時4/30ってなってるのをインストールしてみてもダメでした。インストールしなおしがうまく行ってないのかもしれないものの。)

あと、この問題が解消したバージョンのpdb2mdbのありかがわからず… 

結局、Xamarin.Androidでは動いているはず(= 問題解消済みのpdb2mdbが動いているはず)なので、

- Visual Studio 2015同梱のXamarin.Androidのプロジェクト テンプレートを覗いて
- \Program Files (x86)\MSBuild\Xamarin\Android に関連ファイルがあって
- Xamarin.Android.Build.Tasks.dll ってDLLの中にpdb2mdb相当の処理が入ってることを調べて
- `Pdb2Mdb.Converter.Convert`静的メソッドを呼べばいいこと突き止めて
- PowerShell スクリプト書いて実行

<script src="https://gist.github.com/ufcpp/49fa3e76c1246fd434b4.js"></script>

とかいう作業を今日やったり。

Visual Studio Tools for Unityがちゃんと対応するまでの一時しのぎではあるんですけども。嫌な処理を書いてしまった…
