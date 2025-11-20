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

    private bool isRotatingToTarget = false;
    private Transform rotationTarget;
    public float rotationLerpSpeed = 5f;


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

        if (!dialogueManager.dialogueActive && !branch.branchActive && !isRotatingToTarget)
        {
            // Normal movement and looking logic
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

            curSpeedX = currentSpeed * Input.GetAxis("Vertical");
            curSpeedY = currentSpeed * Input.GetAxis("Horizontal");
        }

        // Gravity and movement code remains the same
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        moveDirection.y = movementDirectionY;

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            moveDirection.y = 0f;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        // Handle rotation logic here

        if (isRotatingToTarget && rotationTarget != null)
        {
            RotateTowardsTarget();
        }
        else if (!dialogueManager.dialogueActive && !branch.branchActive && canLook)
        {
            // Normal mouse look
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * lookSpeed);
        }
    }

    public void RotateCameraToTarget(Transform target)
    {
        rotationTarget = target;
        isRotatingToTarget = true;

        // Disable manual mouse look while rotating
        canLook = false;
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = rotationTarget.position - playerCamera.transform.position;
        direction.y = 0; // keep horizontal only

        if (direction.sqrMagnitude < 0.001f)
        {
            // Target too close, stop rotating
            StopRotation();
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate the player (yaw)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationLerpSpeed * Time.deltaTime);

        // Also smoothly rotate camera pitch to default or target pitch if desired
        // Here, just keep pitch at 0 for simplicity
        rotationX = Mathf.Lerp(rotationX, 0, rotationLerpSpeed * Time.deltaTime);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Stop rotating if close enough
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
        {
            StopRotation();
        }
    }

    private void StopRotation()
    {
        isRotatingToTarget = false;
        rotationTarget = null;
        canLook = true; // re-enable manual look
    }


}
