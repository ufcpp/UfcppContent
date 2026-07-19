---
title: "IL（.NET の中間言語）"
source_url: "https://ufcpp.net/study/il/"
content_type: "Subject"
published_at: "2015-05-06T14:15:50"
updated_at: "2022-05-02T22:20:38"
tags: []
umbraco_id: 1440
parent_id: 1115
sort_order: 2
aliases:
  - "/il/"
---

# IL（.NET の中間言語）

C#やVBで書かれたプログラムは、一度、.NETの中間言語(Intermediate Language、略してIL)にコンパイルされます。ILは、仮想マシンコード(仮想的なCPUを想定して、そのマシン語命令を定義したもの)です。C#などの高級言語と比べるとだいぶ「コンピューター向き」(⇔ 高級言語は人が読むの向き)ですが、.NETのILは、結構高級(物理的なCPUのマシン語命令よりだいぶ高機能)です。高級言語と、(物理的なCPUの)マシン語命令との間をつないで解説するのにはよい題材だと思います。

    
より低級な概念(高級言語 → 中間言語 → マシン語命令)を知ることで、高級言語に関する知識も深まり、より効率がよく、より安全なコードを書けるようになるでしょう。

## 章

### <a id="summary"></a>[概要](summary/index.md)

- [IL の概要](summary/il_about.md)
- [IL 命令の実行例](summary/il_execution.md)
