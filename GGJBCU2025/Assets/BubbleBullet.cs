using UnityEngine;

public class BubbleBullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    //float originalX;

    public float floatStrength = 5;
    public float damage = 1;

    private int movement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lifetime = Time.time + lifetime;
        // this.originalX = this.transform.position.x;
        
    }

    // Update is called once per frame
    void Update()
    {
        //wavemotion();
        if (Time.time > lifetime) {Destroy(gameObject);  Debug.Log("Object destroyed"); }
    }

   

    private void wavemotion()
    {
        //ads the wave movement to log by using sin function
       /* gameObject.transform.position = new Vector3(originalX + ((float)Mathf.Sin() * floatStrength),
          transform.position.y ,
           transform.position.z);*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().Damage(damage);
        }
    }
}
