# SophonDownloader
Download mihoyo update assets using their new download method

---

# How to use
```
Usage:
    Sophon.Downloader.exe <gameId> <updateFrom> <updateTo> <outputDir> [options]

Arguments:
    <gameId>        Game ID, e.g. gopR6Cufr3 for Genshin
    <updateFrom>    Version to update from, e.g. 5.5.0
    <updateTo>      Version to update to, e.g. 5.6.0
    <outputDir>     Output directory to save the downloaded files

Options:
    --matchingField=<value>     Override the matching field in sophon manifest
    --branch=<value>            Override branch name of the game data
    --launcherId=<value>        Override launcher ID used when fetching packages
    --platApp=<value>           Override platform application ID used when fetching packages
    --threads=<value>           Number of threads to use, defaults to the number of processors
    --handles=<value>           Number of HTTP handles to use, defaults to 128
    -h, --help                  Show this help message
```

---

# Credits

- [Hi3Helper.Sophon](https://github.com/CollapseLauncher/Hi3Helper.Sophon) - Sophon assets management
