using System;
using System.Globalization;
using System.Web;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Configuration;

namespace ScHelix.Foundation.HelixCore.Extensions {
    public static class SitecoreExtensions {
        public static string GetNotFoundPath(string requestUrl) {
            string notFoundPath = Context.Site.Properties[Constants.NotFoundItemPropertyKey];

            //If there is no site-specific property, default to the global Sitecore property
            if (string.IsNullOrEmpty(notFoundPath)) {
                notFoundPath = Settings.ItemNotFoundUrl;
            }

            notFoundPath += $"?item={HttpUtility.UrlEncode(requestUrl)}&user={HttpUtility.UrlEncode(Context.User.Domain + "\\" + Context.User.LocalName)}&site={HttpUtility.UrlEncode(Context.Site.Name)}";

            return notFoundPath;
        }

        public static void IdentifyUser(this string username) {
            // Never identify an anonymous user
            if (username.ToLower(CultureInfo.InvariantCulture).Contains("anonymous")) { return; }

            const string identificationSource = "website";

            if (Tracker.Current != null && Tracker.Current.IsActive && Tracker.Current.Session != null) {
                Tracker.Current.Session.IdentifyAs(identificationSource, username);
            }
        }
    }
}
