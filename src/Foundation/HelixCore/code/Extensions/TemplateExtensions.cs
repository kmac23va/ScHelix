using System.Linq;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Templates;

namespace ScHelix.Foundation.HelixCore.Extensions {
    public static class TemplateExtensions {
        public static bool IsDerived(this Template template, ID templateId) => template.ID == templateId || template.GetBaseTemplates().Any(baseTemplate => IsDerived(baseTemplate, templateId));
    }
}
