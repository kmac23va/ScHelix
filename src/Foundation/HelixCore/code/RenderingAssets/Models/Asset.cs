using System;

namespace ScHelix.Foundation.HelixCore.RenderingAssets.Models {
    public class Asset {
        public Asset(AssetType type, string content, string site = null) {
            Type = type;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Site = site;
        }

        public string Site { get; }
        public string Content { get; }
        public string AddOnceToken { get; set; }
        public AssetType Type { get; set; }

        public long GetDataLength() {
            long total = 0L;

            if (Content != null) {
                total += Content.Length;
            }

            if (AddOnceToken != null) {
                total += AddOnceToken.Length;
            }

            return total;
        }
    }
}
