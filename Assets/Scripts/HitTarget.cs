/* London Bielicke 
 * 6/28/2021
 * Throwing Experiment
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    // instantiate array
    private int[] order;

    // constructor 
    public Order(int n, int mult)
    {
        order = new int[n * mult];
        InitOrder(n); 
    }

    // init order of numbers 0-n repeated m times
    void InitOrder(int n)
    {
        for (int i = 0; i < order.Length; i++)
        {
            order[i] = (i % n);
        }
    }

    // Randomized initialized array
    public void Randomize()
    {
        for (int i = order.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = order[i];
            order[i] = order[j];
            order[j] = temp;
        }
    }

    // return order array
    public int[] GetOrder()
    {
        return order;
    } 
}

public class HitTarget : MonoBehaviour
{
    // target prefab and possible positions
    public GameObject target; 
    Vector3[] positions = new Vector3[] { new Vector3(0, .6f, 1), new Vector3(0, .6f, 3), new Vector3(0, .6f, 5), new Vector3(0, .6f, 8) };

    //ball rigid body
    Rigidbody rb;

    // fields for randomizing target spawning
    int[] orderList;
    int rounds = 3;
    int index = 0;
    bool openTarget = false;

    public void Start()
    {
        // INIT rigidbody
        rb = this.GetComponent<Rigidbody>();

        // get randomized order
        Order order = new Order(positions.Length, rounds);
        order.Randomize();
        orderList = order.GetOrder();
    } 

    /*
     * if hits target, allow for continued play
     * upon hitting anything set to kinematic 
     * and then return back so that the ball
     * can move when thrown but does not roll into 
     * the targets upon hitting the ground
     */
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            rb.isKinematic = true;
            Destroy(collision.gameObject);
            openTarget = false;
            rb.isKinematic = false;
        }
        if (collision.gameObject.tag == "ground")
        {
            rb.isKinematic = true;
            rb.isKinematic = false;
        }
    }

    public void Update()
    {
        // if game still running, spawn new target
        if (!openTarget && index < orderList.Length)
        {
            Instantiate(target, positions[orderList[index]], target.transform.rotation);
            index++;
            openTarget = true;
        }
        //if game is over, quit game
        if (index >= orderList.Length && !openTarget)
        {
            Application.Quit();
        }
    }

    

}
