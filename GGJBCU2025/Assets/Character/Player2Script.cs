using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Player2Script : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    //public Camera playerCamera;
    public float lookSpeed = 1f;
    public float health = 10f;


    public float Speed = 1f;
    float fireRate = 0;
    float fireDelay = 0.5f;
    public Rigidbody projectile;
    public float Cooldown;
    public GameObject BubblePrefab;
    public Transform Spawner;

    GameManager gameManager;

    private bool damaged = false;

    [SerializeField]
    private AudioSource hurt;

    private bool RapidFireActive = false;
    private bool ShotBubbleActive = false;

    CharacterController characterController;
    public TMP_Text Player2Hits;
    public TMP_Text powerup;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player2Hits = GameObject.Find("Lives2").GetComponent<TMP_Text>();
        powerup = GameObject.Find("Powerup2").GetComponent<TMP_Text>();
        // Lock cursor
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private IEnumerator Damaged()
    {
        yield return new WaitForEndOfFrame();
        damaged = false;
        Debug.Log(damaged);
        StopCoroutine(Damaged());
    }

    public void Damage(float damage)
    {
        if (!damaged)
        {
            hurt.Play();
            health = health - damage;
            Debug.Log(health);
            Player2Hits.text = health.ToString();
            damaged = true;
            Debug.Log(damaged);
            StartCoroutine(Damaged());
            if (health <= 0)
            {
                Debug.Log("the player has died");
                Destroy(gameObject);
                gameManager.Player1Win = true;
                gameManager.winner = true;

            }
        }
    }

    public void ActivatePowerUp()
    {
        Debug.Log("rapidfire status" + RapidFireActive);
        Debug.Log("shotbubble status" + ShotBubbleActive);
        if (RapidFireActive == false && ShotBubbleActive == false)
        {
            int powerUp = Random.Range(1, 3);
            Debug.Log(powerUp);
            if (powerUp == 1)
            {
                RapidFireActive = true;
                powerup.text = "Rapid Fire";
            }
            if (powerUp == 2)
            {
                ShotBubbleActive = true;
                powerup.text = "Shot-Bubble";
            }
            StartCoroutine(PowerUpTimer());
        }
    }

    public IEnumerator PowerUpTimer()
    {
        yield return new WaitForSeconds(10);
        RapidFireActive = false;
        ShotBubbleActive = false;
        StopCoroutine(PowerUpTimer());
        powerup.text = "";
    }

    void Bubble_Shoot()
    {
        if (Time.time < fireRate) return;
        BubblePrefab.transform.position = Spawner.transform.position;
        Rigidbody BubbleRb = Instantiate(projectile, new Vector3(Spawner.transform.position.x, Spawner.transform.position.y, Spawner.transform.position.z), Spawner.transform.rotation) as Rigidbody;
        BubbleRb.AddForce(transform.forward * 100 * Speed); // indicates the direction and level of force
        fireDelay = Time.time + Cooldown;

    }

    void RapidFire_Shoot()
    {
        BubblePrefab.transform.position = Spawner.transform.position;
        Rigidbody BubbleRb = Instantiate(projectile, new Vector3(Spawner.transform.position.x, Spawner.transform.position.y, Spawner.transform.position.z), Spawner.transform.rotation) as Rigidbody;
        BubbleRb.AddForce(transform.forward * 100 * Speed); // indicates the direction and level of force
        fireDelay = Time.time + (Cooldown / 6);
    }

    void ShotBubble_Shoot()
    {
        if (Time.time < fireRate) return;
        BubblePrefab.transform.position = Spawner.transform.position;
        Rigidbody BubbleRb = Instantiate(projectile, new Vector3(Spawner.transform.position.x, Spawner.transform.position.y, Spawner.transform.position.z), Spawner.transform.rotation) as Rigidbody;
        BubbleRb.AddForce(transform.forward * 100 * Speed);
        BubblePrefab.transform.position = Spawner.transform.position;
        Rigidbody BubbleRb1 = Instantiate(projectile, new Vector3(Spawner.transform.position.x, Spawner.transform.position.y, Spawner.transform.position.z), Spawner.transform.rotation) as Rigidbody;
        BubbleRb1.AddForce(transform.forward * 100 * Speed);
        BubblePrefab.transform.position = Spawner.transform.position;
        Rigidbody BubbleRb2 = Instantiate(projectile, new Vector3(Spawner.transform.position.x, Spawner.transform.position.y, Spawner.transform.position.z), Spawner.transform.rotation) as Rigidbody;
        BubbleRb2.AddForce(transform.forward * 100 * Speed);
        BubblePrefab.transform.position = Spawner.transform.position;
        Rigidbody BubbleRb3 = Instantiate(projectile, new Vector3(Spawner.transform.position.x, Spawner.transform.position.y, Spawner.transform.position.z), Spawner.transform.rotation) as Rigidbody;
        BubbleRb3.AddForce(transform.forward * 100 * Speed);// indicates the direction and level of force
        fireDelay = Time.time + Cooldown;

    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("VerticalSticks") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("HorizontalSticks") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) /*+ (right * curSpeedY)*/;

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            //playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, (Input.GetAxis("HorizontalSticks")) / 8 * lookSpeed, 0);
        }
        if (Time.time > fireDelay)
        {
            // Ctrl was pressed, launch a projectile
            if (Input.GetButtonDown("Fire3"))
            {
                if (RapidFireActive == true)
                {
                    RapidFire_Shoot();
                }
                else if (ShotBubbleActive == true)
                {
                    ShotBubble_Shoot();
                }
                else
                {
                    Bubble_Shoot();
                }

            }

        }
    }
}