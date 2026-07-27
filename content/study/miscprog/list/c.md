---
title: "C言語小ネタ集"
source_url: "https://ufcpp.net/study/miscprog/list/c/"
content_type: "Article"
published_at: "2015-05-06T14:19:18"
updated_at: "2015-05-06T14:19:18"
tags: []
umbraco_id: 1543
parent_id: 1542
sort_order: 0
aliases:
  - "/study/miscprog/c.html"
---

# C言語小ネタ集

## <a id="sec-generated-title-1"></a> <a id="variadic"></a>可変長引数 条件付きデバッグ関数

リリース版には残したくないデバッグ用のコードは、
以下のように <code>#if, #ifdef</code> プリプロセッサ命令を使って条件コンパイルするのが一般的です。

```csharp
#ifdef _DEBUG_
#define debug_puts(str) fputs(str, fp);
#else
#define debug_puts()
#endif
```


また、最近のコンパイラは不要なコードは綺麗さっぱり消してくれるので、
以下のようにフラグと <code>inline</code> 関数を使って条件コンパイルを行うことも出来ます。

```csharp
#ifdef _DEBUG_
#define DEBUG_PUTS_ON 1
#else
#define DEBUG_PUTS_ON 0
#endif

inline void debug_puts(char* str)
{
  if(DEBUG_PUTS_ON)
    fputs(str, fp);
}
```


しかし、これらの方法では可変長引数を取れない(inline 関数を使う方は出来ないこともないけど、やっぱりめんどくさい)という欠点があります。
そこで、Visual C++ 限定の手法なんですが、
可変長引数を使いたい場合には以下のようにします。

```csharp
#ifdef _DEBUG_
#define debug_printf printf
#else
#define SLASH() /
#define debug_printf SLASH()SLASH()
#endif
```


こうすることで、<code>debug_printf</code> はデバッグ時には <code>printf</code> に、
リリース時には <code>//</code> コメントに置き換えられます。
