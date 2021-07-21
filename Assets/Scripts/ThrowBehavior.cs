/* London Bielicke 
 * 6/28/2021
 * Throwing Experiment
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ThrowBehavior : MonoBehaviour
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

    // game objects and components
    Rigidbody rb;

    // Sets velocity coefficient
    public float velocityCoefficient = 1f;

    public void Start()
    {
        // INIT input devices 
        try
        {
            leftHand = detectDevices(leftHandCharacteristics);
            rightHand = detectDevices(rightHandCharacteristics);
        }
        catch (System.ArgumentOutOfRangeException e)
        {
            Debug.Log("Cannot detect controllers");
            Debug.LogError(e);
        }

        // get throwable rigidbody
        rb = this.GetComponent<Rigidbody>();

        // set velocity coefficient from persistent manager (take this out for re-use)
        velocityCoefficient = PersistentManager.Instance.GetVelocityCoef();
    }

    // called when object is picked up by controller (gets correct hand)
    public void OnPickup()
    {
        // haptics
        whichHand();
        currInput.SendHapticImpulse(0, 0.3f, .2f);

        // set prev position and rotation to current pos and rot
        prevPos = transform.position;
        prevRot = transform.eulerAngles;

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
    private void Velocity()
    {
        currPos = currController.transform.position;
        velocity = (currPos-prevPos)/Time.deltaTime;
        prevPos = currPos;
    }

    // tracks angular velocity
    private void AngularVelocity()
    {
        currRot = currController.transform.eulerAngles;
        angularVelocity = (currRot - prevRot) / Time.deltaTime;
        prevRot = currRot;
    }

    int frameStep = 0;
    Vector3[] velocityFrames = new Vector3[5];
    Vector3[] angularVelocityFrames = new Vector3[5];

    //keeps track of 5 most recent frames 
    private void VelocityUpdate()
    {
        // set current velocity frames
        velocityFrames[frameStep] = velocity;
        angularVelocityFrames[frameStep] = angularVelocity;

        // step to next fram
        frameStep++;
        if (frameStep >= velocityFrames.Length)
            frameStep = 0;
    }

    // get max velocity in last 5 frames (most likely what player intended)
    private Vector3 GetVelocityPeak(Vector3[] vectors)
    {
        Vector3 max = vectors[0];
        for (int i = 1; i < vectors.Length; i++)
        {
            if (vectors[i].magnitude > max.magnitude)
            {
                max = vectors[i];
            }
        }

        return max;
    }

    // get which hand the target is in
    private void whichHand()
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
    public void OnRelease()
    {
        // set ball velocity to controller velocity's peak
        rb.velocity = GetVelocityPeak(velocityFrames) * velocityCoefficient;
        rb.angularVelocity = GetVelocityPeak(angularVelocityFrames) * velocityCoefficient;

        // vibration haptics
        currInput.SendHapticImpulse(0, 0.3f, .2f);

        Reset();
    }

    // reset fields to re-calculate when trhown again
    private void Reset()
    {
        currController = null;
        frameStep = 0;
        velocityFrames = new Vector3[5];
        angularVelocityFrames = new Vector3[5];
    }

    // get controller
    private InputDevice detectDevices(InputDeviceCharacteristics handCharacteristics)
    {
        var controllers = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(handCharacteristics, controllers);
        return controllers[0];
    }

}
