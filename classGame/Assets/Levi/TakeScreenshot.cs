using NUnit.Framework.Internal;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class TakeScreenshot : MonoBehaviour
{
    private bool takeScreenshot;
    public InputActionReference pictureAction; // Drag your "Interact" action here
    public RenderTexture renderTexture;

    private void OnEnable()
    { // Turns on the camera rendering and ability to take screenshot
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
        if (pictureAction != null)
            pictureAction.action.Enable();
    }

    private void OnDisable()
    { // Turns off the camera rendering and ability to take screenshot
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
        if (pictureAction != null )
            pictureAction.action.Disable();
    }






    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        if (takeScreenshot)
        {
            takeScreenshot = false;

            int width = Screen.width;
            int height = Screen.height;
            Texture2D screenshotTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            RenderTexture.active = renderTexture;
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            screenshotTexture.ReadPixels(rect, 0, 0);
            screenshotTexture.Apply();

            byte[] byteArray = screenshotTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot" + DateTime.Now.ToString() + ".png", byteArray);
        }
    }

    private void Update()
    {
        if (pictureAction)

        {
            takeScreenshot = true;
            // screenshots without UI, uses the bool and other stuff above

            // ScreenCapture.CaptureScreenshot("GameScreenshot.png");
            // basic screenshot thing, only needs this single line of code.

            // StartCoroutine(CoroutineScreenshot());
            // screenshots including UI, uses the coroutine below
        }
    }



    // includes UI

    private IEnumerator CoroutineScreenshot()
    {
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        screenshotTexture.ReadPixels(rect, 0, 0);
        screenshotTexture.Apply();

        byte[] byteArray = screenshotTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot" + DateTime.Now.ToString() + ".png", byteArray);
    }
}
