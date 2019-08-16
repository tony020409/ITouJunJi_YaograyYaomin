using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightControl : MonoBehaviour {

    public bool _Detector;
    public float _Counter;
    public float _CDTime;

    //public ParticleSystem[] _Fire;
    public Animator _Animator;

	void Awake () {
        _Counter = 0f;
        _CDTime = 1f;
    }

    void Start () {
        _Animator = GetComponent<Animator>();
    }

    void Update () {
        if (_Detector) {
            _Counter += Time.deltaTime;

            if (_Counter >= _CDTime) {
                //if (TeamManager._ins.actionControl == PhotonNetwork.playerName.Substring(0, 1) || PhotonNetwork.playerName.Substring(0, 1) == "S") {
                //    //ChangeColor (new Color(0, 255, 0, 255));
                //    _Animator.SetInteger("Control", 1);
                //    _Detector = false;
                //}
                _Counter = 0f;
            }
        }
    }

    //public void ChangeColor (Color _color) {
    //    for (int i = 0; i < _Fire.Length; i++) {
    //        var main = _Fire[i].main;
    //        main.startColor = _color;
    //    }
    //}
}
