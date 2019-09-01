using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class tree_lightevent : MonoBehaviour {

    public Transform camerarig;
    public float a;
    public float b;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void light_open()
    {
        camerarig.DOShakePosition(a, b);
    }
}
