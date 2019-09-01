using UnityEngine;
using System.Collections;


/// <summary> 模擬物體在水面漂浮的程式 </summary>
public class Boat : MonoBehaviour
{

    float orgY = 0;

    /// <summary> 飄動速度 </summary>
	public float waveSpeed = 1.7f;
    /// <summary> [飄動] 上下位移幅度 </summary>
	public float moveDis = 0.1f;
    /// <summary> [飄動] 左右擺動幅度 </summary>
    public float LeftRight = 1.0f;


    // Use this for initialization
    void Start() {
        orgY = transform.localPosition.y;
    }


    // Update is called once per frame
    void Update() {
        //上下移動
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            orgY + moveDis * Mathf.Sin(Time.time * waveSpeed),
            transform.localPosition.z);

        //左右擺動
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y,
            LeftRight * Mathf.Sin(Time.time * (waveSpeed) + 1.5f));
    }
}
