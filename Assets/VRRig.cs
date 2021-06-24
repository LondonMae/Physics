using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// vars exposed in Inspector AND the value its set to will be saved in the scene file.
[System.Serializable]
public class VRMap
{
    public Transform vrTarget; 
    public Transform rigTarget; 
    public Vector3 trackingPositionOffset; 
    public Vector3 trackingRotationOffset; 

    public void Map()
    {
        //set rig to vr position dispolaced by offset
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        //set rig rotation to vr rotation times rotation offset
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class VRRig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;
    public float turnSmoothness;

    public Transform headConstraint;
    public Vector3 headBodyOffset;
    // Start is called before the first frame update
    void Start()
    {
        //headBody Offset is difference in rig and headconstraint
        headBodyOffset = transform.position - headConstraint.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //maintain headbodyOffset
        transform.position = headConstraint.position + headBodyOffset;
        // - lerp: eases transition between 2 args
        // - transform.forward = pos on Z axis considering rotation
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized,Time.deltaTime * turnSmoothness);

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}
