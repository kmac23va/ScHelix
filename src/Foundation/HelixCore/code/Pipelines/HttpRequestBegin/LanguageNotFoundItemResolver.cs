using System.Linq;
using System.Web;
using ScHelix.Foundation.HelixCore.Extensions;
using Sitecore;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Pipelines.HttpRequest;

namespace ScHelix.Foundation.HelixCore.Pipelines.HttpRequestBegin {
    public class LanguageNotFoundItemResolver : HttpRequestProcessor {
        public override void Process(HttpRequestArgs args) {
            if (Context.Database == null || Constants.SitesToIgnore.Contains(Context.Site.Name)) {
                return;
            }

            //Check if the context language (supplied by the URL/user) is in the list of available languages
            Language language = LanguageManager.GetLanguages(Context.Database).FirstOrDefault(x => x.Name == Context.Language.Name);

            if (language != null) {
                return;
            }

            //If the language is not available, redirect to the site default language 404 page for a graceful failure
            Context.Language = Language.Parse(Context.Site.Language);
            string notFoundPath = SitecoreExtensions.GetNotFoundPath(args.RequestUrl.LocalPath);
            HttpContext.Current.Response.Redirect(notFoundPath);
            HttpContext.Current.Response.Flush();
            args.AbortPipeline();
        }

    }
}
