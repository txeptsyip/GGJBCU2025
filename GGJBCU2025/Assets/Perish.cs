using UnityEngine;

public class Perish : MonoBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bubble"))
        {
            Destroy(gameObject);
        }
    }
}
