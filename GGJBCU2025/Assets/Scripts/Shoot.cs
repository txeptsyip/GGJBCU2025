using Unity.VisualScripting;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float Speed = 1f;
    float fireRate = 0;
     float fireDelay = 0.5f;
    public Rigidbody projectile;
    public float Cooldown;
    public GameObject BubblePrefab;
    public Transform Spawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    
    void Bubble_Shoot()
    {
        if (Time.time < fireRate) return;
        BubblePrefab.transform.position = Spawner.transform.position;
        Rigidbody BubbleRb = Instantiate(projectile,new Vector3(transform.position.x,transform.position.y,transform.position.z), gameObject.transform.rotation) as Rigidbody;
        BubbleRb.AddForce(transform.forward * 500 * Speed); // indicates the direction and level of force
        fireDelay = Time.time + Cooldown;

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > fireDelay)
        {
            // Ctrl was pressed, launch a projectile
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Bubble_Shoot();

            }

        }
    }
}