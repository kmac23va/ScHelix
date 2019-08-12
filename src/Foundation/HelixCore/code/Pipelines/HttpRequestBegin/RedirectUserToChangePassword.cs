using System;
using System.Linq;
using System.Web.Security;
using Sitecore;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Web;

namespace ScHelix.Foundation.HelixCore.Pipelines.HttpRequestBegin {
    public class RedirectUserToChangePassword : HttpRequestProcessor {
        public override void Process(HttpRequestArgs args) {
            string changePwdItem = Context.Site.Properties[Constants.ChangePwdItemPropertyKey];
            int.TryParse(Context.Site.Properties[Constants.ChangePwdDaysPropertyKey], out int changePwdDays);

            if (changePwdDays == 0 || string.IsNullOrEmpty(changePwdItem) || Context.Database == null || Constants.SitesToIgnore.Contains(Context.Site.Name) || (Context.PageMode.IsNormal || Context.PageMode.IsPreview) && Context.Site != null || Context.User == null || Context.User.IsAdministrator || args.Url == null || string.Equals(args.Url.FilePath, changePwdItem, StringComparison.CurrentCultureIgnoreCase)) {
                return;
            }

            MembershipUser membershipUser = Membership.GetUser(Context.User.Name, false);

            if (!HasPasswordExpired(membershipUser, changePwdDays)) {
                return;
            }

            WebUtil.Redirect(changePwdItem);
        }

        private static bool HasPasswordExpired(MembershipUser user, int changePwdDays) => user.LastPasswordChangedDate.Add(new TimeSpan(changePwdDays, 0, 0, 0)) <= DateTime.Now;
    }
}
