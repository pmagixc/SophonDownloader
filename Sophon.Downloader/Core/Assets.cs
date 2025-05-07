using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hi3Helper.Sophon;
using Hi3Helper.Sophon.Structs;

namespace Core
{
    internal class Assets
    {
        public static async Task<Tuple<List<SophonAsset>, long>> GetAssetsFromManifests(HttpClient httpClient, string matchingField, string prevManifestUrl, string newManifestUrl, CancellationTokenSource tokenSource, bool isUpdate = true)
        {
            List<SophonAsset> assets = new List<SophonAsset>();

            SophonChunkManifestInfoPair manifestPairFrom = await SophonManifest.CreateSophonChunkManifestInfoPair(
                httpClient,
                prevManifestUrl,
                matchingField,
                tokenSource.Token
            );

            SophonChunkManifestInfoPair manifestPairTo = await SophonManifest.CreateSophonChunkManifestInfoPair(
                httpClient,
                newManifestUrl,
                matchingField,
                tokenSource.Token
            );

            // process assets
            long updateSize = 0;

            await foreach (SophonAsset asset in SophonUpdate.EnumerateUpdateAsync(
                httpClient,
                manifestPairFrom,
                manifestPairTo,
                true,
                null,
                tokenSource.Token
            ))
            {
                if (!asset.IsDirectory)
                {
                    bool isUpdated = false;
                    SophonChunk[] chunks = asset.Chunks;
                    int chunksLen = chunks.Length;

                    for (int i = 0; i < chunksLen; i++)
                    {
                        if (chunks[i].ChunkOldOffset != -1)
                        {
                            continue;
                        }

                        isUpdated = true;
                    }

                    if (isUpdated || !isUpdate)
                    {
                        updateSize = updateSize + asset.AssetSize;
                        assets.Add(asset);
                    }
                }
            }

            return new Tuple<List<SophonAsset>, long>(assets, updateSize);
        }
    }
}
