---
title: "Set / Get とプロパティ"
source_url: "https://ufcpp.net/study/miscprog/list/accessor/"
content_type: "Article"
published_at: "2007-05-15T00:00:00"
updated_at: "2015-05-06T14:19:20"
tags: []
umbraco_id: 1544
parent_id: 1542
sort_order: 1
aliases:
  - "/study/miscprog/accessor.html"
---

# Set / Get とプロパティ

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

「[プロパティ](../../csharp/oop/oo_property.md)」で説明しているように、
C# にはプロパティという便利な機能が備わっています。
 
ここでは、その C# が出てくる以前、
C++ ではどうやって実装の隠蔽をしていたかについて説明したいと思います。
（ちょっと記憶があいまいだけど、
確か Effective C++ か More Effective C++ 辺りで読んだ話。）

「[C# によるプログラミング入門](../../csharp/index.md)」では、
名前をあらわす name と、
年齢をあらわす age をメンバーとして持つクラス Person を使って説明をしましたので、
ここでも Person クラスの age を例として説明します。


## <a id="sec-generated-title-2"></a> <a id="setter"></a>Set / Get

一番簡単なのは、public にしたいメンバー変数の数だけ、
Set変数名 / Get変数名 という名前のメンバー関数を用意する方法。

age なら、SetAge と GetAge というのを Person クラス内に作る。

```cpp {title="SetAge, GetAge"}
#include<iostream>

class Person
{
private:
  int age;
public:
  void SetAge(int a)
  {
    if(a < 0) return;
    this->age = a;
  }

  int GetAge()
  {
    return this->age;
  }

  Person() { this->SetAge(0); }
  Person(int a) { this->SetAge(a); }
};

int main()
{
  Person p;

  p.SetAge(20);
  std::cout << p.GetAge();

  return 0;
}
```


まあ、これが C++ における実装の隠蔽の基本です。
クラス中 Set/Get だらけになるのがあたりまえ。
 
ちなみに、「[プロパティ](../../csharp/oop/oo_property.md)」でも書いていますが、
こういうのを <strong id="setter" class="keyword">setter</strong> / <strong id="getter" class="keyword">getter</strong> と呼びます。
また、setter / getter をあわせて <strong id="accessor" class="keyword">accessor</strong> と呼んだりします。
 
プログラミングになれた人なら accessor をきっちり書くんですが、
初心者はめんどくさがって accessor を書く癖をなかなか付けてくれなかったりします。


## <a id="sec-generated-title-3"></a> <a id="overload"></a>オーバーロードで Set / Get を省略

で、Set / Get だらけになるのを嫌って、
これを省略する人もいます。
Set / Get だらけだと、
Visual Studio のインテリセンスなどの入力支援（変数名を途中まで書けば残りの部分を補間してくれたり）も働かなくなりますし（Set までタイピングしても、どの Set なのか分からない）。
 
C++ は、引数が異なる同名の関数を定義（オーバーロード）できるので、
void SetAge(int a) と int GetAge() の両方を Age という名前にしてしまっても問題ありません。

```cpp {title="void Age(int a), int Age()"}
#include<iostream>

class Person
{
private:
  int age;
public:
  void Age(int a)
  {
    if(a < 0) return;
    this->age = a;
  }

  int Age()
  {
    return this->age;
  }

  Person() { this->Age(0); }
  Person(int a) { this->Age(a); }
};

int main()
{
  Person p;

  p.Age(20);
  std::cout << p.Age();

  return 0;
}
```


まあ、Set / Get がなくなって、タイピングしやすくはなりました。
でも、それだけです。
C# の「[プロパティ](../../csharp/oop/oo_property.md#property)」のように、
利用側では変数のように扱えたりはしません。


## <a id="sec-generated-title-4"></a> <a id="proxy"></a>プロキシ

実は、C++ でも、かなり無理やりですが、（見た目だけは）プロパティのようなことができたりします。
とりあえず、百聞は一見にしかずということで、以下の例を見てください。

```cpp {title="proxy"}
#include<iostream>

class Person
{
private:
  int age;
public:
  class AgeProxy
  {
    Person& p;
  public:
    AgeProxy(Person& p0) : p(p0) {}

    AgeProxy& operator= (int a)
    {
      if(a >= 0)
        this->p.age = a;
      return *this;
    }

    operator int()
    {
      return this->p.age;
    }
  } Age;

  friend class AgeProxy;

  Person() : Age(*this) { this->Age = 0; }
  Person(int a) : Age(*this) { this->Age = a; }
};

int main()
{
  Person p;

  p.Age = 20;
  std::cout << (int)p.Age;

  return 0;
}
```


Person の中身はちょっと変な感じになっていますが、
利用側、すなわち、main の中では、
まるで普通の変数に対する代入・参照であるかのようなコードになっています。
 
でも、単なる変数への代入とは違って、
ちゃんと、p.Age = 20 のところで、
年齢 age が負にならないようにチェックが行われます。
 
このからくりは、
age の読み書きに、AgeProxy という名前の別のクラスを介することで実現します。
Age は AgeProxy 型の変数です。
AgeProxy の代入演算子（operator =）と int 型へのキャスト（operator int）を通して、
Person クラスの age 変数の読み書きをします。
 
ちなみに、こういう例のように、いったん別のクラスを通して値を読み書きしたりする方法を、
<strong id="proxy" class="keyword">プロキシ</strong>（proxy: 代理）と呼びます。
 
まあ、このパターンは、利用側の見た目は綺麗になりますが、
実装は面倒ですし、実行効率もあまりよいとはいえません。
さらに言うと、プロパティを virtual 化しようとすると、
この例よりもさらに複雑な実装が必要になります。
 
こういう感じの話を振り返った上で、
改めて C# の「[プロパティ](../../csharp/oop/oo_property.md#property)」機能を見ると、
便利な機能だなぁとつくづく思います。
