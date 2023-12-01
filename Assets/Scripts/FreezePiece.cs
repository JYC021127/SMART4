using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePiece : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component of the object
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision occurs with the object you want to freeze
        if (collision.gameObject.CompareTag("Piece") || collision.gameObject.CompareTag("PButton"))
        {
            // Freeze the Y position after collision
            Freeze();
        }
    }

    void Freeze()
    {
        // Freeze the Y position by modifying the Rigidbody constraints
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY 
                | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }
}
