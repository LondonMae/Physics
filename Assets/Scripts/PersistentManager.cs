using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentManager : MonoBehaviour
{
    // Singleton Class
    public static PersistentManager Instance { get; private set; }

    public bool SceneCompleted;
    private int currScene = 0;
    private float[] velocityOrder;
    private float velocityCoef;

    public float GetVelocityCoef()
    {
        return velocityCoef;
    }

    public int GetCurrScene()
    {
        return currScene;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        velocityOrder = GetVelocityOrder();
        velocityCoef = velocityOrder[0];
    }

    // get random order of blocks 
    private float[] GetVelocityOrder()
    {
        float probability = Random.value;
        if (probability <= .5f)
        {
            return new float[] { 1f, .8f, 1f, 1.25f, 1f };
        }
        else
        {
            return new float[] { 1f, 1.25f, 1f, .8f, 1f };
        }
    }

    void Update()
    {
        if (SceneCompleted)
        {
            // if last scene completed, end app
            if (currScene == 4)
            {
                Application.Quit();
            }

            LoadNewBlock();
            
        }
    }

    private void LoadNewBlock()
    {
        SceneManager.LoadScene("Throwing Game");
        SceneCompleted = false;
        currScene++;
        velocityCoef = velocityOrder[currScene];
    }
}
