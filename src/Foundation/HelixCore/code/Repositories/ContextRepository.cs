using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web;
using ScHelix.Foundation.HelixCore.DI;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.Data.Items;

namespace ScHelix.Foundation.HelixCore.Repositories {
    /// <summary>
    ///     Retrieve CMS information, wrapper for Sitecore API calls
    /// </summary>
    [Service(typeof(IContextRepository))]
    public class ContextRepository : IContextRepository {
        private readonly IRequestContext _requestContext;

        public ContextRepository(IRequestContext requestContext) => _requestContext = requestContext;

        public bool IsExperienceEditor => Context.PageMode.IsExperienceEditor;

        public string GetContextSiteRoot() => Context.Site.RootPath;

        public string GetContextStartItem() => Context.Site.StartItem;

        public string GetDatabaseContext() => Context.Database.Name;

        public T GetCurrentItem<T>() where T : class => _requestContext.GetContextItem<T>();

        public object GetCurrentItem(GetKnownOptions options) => _requestContext.GetContextItem(options);

        public T GetCurrentItem<T>(GetKnownOptions options) where T : class => _requestContext.GetContextItem<T>(options);

        public T GetHomeItem<T>() where T : class => _requestContext.GetHomeItem<T>();

        public T GetHomeItem<T>(GetKnownOptions options) where T : class => _requestContext.GetHomeItem<T>(options);

        public T GetRootItem<T>() where T : class => _requestContext.GetRootItem<T>();

        public T GetRootItem<T>(GetKnownOptions options) where T : class => _requestContext.GetRootItem<T>(options);

        public ISearchIndex GetSearchIndexContext(Item contextItem) => ContentSearchManager.GetIndex(new SitecoreIndexableItem(contextItem));
    }
}
