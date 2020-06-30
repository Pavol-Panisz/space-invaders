using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float angularVelocity = 90f;
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = angularVelocity;
    }
}
