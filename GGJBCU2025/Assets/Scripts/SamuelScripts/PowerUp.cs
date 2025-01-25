
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private bool isTaken = false;
    private bool adjusted = false;

    private float percent;

    private Transform Holder;

    [SerializeField]
    private float Speed = 1;


    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().ActivatePowerUp();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player2"))
        {
            collision.gameObject.GetComponent<Player2Script>().ActivatePowerUp();
            Destroy(gameObject);
        }
    }

    public bool IsTaken()
    {
        return isTaken;
    }

    public void CannotBeAccessed()
    {
        Destroy(gameObject);
    }
}