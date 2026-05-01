using UnityEngine;
using UnityEngine.InputSystem;

//This script uses Unity's new Input system to rotate the player and camera
public class FirstPersonLook : MonoBehaviour
{
    [Header("Mouse Sensitivity Settings")]
    public float sensitivityX = 1.0f; //Horizontal mouse sensitivity
    public float sensitivityY = 1.0f; //Vertical mouse sensitivity

    [Header("Camera Setup")]
    public Transform cameraRoot; //Reference to the CameraRoot

    // Internal state to track accumulated look angles
    private float yaw;   //Horizontal rotation
    private float pitch; //Vertical rotation

    private InputAction lookAction; // Look input (mouse delta / right stick)

    // 🔥 NEW: Control whether looking is allowed
    private bool canLook = true;

    // Lock the cursor when the game starts and declare the Look Action
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        lookAction = InputSystem.actions.FindAction("Look");

        if (lookAction != null)
            lookAction.Enable();
        else
            Debug.LogError("Look action not found! Make sure it's set up in your Input Actions.");
    }

    // Actions every frame
    public void Update()
    {
       
        if (!canLook)
            return;

        if (lookAction == null)
            return;

        // Get the movement delta from input
        Vector2 delta = lookAction.ReadValue<Vector2>();

        // Horizontal rotation (player body)
        yaw = delta.x * sensitivityX;
        transform.Rotate(0f, yaw, 0f);

        // Vertical rotation (camera)
        pitch -= delta.y * sensitivityY;
        pitch = Mathf.Clamp(pitch, -60f, 60f);

        cameraRoot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

   
    public void DisableLook()
    {
        canLook = false;
    }

   
    public void EnableLook()
    {
        canLook = true;
    }
}