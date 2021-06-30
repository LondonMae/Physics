/* London Bielicke 
 * 6/28/2021
 * Throwing Experiment
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTarget : MonoBehaviour
{
    // target prefab and possible positions
    public GameObject target; 
    Vector3[] positions = new Vector3[] { new Vector3(0, .6f, 1), new Vector3(0, .6f, 3), new Vector3(0, .6f, 5), new Vector3(0, .6f, 8) };

    //ball rigid body
    Rigidbody rb;

    // fields for randomizing target spawning
    int[] randOrder;
    public int rounds = 3;
    bool openTarget = false;

    public void Start()
    {
        // INIT rigidbody
        rb = this.GetComponent<Rigidbody>();

        // get randomized order
        Order order = new Order(positions.Length, rounds);
        order.Randomize();
        randOrder = order.GetOrder();
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
            rb.isKinematic = false;

            // allows for new target to spawn 
            Destroy(collision.gameObject);
            openTarget = false;
        }
        if (collision.gameObject.tag == "ground")
        {
            rb.isKinematic = true;
            rb.isKinematic = false;
        }
    }

    //index keeps track of targets spawned
    int index = 0;
    public void Update()
    {
        if (readyForNewTarget())
        {
            //spawn new target
            Instantiate(target, positions[randOrder[index]], target.transform.rotation);
            index++;
            openTarget = true;
        }
        if (gameIsOver())
        {
            Application.Quit();
        }
    }

    // if no target currently spawn AND more target to spawn
    private bool readyForNewTarget()
    {
        return !openTarget && index < randOrder.Length;
    }

    // if all targets already spawned and last target was hit
    private bool gameIsOver()
    {
        return index >= randOrder.Length && !openTarget;
    }

}
