using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLDPlayerMovement : MonoBehaviour
{
    Animator animator;
    CharacterController controller;
    new public Transform camera;

    public float runSpeed;    // player movement speed
    public float jumpForce;    // the force sending the player up upon a jump
    public float gravityForce;  // how much gravity affects the player
    Vector3 velocity;

    bool isGrounded;    // I was having issues with CharacterController.isGrounded
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    float turnVelocity;
    public float turnSmoothing = 0.1f;  // smooths player rotation angle

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (controller.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2 * gravityForce);
            }
        }

        velocity.y += gravityForce * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime); // constant force of gravity

        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"),
            0f, Input.GetAxisRaw("Vertical"));   // collecting player movement

        if (dir.magnitude >= 0.1f && controller.isGrounded)
        {
            animator.SetBool("Running", true);
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + camera.eulerAngles.y;   // rotates the player in relation to the camera position
            float realAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                targetAngle, ref turnVelocity, turnSmoothing);  // smoothing of the turn for arrow key movement
            transform.rotation = Quaternion.Euler(0f, realAngle, 0f);   // rotating the player model in the direction it's moving

            dir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(dir * runSpeed * Time.deltaTime);  // applying all of the above to the CharacterController
        }
        else
        {
            animator.SetBool("Running", false);
        }
    }
}
