using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //private Animator animator;
    private CharacterController controller;

    private Vector3 move;
    private Vector3 lastMove;
    private Vector3 relativeMove;

    private float velocity;
    public float moveSpeed = 0.0f;  // player speed modifier
    public float runSpeed = 10.0f;
    public float walkSpeed = 6.0f;
    public float midairSpeed = 0.5f;
    public float jumpHeight = 8.0f;
    public float gravity = 25.0f;

    bool grounded = true;

    public static int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float facingAngle = Camera.main.transform.eulerAngles.y; // the angle the camera is facing

        move = Vector3.zero;
        move.x = Input.GetAxis("Horizontal");
        move.z = Input.GetAxis("Vertical");
        move = Quaternion.Euler(0, facingAngle, 0) * move; // make movement in relation to camera angle
        transform.LookAt(move + transform.position);

        grounded = controller.isGrounded;
        if (grounded)
        {
            velocity = -1;
            if (Input.GetButtonDown("Jump"))
                velocity = jumpHeight;

            /*if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                animator.SetBool("isWalking", true);
            else
                animator.SetBool("isWalking", false);*/

            if (Input.GetKey(KeyCode.LeftShift))
                moveSpeed = runSpeed;
            else
                moveSpeed = walkSpeed;
        }
        else
        {
            velocity -= gravity * Time.deltaTime;
            move.x = Mathf.Lerp(lastMove.x, move.x, midairSpeed);
            move.z = Mathf.Lerp(lastMove.z, move.z, midairSpeed); // allow some movement in the air
        }

        move.y = 0;
        move.Normalize();
        move *= moveSpeed;
        move.y = velocity;

        controller.Move(move * Time.deltaTime);
        lastMove = move;

        if (score >= 65) // accidental animations after collecting all the collectibles
            transform.LookAt(move + transform.position);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!grounded && hit.normal.y < 0.1f)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocity = jumpHeight;
                move = hit.normal * moveSpeed;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collectable")
        {
            score += 1;
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.name == "DeathPlane")
        {
            controller.enabled = false;
            transform.position = new Vector3(0f, 1.5f, 0f);
            controller.enabled = true;
        }
    }
}
