using System.Web.Mvc;
using Sitecore.Pipelines;

namespace ScHelix.Foundation.HelixCore.Pipelines.Initialize {
    public class AppInitialization {
        public void Process(PipelineArgs args) {
            AreaRegistration.RegisterAllAreas();
            Sitecore.Globalization.Translate.ResetCache(true);

            //https://doc.sitecore.net/sitecore_experience_platform/setting_up_and_maintaining/security_hardening/configuring/remove_header_information_from_responses_sent_by_your_website
            MvcHandler.DisableMvcResponseHeader = true;
        }
    }
}
