using UnityEngine;
using UnityEngine.InputSystem; //Add to activate Input System


public class RollABall : MonoBehaviour
{
    public float acceleration = 10f; //Change how fast ball moves in the editor

    Rigidbody rb;//reference to rigidbody
    InputAction moveAction; //reference to "Move" action in input system


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();//get rigidbody to do something to it

        moveAction = InputSystem.actions.FindAction("Move");//get "Move" action to read its value
    }


    // Update is called once per frame
    void Update()
    {

        Vector2 moveInput = moveAction.ReadValue<Vector2>();//take value from "Move"

        //calculate a force in every direction (except up) using Move value * acceleration
        Vector3 force = new Vector3(moveInput.x, 0f, moveInput.y) * acceleration;

        //add force to object
        rb.AddForce(force, ForceMode.Acceleration);

    }
}
