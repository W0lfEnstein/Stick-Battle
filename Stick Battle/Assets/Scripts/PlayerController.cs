using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Move Settings

    [Header("Move Settings")]
    public float currentSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    public float jumpSpeed;
    public float gravity;

    public PlayerState playerState;
    public Vector3 Directing;
    public Vector2 Rotating;

    private Vector3 direction;

    private Animator anim;
    private CharacterController cc;
    private Transform trans;

    #endregion

    #region Look Settings

    [Header("Look Settings")]
    public float sensitivityX;
    public float sensitivityY;

    private float rotationX;
    private float rotationY;

    public Camera FirstPersonCamera;
    public Camera ThirdPersonCamera;
    public Transform Head;

    #endregion

    private void Start()
    {
        #region Move Settings - Start

        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        trans = GetComponent<Transform>();

        currentSpeed = walkSpeed;

        #endregion

        #region Look Settings - Start

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        #endregion
    }

    private void Update()
    {
        #region Move Settings - Update

        //if(Input.GetKeyDown(KeyCode.LeftControl))

        if (Input.GetKeyDown(KeyCode.LeftShift))
            currentSpeed = runSpeed;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            currentSpeed = walkSpeed;

        direction = Input.GetAxis("Horizontal") * trans.right + Input.GetAxis("Vertical") * trans.forward; 
        direction = Vector3.ClampMagnitude(direction, 1f);

        if(Input.GetKey(KeyCode.Space) && playerState != PlayerState.Jumping)
        {
            direction.y = jumpForce;

            playerState = PlayerState.Jumping;
        }

        if(!cc.isGrounded)
        {
            direction.y -= gravity;
        }

        if (direction != Vector3.zero)
        {
            if(playerState != PlayerState.Jumping)
                playerState = (currentSpeed == walkSpeed) ? PlayerState.Walking : PlayerState.Running;

            cc.Move(direction * currentSpeed * Time.deltaTime);
        }
        else if(direction == Vector3.zero)
        {
            playerState = PlayerState.Idle;
        }

        #endregion

        #region Look Settings - Update

        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);

        trans.rotation = Quaternion.Euler(0, rotationX, 0);
        ThirdPersonCamera.transform.localRotation = Quaternion.Euler(-rotationY, 0, 0);

        Rotating = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        #endregion
    }

    private void FixedUpdate()
    {
        #region Move Settings - FixedUpdate



        #endregion
    }

    private void LateUpdate()
    {
        #region Look Settings - LateUpdate

        //head.localRotation = Quaternion.Euler(-rotationY, 0, 0);

        #endregion
    }
}

public enum PlayerState
{
    Idle,
    Walking,
    Running,
    Jumping,
    Crawling
}
