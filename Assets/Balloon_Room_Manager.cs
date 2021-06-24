/*
 * London Bielicke
 * Embodiment Experiment
 *     Balloon Room
 */
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

/*BalloonRoomManager:
 * initially, spawn 3 balloons at spawnpos
 * whenever the user pops all the balloons,
 * respawn 3 more balloons for 3 minutes
 * balloons also pop once they hit the ground
 */
public class Balloon_Room_Manager : MonoBehaviour
{
    public Transform spawnPos; //spawn position of balloons
    public GameObject spawnee;  //prefab of balloons being spawned
    [SerializeField] private List<GameObject> balloons;  //keep track of balloons
    bool wait = false; //boolean value whether ready to drop balloons or not
    
    private void Awake()
    {
        //write data 
        string serializedData = "\n \nNew Log (" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ") \n";
        string fileName = Application.persistentDataPath + "/" + "testData.txt";
        StreamWriter writer = new StreamWriter(fileName, true);
        writer.Write(serializedData);
        writer.Close();
    }

    //update runs every frame
    private void Update()
    {

        if (!cap() && !wait)
        {
            wait = true;
            //create/spawn balloon and add to array
            int balloonsSpawned = Random.Range(1, 5);
            StartCoroutine(LoadBalloonsWithDelay(3f));
            spawn(balloonsSpawned);
        }

        //load end scene after 3 minutes
        StartCoroutine(LoadLevelWithDelay(180f));
    }


    private IEnumerator LoadBalloonsWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        wait = false;
    }


    /*spawn:
    * choose random amount of balloons to spawn
    * and random time between spawns
   */
    private void spawn(int n)
    {
        for (int i = 0; i < n; i++)
        {
            var clone = Instantiate(spawnee, randSpawnPos(), spawnPos.rotation);
            balloons.Add(clone);
        }

    }

    /*randSpawnPos:
     * generate a random spawn position 
     * which is reachable by the player :)
    */
    private Vector3 randSpawnPos()
    {
        float spawnPointX = Random.Range(-1f, 1f);
        float spawnPointZ = Random.Range(-1f, 1f);
        return new Vector3(spawnPos.position.x + spawnPointX, spawnPos.position.y, spawnPos.position.z + spawnPointZ);
    }

    /*cap:
     * if more than 10 balloons in room,
     * then max capacity
    */
    private bool cap()
    {
        Boolean capped = false; //has reached max balloon capacity
        if (activeBalloons() > 10)
        {
            capped = true;
        }

        return capped;

    }

    /*
     * IsEmpty():
     *     - return boolean value
     *         returns if the balloons list is completely empty
     *         returns false if balloons still exist in the scene
     */
    private int activeBalloons()
    {

        int numBalloons = 0;
        foreach (var t in balloons)
        {
            if (t != null)
            {
                numBalloons += 1;
            }
        }
        return numBalloons;
    }

    /*
     * IEnumerator which loads the throwing scene after a 3f delay 
     */
    private IEnumerator LoadLevelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("End");
    }

}
