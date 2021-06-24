using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
/*
 * This is pretty self-explanatory.
 * The script is added to every balloon. When the claymore comes in contact with the balloon, the balloon is destroyed.
 */
public class PopBalloon : MonoBehaviour {
    InputDevice leftHand;
    InputDevice rightHand;
    InputDeviceCharacteristics leftHandCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice;
    InputDeviceCharacteristics rightHandCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice;

    private void Awake()
    {
        leftHand = detectDevices(leftHandCharacteristics);
        rightHand = detectDevices(rightHandCharacteristics);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("rightHand") || other.gameObject.CompareTag("ground") || other.gameObject.CompareTag("leftHand")) {
            Debug.Log("Balloon popped");
            writeData(other);

            if (other.gameObject.CompareTag("rightHand"))
            {
                rightHand.SendHapticImpulse(0, 0.3f, .2f);
            }
            else if (other.gameObject.CompareTag("leftHand"))
            {
                leftHand.SendHapticImpulse(0, 0.3f, .2f);
            }
                

            Destroy(gameObject);
        }
    }

    private void writeData(Collision collider)
    {
        double x = this.gameObject.transform.position.x;
        double y = this.gameObject.transform.position.y;
        double z = this.gameObject.transform.position.z;
        string serializedData = "";
        if (collider.gameObject.CompareTag("ground"))
        {
            serializedData = "(missed)";
        }
        serializedData += " " + x.ToString() + ", " +
                            y.ToString() + ", " +
                            z.ToString() + "\n";
        // Write to disk
        string fileName = Application.persistentDataPath + "/" + "testData.txt";
        StreamWriter writer = new StreamWriter(fileName, true);
        writer.Write(serializedData);
        writer.Close();
    }

    private InputDevice detectDevices(InputDeviceCharacteristics handCharacteristics)
    {
        var controllers = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(handCharacteristics, controllers);
        if (controllers.Count > 0)
        {
            print("Left hand found");
            return controllers[0];
        }
        else
        {
            return controllers[0];
        }
    }
}
