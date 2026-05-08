using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonMove : MonoBehaviour
{

    Animator animator;
    private CharacterController controller;

    InputAction moveAction;
    InputAction jumpAction;

    private Vector2 moveInput;
    private Vector3 velocity;

    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float jumpHeight = 2.0f;

    [Header("Gravity Settings")]
    public float gravity = -9.8f;
    private bool isGrounded;

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.6f;
    public LayerMask groundMask;

    private AudioSource audio;

    [SerializeField] private AudioClip grassFootStep;
    [SerializeField] private AudioClip pathFootStep;
    [SerializeField] private AudioClip bridgeFootStep;
    [SerializeField] private AudioClip waterFootStep;
    [SerializeField] private AudioClip stoneFootStep;



    [SerializeField] private float walkTimerSound = 0.5f;
    [SerializeField] private float runTimerSound = 0.3f;

    private float stepTimer;

    void Start()
    {
        animator = GetComponent<Animator>();

        
        controller = GetComponent<CharacterController>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        moveAction.Enable();
        jumpAction.Enable();

        audio = GetComponent<AudioSource>();

        if (groundMask == 0)
        {
            groundMask = LayerMask.GetMask("Ground");
        }
    }

    void Update()
    {

        if (moveInput.y > 0.1f)
        {
         animator.SetBool("iswalking", true);
        }
        else
        {
          animator.SetBool("iswalking", false);
        }


        if (groundCheck == null) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        moveInput = moveAction.ReadValue<Vector2>();

        Vector3 move = new Vector3(moveInput.x, 0.0f, moveInput.y);
        controller.Move(transform.TransformDirection(move) * moveSpeed * Time.deltaTime);

        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        ManageFootStepSound();
    }

    private void ManageFootStepSound()
    {
        if (!isGrounded) return;

        if (moveInput.magnitude <= 0.1f) return;

        bool isRunning = moveInput.magnitude > 0.5f;
        float currentTimer = isRunning ? runTimerSound : walkTimerSound;

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0)
        {
            PlayFootStepSound();
            stepTimer = currentTimer;
        }
    }

    private void PlayFootStepSound()
    {
        if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 5f))
            return;

        Terrain terrain = hit.collider.GetComponent<Terrain>();

        if (terrain == null)
        {
            if (grassFootStep != null)
                audio.PlayOneShot(grassFootStep);
            return;
        }

        TerrainData data = terrain.terrainData;

        Vector3 terrainPosition = hit.point - terrain.transform.position;

        int mapX = Mathf.FloorToInt(terrainPosition.x / data.size.x * data.alphamapWidth);
        int mapZ = Mathf.FloorToInt(terrainPosition.z / data.size.z * data.alphamapHeight);

        float[,,] splatmap = data.GetAlphamaps(mapX, mapZ, 1, 1);

        int textureIndex = 0;
        float strongest = 0f;

        for (int i = 0; i < splatmap.GetLength(2); i++)
        {
            if (splatmap[0, 0, i] > strongest)
            {
                strongest = splatmap[0, 0, i];
                textureIndex = i;
            }
        }

        AudioClip clip = null;

        if (textureIndex == 0)
            clip = grassFootStep;
        else if (textureIndex == 1)
            clip = pathFootStep;
        else if (textureIndex == 2)
            clip = stoneFootStep;
         else if (textureIndex == 3)
            clip = grassFootStep;
        else if (textureIndex == 4)
            clip = stoneFootStep;

        if (clip != null)
        {
            audio.PlayOneShot(clip);
        }
    }
}