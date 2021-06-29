/* London Bielicke 
 * 6/28/2021
 * Throwing Experiment
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // canvas components to mutate
    public GameObject startButton;
    public GameObject textBox;
    Vector3 initBallPos;
    UnityEngine.UI.Text textUI;

    // components for instructions
    public float wait = 10.0f;
    float time;
    public string[] textValues = new string[] { "Your task is to throw the ball at the upcoming targets", "Once you hit a target, a new target will appear", "if you need a new ball, click the button in the corner", "the game will end automatically once it is over", "begin whenever you are ready" };
    int index = 1;
    bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        textUI = textBox.GetComponent<UnityEngine.UI.Text>();
        Time.timeScale = 0;
        time = wait;
        initBallPos = ball.transform.position;
    }

    // ball that is thrown
    public GameObject ball;
    // reset ball
    public void NewBall()
    {
        ball.transform.position = initBallPos;
    }

    // Ready: begin scene and instructions
    public void Ready()
    {
        Time.timeScale = 1;
        Destroy(startButton);
        pressed = true;
        textUI.text = "Your task is to throw the ball at the upcoming targets";
    }


    // Update is called once per frame
    void Update()
    {
        //if button was pressed, rotate through instructions
        // destroy textbox once reaching the final instruction
        if (pressed)
        {
            if (time <= 0 && index < textValues.Length)
            {
                textUI.text = textValues[index];
                time = wait;
                index++;
            }
            else if (time <= 0 && index >= textValues.Length)
            {
                Destroy(textBox);
                pressed = false;
            }
            time -= Time.deltaTime;
        }
    }

}
