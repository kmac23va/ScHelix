using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ScHelix.Foundation.HelixCore.Methods {
    public static class GetAssemblies {
        public static Assembly[] GetByFilter(params string[] assemblyFilters) {
            HashSet<string> assemblyNames = new HashSet<string>(assemblyFilters.Where(filter => !filter.Contains('*')));
            string[] wildcardNames = assemblyFilters.Where(filter => filter.Contains('*')).ToArray();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => {
                    string nameToMatch = assembly.GetName().Name;

                    return assemblyNames.Contains(nameToMatch) || wildcardNames.Any(wildcard => IsWildcardMatch(nameToMatch, wildcard));
                })
                .ToArray();

            return assemblies;
        }

        /// <summary>
        /// Checks if a string matches a wildcard argument (using regex)
        /// </summary>
        public static bool IsWildcardMatch(string input, string wildcards) => Regex.IsMatch(input, "^" + Regex.Escape(wildcards).Replace("\\*", ".*").Replace("\\?", ".") + "$", RegexOptions.IgnoreCase);
    }
}
