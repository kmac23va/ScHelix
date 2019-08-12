using System.Collections.Generic;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace ScHelix.Foundation.HelixCore.Search {
    public class BaseSearchResultItem : SearchResultItem {
        [IndexField("alltemplates")]
        public IList<ID> Templates { get; set; }
    }
}
