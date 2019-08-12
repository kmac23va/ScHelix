using System.Collections.Generic;
using System.Linq;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Sites;

namespace ScHelix.Foundation.HelixCore.Extensions {
    public static class ItemExtensions {
        public static bool IsDerived([NotNull] this Item item, [NotNull] ID templateId) => TemplateManager.GetTemplate(item).IsDerived(templateId);
        public static IEnumerable<Item> GetChildrenDerivedFrom(this Item item, ID templateId) => item.GetChildren().Where(c => c.IsDerived(templateId));

        public static Item GetItemBySiteProperty(this SiteContext siteContext, string propertyKey, string language = "en") {
            string property = siteContext.Properties[propertyKey];
            string path = property;

            if (string.IsNullOrEmpty(property)) {
                return null;
            }

            if (ID.IsID(path) || path.StartsWith(Sitecore.Constants.ContentPath)) {
                return ItemManager.GetItem(path, Language.Parse(language), Version.Latest, Context.Database);
            }

            path = siteContext.GetItemByShortPath(property);

            return ItemManager.GetItem(path, Language.Parse(language), Version.Latest, Context.Database);
        }

        private static string GetItemByShortPath(this SiteContext siteContext, string shortPath) {
            shortPath = shortPath.StartsWith("/") ? shortPath.Substring(1) : shortPath;
            string fullPath = string.Concat(StringUtil.EnsurePostfix('/', siteContext.StartPath), shortPath);

            return fullPath;
        }
    }
}
