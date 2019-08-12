using System;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace ScHelix.Foundation.HelixCore.RenderingAssets.Util {
    public static class SitecoreUtil {
        static readonly string[] SizeSuffixes = {
            "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"
        };

        public static long GetSitecoreMediaItemSizeInBytes(string itemId) {
            long mediaItemSize = 0;
            MediaItem mediaItem = GetSitecoreMediaItem(itemId);

            //Get media item Size
            mediaItemSize = mediaItem?.Size ?? default(long);

            return mediaItemSize;
        }

        public static MediaItem GetSitecoreMediaItem(string itemId) => ID.IsID(itemId) ? GetSitecoreMediaItem(ID.Parse(itemId)) : null;

        public static MediaItem GetSitecoreMediaItem(Guid itemId) => Guid.Empty != itemId ? GetSitecoreMediaItem(ID.Parse(itemId)) : null;

        public static MediaItem GetSitecoreMediaItem(ID itemId) {
            MediaItem mediaItem = null;

            Item sitecoreItem = Sitecore.Context.Database.GetItem(itemId);
            if (sitecoreItem != null) {
                mediaItem = new MediaItem(sitecoreItem);
            } else {
                Sitecore.Diagnostics.Log.Error("Could not find given sitecore media item with id '" + itemId + "' ", typeof(SitecoreUtil));
            }

            return mediaItem;
        }

        public static string SizeSuffix(long value, int decimalPlaces = 1) {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }

            if (value < 0) { return "-" + SizeSuffix(-value); }

            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int) Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal) value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000) {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }
    }
}
