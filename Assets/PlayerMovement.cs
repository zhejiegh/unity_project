using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 0f; // 0 = strafe, >0 = tourner
    public float inputSmoothing = 10f;

    [Header("Jump / Gravity")]
    public float jumpHeight = 2f;
    public float gravity = -20f;

    [Header("Animator Params")]
    public string speedParam = "Speed";       // Blend Tree param
    public string groundedParam = "Grounded"; // Bool
    public string jumpTriggerParam = "Jump";  // Trigger

    [Header("Ground Detection")]
    public float groundRayDistance = 0.2f;     // Distance pour détecter le sol
    public LayerMask groundLayer;              // Layer du terrain/sol

    private CharacterController controller;
    private Animator anim;

    private float verticalVelocity;
    private float smoothedSpeed;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // --- INPUT (flèches directionnelles) ---
        float rawX = Input.GetAxisRaw("Horizontal"); // gauche/droite
        float rawY = Input.GetAxisRaw("Vertical");   // haut/bas

        // --- ROTATION OPTIONNELLE ---
        if (rotationSpeed > 0f)
        {
            transform.Rotate(0f, rawX * rotationSpeed * Time.deltaTime, 0f);
            rawX = 0f;
        }

        // --- CALCUL DE LA VITESSE ---
        float targetSpeed = new Vector2(rawX, rawY).magnitude; // 0 à 1
        smoothedSpeed = Mathf.Lerp(smoothedSpeed, targetSpeed, inputSmoothing * Time.deltaTime);

        // --- GROUND DETECTION VIA RAYCAST ---
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // légèrement au-dessus du sol
        bool grounded = Physics.Raycast(rayOrigin, Vector3.down, groundRayDistance + 0.1f, groundLayer);
        Debug.DrawRay(rayOrigin, Vector3.down * (groundRayDistance + 0.1f), grounded ? Color.green : Color.red);

        anim.SetBool(groundedParam, grounded);

        // --- COLLAGE AU SOL ---
        if (grounded && verticalVelocity < 0f)
            verticalVelocity = -0.1f;

        // --- JUMP SUR TOUCHE A ---
        if (Input.GetMouseButtonDown(0) && grounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetTrigger(jumpTriggerParam);
            Debug.Log("Jump Triggered!");
        }

        // --- GRAVITY ---
        verticalVelocity += gravity * Time.deltaTime;

        // --- MOVE ---
        Vector3 localMove = new Vector3(rawX, 0f, rawY);
        if (localMove.sqrMagnitude > 1f) localMove.Normalize();

        Vector3 worldMove = transform.TransformDirection(localMove) * moveSpeed;
        Vector3 motion = new Vector3(worldMove.x, verticalVelocity, worldMove.z);
        controller.Move(motion * Time.deltaTime);

        // --- ANIMATOR ---
        anim.SetFloat(speedParam, smoothedSpeed);

        // --- DEBUG ---
        Debug.Log($"Speed: {smoothedSpeed}, Grounded: {grounded}, VerticalVelocity: {verticalVelocity}");
    }
}
