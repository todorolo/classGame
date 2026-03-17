using UnityEngine;

public class FallDestroy : MonoBehaviour
{
    public float yThreshold = -10f; //Change how the Vertical (Y distance) where the object is destroyed

    //Check position of this object every frame
    void Update()
    {
        if (transform.position.y < yThreshold) //if the object’s position in Y is less than the threshold…
        {
            Destroy(gameObject); //Destroy the object
        }
    }
}
