using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMotor : MonoBehaviour
{ 
    
    private Vector3 PlayerMovementInput;
    private Vector3 PlayerMouseInput;
    private float xRot;
    private float Speed;
    public float attackDelayCurrent;

    [SerializeField] private LayerMask FloorMask;
    [SerializeField] private Transform FeetTransform;
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private Rigidbody PlayerBody;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float Sensitivity;
    [SerializeField] private float JumpForce;
    [SerializeField] private float attackDelay;
    [SerializeField] public Animator playerAnim;
    [SerializeField] public Animator weaponAnim;
    public Transform respawnPoint;
    public Transform player;


    // Update is called once per frame

    private void Start()
    {
        attackDelayCurrent = attackDelay;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }
    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        Move();
        MovePlayerCamera();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            weaponAnim.SetBool("isShooting", true);
            weaponAnim.SetBool("hasShot", true);
            weaponAnim.SetBool("isReloading", false);
            weaponAnim.SetBool("hasReloaded", true);
        }
        else
        {
            weaponAnim.SetBool("isShooting", false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            weaponAnim.SetBool("isReloading", true);
            weaponAnim.SetBool("hasReloaded", true);
            weaponAnim.SetBool("isShooting", false);
            weaponAnim.SetBool("hasShot", false);
        }
        else
        {
            weaponAnim.SetBool("isReloading", false);
        }

        
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = sprintSpeed;
        }
        else
        {
            Speed = walkSpeed;
        }

        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * Speed;
        PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);
      

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.CheckSphere(FeetTransform.position, 0.1f, FloorMask)) 
            {
                PlayerBody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }
        }

    }

    void MovePlayerCamera() {

        xRot -= PlayerMouseInput.y * Sensitivity;

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        player.transform.position = respawnPoint.transform.position;
    }
}
