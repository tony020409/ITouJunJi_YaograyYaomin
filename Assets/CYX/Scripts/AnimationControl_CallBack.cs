using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RootMotion.FinalIK;

public class AnimationControl_CallBack : MonoBehaviour {

    public Animator[] _Childs;
    public GameObject _GameObject;
    public ParticleSystem _Particle;

    public GameObject[] _GamebjectStage06;
    public GameObject[] _GamebjectStage07;
    public GameObject[] _GamebjectStage08;

    public void Animation_CallBack (string type) {
        if (type == "Animator_Integer_0") {
            DoAnimator_Integer_0();
        } else if (type == "Animator_Integer_1") {
            DoAnimator_Integer_1();
        } else if (type == "Animator_Integer_2") {
            DoAnimator_Integer_2();
        } else if (type == "Gameplay01_SearchTime") {
            DoGameplay01_SearchTime();
        } else if (type == "PlayerJump") {
            DoPlayerJump();
        } else if (type == "ParticlePlay") {
            DoParticlePlay();
        } else if (type == "LookAt") {
            DoLookAt();
        } else if (type == "ActiveFalse") {
            DoActiveFalse();
        } else if (type == "Stage07_1") {
            DoStage07_1();
        } else if (type == "Stage07_2") {
            DoStage07_2();
        } else if (type == "Stage07_3") {
            DoStage07_3();
        } else if (type == "Stage07_4") {
            DoStage07_4();
        } else if (type == "Stage07_5") {
            DoStage07_5();
        } else if (type == "Stage08_1") {
            DoStage08_1();
        } else if (type == "Stage08_2") {
            DoStage08_2();
        } else if (type == "Stage09_1") {
            DoStage09_1();
        }
    }

    private void DoAnimator_Integer_0 () {
        transform.GetComponent<Animator>().SetInteger("Control", 0);
    }

    private void DoAnimator_Integer_1 () {
        transform.GetComponent<Animator>().SetInteger("Control", 1);
    }

    private void DoAnimator_Integer_2 () {
        transform.GetComponent<Animator>().SetInteger("Control", 2);
    }

    private void DoGameplay01_SearchTime () {
        //Gameplay01Manager._ins.SearchTime();
    }

    private void DoPlayerJump () {
        //Launcher._ins.PlayerJump(2f);
    }

    private void DoParticlePlay () {
        //Debug.LogWarning("test");
        _Particle.Play();
    }

    private void DoLookAt () {
        //if (GetComponent<AimController>() != null) {
        //    GetComponent<AimController>().weight = 1f;
        //}
    }

    private void DoActiveFalse () {
        this.gameObject.SetActive(false);
    }

    private void DoStage07_1 () {
        _GamebjectStage07[0].SetActive(true);
    }

    private void DoStage07_2 () {
        _GamebjectStage07[1].SetActive(true);
    }

    private void DoStage07_3 () {
        _GamebjectStage07[2].SetActive(true);
    }

    private void DoStage07_4 () {
        _GamebjectStage07[3].SetActive(true);
    }

    private void DoStage07_5 () {
        _GamebjectStage07[4].SetActive(true);
    }

    private void DoStage08_1 () {
        _GamebjectStage08[0].transform.DOShakePosition(0.5f, 0.1f);
        _GamebjectStage08[1].transform.DOShakePosition(0.5f, 0.1f);
        _GamebjectStage08[2].transform.DOShakePosition(0.5f, 0.1f);
        _GamebjectStage08[3].transform.DOShakePosition(0.5f, 0.1f);
    }

    private void DoStage08_2 () {
        _GamebjectStage08[0].transform.DOShakePosition(0.5f, 0.1f);
        _GamebjectStage08[1].transform.DOLocalJump(new Vector3(0.045f, 2.733f, -0.255f), 1f, 1, 0.5f);
        _GamebjectStage08[2].transform.DOLocalJump(new Vector3(0.119f, 2.462f, 0.507f), 1f, 1, 0.5f);
        _GamebjectStage08[3].transform.DOLocalJump(new Vector3(2.169f, 2.453f, -2.368f), 1f, 1, 0.5f);

        //Launcher._ins._rig.transform.DOJump(new Vector3(-2f, 0f, -3.5f), 1f, 1, 0.5f);
    }

    private void DoStage09_1 () {
        //TeamManager._ins.GameOver();
    }
}
