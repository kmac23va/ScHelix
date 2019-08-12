using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Abstractions;
using Sitecore.Data;
using Sitecore.Pipelines.GetSignInUrlInfo;

namespace ScHelix.Foundation.HelixCore.RenderingAssets.Util {
    public static class AuthUtility {
        private const string RoleType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        public static string GetBearerToken(this IIdentity identity) {
            ClaimsIdentity cid = (ClaimsIdentity) identity;

            return cid.Claims.First(r => r.Type == "idtoken").Value;
        }

        public static bool HasRole(this IIdentity identity, string role) => identity is ClaimsIdentity cid && cid.Claims.Any(r => r.Type == RoleType && r.Value.Equals(role, StringComparison.CurrentCultureIgnoreCase));

        public static string GetSignInUrl(string siteName, string returnUrl) {
            string signInUrl = string.Empty;
            BaseCorePipelineManager corePipelineManager = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<BaseCorePipelineManager>();
            GetSignInUrlInfoArgs args = new GetSignInUrlInfoArgs(site: siteName, returnUrl: returnUrl);
            GetSignInUrlInfoPipeline.Run(corePipelineManager, args);
            Collection<SignInUrlInfo> signInCollection = args.Result;

            if (signInCollection.Count <= 0) {
                return signInUrl;
            }

            SignInUrlInfo signInInfo = signInCollection[0];
            signInUrl = signInInfo.Href;

            return signInUrl;
        }
    }
}
