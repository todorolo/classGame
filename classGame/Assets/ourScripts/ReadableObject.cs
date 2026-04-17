using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.ProBuilder.MeshOperations;

// Example of an interactable object that implements the IInteractable interface.
// In this case, the object might be a diary, note, or item with descriptive text.

public class ReadableObject : MonoBehaviour, IInteractable
{
    public string objectName = "A mysterious book"; // Name shown in logs (can customize per object)
    public GameObject InteractText;
    public GameObject PlayerCamera;

    // Triggered when the player looks at the object
    public void OnLookAt()
    {
        Debug.Log("Looking at: " + objectName);
        InteractText.SetActive(true);
    }

    // Triggered when the player presses the interact button while looking at the object
    public void OnInteract()
    {
        Debug.Log("Interacting with: " + objectName);
        // You could show a UI panel here, play a sound, etc.
        PlayerCamera.SetActive(true);
        InteractText.SetActive(false);
        Object.Destroy(gameObject);
    }

    // Triggered when the player stops looking at the object
    public void OnDisengage()
    {
        if (PlayerCamera.activeInHierarchy)
        {
            Debug.Log("Disengage: " + objectName);
        }
        else
        {
            Debug.Log("Disengage: " + objectName);
            InteractText.SetActive(false);
        }
    }
}
