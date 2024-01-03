// Created by Long Nguyen Huu
// 2016.05.15
// MIT License

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.IO;

public class EditorScreenshot : EditorWindow
{

	string screenshotFolderPath = "Screenshots";
	string screenshotFilenamePrefix = "screenshot_";
	int nextScreenshotIndex = 0;
	int superSizeResolution = 1;
	Vector2Int resolutionCapture = new Vector2Int(1080, 1920);
	bool manuallySetSuperSize;
	[MenuItem("Virsabi Tools/Utilities/Editor Screenshot _F11")]
	static void Init()
	{
		GetOrCreateWindow();
	}

	//[MenuItem("Virsabi Tools/Utilities/Editor Screenshot _F11")]
	//static void StaticTakeScreenshot()
	//{
	//	GetOrCreateWindow().TakeScreenshot();
	//}

	static EditorScreenshot GetOrCreateWindow()
	{
		EditorScreenshot editorScreenshot = GetWindow<EditorScreenshot>(title: "Screenshot");

		if (EditorPrefs.HasKey("EditorScreenshot.screenshotFolderPath"))
			editorScreenshot.screenshotFolderPath = EditorPrefs.GetString("EditorScreenshot.screenshotFolderPath");
		if (EditorPrefs.HasKey("EditorScreenshot.screenshotFilenamePrefix"))
			editorScreenshot.screenshotFilenamePrefix = EditorPrefs.GetString("EditorScreenshot.screenshotFilenamePrefix");
		if (EditorPrefs.HasKey("EditorScreenshot.nextScreenshotIndex"))
			editorScreenshot.nextScreenshotIndex = EditorPrefs.GetInt("EditorScreenshot.nextScreenshotIndex");

		return editorScreenshot;
	}

	void OnGUI()
	{
		string folderPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.BeginHorizontal();
		GUIContent savePathLabel = new GUIContent("", "Save path of the screenshots, relative from the project root");

		if (GUILayout.Button("Select Folder"))
        {
			string checkPath = EditorUtility.OpenFolderPanel("title", folderPath, "").Replace(folderPath, "");
			if (checkPath.Length != 0)
				screenshotFolderPath = checkPath;
		}
			
		
		EditorGUI.BeginDisabledGroup(true);
		screenshotFolderPath = EditorGUILayout.TextField(savePathLabel, screenshotFolderPath); 
		EditorGUI.EndDisabledGroup();
		EditorGUILayout.EndHorizontal();

		screenshotFilenamePrefix = EditorGUILayout.TextField("Screenshot prefix", screenshotFilenamePrefix);
		nextScreenshotIndex = EditorGUILayout.IntField("Next screenshot index", nextScreenshotIndex);


		superSizeResolution = EditorGUILayout.IntField("Supersize Resolution", superSizeResolution);

		//tried calculating supersize but only int allowed in ScreenCapture.CaptureScreenshot(path, superSizeResolution); .... please.....
		/*manuallySetSuperSize = EditorGUILayout.Toggle("Manually Set Supersize", manuallySetSuperSize);
		EditorGUI.BeginDisabledGroup(!manuallySetSuperSize);
		EditorGUI.EndDisabledGroup();
		EditorGUI.BeginDisabledGroup(manuallySetSuperSize);
		resolutionCapture = EditorGUILayout.Vector2IntField("Supersize Resolution", resolutionCapture);
		EditorGUI.EndDisabledGroup();*/

		if (EditorGUI.EndChangeCheck()) {
			EditorPrefs.SetString("EditorScreenshot.screenshotFolderPath", screenshotFolderPath);
			EditorPrefs.SetString("EditorScreenshot.screenshotFilenamePrefix", screenshotFilenamePrefix);
			EditorPrefs.SetInt("EditorScreenshot.nextScreenshotIndex", nextScreenshotIndex);
		}

		if (GUILayout.Button("Take screenshot")) TakeScreenshot();

		folderPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets")) + screenshotFolderPath;

		if (GUILayout.Button("OpenFolder")) OpenInFileBrowser.Open(folderPath);
	}

	void TakeScreenshot()
	{
		string folderPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets")) + screenshotFolderPath;
		if (!Directory.Exists(folderPath))
			Directory.CreateDirectory(folderPath);

		// get name of current focused window, which should be "  (UnityEditor.GameView)" if it is a Game view
		string focusedWindowName = EditorWindow.focusedWindow.ToString();
		if (!focusedWindowName.Contains("UnityEditor.GameView")) {
			// since no Game view is focused right now, focus on any Game view, or create one if needed
			Type gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
			EditorWindow.GetWindow(gameViewType);
		}

		// Tried getting the last focused window, but does not always work (even for focused window!)
		// Type gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
		// EditorWindow lastFocusedGameView = (EditorWindow) gameViewType.GetField("s_LastFocusedGameView", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
		// if (lastFocusedGameView != null) {
		// 	lastFocusedGameView.Focus();
		// } else {
		// 	// no Game view created since editor launch, create one
		// 	Type gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
		// 	EditorWindow.GetWindow(gameViewType);
		// }

		string path = string.Format("{0}/{1}{2:00}.png", screenshotFolderPath, screenshotFilenamePrefix, nextScreenshotIndex);

		//tried calculating supersize but only int allowed in ScreenCapture.CaptureScreenshot(path, superSizeResolution); .... please.....
		/*float supersizeFactor = 0;

		if (Screen.width < resolutionCapture.x)
			supersizeFactor = resolutionCapture.x / Screen.width;
        else
			supersizeFactor = Screen.width / resolutionCapture.x;

		Debug.Log("supersizeFactor: " + supersizeFactor);
		*/

		ScreenCapture.CaptureScreenshot(path, superSizeResolution);

		Debug.LogFormat("Screenshot recorded at {0}", path);

		++nextScreenshotIndex;
		EditorPrefs.SetInt("EditorScreenshot.nextScreenshotIndex", nextScreenshotIndex);
	}

}
