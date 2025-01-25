using UnityEngine;

public class Perish : MonoBehaviour
{
    public GameObject CharSpawner;
    public GameObject Char;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bubble"))
        {
            Char.transform.position = CharSpawner.transform.position;
        }
    }
}
