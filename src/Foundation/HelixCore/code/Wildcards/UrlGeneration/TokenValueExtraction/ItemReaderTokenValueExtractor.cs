using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace ScHelix.Foundation.HelixCore.Wildcards.UrlGeneration.TokenValueExtraction {
    public class ItemReaderTokenValueExtractor : TokenValueExtractor {
        public override string ExtractTokenValue(string tokenPattern, Item item) {
            if (string.IsNullOrEmpty(tokenPattern)) {
                return string.Empty;
            }

            string[] tokens = HttpUtility.UrlDecode(tokenPattern)
                .Split(new[] {
                    "."
                }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToArray();

            return ExtractValue(tokens, item);

        }

        protected virtual string ExtractValue(ICollection<string> tokens, object obj) {
            if (obj == null) {
                return "null";
            }

            if (!tokens.Any()) {
                if (obj is Item item) {
                    return item.Name;
                }

                return obj.ToString();
            }

            string tokenPattern = tokens.FirstOrDefault();

            if (string.IsNullOrEmpty(tokenPattern)) {
                return string.Empty;
            }

            object value = string.Empty;

            // this is a property
            if (tokenPattern.StartsWith("@@")) {
                tokenPattern = tokenPattern.Replace("@@", string.Empty);
                value = GetProperty(tokenPattern, obj);
            }
            // this is a field
            else if (tokenPattern.StartsWith("@")) {
                tokenPattern = tokenPattern.Replace("@", string.Empty);
                value = GetItemField(tokenPattern, obj);
            }

            string[] remainingTokens = tokens.Skip(1).ToArray();

            if (value is Item) {
                return ExtractValue(remainingTokens, value as Item);
            }

            return !(value is string) ? ExtractValue(remainingTokens, value) : value.ToString();
        }

        protected virtual object GetItemField(string fieldName, object item) {
            if (!(item is Item)) {
                throw new WildcardException("Given object is not an item.");
            }

            Item castedItem = (Item) item;
            object fieldValue = castedItem[fieldName];
            Item fieldValiueItem = castedItem.Database.GetItem(fieldValue.ToString());

            return fieldValiueItem ?? fieldValue;
        }

        protected virtual object GetProperty(string propertyName, object obj) => obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);

        public override string HelpText =>
            "Prefix your fields with '@'. For example: @__Display Name <br/>" +
            "Prefix your properties with '@@. For example: @@Name <br/>" +
            "Use '.' delimiter for creating a chain <br/>" +
            "<h3>Examples</h3>" +
            "<ul>" +
            "<li>@@Name</li>" +
            "<li>@__Display Name</li>" +
            "<li>@@Paths.@@Path</li>" +
            "<li>@@Parent.@@Name</li>" +
            "<li>@Custom Link Field.@@Name</li>" +
            "<li>@@Template.@@InnerItem.@__Standard values.@@Paths.@@Path</li>" +
            "</ul>" +
            "<h3>Notes:</h3>" +
            "Property names ARE case sensitive <br/>" +
            "Field names are NOT case sensitive";
    }
}
