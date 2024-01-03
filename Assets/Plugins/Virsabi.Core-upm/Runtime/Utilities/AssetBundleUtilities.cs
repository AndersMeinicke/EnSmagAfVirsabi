using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Virsabi.Utility
{
    /// <summary>
    /// Methods for manually managing obb files - if you need to publish to official stores only main|patch prefixes are allowed. Please be aware!
    /// </summary>
    public static class AssetBundleUtilities
    {
        /// <summary>
        /// Generate the full .obb name of a bundle.
        /// </summary>
        /// <param name="BundleName">The name (prefix) of the bundle, fx "scene"</param>
        /// <returns>bundlename.version.com.company.product.obb</returns>
        public static string GenerateObbFileName(string BundleName)
        {
            if (AssertObbFileNameValid(BundleName))
                return BundleName + "." + Application.version.Substring(0, 2) + Application.identifier + ".obb";
            else
                return "INVALID_BUNDLE_NAME";
        }

        /// <summary>
        /// Spaces, underscores, dashes, and numbers are not allowed + only lower case. 
        /// </summary>
        /// <returns></returns>
        private static bool AssertObbFileNameValid(string BundleName)
        {
            if (BundleName.All(char.IsDigit) // if a digit exists
                || !BundleName.Equals(BundleName.ToLower()) //if all characters are not lowercase
                || BundleName.Contains("_")
                || BundleName.Contains("-"))
            {
                Debug.LogError("The bundlename is not valid!");
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Generate the full path to an obb file named BundleName
        /// </summary>
        /// <param name="BundleName">The name (prefix) of the bundle, fx "scene"</param>
        /// <returns> "/sdcard/Android/obb/com.Company.Product/bundlename.version.com.company.product.obb </returns>
        public static string GenerateFullObbFilePath(string BundleName)
        {
            return "/sdcard/Android/obb/" + Application.identifier + "/" + GenerateObbFileName(BundleName);
        }
    }
}
