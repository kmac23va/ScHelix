using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Maps;
using ScHelix.Foundation.HelixCore.Models;
using Sitecore.Data;

namespace ScHelix.Foundation.HelixCore.Mappings {
    public class MetaTemplateMap : SitecoreGlassMap<IMetaTemplate> {
        public static readonly ID TemplateId = new ID("{E47F6069-8300-44C8-B61D-43A84FE5CB94}");

        public override void Configure() =>
            Map(config => {
                config.AutoMap();
                config.TemplateId(TemplateId);
                config.EnforceTemplateAndBase();

                config.Id(f => f.Id);
                config.Info(f => f.Name).InfoType(SitecoreInfoType.Name);
                config.Info(f => f.Language).InfoType(SitecoreInfoType.Language);
                config.Info(f => f.Version).InfoType(SitecoreInfoType.Version);
                config.Info(f => f.BaseTemplateIds).InfoType(SitecoreInfoType.BaseTemplateIds);
                config.Info(f => f.TemplateName).InfoType(SitecoreInfoType.TemplateName);
                config.Info(f => f.TemplateId).InfoType(SitecoreInfoType.TemplateId);
                config.Item(f => f.InnerItem);
            });
    }
}
