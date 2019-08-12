using System;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Rules;
using Sitecore.Rules.Actions;

namespace ScHelix.Foundation.HelixCore.Rules.VersionTrim {
    /// <summary>
    ///     https://community.sitecore.net/technical_blogs/b/sitecorejohn_blog/posts/rules-engine-actions-to-remove-old-versions-in-the-sitecore-asp-net-cms
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MinVersionsAction<T> : RuleAction<T> where T : RuleContext {
        /// <summary>
        /// Backs the MinVersions property.
        /// </summary>
        private int _minVersions;

        /// <summary>
        /// Gets or sets a value that indicates 
        /// the minimum number of versions to retain. 
        /// Set by rule parameters, or defaults to 30. 
        /// </summary>
        public int MinVersions {
            get => _minVersions < 1 ? 30 : _minVersions;
            set => _minVersions = value;
        }

        /// <summary>
        /// Gets or sets a value that indicates the minimum age of versions to 
        /// remove, in days. Actions that derive from this abstract base class
        /// will not process Versions updated within this number of days.
        /// </summary>
        public int MinUpdatedDays { get; set; }

        /// <summary>
        /// Apply the rule.
        /// </summary>
        /// <param name="ruleContext">Rule processing context.</param>
        public override void Apply(T ruleContext) {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");
            Assert.ArgumentNotNull(ruleContext.Item, "ruleContext.Item");

            // for each language available in the item
            foreach (Language lang in ruleContext.Item.Languages) {
                Item item = ruleContext.Item.Database.GetItem(ruleContext.Item.ID, lang);

                if (item == null) {
                    continue;
                }

                // to prevent the while loop from reaching MinVersions,
                // only process this number of items
                int limit = item.Versions.Count - MinVersions;
                int i = 0;

                while (item.Versions.Count > MinVersions && i < limit) {
                    Item version = item.Versions.GetVersions()[i++];
                    Assert.IsNotNull(version, "version");

                    if (MinUpdatedDays < 1 || version.Statistics.Updated.AddDays(MinUpdatedDays) < DateTime.Now) {
                        HandleVersion(version);
                    }
                }
            }
        }

        /// <summary>
        /// Classes that derive from this abstract base class
        /// implement this method to remove versions.
        /// </summary>
        /// <param name="version">The version to process.</param>
        public abstract void HandleVersion(Item version);
    }
}
