using System;
using System.Reflection;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.Web;
using ScHelix.Foundation.HelixCore.Models;
using ScHelix.Foundation.HelixCore.Repositories;

namespace ScHelix.Foundation.HelixCore.Extensions {
    public static class GlassExtensions {
        private static readonly ILogRepository LogRepository = new LogRepository();

        /// <summary>
        /// Assemblies the version for front end.
        /// </summary>
        /// <param name="glassHtml"></param>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>System.String object.</returns>
        public static string AssemblyVersion(this IGlassHtml glassHtml, string assemblyName) {
            try {
                AssemblyName assembly = AssemblyName.GetAssemblyName(AppDomain.CurrentDomain.BaseDirectory + "bin\\" + assemblyName + ".dll");

                return assembly == null ? "1.0.0.1" : assembly.Version.ToString();
            } catch (Exception e) {
                LogRepository.Error($"HelperExtensions/AssemblyVersionForFrontEnd: {e.Message}", e);
            }

            return "1.0.0.1";
        }

        public static string BrowserTitle(this IGlassHtml glassHtml) {
            IRequestContext requestContext = new RequestContext(glassHtml.SitecoreService);
            IMetaRoot rootItem = requestContext.GetRootItem<IMetaRoot>();

            return rootItem?.BrowserTitle;
        }

        public static string FormatBgImg(this Image imgField, bool isInEditingMode) {
            string bgImage = string.Empty;

            if (imgField != null && !isInEditingMode && !string.IsNullOrEmpty(imgField.Src)) {
                bgImage = imgField.Src.Replace(" ", "%20");
            }

            return bgImage;
        }
    }
}
