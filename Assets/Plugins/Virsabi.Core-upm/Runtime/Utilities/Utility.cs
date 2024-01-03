using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Virsabi.Utility
{
    public static class Utility
    {
        /// <summary>
        /// Maps a number "s" from the range [a1, a2] to range [b1, b2].
        /// </summary>
        public static float Map(float a1, float a2, float b1, float b2, float s)
        {

            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);

        }

        /// <summary>
        /// Plays a random sound on an audiosource.
        /// </summary>
        /// <param name="audioClips">The list of clips to choose from</param>
        /// <param name="audioSource">the source to play on </param>
        /// <param name="noRepeat">if true, the same clip can never be played twice in a row.</param>
        /// <param name="lastIndex">Save this parameter and pass in to tell the method which index was played last - so that noRepeat works.</param>
        /// <returns></returns>
        public static int PlayRandomSound(List<AudioClip> audioClips, AudioSource audioSource, bool noRepeat, int lastIndex)
        {
            int randomIndex;

            if (audioClips.Count == 0)
            {
                return lastIndex;
            }

            if (!noRepeat || audioClips.Count == 1)
            {
                randomIndex = 0;
            }
            else
            {
                do
                {
                    randomIndex = Random.Range(0, audioClips.Count - 1);
                } while (randomIndex == lastIndex && randomIndex != -1);

                lastIndex = randomIndex;
            }

            if (randomIndex != -1)
            {
                var clip = audioClips[randomIndex] as AudioClip;
                if (clip != null)
                {
                    audioSource.PlayOneShot(clip);
                }
            }

            return lastIndex;
        }

        public static int RandomRangeExcept(int min, int max, int except)
        {
            var number = Random.Range(min, max);
            while (number == except)
                number = Random.Range(min, max);
            return number;
        }


#if UNITY_EDITOR
        public static T LoadFirstAssetOfType<T>(string type)
        {
            //Debug.Log("Loading asset of type " + type);

            string[] guids = AssetDatabase.FindAssets("t:" + type);

            if (guids.Length == 0)
                Debug.LogError("Couldn't find any assets of type " + type);

            if (guids.Length > 1)
                Debug.LogError("Too many " + type + " assets!!!");

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);

            return (T)Convert.ChangeType(AssetDatabase.LoadAssetAtPath(path, typeof(T)), typeof(T));
        }
#endif


        public static string HumanizeTime(float timeInSeconds, TimeFormat format)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);
            string timeText = "";

            switch (format)
            {
                case TimeFormat.hhMMSS:
                    timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                    break;
                case TimeFormat.MMSS:
                    timeText = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
                    break;
                case TimeFormat.MMSSmm:
                    timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
                    break;
                case TimeFormat.SS:
                    timeText = string.Format("{0:D2}", timeSpan.Seconds);
                    break;
                case TimeFormat.SSmm:
                    timeText = string.Format("{0:D2}:{1:D2}", timeSpan.Seconds, timeSpan.Milliseconds);
                    break;
                default:
                    break;
            }

            return timeText;
        }

        public enum TimeFormat
        {
            hhMMSS, 
            MMSS,
            MMSSmm,
            SS,
            SSmm
        }

        /// <summary>
        /// Check if renderer, or a collection of renderers, are visible on camera - uses bounding box.
        /// </summary>
        /// <param name="renderers">The list of renderers</param>
        /// <param name="onCamera">Parse a camera to check on</param>
        /// <returns></returns>
        public static bool IsFullyVisible(List<Renderer> renderers, Camera onCamera)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(onCamera);

            foreach (Renderer renderer in renderers)
            {
                Bounds bounds = renderer.bounds;
                Vector3 size = bounds.size;
                Vector3 min = bounds.min;

                //Calculate the 8 points on the corners of the bounding box
                List<Vector3> boundsCorners = new List<Vector3>(8) {
             min,
             min + new Vector3(0, 0, size.z),
             min + new Vector3(size.x, 0, size.z),
             min + new Vector3(size.x, 0, 0),
         };
                for (int i = 0; i < 4; i++)
                    boundsCorners.Add(boundsCorners[i] + size.y * Vector3.up);

                //Check each plane on every one of the 8 bounds' corners
                for (int p = 0; p < planes.Length; p++)
                {
                    for (int i = 0; i < boundsCorners.Count; i++)
                    {
                        if (planes[p].GetSide(boundsCorners[i]) == false)
                            return false;
                    }
                }
            }
            return true;
        }

        public static Bounds GetBoundsWithChildren(this GameObject gameObject)
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

            Bounds bounds = renderers.Length > 0 ? renderers[0].bounds : new Bounds();

            // Start from 1 because we've already encapsulated renderers[0]
            for (int i = 1; i < renderers.Length; i++)
            {
                if (renderers[i].enabled)
                {
                    bounds.Encapsulate(renderers[i].bounds);
                }
            }

            return bounds;
        }

        public static Bounds GetBoundsOfRenderers(this List<Renderer> renderers)
        {
            Bounds bounds = renderers.Count > 0 ? renderers[0].bounds : new Bounds();

            // Start from 1 because we've already encapsulated renderers[0]
            for (int i = 1; i < renderers.Count; i++)
            {
                if (renderers[i].enabled)
                {
                    bounds.Encapsulate(renderers[i].bounds);
                }
            }

            return bounds;
        }

        public static void FocusOn(this Camera camera, GameObject focusedObject, float marginPercentage)
        {
            Bounds bounds = focusedObject.GetBoundsWithChildren();
            float maxExtent = bounds.extents.magnitude;
            float minDistance = (maxExtent * marginPercentage) / Mathf.Sin(Mathf.Deg2Rad * camera.fieldOfView / 2f);
            camera.transform.position = focusedObject.transform.position - Vector3.forward * minDistance;
        }
        public static float GetDistanceForFocus(this Camera camera, List<Renderer> renderers, float marginPercentage)
        {
            Bounds bounds = renderers.GetBoundsOfRenderers();
            float maxExtent = bounds.extents.magnitude;
            float minDistance = (maxExtent * marginPercentage) / Mathf.Sin(Mathf.Deg2Rad * camera.fieldOfView / 2f);
            return minDistance;
        }
        public static Vector3 GetPositionForFocus(this Camera camera, List<Renderer> renderers, float marginPercentage)
        {
            Bounds bounds = renderers.GetBoundsOfRenderers();
            float maxExtent = bounds.extents.magnitude;
            float minDistance = (maxExtent * marginPercentage) / Mathf.Sin(Mathf.Deg2Rad * camera.fieldOfView / 2f);
            return bounds.center - Vector3.forward * minDistance;
        }

    }

}
