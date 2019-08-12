namespace ScHelix.Foundation.HelixCore.Extensions {
    public static class UserExtensions {
        public static bool IsAuthenticatedOnDomain(this Sitecore.Security.Accounts.User user) => user.IsAuthenticated && user.Domain.Name == Sitecore.Context.Site.Domain.Name;
    }
}
