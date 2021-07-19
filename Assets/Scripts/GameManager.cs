/* London Bielicke 
 * 6/28/2021
 * Throwing Experiment
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // canvas components to mutate
    public GameObject startButton;
    public GameObject textBox;
    public GameObject initCanvas;
    public GameObject ball;

    Vector3 initBallPos;
    Text textUI;

    // components for instructions
    public float wait = 10.0f;
    float time;
    public string[] textValues = new string[] { "Your task is to throw the ball at the upcoming targets", "Once you hit a target, a new target will appear", "if you need a new ball, click the button in the corner", "the game will end automatically once it is over", "begin whenever you are ready" };
    int index = 1;
    bool pressed = false;
    private int currScene;

    // Start is called before the first frame update
    void Start()
    {
        // access which set player is on
        currScene = PersistentManager.Instance.currScene;

        // get textbox for instructions
        textUI = textBox.GetComponent<Text>();
        textUI.text = "Are you ready to begin round " + (currScene + 1) + "?";

        // game not playable during instructions
        Time.timeScale = 0;
        time = wait;

        // pos for respawn
        initBallPos = ball.transform.position;
    }

    // reset ball
    public void NewBall()
    {
        ball.transform.position = initBallPos;
    }

    // Ready: begin scene and instructions
    public void Ready()
    {
        if (currScene > 0)
        {
            Time.timeScale = 1;
            Destroy(initCanvas);
            pressed = false;
        }
        else
        {
            pressed = true;
            textUI.text = textValues[0];

            Time.timeScale = 1;
            Destroy(startButton);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (pressed)
        {
            RotateInstructions();
        }
    }

    /* rotate through instructions.
     * destroy textbox once reaching 
     * the final instruction
     */
    public void RotateInstructions()
    {
        bool instructionsDone = (time <= 0 && index >= textValues.Length);
        bool ballGrabbed = (ball.transform.position.z != initBallPos.z);
        if (time <= 0 && index < textValues.Length)
        {
            textUI.text = textValues[index];
            time = wait;
            index++;
        }
        else if (instructionsDone || ballGrabbed)
        {
            Destroy(initCanvas);
            pressed = false;
        }
        time -= Time.deltaTime;
    }

}
