using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Rules;

namespace ScHelix.Foundation.HelixCore.Rules.VersionTrim {
    /// <summary>
    /// Rules engine action that recycles old versions of items.
    /// </summary>
    public class RecycleOldVersions<T> : MinVersionsAction<T> where T : RuleContext {
        /// <summary>
        /// Recycle the old version.
        /// </summary>
        /// <param name="version">The old version to recycle.</param>
        public override void HandleVersion(Item version) {
            Assert.ArgumentNotNull(version, "version");
            Log.Audit(this, "Recycle version : {0}", AuditFormatter.FormatItem(version));
            version.RecycleVersion();
        }
    }
}
