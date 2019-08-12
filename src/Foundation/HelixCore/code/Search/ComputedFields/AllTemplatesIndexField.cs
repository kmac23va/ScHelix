using System.Collections.Generic;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data.Items;

namespace ScHelix.Foundation.HelixCore.Search.ComputedFields {
    public class AllTemplatesIndexField : IComputedIndexField  {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable) {
            if (!(indexable is SitecoreIndexableItem indexItem)) {
                return null;
            }

            Item item = indexItem.Item;
            List<string> templates = new List<string>();
            GetAllTemplates(item.Template, templates);

            return templates.Distinct().ToList();
        }

        private static void GetAllTemplates(TemplateItem baseTemplate, ICollection<string> templates) {
            if (baseTemplate.ID == Sitecore.TemplateIDs.StandardTemplate) {
                return;
            }

            string id = IdHelper.NormalizeGuid(baseTemplate.ID);
            templates.Add(id);

            foreach (TemplateItem item in baseTemplate.BaseTemplates) {
                GetAllTemplates(item, templates);
            }
        }
    }
}
