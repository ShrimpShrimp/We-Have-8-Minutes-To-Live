using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float sprintSpeed = 10f; // added sprint speed
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;

    public bool canMove = true;  // made public
    public bool canLook = true;  // added public bool for looking

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    public DialogueManager dialogueManager;
    public BranchManager branch;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = 0f;
        float curSpeedY = 0f;

        if (!dialogueManager.dialogueActive && !branch.branchActive)
        {
            // Check sprint key
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

            curSpeedX = currentSpeed * Input.GetAxis("Vertical");
            curSpeedY = currentSpeed * Input.GetAxis("Horizontal");
        }

        // Preserve the vertical velocity (gravity)
        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        moveDirection.y = movementDirectionY;

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            // On ground, reset gravity effect
            moveDirection.y = 0f;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (!dialogueManager.dialogueActive && !branch.branchActive)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * lookSpeed);
        }
    }
}
