using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mono.Options;

namespace Core
{
    public class Program
    {
        public static async Task<int> Main(params string[] args)
        {
            bool showHelp = false;
            string action = "";
            string gameId = "";
            string updateFrom = "";
            string updateTo = "";
            string outputDir = "";
            // options
            string matchingField = "game";
            string branch = "";
            string launcherId = "";
            string platApp = "";
            int threads = Environment.ProcessorCount;
            int maxHttpHandle = 128;

            Console.WriteLine($"Sophon.Downloader v{Assembly.GetExecutingAssembly().GetName().Version} - Made with love by @Escartem <3");

            var options = new OptionSet {
                { "matchingField=", "", v => matchingField = v },
                { "branch=", "", v => branch = v },
                { "launcherId=", "", v => launcherId = v },
                { "platApp=", "", v => platApp = v },
                { "threads=", "", v => threads = int.Parse(v) },
                { "handles=", "", v => maxHttpHandle = int.Parse(v) },
                { "h|help", "show help", v => showHelp = v != null },
            };

            List<string> extra;
            try
            {
                extra = options.Parse(args);
                action = "";

                if (extra.Count > 1)
                {
                    action = extra[0].ToLower();
                }

                if (action == "full" && extra.Count >= 4)
                {
                    gameId = extra[1];
                    updateFrom = extra[2];
                    outputDir = extra[3];
                }
                else if (action == "update" && extra.Count >= 5)
                {
                    gameId = extra[1];
                    updateFrom = extra[2];
                    updateTo = extra[3];
                    outputDir = extra[4];
                }
                else
                {
                    showHelp = true;
                }
            }
            catch (OptionException e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.WriteLine("Use --help for usage.");
                return 0;
            }

            if (showHelp)
            {
                string exeName = Process.GetCurrentProcess().ProcessName + ".exe";
                Console.WriteLine($"""
                    Usage:
                        {exeName} full <gameId> <version> <outputDir> [options]                     Download full game assets
                        {exeName} update <gameId> <updateFrom> <updateTo> <outputDir> [options]     Download update assets

                    Arguments:
                        <gameId>        Game ID, e.g. gopR6Cufr3 for Genshin
                        <version>       Version to download, e.g. 5.6.0
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
                 """);
                return 0;
            }

            // main
            SophonUrl sophonUrl = new SophonUrl(gameId, branch, launcherId, platApp);
            await sophonUrl.GetBuildData();

            Console.WriteLine($"Running with {threads} threads and {maxHttpHandle} handles");

            string prevManifest = sophonUrl.GetBuildUrl(updateFrom);
            string newManifest = "";
            if (action == "update")
            {
                newManifest = sophonUrl.GetBuildUrl(updateTo);
            }

            await Downloader.StartDownload(prevManifest, newManifest, threads, maxHttpHandle, outputDir, matchingField);

            return 0;
        }
    }
}
