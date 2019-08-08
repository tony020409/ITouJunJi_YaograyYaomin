
using System.Collections;
using UnityEngine;

public class KillTime : MonoBehaviour {


	public float KillDelayTime = 1;


    private void Awake () {
        if (!this.enabled) {
            return;
        }

        Destroy(gameObject, KillDelayTime);
	}

    private void Start(){
    }
    


}