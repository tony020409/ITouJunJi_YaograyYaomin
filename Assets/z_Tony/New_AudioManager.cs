using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_AudioManager : MonoBehaviour {




    public AudioSource AudioSource;



    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void PlayAudioClip(string _AudioName)
    {
        AudioClip _AudioClip = null;
        _AudioClip = Resources.Load<AudioClip>("audio/" +_AudioName);

        if (_AudioClip == null)
        {
            Debug.LogWarning("找無音效:" + _AudioName);
            return;
        }
        AudioSource.PlayOneShot(_AudioClip,1);


    }








}
