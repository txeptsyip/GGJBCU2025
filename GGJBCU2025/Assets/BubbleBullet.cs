using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleBullet : MonoBehaviour
{
    public Rigidbody rb;
    public float Force_X = 1000f;
    public Transform Spawner;
    public GameObject BubblePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            Projectile();
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.F) == true) 
        {

            Projectile();
        }
    }


    public void Projectile()
    {
        BubblePrefab.transform.position = Spawner.transform.position;
        
        rb.AddForce(Force_X, 0, 0);
    }
}
