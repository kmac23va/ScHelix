using System;
using System.Collections.Generic;
using System.Linq;
using ScHelix.Foundation.HelixCore.Repositories;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Sites;

namespace ScHelix.Foundation.HelixCore.Events {
    public class PublisherCacheClearer {
        private readonly ILogRepository _logRepository;

        public PublisherCacheClearer(ILogRepository logRepository) => _logRepository = logRepository;

        public void ClearCache(object sender, EventArgs args) {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            //Get all sites from site definition where domain is not 'sitecore'
            List<SiteContext> sites = Factory.GetSiteInfoList().Where(s => s.Domain != null && s.Domain != "sitecore").Select(s => Factory.GetSite(s.Name)).ToList();
            _logRepository.Info("PublisherCacheClearer clearing HTML caches for all sites (" + sites.Count + ").");

            if (sites.Any()) {
                foreach (SiteContext site in sites) {
                    Assert.IsNotNull(site, "siteName: " + site.Name);

                    if (!site.CacheHtml) {
                        continue;
                    }

                    HtmlCache htmlCache = CacheManager.GetHtmlCache(site);
                    htmlCache?.Clear();
                }
            }

            //Reset the dictionary cache on publish
            Translate.ResetCache();
            _logRepository.Info("PublisherCacheClearer done.");
        }
    }
}
