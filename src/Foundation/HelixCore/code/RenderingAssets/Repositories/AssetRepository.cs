using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore;
using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using ScHelix.Foundation.HelixCore.RenderingAssets.Models;

namespace ScHelix.Foundation.HelixCore.RenderingAssets.Repositories {
    /// <summary>A Repository for all assets required by renderings</summary>
    public class AssetRepository {
        private static readonly AssetRequirementCache Cache = new AssetRequirementCache(StringUtil.ParseSizeString("10MB"));
        [ThreadStatic] private static AssetRepository _current;
        private readonly List<Asset> _items = new List<Asset>();
        private readonly List<ID> _seenRenderings = new List<ID>();
        public static AssetRepository Current => _current ?? (_current = new AssetRepository());
        internal IEnumerable<Asset> Items => _items;

        internal void Clear() => _items.Clear();

        public Asset Add(Asset asset, bool preventAddToCache = false) {
            if (asset == null) {
                throw new ArgumentNullException(nameof(asset));
            }

            if (asset.AddOnceToken != null) {
                if (_items.Any(x => x.AddOnceToken != null && x.AddOnceToken == asset.AddOnceToken)) {
                    return null;
                }
            }

            if (asset.Content != null) {
                if (_items.Any(x => x.Content != null && x.Content == asset.Content)) {
                    return null;
                }
            }

            if (!preventAddToCache) {
                if (RenderingContext.CurrentOrNull != null) {
                    Rendering rendering = RenderingContext.CurrentOrNull.Rendering;

                    if (rendering != null && rendering.Caching.Cacheable) {
                        AssetRequirementList cachedRequirements;
                        ID renderingId = rendering.RenderingItem.ID;

                        if (!_seenRenderings.Contains(renderingId)) {
                            _seenRenderings.Add(renderingId);
                            cachedRequirements = new AssetRequirementList();
                        } else {
                            cachedRequirements = Cache.Get(renderingId) ?? new AssetRequirementList();
                        }

                        cachedRequirements.Add(asset);
                        Cache.Set(renderingId, cachedRequirements);
                    }
                }
            }

            // Passed the checks, add the requirement.
            _items.Add(asset);
            return asset;
        }

        public void Add(ID renderingID) {
            // Check if rendering has already been executed in this page request and if so, no need to add it again.
            if (_seenRenderings.Contains(renderingID)) {
                return;
            }

            AssetRequirementList list = Cache.Get(renderingID);

            if (list == null) {
                return;
            }

            foreach (Asset requirement in list) {
                Add(requirement, true);
            }
        }

        public Asset AddScriptFile(string file, bool preventAddToCache = false) => AddAsset(file, preventAddToCache, AssetType.JavaScript);
        public Asset AddStylingFile(string file, bool preventAddToCache = false) => AddAsset(file, preventAddToCache, AssetType.Css);

        private Asset AddAsset(string content, bool preventAddToCache, AssetType assetType, string site = null) {
            Asset asset = CreateAsset(content, assetType, site);
            return asset == null ? null : Add(asset, preventAddToCache);
        }

        private Asset CreateAsset(string content, AssetType assetType, string site) {
            string cleanContent = CleanContent(content);

            if (cleanContent == null) {
                return null;
            }

            Asset asset = new Asset(assetType, cleanContent, site);

            return asset;
        }

        private string CleanContent(string content) {
            content = content?.Trim();

            return string.IsNullOrWhiteSpace(content) ? null : content;
        }
    }
}
