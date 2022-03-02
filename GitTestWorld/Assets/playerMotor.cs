using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playerMotor : MonoBehaviour
{ 
    
    private Vector3 PlayerMovementInput;
    private Vector3 PlayerMouseInput;
    private float xRot;
    private float Speed;
    public float attackDelayCurrent;
    private bool bossDead = false;
    public bool enemyHit = false;
    public bool playerDead = false;

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
    public Transform bossPoint;
    public TextMeshProUGUI winText;
    public BossMotion bossClass;
    public Image leftCrosshair;
    public Image rightCrosshair;
    public Image upCrosshair;
    public Image downCrosshair;
    public FloppyLauncherScript floppyLauncher;
    public WaveSpawner waveClass;
    public Camera camera;
    public PauseMenu pauseMenu;

    // Update is called once per frame

    private void Start()
    {
        attackDelayCurrent = attackDelay;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        player.transform.position = respawnPoint.transform.position;
        Transform camTransform = camera.GetComponent<Transform>();
        float xRot = Mathf.Clamp(camTransform.eulerAngles.x, -5f, 5f);
        camTransform.eulerAngles = new Vector3(xRot, camTransform.eulerAngles.y, camTransform.eulerAngles.z);

    }
    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        Move();
        if (!pauseMenu.gameIsPaused)
        {
            MovePlayerCamera();
        }
        

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

        if (bossDead) {
            bossDead = false;
            bossClass.bossHealth.value = 0;
            bossClass.bossHealthText.SetText("0 / " + bossClass.maxHealth + " HP");
            if (winText != null)
            {
                winText.SetText("You Win!");
            }
            Invoke("RespawnPlayer", 3f);
            waveClass.nextWave = 0;
            waveClass.waveCountdown = waveClass.timeBetweenWaves;
            waveClass.state = WaveSpawner.SpawnState.COUNTING;
            Invoke("Restart", 3f);
        }

        if (enemyHit) {
            enemyHit = false;
            var tempColorLeft = leftCrosshair.color;
            var tempColorRight = rightCrosshair.color;
            var tempColorUp = upCrosshair.color;
            var tempColorDown = downCrosshair.color;
            tempColorLeft.a = 1f;
            tempColorRight.a = 1f;
            tempColorUp.a = 1f;
            tempColorDown.a = 1f;
            leftCrosshair.color = tempColorLeft;
            rightCrosshair.color = tempColorRight;
            upCrosshair.color = tempColorUp;
            downCrosshair.color = tempColorDown;
            Invoke("ClearHitmarker", 0.1f);
            FindObjectOfType<AudioManager>().Play("Hitmarker");
        }

        if (playerDead)
        {
            playerDead = false;
            bossClass.SetPlayerInBossArena(false);
            winText.SetText("You Lose.");
            waveClass.nextWave = 0;
            waveClass.waveCountdown = waveClass.timeBetweenWaves;
            waveClass.state = WaveSpawner.SpawnState.COUNTING;
            FindObjectOfType<AudioManager>().Play("PlayerDeath");
            Invoke("Restart", 3f);
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
        else
        {
            if (Physics.CheckSphere(FeetTransform.position, 0.1f, FloorMask))
            {
                PlayerBody.velocity = Vector3.ClampMagnitude(PlayerBody.velocity, Speed);
            }
        }

    }

    void MovePlayerCamera() {

        xRot -= PlayerMouseInput.y * Sensitivity;

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CasinoTeleport")
        {
            player.transform.position = bossPoint.transform.position;
            bossClass.SetPlayerInBossArena(true);
        }
    }

    public void SetBossDead(bool death)
    {
        bossDead = death;
    }

    public void RespawnPlayer()
    {
        player.transform.position = respawnPoint.transform.position;
    }

    public void ClearText()
    {
        if (winText != null)
        {
            winText.SetText("");
        }
    }

    public void ClearHitmarker()
    {
        var tempColorLeft = leftCrosshair.color;
        var tempColorRight = rightCrosshair.color;
        var tempColorUp = upCrosshair.color;
        var tempColorDown = downCrosshair.color;
        tempColorLeft.a = 0f;
        tempColorRight.a = 0f;
        tempColorUp.a = 0f;
        tempColorDown.a = 0f;
        leftCrosshair.color = tempColorLeft;
        rightCrosshair.color = tempColorRight;
        upCrosshair.color = tempColorUp;
        downCrosshair.color = tempColorDown;
    }

    public void Restart()
    {
        winText.SetText("");
        bossClass.bossHealth.value = 0;
        bossClass.bossNameText.SetText("");
        bossClass.bossHealthText.SetText("");
        floppyLauncher.bulletsLeft = floppyLauncher.magazineSize;
        floppyLauncher.ammoLeft = floppyLauncher.fullAmmo;
    }
}
