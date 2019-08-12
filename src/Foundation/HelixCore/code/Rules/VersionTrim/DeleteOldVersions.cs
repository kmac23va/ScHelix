using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Rules;

namespace ScHelix.Foundation.HelixCore.Rules.VersionTrim {
    /// <summary>
    /// Rules engine action that deletes old versions of items.
    /// </summary>
    public class DeleteOldVersions<T> : MinVersionsAction<T> where T : RuleContext {
        /// <summary>
        /// Deletes the old version.
        /// </summary>
        /// <param name="version">The old version to delete.</param>
        public override void HandleVersion(Item version) {
            Assert.ArgumentNotNull(version, "version");
            Log.Audit(this, "Delete version : {0}", AuditFormatter.FormatItem(version));
            version.Versions.RemoveVersion();
        }
    }
}
