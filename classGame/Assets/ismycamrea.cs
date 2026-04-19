using Unity.Netcode;
using UnityEngine;

public class ismycamrea : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private GameObject cinemachineCamera;

    public override void OnNetworkSpawn()
    {
        bool isMine = IsOwner;

        // Enable ONLY for local player
        playerCamera.enabled = isMine;

        if (audioListener != null)
            audioListener.enabled = isMine;

        if (cinemachineCamera != null)
            cinemachineCamera.SetActive(isMine);
    }
}
