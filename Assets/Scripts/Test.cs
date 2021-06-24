using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Test : MonoBehaviour
{
    // instantiate controller vars
    InputDevice leftHand;
    InputDevice rightHand;
    InputDeviceCharacteristics leftHandCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice;
    InputDeviceCharacteristics rightHandCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice;
    public XRBaseInteractor rHand;
    public XRBaseInteractor lHand;
    
    // instantiate controller fields for hand that object is currently in
    InputDevice currInput;
    private XRBaseInteractor currController;

    // fields to calculate controller velocity
    Vector3 prevPos;
    Vector3 currPos;
    Vector3 velocity;

    // fields to calculate controller angular velocity
    Vector3 prevRot;
    Vector3 currRot;
    Vector3 angularVelocity;


    public void Start()
    {
        // INIT input devices 
        leftHand = detectDevices(leftHandCharacteristics);
        rightHand = detectDevices(rightHandCharacteristics);
    }

    // called when onject is picked up by controller (gets correct hand)
    public void Up()
    {
        prevPos = transform.position;
        prevRot = transform.eulerAngles;
        whichHand();
        currInput.SendHapticImpulse(0, 0.3f, .2f);
    }

    // only calculate velocity if ball is in hand 
    public void FixedUpdate()
    {
        if (currController != null)
        {
            Velocity();
            AngularVelocity();
            VelocityUpdate();
        }
    }

    // tracks velocity
    public void Velocity()
    {
        currPos = currController.transform.position;
        velocity = (currPos-prevPos)/Time.deltaTime;
        prevPos = currPos;
    }

    // tracks angular velocity
    public void AngularVelocity()
    {
        currRot = currController.transform.eulerAngles;
        angularVelocity = (currRot - prevRot) / Time.deltaTime;
        prevRot = currRot;
    }

    int frameStep = 0;
    Vector3[] velocityFrames = new Vector3[5];
    Vector3[] angularVelocityFrames = new Vector3[5];

    //keeps track of 5 most recent frames 
    public void VelocityUpdate()
    {
        if (velocityFrames != null)
        {
            frameStep++;
        }

        if (frameStep >= velocityFrames.Length)
        {
            frameStep = 0;
        }

        velocityFrames[frameStep] = velocity;
        angularVelocityFrames[frameStep] = angularVelocity;
    }

    // get max velocity in last 5 frames (most likely what player intended)
    public Vector3 GetVelocityPeak(Vector3[] vectors)
    {
        Vector3 max = vectors[0];
        for (int i = 1; i < vectors.Length; i++)
        {
            if (vectors[i].magnitude > sum.magnitude)
            {
                max = vectors[i];
            }
        }

        return max;
    }

    // get which hand the target is in
    public void whichHand()
    {
        if (rHand.selectTarget != null)
        {
            currInput = rightHand;
            currController = rHand;
        }
        else
        {
            currInput = leftHand;
            currController = lHand;
        }
    }

    // called when object is dropped
    public void Testing()
    {
        GetComponent<Rigidbody>().velocity = GetVelocityPeak(velocityFrames);
        GetComponent<Rigidbody>().angularVelocity = GetVelocityPeak(angularVelocityFrames);
        currController = null;
        frameStep = 0;
        velocityFrames = new Vector3[5];
        angularVelocityFrames = new Vector3[5];
    }

    // get controllers;
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
