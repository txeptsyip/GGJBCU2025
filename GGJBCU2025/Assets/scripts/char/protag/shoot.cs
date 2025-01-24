using UnityEngine;

public class shoot : MonoBehaviour
{
    float fireRate = 0;
    public Rigidbody projectile;
    public Transform gunBarrel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Shoot()
    {
        if (Time.time < fireRate) return;
        fireRate = Time.time + 0.5f;
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