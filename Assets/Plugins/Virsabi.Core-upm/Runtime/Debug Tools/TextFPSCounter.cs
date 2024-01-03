﻿using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Display FPS on a Unity UGUI Text Panel
// To use: Drag onto a game object with Text component
//         Press 'F' key to toggle show/hide

namespace Virsabi.DebugTools
{
	public class TextFPSCounter : MonoBehaviour
	{
		[SerializeField, ReadOnly]
		public TextMeshProUGUI text;
		public bool show = false;

		public int targetFPS = 90;
		private const float updateInterval = 0.5f;

		private int framesCount;
		private float framesTime;

		void OnValidate()
		{
			// no text object set? see if our gameobject has one to use
			if (text == null)
			{
				text = GetComponent<TextMeshProUGUI>();
			}
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				show = !show;
			}

			// monitoring frame counter and the total time
			framesCount++;
			framesTime += Time.unscaledDeltaTime;

			// measuring interval ended, so calculate FPS and display on Text
			if (framesTime > updateInterval)
			{
				if (text != null)
				{
					if (show)
					{
						float fps = framesCount / framesTime;
						text.text = ((int)fps).ToString();
						text.color = (fps > (targetFPS - 5) ? Color.green :
									 (fps > (targetFPS - 30) ? Color.yellow :
									  Color.red));
					}
					else
					{
						text.text = "";
					}
				}
				// reset for the next interval to measure
				framesCount = 0;
				framesTime = 0;
			}

		}
	}

}
