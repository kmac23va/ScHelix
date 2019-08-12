using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace ScHelix.Foundation.HelixCore.Extensions {
    public static class RazorExtensions {
        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper) {
            List<HelperResult> templates = (from object key in htmlHelper.ViewContext.HttpContext.Items.Keys
                                            where key.ToString().StartsWith("_script_")
                                            select htmlHelper.ViewContext.HttpContext.Items[key]).OfType<Func<object, HelperResult>>()
                .Select(template => template(null)).ToList();

            foreach (HelperResult template in templates) {
                htmlHelper.ViewContext.Writer.Write(template);
            }

            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString Script(this HtmlHelper htmlHelper, Func<object, HelperResult> template) {
            htmlHelper.ViewContext.HttpContext.Items["_script_" + Guid.NewGuid()] = template;
            return MvcHtmlString.Empty;
        }
    }
}
