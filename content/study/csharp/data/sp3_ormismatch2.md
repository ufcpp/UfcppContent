---
title: "[雑記] O/R インピーダンスミスマッチ（クラスの継承）"
source_url: "https://ufcpp.net/study/csharp/data/sp3_ormismatch2/"
content_type: "Article"
published_at: "2008-04-17T00:00:00"
updated_at: "2011-05-28T00:00:00"
tags:
  - "Ver. 3.0"
umbraco_id: 1308
parent_id: 1298
sort_order: 10
aliases:
  - "/csharp/data/sp3_ormismatch2/"
  - "/csharp/sp3_ormismatch2"
  - "/csharp/sp3_ormismatch2.html"
  - "/study/csharp/sp3_ormismatch2"
  - "/study/csharp/sp3_ormismatch2.html"
---

# \[雑記\] O/R インピーダンスミスマッチ（クラスの継承）

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<h5 class="version version3">Ver. 3.0</h5>

「[[雑記] O/R インピーダンスミスマッチ](sp3_ormismatch.md)」では、オブジェクト指向とリレーショナルデータベースの間のデータ構造の差、
階層構造とテーブル結合の差について話をしました。
これに加えて、オブジェクト指向独特の概念として、クラスの「[継承](../oop/oo_inherit.md#derive)」というものがあります。

ここでは、Entity Framework を使って、クラスの継承階層をテーブルにマッピングする例を紹介します。

* サンプル プログラム:[EntityFrameworkSample.zip](../../../../assets/source/EntityFrameworkSample.zip)



## <a id="sec-generated-title-2"></a> <a id="inherit"></a>クラスの継承階層

「[継承](../oop/oo_inherit.md)」や「[多態性](../oop/oo_polymorphism.md)」で説明したように、オブジェクト指向の基本的な概念の1つに継承というものがあります。

例えば、矩形や円などの図形を考えたとき、これらの図形には「面積を求められる」という共通の性質があります。
このような場合、共通の性質を基底クラスにまとめてしまうのがオブジェクト指向のやり方です。
この様子を図示したものと、サンプルコードを以下に示します。

<figure>
	[![クラスの継承階層の例](../../../../assets/media/ufcpp2000/csharp/fig/ormismatch_inheritance.png)](../../../../assets/media/ufcpp2000/csharp/fig/ormismatch_inheritance.png)
	<figcaption>クラスの継承階層の例</figcaption>
</figure>


<pre class="source" title="クラスの継承階層の例" lang="">
<code><span class="reserved">public abstract class</span> <span class="type">Shape</span>
{
    <span class="reserved">public abstract float</span> GetArea();
}

<span class="reserved">public class</span> <span class="type">Rectangle</span> : <span class="type">Shape</span>
{
    <span class="reserved">public float</span> Width { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="reserved">public float</span> Height { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="reserved">public override float</span> GetArea()
    {
        <span class="reserved">return this</span>.Width * <span class="reserved">this</span>.Height;
    }
}

<span class="reserved">public class</span> <span class="type">Circle</span> : <span class="type">Shape</span>
{
    <span class="reserved">public float</span> Radius { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="reserved">public override float</span> GetArea()
    {
        <span class="reserved">return</span> (<span class="reserved">float</span>)(<span class="type">Math</span>.PI * <span class="reserved">this</span>.Radius * <span class="reserved">this</span>.Radius);
    }
}
</code></pre>



## <a id="sec-generated-title-3"></a> <a id="rdb"></a>継承階層を RDB のテーブルで表現

前節で説明したような継承階層を RDB 上で表現するにはいくつか方法がありますが、
ここでは2つほど紹介します。


##### <a id="sec-generated-title-4"></a>テーブルの共有

1つ目の方法は、クラスの継承階層で1つのテーブルを共有します。
（table per hierarchy と呼びます。）
テーブルの各行がどの型かを判別するための列（discriminator: discriminate は「区別・識別する」）を作ります。

<figure>
	[![継承階層を共有テーブル化](../../../../assets/media/ufcpp2000/csharp/fig/TablePerHierarchy1.png)](../../../../assets/media/ufcpp2000/csharp/fig/TablePerHierarchy1.png)
	<figcaption>継承階層を共有テーブル化</figcaption>
</figure>


シンプルですが、型によって使われない列が出るという問題もあります。


##### <a id="sec-generated-title-5"></a>別テーブルを作成

もう1つは、クラスごとに別のテーブルを作ります。
（table per type と呼びます。）

<figure>
	[![継承階層を複数のテーブルに分割](../../../../assets/media/ufcpp2000/csharp/fig/TablePerType1.png)](../../../../assets/media/ufcpp2000/csharp/fig/TablePerType1.png)
	<figcaption>継承階層を複数のテーブルに分割</figcaption>
</figure>


共有テーブルのように無駄な列ができることはありませんが、
複数のテーブルが見かけ上、1つのテーブルであるかのように見せる仕組みが必要になります。


## <a id="sec-generated-title-6"></a> <a id="ormap"></a>Entity Framework における継承構造の O/R マッピング

Entity Framework では、データベース コンテキストを作る際に、
DbContext クラスの OnModelCreating メソッドをオーバーライドすることで、
継承階層のテーブル化方法をカスタマイズできます。

table per hierarchy にしたい場合は以下のように書きます。

<pre class="source" title="table per hierarchy なデータベース コンテキスト" lang="">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">TablePerHierarchyContext</span> : <span class="type">DbContext</span>
{
    <span class="reserved">public</span> <span class="type">DbSet</span>&lt;<span class="type">Shape</span>&gt; Shapes { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="reserved">protected</span> <span class="reserved">override</span> <span class="reserved">void</span> OnModelCreating(<span class="type">DbModelBuilder</span> modelBuilder)
    {
        modelBuilder.Entity&lt;<span class="type">Shape</span>&gt;()
            .Map&lt;<span class="type">Rectangle</span>&gt;(x =&gt; x.Requires(<span class="literal">"type"</span>).HasValue(<span class="literal">"R"</span>))
            .Map&lt;<span class="type">Circle</span>&gt;(x =&gt; x.Requires(<span class="literal">"type"</span>).HasValue(<span class="literal">"C"</span>));
    }
}
</code></pre>


一方、
table per type にしたい場合は以下のように書きます。

<pre class="source" title="table per type なデータベース コンテキスト" lang="">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">TablePerTypeContext</span> : <span class="type">DbContext</span>
{
    <span class="reserved">public</span> <span class="type">DbSet</span>&lt;<span class="type">Shape</span>&gt; Shapes { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="reserved">protected</span> <span class="reserved">override</span> <span class="reserved">void</span> OnModelCreating(<span class="type">DbModelBuilder</span> modelBuilder)
    {
        modelBuilder.Entity&lt;<span class="type">Rectangle</span>&gt;().ToTable(<span class="literal">"Rectangle"</span>);
        modelBuilder.Entity&lt;<span class="type">Circle</span>&gt;().ToTable(<span class="literal">"Circle"</span>);
    }
}
</code></pre>



##### <a id="sec-generated-title-7"></a>サンプル データ作成

作成した2つのデータベース コンテキストを使って、サンプル データを作成してみましょう。

<pre class="source" title="サンプル データの作成" lang="">
<code><span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> Create()
{
    <span class="reserved">using</span> (<span class="reserved">var</span> db = <span class="reserved">new</span> <span class="type">TablePerHierarchyContext</span>())
    {
        Create(db.Shapes);
        db.SaveChanges();
    }

    <span class="comment">// ↑↓見ての通り、コンテキストが違う以外は全く一緒。</span>

    <span class="reserved">using</span> (<span class="reserved">var</span> db = <span class="reserved">new</span> <span class="type">TablePerTypeContext</span>())
    {
        Create(db.Shapes);
        db.SaveChanges();
    }
}

<span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> Create(System.Data.Entity.<span class="type">DbSet</span>&lt;<span class="type">Shape</span>&gt; shapes)
{
    shapes.Add(<span class="reserved">new</span> <span class="type">Rectangle</span> { Width = 10, Height = 20 });
    shapes.Add(<span class="reserved">new</span> <span class="type">Rectangle</span> { Width = 15, Height = 12 });
    shapes.Add(<span class="reserved">new</span> <span class="type">Circle</span> { Radius = 1.5f });
    shapes.Add(<span class="reserved">new</span> <span class="type">Circle</span> { Radius = 3 });
}
</code></pre>


TablePerHierarchyContext によって作られるデータベースは以下のようになります。

<figure>
	[![TablePerHierarchyContext によって作られるデータベース](../../../../assets/media/ufcpp2000/csharp/fig/TablePerHierarchy2.png)](../../../../assets/media/ufcpp2000/csharp/fig/TablePerHierarchy2.png)
	<figcaption>TablePerHierarchyContext によって作られるデータベース</figcaption>
</figure>


これに対して、
TablePerTypeContext によって作られるデータベースは以下のようになります。

<figure>
	[![TablePerHierarchyContext によって作られるデータベース](../../../../assets/media/ufcpp2000/csharp/fig/TablePerType2.png)](../../../../assets/media/ufcpp2000/csharp/fig/TablePerType2.png)
	<figcaption>TablePerHierarchyContext によって作られるデータベース</figcaption>
</figure>



##### <a id="sec-generated-title-8"></a>データの参照

作成したデータを参照してみましょう。

<pre class="source" title="データの参照" lang="">
<code><span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> Query()
{
    <span class="reserved">using</span> (<span class="reserved">var</span> db = <span class="reserved">new</span> <span class="type">TablePerTypeContext</span>())
    {
        Query(db.Shapes);
    }

    <span class="comment">// ↑↓見ての通り、コンテキストが違う以外は全く一緒。</span>

    <span class="reserved">using</span> (<span class="reserved">var</span> db = <span class="reserved">new</span> <span class="type">TablePerHierarchyContext</span>())
    {
        Query(db.Shapes);
    }
}

<span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> Query(System.Data.Entity.<span class="type">DbSet</span>&lt;<span class="type">Shape</span>&gt; shapes)
{
    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> shapes)
    {
        <span class="type">Console</span>.WriteLine(<span class="literal">"{0}: {1}"</span>, x.GetType().Name, x.GetArea());
    }
}
</code></pre>


<pre class="console" title="実行結果">
Table Per Hierarchy
Rectangle: 200
Rectangle: 180
Circle: 7.068583
Circle: 28.27433
Table Per Type
Circle: 7.068583
Circle: 28.27433
Rectangle: 200
Rectangle: 180
</pre>



## <a id="sec-generated-title-9"></a> <a id="linq-to-sql"></a>LINQ to SQL 版

<span class="expand-button" title="展開/折畳">（LINQ to SQL 版）</span>
<div class="expand-panel" markdown="1" title="（LINQ to SQL 版）">
      

##### <a id="sec-generated-title-10"></a>継承構造を RDB のテーブルで表現

前節で説明したような継承構造を RDB 上で表現するには、
型識別用の情報を格納した列（discriminator: discriminate は「区別・識別する」）を作ります。

        
引き続き、図形（shape, rectangle, circle）を例にとって説明すると、
例えば、図2か図3のようなテーブルを定義することになります。

        
<figure>
	[![継承構造をテーブル化(1)](../../../../assets/media/ufcpp2000/csharp/fig/ormismatch_table1.png)](../../../../assets/media/ufcpp2000/csharp/fig/ormismatch_table1.png)
	<figcaption>継承構造をテーブル化(1)</figcaption>
</figure>


        
<figure>
	[![継承構造をテーブル化(2)](../../../../assets/media/ufcpp2000/csharp/fig/ormismatch_table2.png)](../../../../assets/media/ufcpp2000/csharp/fig/ormismatch_table2.png)
	<figcaption>継承構造をテーブル化(2)</figcaption>
</figure>


        
この例の場合、Type 列が discrimitator になります。


      

##### <a id="sec-generated-title-11"></a>LINQ to SQL における継承構造の O/R マッピング

「[クラスの継承階層](#inherit)」で説明したクラスの継承構造と、
「[継承階層を RDB のテーブルで表現](#rdb)」で説明したテーブル構造を対応付けるため、
LINQ to SQL では、継承構造をもつクラスに InheritanceMapping 属性をつけ、
discriminator にしたいプロパティの Column 属性に <code>IsDiscriminator = true</code> をつけます。

        
例えば、図2のようにしたい場合には、以下のようなコードを書きます。

        
<pre class="source" title="InheritanceMapping の例1" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Data.Linq;
<span class="reserved">using</span> System.Data.Linq.Mapping;

<span class="reserved">namespace</span> LinqToSqlSample
{
  <span class="reserved">public sealed class</span> <span class="type">ShapeDataContext</span> : <span class="type">DataContext</span>
  {
    <span class="reserved">public</span> ShapeDataContext(<span class="reserved">string</span> connection) : <span class="reserved">base</span>(connection) {}

    <span class="reserved">public</span> <span class="type">Table</span>&lt;<span class="type">Shape</span>&gt; Shapes;
  }

  <span class="reserved">public enum</span> <span class="type">ShapeType</span> : <span class="reserved">int</span>
  {
    Invalid,
    Rectangle,
    Circle,
  }

  [<span class="type">Table</span>(Name = <span class="literal">"Shape"</span>)]
  [<span class="type"><em>InheritanceMapping</em></span>(Code = <span class="literal">"0"</span>, Type = <span class="reserved">typeof</span>(<span class="type">Shape</span>), IsDefault = <span class="reserved">true</span>)]
  [<span class="type"><em>InheritanceMapping</em></span>(Code = <span class="literal">"1"</span>, Type = <span class="reserved">typeof</span>(<span class="type">Rectangle</span>))]
  [<span class="type"><em>InheritanceMapping</em></span>(Code = <span class="literal">"2"</span>, Type = <span class="reserved">typeof</span>(<span class="type">Circle</span>))]
  <span class="reserved">public class</span> <span class="type">Shape</span>
  {
    [<span class="type">Column</span>(AutoSync = <span class="type">AutoSync</span>.OnInsert, IsDbGenerated = <span class="reserved">true</span>, IsPrimaryKey = <span class="reserved">true</span>)]
    <span class="reserved">public int</span> ID;

    [<span class="type">Column</span>(<em>IsDiscriminator</em> = <span class="reserved">true</span>)]
    <span class="reserved">public</span> <span class="type">ShapeType</span> Type;

    <span class="reserved">public virtual float</span> GetArea() { <span class="reserved">return</span> <span class="literal">0</span>; }
  }

  <span class="reserved">public class</span> <span class="type">Rectangle</span> : <span class="type">Shape</span>
  {
    [<span class="type">Column</span>(CanBeNull = <span class="reserved">true</span>)]
    <span class="reserved">public float</span> Width;

    [<span class="type">Column</span>(CanBeNull = <span class="reserved">true</span>)]
    <span class="reserved">public float</span> Height;

    <span class="reserved">public override float</span> GetArea()
    {
      <span class="reserved">return this</span>.Width * <span class="reserved">this</span>.Height;
    }
  }

  <span class="reserved">public class</span> <span class="type">Circle</span> : <span class="type">Shape</span>
  {
    [<span class="type">Column</span>(CanBeNull = <span class="reserved">true</span>)]
    <span class="reserved">public float</span> Radius;

    <span class="reserved">public override float</span> GetArea()
    {
      <span class="reserved">return</span> (<span class="reserved">float</span>)(<span class="type">Math</span>.PI * <span class="reserved">this</span>.Radius * <span class="reserved">this</span>.Radius);
    }
  }
}
</code></pre>


        
あるいは、図3のようにしたい場合には、以下のようなコードを書きます。

        
<pre class="source" title="InheritanceMapping の例2" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Data.Linq;
<span class="reserved">using</span> System.Data.Linq.Mapping;

<span class="reserved">namespace</span> LinqToSqlSample
{
  <span class="reserved">public sealed class</span> <span class="type">ShapeDataContext</span> : <span class="type">DataContext</span>
  {
    <span class="reserved">public</span> ShapeDataContext(<span class="reserved">string</span> connection) : <span class="reserved">base</span>(connection) {}

    <span class="reserved">public</span> <span class="type">Table</span>&lt;<span class="type">Shape</span>&gt; Shapes;
  }

  <span class="reserved">public enum</span> <span class="type">ShapeType</span> : <span class="reserved">int</span>
  {
    Invalid,
    Rectangle,
    Circle,
  }

  [<span class="type">Table</span>(Name = <span class="literal">"Shape"</span>)]
  [<span class="type"><em>InheritanceMapping</em></span>(Code = <span class="literal">"0"</span>, Type = <span class="reserved">typeof</span>(<span class="type">Shape</span>), IsDefault = <span class="reserved">true</span>)]
  [<span class="type"><em>InheritanceMapping</em></span>(Code = <span class="literal">"1"</span>, Type = <span class="reserved">typeof</span>(<span class="type">Rectangle</span>))]
  [<span class="type"><em>InheritanceMapping</em></span>(Code = <span class="literal">"2"</span>, Type = <span class="reserved">typeof</span>(<span class="type">Circle</span>))]
  <span class="reserved">public class</span> <span class="type">Shape</span>
  {
    [<span class="type">Column</span>(AutoSync = <span class="type">AutoSync</span>.OnInsert, IsDbGenerated = <span class="reserved">true</span>, IsPrimaryKey = <span class="reserved">true</span>)]
    <span class="reserved">public int</span> ID;

    [<span class="type">Column</span>(<em>IsDiscriminator</em> = <span class="reserved">true</span>)]
    <span class="reserved">public</span> <span class="type">ShapeType</span> Type;

    [<span class="type">Column</span>(Name = <span class="literal">"a"</span>, CanBeNull = <span class="reserved">true</span>)]
    <span class="reserved">protected float</span> a;

    [<span class="type">Column</span>(Name = <span class="literal">"b"</span>, CanBeNull = <span class="reserved">true</span>)]
    <span class="reserved">protected float</span> b;

    <span class="reserved">public virtual float</span> GetArea() { <span class="reserved">return</span> <span class="literal">0</span>; }
  }

  <span class="reserved">public class</span> <span class="type">Rectangle</span> : <span class="type">Shape</span>
  {
    <span class="reserved">public float</span> Width
    {
      <span class="reserved">get</span> { <span class="reserved">return this</span>.a; }
      <span class="reserved">set</span> { <span class="reserved">this</span>.a = <span class="reserved">value</span>; }
    }

    <span class="reserved">public float</span> Height
    {
      <span class="reserved">get</span> { <span class="reserved">return this</span>.b; }
      <span class="reserved">set</span> { <span class="reserved">this</span>.b = <span class="reserved">value</span>; }
    }

    <span class="reserved">public override float</span> GetArea()
    {
      <span class="reserved">return this</span>.Width * <span class="reserved">this</span>.Height;
    }
  }

  <span class="reserved">public class</span> <span class="type">Circle</span> : <span class="type">Shape</span>
  {
    <span class="reserved">public float</span> Radius
    {
      <span class="reserved">get</span> { <span class="reserved">return this</span>.a; }
      <span class="reserved">set</span> { <span class="reserved">this</span>.a = <span class="reserved">value</span>; }
    }

    <span class="reserved">public override float</span> GetArea()
    {
      <span class="reserved">return</span> (<span class="reserved">float</span>)(<span class="type">Math</span>.PI * <span class="reserved">this</span>.Radius * <span class="reserved">this</span>.Radius);
    }
  }
}
</code></pre>


        
要するに、<code>IsDiscriminator = true</code> のついた列の値の基づいて、
InheritanceMapping 属性の情報を元にどの派生クラスになるか決定されます。

        
以下のようなコードで動作確認ができます。

        
<pre class="source" title="確認用のコード" lang="">
<code><span class="reserved">var</span> db = <span class="reserved">new</span> <span class="type">ShapeDataContext</span>(<span class="literal">"shape.sdf"</span>);

<span class="reserved">if</span> (!db.DatabaseExists())
{
  db.CreateDatabase();

  db.Shapes.InsertOnSubmit(<span class="reserved">new</span> <span class="type">Rectangle</span> { Width = <span class="literal">2</span>, Height = <span class="literal">3</span> });
  db.Shapes.InsertOnSubmit(<span class="reserved">new</span> <span class="type">Circle</span> { Radius = <span class="literal">1</span> });
  db.Shapes.InsertOnSubmit(<span class="reserved">new</span> <span class="type">Rectangle</span> { Width = <span class="literal">1</span>, Height = <span class="literal">2</span> });
  db.Shapes.InsertOnSubmit(<span class="reserved">new</span> <span class="type">Circle</span> { Radius = <span class="literal">2</span> });
  db.Shapes.InsertOnSubmit(<span class="reserved">new</span> <span class="type">Rectangle</span> { Width = <span class="literal">2</span>, Height = <span class="literal">1</span> });
  db.Shapes.InsertOnSubmit(<span class="reserved">new</span> <span class="type">Circle</span> { Radius = <span class="literal">0.5F</span> });
  db.Shapes.InsertOnSubmit(<span class="reserved">new</span> <span class="type">Rectangle</span> { Width = <span class="literal">0.5F</span>, Height = <span class="literal">0.5F</span> });
  db.Shapes.InsertOnSubmit(<span class="reserved">new</span> <span class="type">Circle</span> { Radius = <span class="literal">0.1F</span> });

  db.SubmitChanges();
}

<span class="reserved">foreach</span> (<span class="reserved">var</span> s <span class="reserved">in</span> db.Shapes)
{
  <span class="type">Console</span>.Write(<span class="literal">"{0}, area = {1}\n"</span>, s.Type, s.GetArea());
}
</code></pre>


        
<pre class="console" title="実行結果">
Rectangle, area = 6
Circle, area = 3.141593
Rectangle, area = 2
Circle, area = 12.56637
Rectangle, area = 2
Circle, area = 0.7853982
Rectangle, area = 0.25
Circle, area = 0.03141593
</pre>


    
</div>
