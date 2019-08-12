using Sitecore.Data;

namespace ScHelix.Foundation.HelixCore {
    /// <summary>
    /// https://staticreadonly.com/2019/02/06/structures-vs-static-classes-for-sitecore-template-references/
    /// </summary>
    public static class Constants {
        public static readonly string[] SitesToIgnore = { "shell", "login", "admin", "service", "modules_shell", "modules_website", "website", "scheduler", "system", "publisher", "exm" };
        public const string NotFoundItemPropertyKey = "notFoundItem";
        public const string ChangePwdItemPropertyKey = "changePwdItem";
        public const string ChangePwdDaysPropertyKey = "changePwdDays";
        public const string ForgotPwdItemPropertyKey = "forgotPwdItem";
        public const string WebApiPrefix = "api";
        public const string StandardValues = "__Standard Values";
        public const string NameToken = "$name";

        public static class Fields {
            public static ID SortOrder = new ID("{BA3F86A2-4A1C-4D78-B63D-91C2779C1B5E}");
        }

        public static class MediatorResponse {
            public const string InvalidCodeReturned = "Invalid Mediator Code Returned";
            public const string DataSourceError = "DataSourceError";
            public const string ViewModelError = "ViewModelError";
            public const string Ok = "Ok";
        }
    }
}
