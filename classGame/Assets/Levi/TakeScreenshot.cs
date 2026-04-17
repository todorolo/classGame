using NUnit.Framework.Internal;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class TakeScreenshot : MonoBehaviour
{
    private bool takeScreenshot;

    [Header("Raycast")]
    public Transform rayOrigin;                  // Where does the ray start from
    public float maxDistance = 3f;               // How far the ray reaches
    public LayerMask cryptidMask;                // What layers the camera's cryptid detection works on

    [Header("Input")]
    public InputActionReference pictureAction;   // Drag input key here
    public RenderTexture renderTexture;          // Drag render texture here

    public bool canPhotograph = true;            // Use this outside to stop interactions ???

    private void OnEnable()
    { // Turns on the camera rendering and take picture button when script is enabled
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
        if (pictureAction != null)
            pictureAction.action.Enable();
    }

    private void OnDisable()
    { // Turns off the camera rendering and take picture button when script is disabled
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
        if (pictureAction != null)
            pictureAction.action.Disable();
    }

    private void Update()
    {
        bool picturePressed = (pictureAction != null) && pictureAction.action.WasPressedThisFrame(); // Detects if key pressed

        if (picturePressed) ; // When key is pressed, screenshots and checks for cryptids
        {
            takeScreenshot = true; // screenshots without UI, uses stuff above
            CryptidCheck();
        }
    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext arg1, Camera arg2)
    {
        if (takeScreenshot)
                {
            takeScreenshot = false; // if screenshot is true, turns it off, then does below

            int width = Screen.width;
            int height = Screen.height;
            Texture2D screenshotTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            RenderTexture.active = renderTexture;
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            screenshotTexture.ReadPixels(rect, 0, 0);
            screenshotTexture.Apply();

            byte[] byteArray = screenshotTexture.EncodeToPNG(); // Writes screenshot to file with current date+time for iterations
            System.IO.File.WriteAllBytes(Application.dataPath + "/CameraScreenshot" + DateTime.Now.ToString() + ".png", byteArray);
        }
    }

    private void CryptidCheck()
    {
        // use raycasts at parts of camera to check for targets. use multiple for better accuracy
        // for now the basic raycast from the interactable script, need to alter it
        
    }

    // Draws a debug ray in the Scene view to visualize the interaction line.
    void OnDrawGizmos()
    {
        if (!rayOrigin) return;

        Gizmos.color = Color.red;

        Vector3 start = rayOrigin.position;
        Vector3 end = start + rayOrigin.forward * maxDistance;
        Gizmos.DrawLine(start, end);
    }
}
