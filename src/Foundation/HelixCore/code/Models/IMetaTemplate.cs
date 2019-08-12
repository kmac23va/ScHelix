using System.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace ScHelix.Foundation.HelixCore.Models {
    public interface IMetaTemplate {
        ID Id { get; set; }
        string Name { get; set; }
        Language Language { get; set; }
        int Version { get; set; }
        IEnumerable BaseTemplateIds { get; set; }
        string TemplateName { get; set; }
        ID TemplateId { get; set; }
        Item InnerItem { get; set; }
    }
}
