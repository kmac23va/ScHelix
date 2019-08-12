using System.Web;
using Sitecore.Configuration;
using Sitecore.Mvc.Helpers;

namespace ScHelix.Foundation.HelixCore.Extensions {
    /// <summary>
    /// Sets up chrome around the renderings and placeholders
    /// Usage: https://github.com/jammykam/Rendering-Chrome-for-Sitecore-Components
    /// Needs the item in the TDS.Core project under /sitecore/content/Applications/WebEdit/Ribbons/WebEdit/View/Show/Highlight Renderings
    /// </summary>
    public static class RenderingChrome {
        private const string DisplayExtendedInfo = "RenderingChrome.DisplayExtendedInfo";

        public static IHtmlString ContainerChrome(this SitecoreHelper helper) {
            return helper.ContainerChrome(null);
        }

        public static IHtmlString ContainerChrome(this SitecoreHelper helper, string title) {
            return helper.GetChromeAttribute(title, "container");
        }

        public static IHtmlString WidgetChrome(this SitecoreHelper helper) {
            return helper.WidgetChrome(null);
        }

        public static IHtmlString WidgetChrome(this SitecoreHelper helper, string title) {
            return helper.GetChromeAttribute(title, "widget");
        }

        private static IHtmlString GetChromeAttribute(this SitecoreHelper helper, string title, string attribute) {
            if (!Sitecore.Context.PageMode.IsExperienceEditor) {
                return null;
            }

            if (string.IsNullOrWhiteSpace(title)) {
                title = helper.CurrentRendering.RenderingItem.DisplayName;
            }

            if (Settings.GetBoolSetting(DisplayExtendedInfo, false)) {
                title += $" ({helper.CurrentRendering.Renderer})";
            }

            string htmlString = $"data-{attribute}-title=\"{title}\"" + WrapperTitle(title);

            return new HtmlString(htmlString);
        }

        private static string WrapperTitle(string title) {
            return Settings.GetBoolSetting(DisplayExtendedInfo, false) ? $" title=\"{title}\"" : null;
        }
    }
}
