using Glass.Mapper.Sc.Fields;

namespace ScHelix.Foundation.HelixCore.Models {
    public interface IMetaPage : IMetaTemplate {
        string MetaPageTitle { get; set; }
        string MetaDescription { get; set; }
        string MetaKeywords { get; set; }
        Image MetaImage { get; set; }
        string PageTitle { get; set; }
        string PageDescription { get; set; }
        string Url { get; }
        string MetaUrl { get; }
    }
}
