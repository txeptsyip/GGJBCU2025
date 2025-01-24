using UnityEngine;
using UnityEngine.Rendering;

public class shoot : MonoBehaviour
{
    float fireRate = 0;
    float fireDelay = 0.5f;
    public Rigidbody projectile;
    public Transform gunBarrel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Shoot()
    {
        if (Time.time < fireRate) return;
        fireRate = Time.time + fireDelay;
        Instantiate(projectile, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        // Ctrl was pressed, launch a projectile
        if (Input.GetKeyDown(KeyCode.E))
        {
            Shoot();
        }
    }
}