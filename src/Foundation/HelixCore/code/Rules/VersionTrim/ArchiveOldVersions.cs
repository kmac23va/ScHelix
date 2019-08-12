using Sitecore.Data.Archiving;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Rules;

namespace ScHelix.Foundation.HelixCore.Rules.VersionTrim {
    /// <summary>
    /// Rules engine action that archives old versions of items.
    /// </summary>
    public class ArchiveOldVersions<T> : MinVersionsAction<T> where T : RuleContext {
        /// <summary>
        /// Archives the old version.
        /// </summary>
        /// <param name="version">The old version to archive.</param>
        public override void HandleVersion(Item version) {
            Assert.ArgumentNotNull(version, "version");
            Archive archive = version.Database.Archives["archive"];

            if (archive != null) {
                Log.Audit(new object(), "Archive version: {0}", AuditFormatter.FormatItem(version));
                archive.ArchiveVersion(version);
            } else {
                Log.Audit(this, "Recycle version : {0}", AuditFormatter.FormatItem(version));
                version.RecycleVersion();
            }
        }
    }
}
