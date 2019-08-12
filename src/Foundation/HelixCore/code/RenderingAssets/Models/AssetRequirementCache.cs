using Sitecore.Caching;
using Sitecore.Data;

namespace ScHelix.Foundation.HelixCore.RenderingAssets.Models {
    internal class AssetRequirementCache : CustomCache {
        public AssetRequirementCache(long maxSize) : base("Foundation.AssetRequirements", maxSize) {
        }

        public AssetRequirementList Get(ID cacheKey) => (AssetRequirementList) GetObject(cacheKey.ToString());

        public void Set(ID cacheKey, AssetRequirementList requirementList) => SetObject(cacheKey.ToString(), requirementList);
    }
}
