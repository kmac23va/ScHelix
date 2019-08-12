using Glass.Mapper.Sc.Fields;

namespace ScHelix.Foundation.HelixCore.Models {
    public interface IMetaRoot : IMetaTemplate {
        string BrowserTitle { get; set; }
        Link HomeLink { get; set; }
    }
}
