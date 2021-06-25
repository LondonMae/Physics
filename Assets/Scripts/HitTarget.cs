using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTarget : MonoBehaviour
{
    Rigidbody rb;
    public void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    } 
   void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            rb.isKinematic = true;
        }
    }
}
