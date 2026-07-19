---
title: "コマンドライン引数"
source_url: "https://ufcpp.net/study/csharp/structured/st_command/"
content_type: "Article"
published_at: "2015-05-06T14:08:57"
updated_at: "2018-04-14T23:45:53"
tags: []
umbraco_id: 1239
parent_id: 1217
sort_order: 11
aliases:
  - "/csharp/st_command"
  - "/csharp/st_command.html"
  - "/csharp/structured/st_command/"
  - "/study/csharp/st_command"
  - "/study/csharp/st_command.html"
---

# コマンドライン引数

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
C# にはコマンドライン引数は <code>Main</code> 関数の引数としてプログラムに渡されます。
ここでは、コマンドライン引数というものが何なのかを簡単に説明した後、
C# でコマンドライン引数を受け取る方法について説明します。


##### <a id="sec-generated-title-2"></a>ポイント
* コマンドライン引数: プログラム起動の際に渡されるオプションの値

* C# では、Main 関数の引数として受け取れる



##<a id="sec-generated-title-3"></a> <a id="cmd"></a>コマンドライン引数とは
コマンドプロンプト(Win9x の場合は「DOS プロンプト」と呼ばれる)上でファイルのコピーを行う場合、
copy というコマンドを利用します。copy は以下のようにして、コピー元のファイルとコピー先のディレクトリ(フォルダ)を指定することによってファイルのコピーを行います。

<pre class="console" title="copy コマンド">
copy <span class="input">コピーするファイル</span> <span class="input">コピー先のディレクトリ</span>
</pre>


このように、コマンドやプログラムを起動する際に、プログラム名の後に続けて入力した文字列はパラメータとしてプログラムに渡されます。
このようなプログラム起動時に渡されるパラメータのことを<em>コマンドライン引数</em>と呼びます。

また、コマンドライン引数はコンソールアプリケーション(コマンドプロンプトで呼び出される文字ベースのプログラム)だけでなく、GUI アプリケーションでも利用することが出来ます。
例えば、スタートメニューから [プログラム名を指定して実行] を選んで、
以下のように入力してみてください。

<pre class="source" title="エクスプローラ起動" lang="">
<code>explorer.exe
</code></pre>


以下のようにエクスプローラのウィンドウが開くと思います。
(以下のものは Windows XP で実行した結果)

<figure>
	[![エクスプローラ オプション無し](../../../../assets/media/ufcpp2000/csharp/fig/explorer1.png)](../../../../assets/media/ufcpp2000/csharp/fig/explorer1.png)
	<figcaption>エクスプローラ オプション無し</figcaption>
</figure>


同様にスタートメニューから[プログラム名を指定して実行]を選んで、今度は以下のように入力してみてください。

<pre class="source" title="オプション付きでエクスプローラ起動" lang="">
<code>explorer.exe /e,/root,"C:\Program Files\Internet Explorer"
</code></pre>


以下のように、先ほどと内容の異なる形式でエクスプローラが起動します。

<figure>
	[![エクスプローラ オプションあり](../../../../assets/media/ufcpp2000/csharp/fig/explorer2.png)](../../../../assets/media/ufcpp2000/csharp/fig/explorer2.png)
	<figcaption>エクスプローラ オプションあり</figcaption>
</figure>


「<code>/e,/root,"C:\Program Files\Internet Explorer"</code>」という文字列がコマンドライン引数としてエクスプローラに渡され、その結果としてエクスプローラの表示形式が変わったわけです。


##<a id="sec-generated-title-4"></a> <a id="arg"></a>C#でコマンドライン引数を利用する
今まで、<code>Main</code> 関数には引数を書いていませんでしたが、
コマンドライン引数を受け取りたい場合には、以下のように <code>Main</code> 関数に <code>string[]</code> 型の引数を書きます。

<pre class="source" title="" lang="">
<code><span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
</code></pre>


プログラムに与えた引数はこの <code>args</code> に格納されます。
(args は arguments (引数)の略で、慣習的にこの名前が良く用いられます。)
コマンドライン引数はスペースで区切って複数与えることが出来ます。
この際、コマンドライン引数は先に入力されたものから順に <code>args</code> に格納されていきます。
例えば、以下のようなプログラムを作成し、

<pre class="source" title="コマンドライン引数を受け取るプログラム" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">public class</span> CommandLineSample
{
  <span class="reserved">public static void</span> Main(<span class="reserved">string</span>[] args)
  {
    <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;args.Length; ++i)
      Console.Write(<span class="literal">"{0}番目のコマンドライン引数は{1}です。\n"</span>, i, args[i]);
  }
}
</code></pre>


以下のようにして(ただし、<code>test.exe</code>という名前で作成した実行ファイルを作成したとします)
実行すると、

<pre class="console" title="">
test aaa bbb ccc ddd
</pre>


以下のような結果が得られます。

<pre class="console" title="">
0番目のコマンドライン引数はaaaです。
1番目のコマンドライン引数はbbbです。
2番目のコマンドライン引数はcccです。
3番目のコマンドライン引数はdddです。
</pre>



##<a id="sec-generated-title-5"></a> <a id="return"></a>終了コード
コマンドライン引数の他に、プログラムには終了コードというものがあります。
終了コードとは、プログラムが正しく終了したかどうかなどの情報を得るために、
プログラム終了時に返す値のことです。

C# でプログラムを作る際、自分で終了コードを指定したい場合、<code>Main</code> 関数の戻り値の型を<code>int</code>型にします。
<code>Main</code> 関数の戻り値がそのままプログラムの終了コードになります。
例えば、以下のようなプログラムを書いた場合、終了コードは0になります。

<pre class="source" title="終了コードを返す例" lang="">
<code><span class="reserved">public class</span> CommandLineSample
{
  <span class="reserved">public static int</span> Main()
  {
    <span class="reserved">return</span> 0;
  }
}
</code></pre>


習慣的に、正常終了したときに0を返し、それ以外のときには0以外の値(エラーの原因に応じて値を変える)を返すようにします。


##### <a id="sec-generated-title-6"></a>サンプル
<pre class="source" title="コマンドライン引数のサンプル" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.IO;

<span class="reserved">public class</span> CommandLineSample
{
  <span class="comment">/// &lt;summary&gt;
  /// コマンドライン引数でファイル名を受け取り、そのファイルの中身を表示する。
  /// コマンドライン引数の数がおかしかった場合や、
  /// ファイルが見つからない場合や、ファイルのアクセス権限がない場合、
  /// 終了コード -1 を返して終了する。
  /// 正常終了した場合には終了コード 0 を返す。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public static int</span> Main(<span class="reserved">string</span>[] args)
  {
    <span class="comment">// 引数チェック</span>
    <span class="reserved">if</span>(args.Length != 1)
    {
      Console.Write(<span class="literal">"引数の数がおかしいです\n"</span>);
      <span class="reserved">return</span> -1;
    }

    StreamReader reader = <span class="reserved">null</span>;
    <span class="reserved">try</span>
    {
      <span class="comment">// ファイルを開いて中身を表示</span>
      reader = <span class="reserved">new</span> StreamReader(args[0]);
      <span class="reserved">string</span> text = reader.ReadToEnd();
      Console.Write(text);
    }
    <span class="reserved">catch</span>(Exception e)
    {
      <span class="comment">// エラー処理
      // 詳しくは「例外処理」で説明します。
      // ファイルが存在しなかったり、アクセス権限がない場合にここが実行される。</span>
      Console.Write(e.Message+<span class="literal">"\n"</span>);
      <span class="reserved">return</span> -1;
    }
    <span class="reserved">finally</span>
    {
      <span class="comment">// 後処理
      // これも「例外処理」で説明します。</span>
      <span class="reserved">if</span>(reader != <span class="reserved">null</span>)
        reader.Close();
    }

    <span class="reserved">return</span> 0;
  }
}
</code></pre>


プログラムの実行ファイル名は<code>test.exe</code>とする。
<code>test.exe</code>は<code>C:\mydoc</code>にあるものとして、
同じディレクトリ中に<code>test.txt</code>というファイルがあって、
その中身が

<pre class="source" title="test.txt の中身" lang="">
<code> test test test test
テスト テスト テスト
</code></pre>


であるとき、実行結果は以下のようになる。

<pre class="console" title="">
C:\mydoc&gt; <span class="input">test</span>
引数の数がおかしいです
</pre>


<pre class="console" title="">
C:\mydoc&gt; <span class="input">test aaa</span>
Could not find file "C:\mydoc\aaa".
</pre>


<pre class="console" title="">
C:\mydoc&gt; <span class="input">test test.txt</span>
 test test test test
テスト テスト テスト
</pre>
