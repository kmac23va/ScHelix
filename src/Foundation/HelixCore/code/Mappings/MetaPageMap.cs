using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Maps;
using ScHelix.Foundation.HelixCore.Models;
using Sitecore.Data;

namespace ScHelix.Foundation.HelixCore.Mappings {
    public class MetaPageMap : SitecoreGlassMap<IMetaPage> {
        public static readonly ID TemplateId = new ID("{E47F6069-8300-44C8-B61D-43A84FE5CB94}");

        public override void Configure() =>
            Map(config => {
                ImportMap<IMetaTemplate>();
                config.AutoMap();
                config.TemplateId(TemplateId);
                config.EnforceTemplateAndBase();

                config.Field(f => f.MetaPageTitle).FieldId("{01C8FB5C-69FC-4D0A-8742-23101FD62276}");
                config.Field(f => f.MetaDescription).FieldId("{A6C897D6-CB08-4205-BD02-8C756DD7A80B}");
                config.Field(f => f.MetaKeywords).FieldId("{1406639A-D6FA-4E06-9B45-1FD22112C266}");
                config.Field(f => f.MetaImage).FieldId("{3BD296DE-BFD7-43B5-8F3B-D8BB65B8C641}").MediaUrlOptions(SitecoreMediaUrlOptions.AlwaysIncludeServerUrl);
                config.Field(f => f.PageTitle).FieldId("{0D6383BD-3612-4D85-AFE1-E57D2E83ECD9}");
                config.Field(f => f.PageDescription).FieldId("{147FFB8A-2F2D-4D27-A197-A89F300374AA}");
                config.Info(f => f.Url).InfoType(SitecoreInfoType.Url);
                config.Info(f => f.MetaUrl).InfoType(SitecoreInfoType.Url).UrlOptions(SitecoreInfoUrlOptions.AlwaysIncludeServerUrl);
            });
    }
}
