namespace ScHelix.Foundation.HelixCore.Search {
    public static class SearchUtilities {
        public static string GetSearchIndexName(string database = null, string name = "sitecore") {
            if (database == null) {
                database = Sitecore.Context.Database.Name.ToLowerInvariant();
            }

            return $"{name}_{database}_index";
        }
    }
}
