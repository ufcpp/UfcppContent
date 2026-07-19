---
title: "list"
source_url: "https://ufcpp.net/study/stl/seq_container/list/"
content_type: "Article"
published_at: "2015-05-06T14:23:26"
updated_at: "2015-05-06T14:23:26"
tags: []
umbraco_id: 1633
parent_id: 1630
sort_order: 2
aliases:
  - "/stl/list.html"
  - "/stl/seq_container/list/"
  - "/study/stl/list.html"
---

# list

## <a id="sec-generated-title-1"></a> <a id="about"></a>listとは

STLのlistはいわゆる双方向連結リストと言われているものです。

双方向連結リストを言うものを説明する前に、まずリストについての説明をします。
リストとは任意の位置への要素の挿入・削除を要素の順序を変えることなく行うことにできるデータ構造のことです。
このような操作を配列を用いてやろうとすると、

<pre class="source" title="" lang="">
<code><span class="reserved">int</span> array[SIZE];
<span class="reserved">int</span> rear;<span class="comment">//データの末尾</span>

<span class="comment">//ポインターpの指す位置に新しいデータを挿入する。</span>
<span class="reserved">void</span> insert(<span class="reserved">int</span>* p, <span class="reserved">int</span> data)
{
  <span class="reserved">int</span>* q;
  rear++;
  <span class="reserved">if</span>(rear==SIZE)<span class="comment">//full</span>
    <span class="reserved">return</span>;

  <span class="reserved">for</span>(q=array+rear; q&gt;p; q--)
    *q = *(q-1); <span class="comment">//要素を1つずつずらす</span>

  *p =data; <span class="comment">//そして、空いた場所に新しいデータを挿入</span>
}

<span class="comment">//ポインターpの指す位置のデータを削除する</span>
<span class="reserved">void</span> erase(<span class="reserved">int</span>* p)
{
  <span class="reserved">int</span>* q;
  <span class="reserved">for</span>(q=p; q&lt;array+rear; q++)
    *q = *(a+1); <span class="comment">//要素を1つずつ詰める</span>
  rear--;
}
</code></pre>


という風になります。
しかし、この方法では配列が満員になった場合に、それ以上要素の追加ができなくなります。また、挿入・削除が行われるたびに要素をずらしていく必要があるので
かなりの時間が(平均時間計算量 O(SIZE) )かかります。

そこで、連結リストというものを使います。
連結リストには線形リスト(linear list、または単方向リスト(one-way list))と呼ばれるものと
双方向リスト(doubly linked list、two-way list)と呼ばれるものがあります。

線形リストから説明します。
まず、ノード(node:節目、結び目)といわれる構造体を定義します。

<pre class="source" title="" lang="">
<code><span class="reserved">struct</span> node
{
  node* next;
  <span class="reserved">int</span> data;
};
</code></pre>


そして、このノードを数珠繋ぎしていくと線形リストになります。<br></br>	[![onewaylist.png](../../../../assets/media/ufcpp2000/stl/fig/onewaylist.png)](../../../../assets/media/ufcpp2000/stl/fig/onewaylist.png)
<br></br>
この際、一番後ろのノードの<code>next</code>にはNULLポインターを入れておきます。

<pre class="source" title="" lang="">
<code>node *root, *rear;

<span class="reserved">void</span> insert(node* p, <span class="reserved">int</span> data)
{
  node* prev;
  <span class="reserved">for</span>(prev=root; prev-&gt;next!=p &amp;&amp; prev-&gt;next!=NULL; prev=prev-&gt;next);

  <span class="reserved">if</span>(prev-&gt;next == NULL)<span class="comment">//p don't exist;</span>
    <span class="reserved">return</span>;

  node* tmp = <span class="reserved">new</span> node;
  tmp-&gt;data = data;
  tmp-&gt;next = p;
  prev-&gt;next = tmp;
}

<span class="reserved">void</span> erase(node* p);
{
  node* prev;
  <span class="reserved">for</span>(prev=root; prev-&gt;next!=p &amp;&amp; prev-&gt;next!=NULL; prev=prev-&gt;next);

  <span class="reserved">if</span>(prev-&gt;next == NULL)<span class="comment">//p don't exist;</span>
    <span class="reserved">return</span>;

  prev-&gt;next = p-&gt;next;
  <span class="reserved">delete</span> p;
}
</code></pre>


これで要素をいくらでも追加できるようになりました。
しかし、先頭以外への要素の挿入・削除にはやはり時間がかかります(平均時間計算量O(N))。

次は双方向リストについて。
双方向リストでも線形リストと同様にまずノードという構造体を定義します。
線形リストと違うところは、次の要素を指すポインター<code>next</code>の他に、
一つ前の要素を指すポインターも持っていることです。

<pre class="source" title="" lang="">
<code><span class="reserved">struct</span> node
{
  node *next, *prev;
  <span class="reserved">int</span> data;
};
</code></pre>


そして、このノードをつないでいくことで双方向リストになります。<br></br>	[![twowaylist.png](../../../../assets/media/ufcpp2000/stl/fig/twowaylist.png)](../../../../assets/media/ufcpp2000/stl/fig/twowaylist.png)
<br></br>

<pre class="source" title="" lang="">
<code><span class="reserved">void</span> insert_prev(node* p, <span class="reserved">int</span> data)
{
  node* q = <span class="reserved">new</span> node;
  q-&gt;prev = p-&gt;prev;
  q-&gt;next = p;
  p-&gt;prev = q;
  q-&gt;prev-&gt;next = q;
  q-&gt;data = data;
}

<span class="reserved">void</span> insert_next(node* p, <span class="reserved">int</span> data)
{
  node* q = <span class="reserved">new</span> node;
  q-&gt;next = p-&gt;next;
  q-&gt;prev = p;
  p-&gt;next = q;
  q-&gt;next-&gt;prev = q;
  q-&gt;data = data;
}

<span class="reserved">void</span> erase(node* p)
{
  node* next = p-&gt;next;
  p-&gt;next-&gt;prev = p-&gt;prev;
  p-&gt;prev-&gt;next = p-&gt;next;
  <span class="reserved">delete</span> p;
}
</code></pre>


これで、任意の位置への要素の挿入がO(1)で行えるようになります。

ところで、リストの先頭の<code>prev</code>、末尾の<code>next</code>を<code>NULL</code>にしていると、
リストの先頭への要素の挿入がうまくいきません。

<pre class="source" title="" lang="">
<code>node* root=NULL;

<span class="reserved">void</span> insert_prev(node* p, <span class="reserved">int</span> data)
{
  <span class="reserved">if</span>(p==root)<span class="comment">//挿入位置が先頭の場合、特別な処理が必要。</span>
  {
    root = <span class="reserved">new</span> node;
    root-&gt;prev = NULL;
    root-&gt;next = p;
    p-&gt;prev = root;
    <span class="reserved">return</span>;
  }

  node* q = <span class="reserved">new</span> node;
  q-&gt;prev = p-&gt;prev;
  q-&gt;next = p;
  p-&gt;prev = q;
  q-&gt;prev-&gt;next = q;
  q-&gt;data = data;
}

<span class="reserved">void</span> erase(node* p)
{
  <span class="reserved">if</span>(p==root)<span class="comment">//挿入位置が先頭の場合、特別な処理が必要。</span>
  {
    p = p-&gt;next;
    <span class="reserved">delete</span> root;
    root = p;
    <span class="reserved">return</span>;
  }

  node* next = p-&gt;next;
  p-&gt;next-&gt;prev = p-&gt;prev;
  p-&gt;prev-&gt;next = p-&gt;next;
  <span class="reserved">delete</span> p;
}
</code></pre>


ここで、リストの先頭<code>root</code>には要素を格納しないダミーノードを付けておき、
リストの末尾の<code>next</code>には<code>NULL</code>ではなく、<code>root</code>を代入しておきます。

<pre class="source" title="" lang="">
<code>node* root;<span class="comment">//ダミーノード</span>

<span class="reserved">void</span> init()
{
  root = <span class="reserved">new</span> node;
  root-&gt;next = root;
  root-&gt;prev = root;
}
</code></pre>


こうすることで、先頭、末尾への要素の追加が簡単になり、
<code>
        <span class="reserved">if</span>(p==root)
      </code>の部分を書く必要がなくなります。
また、リストの末尾(<code>root-&gt;prev</code>はリストの末尾を指している)へのアクセスも容易になります。
こういう構造のリストを循環リストといいます。


## <a id="sec-generated-title-2"></a> <a id="feature"></a>listの特徴

* 先頭から順番にしかアクセスできない(<code>[]</code>を使って添え字を指定してのアクセスができない)

* 任意の箇所への要素の追加、削除が O(1) で行える

* 常にリスト中の要素数分のメモリだけを利用(vectorやdequeは、コンテナ中の要素数よりも多目のメモリを確保し、足りなくなったときにメモリを確保しなおす)
