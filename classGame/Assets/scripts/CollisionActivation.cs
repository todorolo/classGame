using UnityEngine;

public class CollisionActivation : MonoBehaviour
{
    public GameObject objectToActivate; //The disabled object to be activated by the collision, assigned in the editor
    private bool activatedB; //The disabled object to be activated by the collision, assigned in the editor

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) //If the object that collided with me is tagged as “Player”...
        {

            objectToActivate.SetActive(true); //Activate the objectToActivate
        }
    }
}