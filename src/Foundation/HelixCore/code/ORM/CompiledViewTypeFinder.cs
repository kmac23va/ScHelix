using System;
using System.Web.Compilation;
using Glass.Mapper.Sc.Pipelines.Response;

namespace ScHelix.Foundation.HelixCore.ORM {
    public class CompiledViewTypeFinder : IViewTypeResolver {
        public Type GetType(string path) {
            try {
                Type compiledViewType = BuildManager.GetCompiledType(path);
                Type baseType = compiledViewType.BaseType;

                if (baseType == null || !baseType.IsGenericType) {
                    Sitecore.Diagnostics.Log.Warn($"View {path} compiled type {compiledViewType} base type {baseType} does not have a single generic argument.", this);

                    return typeof(NullModel);
                }

                Type proposedType = baseType.GetGenericArguments()[0];

                return proposedType == typeof(object) ? typeof(NullModel) : proposedType;
            } catch (Exception ex) {
                Sitecore.Diagnostics.Log.Error("CompiledViewTypeFinder Path = " + path, ex, this);
                return null;
            }
        }
    }
}
