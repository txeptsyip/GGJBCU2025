
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

    public bool IsTaken()
    {
        return isTaken;
    }

    public void CannotBeAccessed()
    {
        Destroy(gameObject);
    }
}