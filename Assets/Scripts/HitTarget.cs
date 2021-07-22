/* London Bielicke 
 * 6/28/2021
 * Throwing Experiment
 */
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HitTarget : MonoBehaviour
{
    // target prefab and possible positions
    public GameObject target; 
    public Vector3[] positions = new Vector3[] { new Vector3(0, .5f, 3), new Vector3(0, .5f, 4), new Vector3(0, .5f, 5)};

    //ball rigid body
    Rigidbody rb;

    // fields for randomizing target spawning
    int[] randOrder;
    public int rounds = 10;
    Vector3 spawnPos;

    //index keeps track of targets spawned
    int index = 0;
    bool hit = false;

    // for data collection
    StreamWriter writer;

    public void Start()
    {
        // INIT rigidbody
        rb = this.GetComponent<Rigidbody>();

        // get randomized order
        Order order = new Order(positions.Length, rounds);
        order.Randomize();
        randOrder = order.GetOrder();

        Instantiate(target, positions[randOrder[index]], target.transform.rotation);
        spawnPos = this.transform.position;

        // string fileName = Application.persistentDataPath + "/" + "throwingData.csv";
        string fileName = Application.persistentDataPath + "/" + "throwingData.csv";
        writer = new StreamWriter(fileName, true);
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

            yield return null;

            WriteData();
            ResetGameObject();
            NextTarget();
            CheckEndOfScene();

            hit = false;
        }
    }

    /*
     Example trial data:
         targetPosition, ballPosition, displacement, whichScene, velocityCoefficient, throw number
         6f,             5.7f,         .3f,          0,          1,                   30
    */
    private void WriteData()
    {
        Vector3 targetPos = GameObject.FindWithTag("Target").transform.position;
        Vector3 ballPos = this.transform.position;
        targetPos.y = 0;
        ballPos.y = 0;

        // Log in console
        Debug.Log(targetPos);
        Debug.Log(ballPos);

        var vectorToTarget = ballPos - targetPos;
        var distanceToTarget = vectorToTarget.magnitude;

        int currBlock = PersistentManager.Instance.GetCurrScene();
        float velocityCoefficient = PersistentManager.Instance.GetVelocityCoef();

        // Log in file
        string serializedData = targetPos + "," +
                                ballPos + "," +
                                distanceToTarget + "," + 
                                currBlock + "," + 
                                velocityCoefficient + "," +
                                (index+1) + "\n";
        writer.Write(serializedData);
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
        index++;
        if (index < randOrder.Length)
        {
            Destroy(GameObject.FindWithTag("Target"));
            Debug.Log(index);
            Instantiate(target, positions[randOrder[index]], target.transform.rotation);
        }
    }

    // checks if user completed task
    private void CheckEndOfScene()
    {
        if (index >= randOrder.Length)
        {
            PersistentManager.Instance.SceneCompleted = true;
            writer.Close();
        }
    }
}
