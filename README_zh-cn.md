# SophonDownloader
使用新的下载方法下载 mihoyo 资产

[English][p:en-us] | [中文][p:zh-cn]

---

由于《原神》在 5.6 版本使用 SophonChunks 下载模式替代原本的 zip 模式，在版本更新之初，曾被认为没有其他方法解决。

但在经历数日的研究后，找到一方法可使用 SophonChunks 进行版本更新，现将该方法分享出来。

---

# 下载

* [最新的自动构建版本可在此处获取](https://nightly.link/Escartem/SophonDownloader/workflows/build/master/Sophon.Downloader.zip) ✨

---

# 用法
```
指令:
    Sophon.Downloader.exe full <gameId> <version> <outputDir> [options]                  下载全部资源（FullPackage）
    Sophon.Downloader.exe update <gameId> <updateFrom> <updateTo> <outputDir> [options]  下载更新资源（DiffPackage）

语法:
    <gameId>        游戏 ID，hoyo id（hk4e、hkrpg、nap、bh2）或 REL id（gopR6Cufr3，...）
    <version>       想要下载的对应游戏版本
    <updateFrom>    游戏源版本
    <updateTo>      游戏目标版本
    <outputDir>     输出文件夹，如输入的目录不存在则会自动创建对应目录

选项:
    --region=<value>            要使用的区域，OSREL 或 CNREL，默认为 OSREL
    --matchingField=<value>     覆写 Sophon manifest 中的 matchingField 字段值（game，zh-cn，en-us，ja-jp，ko-kr）
    --branch=<value>            覆写游戏数据的 branch name
    --launcherId=<value>        覆写获取包体数据时使用的 launcherId 值
    --platApp=<value>           覆写获取包体数据时使用的 platApp 值
    --threads=<value>           线程数，默认与 CPU 线程数相等
    --handles=<value>           HTTP 句柄数，默认为 128
    -h, --help                  显示指令帮助
```

---

# 笔记

这是在 Genshin 停止提供更新的 zip 文件后匆忙制作的，请报告任何问题，并请注意 ZZZ 也使用 Sophon，但它没有针对该游戏进行测试

---

# 致谢

- [Hi3Helper.Sophon](https://github.com/CollapseLauncher/Hi3Helper.Sophon) - Sophon资产管理

[p:en-us]: README.md
[p:zh-cn]: README_zh-cn.md