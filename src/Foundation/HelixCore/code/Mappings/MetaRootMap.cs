using Glass.Mapper.Sc.Maps;
using ScHelix.Foundation.HelixCore.Models;
using Sitecore.Data;

namespace ScHelix.Foundation.HelixCore.Mappings {
    public class MetaRootMap : SitecoreGlassMap<IMetaRoot> {
        public static readonly ID TemplateId = new ID("{52ACABAC-5CCF-4698-B4AC-5E313B57A446}");

        public override void Configure() =>
            Map(config => {
                ImportMap<IMetaTemplate>();
                config.AutoMap();
                config.TemplateId(TemplateId);
                config.EnforceTemplateAndBase();

                config.Field(f => f.BrowserTitle).FieldId("{A57AB51E-29F7-4F4A-BA8F-795E83448210}");
                config.Field(f => f.HomeLink).FieldId("{FE9C9B8C-33E5-4654-A1CD-A22A55192823}");
            });
    }
}
