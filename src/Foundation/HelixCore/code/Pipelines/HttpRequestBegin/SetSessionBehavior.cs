using System.Web;
using System.Web.SessionState;
using Sitecore.Pipelines.HttpRequest;

namespace ScHelix.Foundation.HelixCore.Pipelines.HttpRequestBegin {
    public class SetSessionBehavior : HttpRequestProcessor {
        private static readonly string WebApiExecutionPath = $"~/{Constants.WebApiPrefix}";

        public override void Process(HttpRequestArgs args) {
            if (IsWebApiRequest()) {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        private static bool IsWebApiRequest() => HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath != null && HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiExecutionPath);
    }
}
