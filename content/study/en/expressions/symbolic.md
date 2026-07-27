---
title: "Symbolic Computation with Expression Trees in C# 3.0"
source_url: "https://ufcpp.net/study/en/expressions/symbolic/"
content_type: "Article"
published_at: "2007-10-07T00:00:00"
updated_at: "2016-11-20T02:27:59"
tags: []
umbraco_id: 1439
parent_id: 1438
sort_order: 0
aliases:
  - "/study/en/symbolic.html"
---

# Symbolic Computation with Expression Trees in C# 3.0

## <a id="sec-generated-title-1"></a> <a id="description"></a>Description

[source files](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/ufcpp2000/csharp/source/Differential.zip)
.
 
This library calculates symbolically derivative of Expression Tree in C# 3.0.
 
Expression Tree is a powerful functionality in C# 3.0, which can be written in the same syntax as anonymous delegates, but treated as data, and dynamically compiled into the MSIL on runtime.
In the library, Expression Tree is differentiated symbolically and its derivative is executable as a delegate after compilation, for example, as follow:

```csharp
Expression<Func<double, double>> f = x => x * x;
var df = f.Derive();

Console.Write("f  = {0}\n", f);
Console.Write("df = {0}\n", df);

var df_ = df.Compile();

for (int i = -2; i <= 2; ++i)
  Console.Write("df({0}) = {1}\n", i, df_(i));
```


```console
f  = x => (x * x)
df = x => (2 * x)
df(-2) = -4
df(-1) = -2
df(0) = 0
df(1) = 2
df(2) = 4
```



##### <a id="sec-generated-title-2"></a>Differentiation

This library partially refers to "[Symbolic computation with C# 3.0](http://www.elguille.info/NET/futuro/firmas_octavio_symbolic_computation_EN.htm)".
With some optimization, it achieves good results such as follows:

* <code>x * x * x + 2 * x * x + 3 * x + 1</code>→<code>3 * x * x + 4 * x + 3</code>

* <code>Math.Log(Math.Exp(x))</code>→<code>1</code>

* <code>x * 3 / x * 2 / x * 4 * x / 24</code>→<code>0</code>



##### <a id="sec-generated-title-3"></a>Differential Operator

Additionally, differential operator class is implemented.

```csharp
Expression<Func<double, double, double>> f =
  (x, y) => x * x * y + 2 * x * y;
var dx = new DifferentialOperator("x");
var dy = new DifferentialOperator("y");
var laplacian = dx * dx + dy * dy;

Console.Write("f     = {0}\n", f);      
Console.Write("df/dx = {0}\n", dx.Apply(f));
Console.Write("Δf   = {0}\n", laplacian.Apply(f));
```


```console
f     = (x, y) => (((x * x) * y) + ((2 * x) * y))
df/dx = (x, y) => ((2 * (x * y)) + (2 * y))
Δf   = (x, y) => (2 * y)
```



##### <a id="sec-generated-title-4"></a>Differential Operator Initialized by Lambda

The differential operator can be initialized with a characteristic polynomial written in Lambda.

```csharp
Expression<Func<double, double, double>> characteristic =
  (x, y) => x * x + y * y;
var laplacian = new DifferentialOperator(characteristic);
Console.Write("Δf = {0}\n", laplacian.Apply(f));
```


In this example, 
DifferentialOperator(x * x + y * y) means Laplacian operator
<span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂x</td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂x</td></tr></table><span class="normal">+</span><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂y</td></tr></table><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂y</td></tr></table></span>.


##### <a id="sec-generated-title-5"></a>Dynamic Creation of Expression from String

The library has a class which create Expression dynamically from a string.
Instead that I implement an original parser, the class uses standard library classes in System.CodeDom and Microsoft.CSharp namespace.
The usage is as follow:

```csharp
var f = (Expression<Func<double, double>>)CodeDom.GetExpressionFrom(
  "x => x * x"
  );
```


The solution (
[source files](../../../../assets/media/ufcpp2000/en/source/Differential.zip)
) has a demo project of a command-line program which reads a lambda expression as string, dynamically creates an Expression, and then shows the created result and its derivative, like as follow:

```console
x => x * x + 2 * x + 1
function  : x => (((x * x) + (2 * x)) + 1)
derivative: x => ((2 * x) + 2)
x => x * Math.Log(x) - x
function  : x => ((x * Log(x)) - x)
derivative: x => Log(x)
x => Math.Sin(x) * Math.Sin(x) + Math.Cos(x) * Math.Cos(x)
function  : x => ((Sin(x) * Sin(x)) + (Cos(x) * Cos(x)))
derivative: x => 0
x => Math.Log(Math.Cos(x))
function  : x => Log(Cos(x))
derivative: x => (-1 * (Sin(x) / Cos(x)))
```
