using UnityEngine;

public class ScreenshotCapture : MonoBehaviour
{
    private int screenshotCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Replace KeyCode.Space with your desired input key
        {
            string screenshotFilename = "Screenshot_" + screenshotCount + ".png";
            ScreenCapture.CaptureScreenshot(screenshotFilename);
            Debug.Log("Screenshot saved as " + screenshotFilename);
            screenshotCount++;
        }
    }
}