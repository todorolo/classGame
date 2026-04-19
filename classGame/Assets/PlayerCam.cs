using Unity.Netcode;
using UnityEngine;

public class PlayerCam : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;

    public override void OnNetworkSpawn()
    {
        // Only enable camera for the LOCAL player
        if (IsOwner)
        {
            playerCamera.gameObject.SetActive(true);

            // Disable UI camera
            Camera uiCamera = GameObject.Find("UICamera")?.GetComponent<Camera>();
            if (uiCamera != null)
            {
                uiCamera.gameObject.SetActive(false);
            }
        }
        else
        {
            // Disable camera for other players
            playerCamera.gameObject.SetActive(false);
        }
    }
}