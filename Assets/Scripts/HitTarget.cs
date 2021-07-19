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
    Vector3[] positions = new Vector3[] { new Vector3(0, .5f, 3), new Vector3(0, .5f, 4), new Vector3(0, .5f, 5), 
                                            new Vector3(0, .5f, 6), new Vector3(0, .5f, 7) };

    //ball rigid body
    Rigidbody rb;

    // fields for randomizing target spawning
    int[] randOrder;
    public int rounds = 3;
    Vector3 spawnPos;

    //index keeps track of targets spawned
    int index = 1;
    bool hit = false;

    public void Start()
    {
        // INIT rigidbody
        rb = this.GetComponent<Rigidbody>();

        // get randomized order
        Order order = new Order(positions.Length, 10);
        order.Randomize();
        randOrder = order.GetOrder();

        Instantiate(target, positions[randOrder[0]], target.transform.rotation);
        spawnPos = this.transform.position;
    }

    /*
     * throw is detected whether player
     * hits target or not. on collision 
     * with either a target or the gorund, 
     * reset the position of the ball, and
     * spawn the next target to hit 
     */
    IEnumerator OnTriggerEnter(Collider other)
    {
        // *** also if not already collided without being grabbed again ***
        if ((other.tag == "Target" || other.tag == "ground") && !hit)
        {
            hit = true;

            // debugging
            Debug.Log("index: " + index);
            Debug.Log(other.tag);

            yield return null;
            ResetGameObject();
            NextTarget();
            CheckEndOfScene();

            hit = false;
        }
    }

    // prevents ball from rolling and colliding more than once
    private void ResetGameObject()
    {
        rb.isKinematic = true;
        this.transform.position = spawnPos;
        rb.isKinematic = false;
    }

    // despawn and respawn in order
    private void NextTarget()
    {
        Destroy(GameObject.FindWithTag("Target"));
        if (index < randOrder.Length)
        {
            Instantiate(target, positions[randOrder[index]], target.transform.rotation);
            index++;
        }
    }

    // checks if user completed task
    private void CheckEndOfScene()
    {
        if (index >= randOrder.Length)
        {
            PersistentManager.Instance.SceneCompleted = true;
        }
    }
}
