using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.LanguageFallback;
using Sitecore.Pipelines.GetFieldValue;

namespace ScHelix.Foundation.HelixCore.Pipelines.GetFieldValue {
    public class IsValidForLanguageFallback {
        public void Process(GetFieldValueArgs args) {
            if (args.AllowFallbackValue && !IsUiStatic(args.Field) && !IsOldLayout(args.Field)) {
                args.IsValidForLanguageFallback = LanguageFallbackFieldValuesManager.IsValidForFallback(args.Field);
            }
        }

        private static bool IsUiStatic(Field field) => field.ID == FieldIDs.UIStaticItem;

        private static bool IsOldLayout(Field field) => field.ID == new ID("{E1D68787-D22B-4EA2-82B3-84C282E375EB}");
    }
}
