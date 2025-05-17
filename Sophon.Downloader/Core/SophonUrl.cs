using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core
{
    #region getGamesBranches structure
    public class BranchesRoot
    {
        public int retcode { get; set; }
        public string message { get; set; }
        public BranchesData data { get; set; }
    }

    public class BranchesData
    {
        public List<BranchesGameBranch> game_branches { get; set; }
    }

    public class BranchesGameBranch
    {
        public BranchesGame game { get; set; }
        public BranchesMain main { get; set; }
    }

    public class BranchesGame
    {
        public string id { get; set; }
        public string biz { get; set; }
    }

    public class BranchesMain
    {
        public string package_id { get; set; }
        public string branch { get; set; }
        public string password { get; set; }
        public string tag { get; set; }
        public List<string> diff_tags { get; set; }
        public List<BranchesCategory> categories { get; set; }
    }

    public class BranchesCategory
    {
        public string category_id { get; set; }
        public string matching_field { get; set; }
    }
    #endregion

    public class SophonUrl
    {
        private static string apiBase = "https://hyp-api.mihoyo.com/hyp/hyp-connect/api/getGameBranches";
        private static string sophonBase = "https://api-takumi.mihoyo.com/downloader/sophon_chunk/api/getBuild";
        private static string defaultBranch = "main";
        private static string defaultLauncherId = "jGHBHlcOq1";
        private static string defaultPlatApp = "ddxf5qt290cg";

        private string gameId { get; set; }
        private string branch {  get; set; } = defaultBranch;
        private string launcherId { get; set; } = defaultLauncherId;
        private string platApp { get; set; } = defaultPlatApp;
        private string gameBiz { get; set; } = "";
        private string packageId { get; set; } = "";
        private string password { get; set; } = "";

        public SophonUrl(string gameId, string branch = "", string launcherId = "", string platApp = "")
        {
            this.gameId = gameId;
            if (!string.IsNullOrEmpty(branch))
            {
                this.branch = branch;
            }
            if (!string.IsNullOrEmpty(launcherId))
            {
                this.launcherId = launcherId;
            }
            if (!string.IsNullOrEmpty(platApp))
            {
                this.platApp = platApp;
            }
        }

        public async Task<int> GetBuildData()
        {
            var uri = new UriBuilder(apiBase);
            var query = HttpUtility.ParseQueryString(uri.Query);

            query["game_ids[]"] = this.gameId;
            query["launcher_id"] = this.launcherId;
            uri.Query = query.ToString();

            var json = await FetchUrl(uri.ToString());
            var obj = JsonSerializer.Deserialize<BranchesRoot>(json);

            if ((!obj.retcode.Equals(0) && obj.message == "OK"))
            {
                Console.WriteLine($"Error: {obj.message}");
                return -1;
            }

            var branchObj = GetBranch(obj, this.branch);
            if (branchObj == null)
            {
                Console.WriteLine($"Error: Branch {this.branch} not found");
                return -1;
            }
            gameBiz = branchObj.game.biz;

            packageId = branchObj.main.package_id;
            password = branchObj.main.password;

            return 0;
        }

        public string GetBuildUrl(string version)
        {
            var uri = new UriBuilder(sophonBase);
            var query = HttpUtility.ParseQueryString(uri.Query);

            query["branch"] = this.branch;
            query["package_id"] = this.packageId;
            query["password"] = this.password;
            query["plat_app"] = this.platApp;
            query["tag"] = version;

            uri.Query = query.ToString();
            return uri.ToString();
        }

        private static async Task<string> FetchUrl(string url)
        {
            using var client = new HttpClient();
            return await client.GetStringAsync(url);
        }

        private static BranchesGameBranch? GetBranch(BranchesRoot obj, string searchBranch)
        {
            foreach (var branch in obj.data.game_branches)
            {
                if (branch.main.branch == searchBranch)
                {
                    return branch;
                }
            }

            return null;
        }
    }
}
