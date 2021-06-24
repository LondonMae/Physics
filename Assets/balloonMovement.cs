using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balloonMovement : MonoBehaviour
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        float dragChange = Random.Range(-2f, 2f);
        float scaleChange = Random.Range(-.1f, .1f);
        rb.drag = 9 + dragChange;
        transform.localScale += new Vector3(scaleChange, scaleChange, scaleChange);
    }

}
