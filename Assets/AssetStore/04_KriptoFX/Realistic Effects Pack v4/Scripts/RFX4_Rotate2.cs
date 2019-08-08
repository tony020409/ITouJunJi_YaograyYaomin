using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RFX4_Rotate2 : MonoBehaviour {


    public Vector3 RotateVector = Vector3.forward;
    private Transform t;

    // Use this for initialization
    void Start () {
        t = transform;
    }


     void Update() {
        t.Rotate(RotateVector * Time.deltaTime);
    }


}
