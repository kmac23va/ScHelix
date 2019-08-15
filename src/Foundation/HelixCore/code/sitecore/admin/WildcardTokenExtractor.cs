using System;
using System.Web.UI.WebControls;
using ScHelix.Foundation.HelixCore.Wildcards;
using ScHelix.Foundation.HelixCore.Wildcards.UrlGeneration.TokenValueExtraction;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.sitecore.admin;
using Sitecore.Sites;

namespace ScHelix.Foundation.HelixCore.sitecore.admin {
    public class WildcardTokenExtractor : AdminPage {
        protected TextBox tbxItemID;
        protected TextBox tbxPattern;
        protected Button btnSubmit;
        protected TextBox tbxResult;
        protected TextBox tbxCurrentUrl;

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            CheckSecurity();
        }

        protected string SiteName {
            get {
                string siteName = Request.QueryString["sc_site"];

                if (string.IsNullOrEmpty(siteName) && Sitecore.Context.Site == null) {
                    throw new WildcardException("Site context can't be resolved, pass the 'sc_site' query string parameter");
                }

                return string.IsNullOrEmpty(siteName) ? Sitecore.Context.Site.Name : siteName;
            }
        }

        protected virtual void OnSubmit(object sender, EventArgs e) {
            try {
                using (new SiteContextSwitcher(SiteContextFactory.GetSiteContext(SiteName))) {
                    Item item = Database.GetDatabase("master").GetItem(tbxItemID.Text);
                    string itemUrl = LinkManager.GetItemUrl(item);
                    string value = UrlGenerationTokenValueExtractor.Current.ExtractTokenValue(tbxPattern.Text, item);
                    tbxResult.Text = value;
                    tbxCurrentUrl.Text = itemUrl;
                }
            } catch (Exception ex) {
                tbxResult.Text = ex.ToString();
            }
        }

        public virtual string HelpText => UrlGenerationTokenValueExtractor.Current.HelpText;
    }
}
