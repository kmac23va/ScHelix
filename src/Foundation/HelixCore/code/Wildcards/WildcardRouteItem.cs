using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace ScHelix.Foundation.HelixCore.Wildcards {
    public class WildcardRouteItem {
        public static readonly ID TemplateId = new ID("{B4C339CC-57FD-4FF2-ACBA-120B72C5FE78}");

        public static class FieldNames {
            public const string WildcardItems = "Wildcard Items";
            public const string UrlGenerationRules = "Url Generation Rules";
            public const string ItemResolvingRules = "Item Resolving Rules";
            public const string ItemTemplates = "Item Templates";
            public const string ItemSearchRootNode = "Item Search Root Node";
        }

        internal readonly Item Item;

        public WildcardRouteItem(Item wildcardRouteItem) => Item = wildcardRouteItem;

        private IDictionary<int, string> _urlGenerationRules;

        public IDictionary<int, string> UrlGenerationRules =>
            _urlGenerationRules ??
            (_urlGenerationRules = GetNameValueList(FieldNames.UrlGenerationRules));

        private IDictionary<int, string> _itemResolvingRules;

        public IDictionary<int, string> ItemResolvingRules =>
            _itemResolvingRules ??
            (_itemResolvingRules = GetNameValueList(FieldNames.ItemResolvingRules));

        private ICollection<TemplateID> itemTemplates;

        public ICollection<TemplateID> ItemTemplates =>
            itemTemplates ?? (itemTemplates = GetMultilist(FieldNames.ItemTemplates)
                .Select(x => new TemplateID(x))
                .ToArray());

        private ICollection<Item> _wildcardItems;
        public IEnumerable<Item> WildcardItems =>
            _wildcardItems ?? (_wildcardItems = WildcardItemIds
                .Select(x => Item.Database.GetItem(x))
                .ToArray());
        private ICollection<ID> _wildcardItemIds;
        public ICollection<ID> WildcardItemIds => _wildcardItemIds ?? (_wildcardItemIds = GetMultilist(FieldNames.WildcardItems).ToArray());
        private Item _itemSearchRootNode;
        public Item ItemSearchRootNode => _itemSearchRootNode ?? (_itemSearchRootNode = GetInternalLink(FieldNames.ItemSearchRootNode));

        protected virtual Item GetInternalLink(string fieldName) {
            InternalLinkField field = Item.Fields[fieldName];
            if (field == null) {
                throw new WildcardException($"Route item is invalid. There is no '{fieldName}' field.");
            }

            if (field.TargetID == ID.Null) {
                throw new WildcardException($"'{fieldName}' field should not be empty on wildcard route item.");
            }

            return field.TargetItem;
        }

        protected virtual IEnumerable<ID> GetMultilist(string fieldName) {
            MultilistField field = Item.Fields[fieldName];

            if (field == null) {
                throw new WildcardException($"Route item is invalid. There is no '{fieldName}' field.");
            }

            if (!field.TargetIDs.Any()) {
                throw new WildcardException($"'{fieldName}' field should not be empty on wildcard route item.");
            }

            return field.TargetIDs;
        }

        protected virtual IDictionary<int, string> GetNameValueList(string fieldName) {
            NameValueListField field = Item.Fields[fieldName];

            if (field == null) {
                throw new WildcardException($"Route item is invalid. There is no '{fieldName}' name value list field.");
            }

            Dictionary<int, string> ret = new Dictionary<int, string>();

            foreach (string ruleKey in field.NameValues.AllKeys) {
                if (!int.TryParse(ruleKey, out int wildcardIndex)) {
                    throw new WildcardException($"'{fieldName}' field contains invalid integer value in key.");
                }

                ret.Add(wildcardIndex, field.NameValues[ruleKey]);
            }

            return ret;
        }
    }
}
