using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentManager : MonoBehaviour
{
    // Singleton Class
    public static PersistentManager Instance { get; private set; }

    public bool SceneCompleted;
    public int currScene = 0;
    private float[] velocityOrder;
    public float velocityCoef;

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
            Debug.Log(velocityCoef);
            if (currScene == 4)
            {
                currScene = 0;
                SceneCompleted = false;
                velocityCoef = velocityOrder[currScene];
                Application.Quit();
            }

            SceneManager.LoadScene("Throwing Game");
            SceneCompleted = false;
            currScene++;
            velocityCoef = velocityOrder[currScene];
        }
    }
}
