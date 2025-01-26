using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class ShootP2 : MonoBehaviour
{
    public float Speed = 1f;
    float fireRate = 0;
    float fireDelay = 0.5f;
    public Rigidbody projectile;
    public float Cooldown;
    public GameObject BubblePrefab;
    public Transform Spawner;
    public bool isController = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Bubble_Shoot()
    {
        if (Time.time < fireRate) return;
        BubblePrefab.transform.position = Spawner.transform.position;
        Rigidbody BubbleRb = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), gameObject.transform.rotation) as Rigidbody;
        BubbleRb.AddForce(transform.forward * 500 * Speed); // indicates the direction and level of force
        fireDelay = Time.time + Cooldown;

    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > fireDelay)
        {
            if (isController)
            {


                // Ctrl was pressed, launch a projectile
                if (Input.GetAxis("Fire3") > 0.8)
                {
                    Bubble_Shoot();
                }





            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Comma))
                {
                    Bubble_Shoot();
                }

            }
        }
    }
}
