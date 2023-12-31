using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float gravity = -20f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private int availableJumps;

    public DoubleJumpAbility doubleJumpAbility; // Reference to the DoubleJumpAbility scriptable object
    public Animator animator; // Reference to the Animator component

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Initialize the animator here
    }

    private void Start()
    {
        availableJumps = doubleJumpAbility.hasDoubleJump ? 2 : 1; // Set available jumps based on ability
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        // Horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(0, 0, moveInput);
        Vector3 move = transform.TransformDirection(moveDirection) * moveSpeed;

        // Apply gravity
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0;
            availableJumps = doubleJumpAbility.hasDoubleJump ? 2 : 1; // Reset jumps when grounded
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && availableJumps > 0)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);
            availableJumps--;

            // Set the animator parameter "IsJumping" to trigger the jump animation
            animator.SetTrigger("IsJumping");
        }

        // Calculate the character's speed (you can adjust this as needed)
        float speed = Mathf.Abs(moveInput);

        // Set the animator parameter "Speed"
        animator.SetFloat("Speed", speed);

        // Set the animator parameter "IsGrounded" to control the idle animation
        animator.SetBool("IsGrounded", isGrounded);

        // Apply movement and gravity
        controller.Move((move + velocity) * Time.deltaTime);
    }
}
