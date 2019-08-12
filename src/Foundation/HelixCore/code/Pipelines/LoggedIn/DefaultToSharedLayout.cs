using Sitecore.Pipelines.LoggedIn;
using Sitecore.Security.Accounts;
using Sitecore.Web.UI.HtmlControls;
using ScConstants = Sitecore.ExperienceEditor.Constants;

namespace ScHelix.Foundation.HelixCore.Pipelines.LoggedIn {
    public class DefaultToSharedLayout : LoggedInProcessor {
        public override void Process(LoggedInArgs args) {
            if (SkipProcessor(args)) {
                return;
            }

            Registry.SetString(ScConstants.RegistryKeys.EditAllVersions, ScConstants.Registry.CheckboxTickedRegistryValue);
        }

        protected virtual bool SkipProcessor(LoggedInArgs args) {
            User user = User.FromName(args.Username, true);
            string existing = Registry.GetString(ScConstants.RegistryKeys.EditAllVersions);
            return user == null && string.IsNullOrEmpty(existing);
        }
    }
}
