using System;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using Sitecore.SecurityModel;

namespace ScHelix.Foundation.HelixCore.Pipelines.BranchPresets {
    public static class LayoutHelper {
        /// <summary>
        ///     Helper method that loops over all Shared and Final renderings in all devices attached to an item and invokes a function on each of them. The function may request the deletion of the item
        ///     by returning a specific enum value.
        /// </summary>
        public static void ApplyActionToAllRenderings(Item item, Func<RenderingDefinition, RenderingActionResult> action) {
            ApplyActionToAllSharedRenderings(item, action);
            ApplyActionToAllFinalRenderings(item, action);
        }

        /// <summary>
        ///     Helper method that loops over all Shared renderings in all devices attached to an item and invokes a function on each of them. The function may request the deletion of the item
        ///     by returning a specific enum value.
        /// </summary>
        private static void ApplyActionToAllSharedRenderings(Item item, Func<RenderingDefinition, RenderingActionResult> action) => ApplyActionToAllRenderings(item, FieldIDs.LayoutField, action);

        /// <summary>
        ///     Helper method that loops over all Final renderings in all devices attached to an item and invokes a function on each of them. The function may request the deletion of the item
        ///     by returning a specific enum value.
        /// </summary>
        private static void ApplyActionToAllFinalRenderings(Item item, Func<RenderingDefinition, RenderingActionResult> action) => ApplyActionToAllRenderings(item, FieldIDs.FinalLayoutField, action);

        private static void ApplyActionToAllRenderings(Item item, ID fieldId, Func<RenderingDefinition, RenderingActionResult> action) {
            string currentLayoutXml = LayoutField.GetFieldValue(item.Fields[fieldId]);

            if (string.IsNullOrEmpty(currentLayoutXml)) {
                return;
            }

            string newXml = ApplyActionToLayoutXml(currentLayoutXml, action);

            // save a modified layout value if necessary
            if (newXml == null) {
                return;
            }

            using (new SecurityDisabler()) {
                using (new EditContext(item)) {
                    LayoutField.SetFieldValue(item.Fields[fieldId], newXml);
                }
            }
        }

        private static string ApplyActionToLayoutXml(string xml, Func<RenderingDefinition, RenderingActionResult> action) {
            LayoutDefinition layout = LayoutDefinition.Parse(xml);
            xml = layout.ToXml(); // normalize the output in case of any minor XML differences (spaces, etc)

            // loop over devices in the rendering
            for (int deviceIndex = layout.Devices.Count - 1; deviceIndex >= 0; deviceIndex--) {
                if (!(layout.Devices[deviceIndex] is DeviceDefinition device)) {continue;}

                // loop over renderings within the device
                for (int renderingIndex = device.Renderings.Count - 1; renderingIndex >= 0; renderingIndex--) {
                    if (!(device.Renderings[renderingIndex] is RenderingDefinition rendering)) {continue;}

                    // run the action on the rendering
                    RenderingActionResult result = action(rendering);

                    // remove the rendering if the action method requested it
                    if (result == RenderingActionResult.Delete) {
                        device.Renderings.RemoveAt(renderingIndex);
                    }
                }
            }

            string layoutXml = layout.ToXml();

            // save a modified layout value if necessary
            return layoutXml != xml ? layoutXml : null;
        }
    }
}
