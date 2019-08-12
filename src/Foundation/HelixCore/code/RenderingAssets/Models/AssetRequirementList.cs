using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Caching;

namespace ScHelix.Foundation.HelixCore.RenderingAssets.Models {
    internal class AssetRequirementList : ICacheable, IEnumerable<Asset> {
        private readonly List<Asset> _items = new List<Asset>();

        public AssetRequirementList() => Cacheable = true;

        public event DataLengthChangedDelegate DataLengthChanged {
            add { }
            remove { }
        }

        public bool Cacheable { get; set; }
        public bool Immutable => true;
        public long GetDataLength() => _items.Sum(x => x.GetDataLength());
        public IEnumerator<Asset> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(Asset requirement) => _items.Add(requirement);
    }
}
