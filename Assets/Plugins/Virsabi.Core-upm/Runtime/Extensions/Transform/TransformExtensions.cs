using UnityEngine;
using MyBox;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Virsabi.Extensions
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Destroy all children
        /// </summary>
        public static Transform Clear(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            return transform;
        }

        /// <summary>
        /// Is this a prefab Asset? Use to avoid onvalidate runs when selecting a prefab in the project view.
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static bool IsPrefab(this Transform This)
        {
            return !This.gameObject.scene.IsValid();
        }
    }
}
