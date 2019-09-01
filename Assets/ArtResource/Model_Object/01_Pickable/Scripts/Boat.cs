using UnityEngine;
using System.Collections;


/// <summary> ��������b�����}�B���{�� </summary>
public class Boat : MonoBehaviour
{

    float orgY = 0;

    /// <summary> �ưʳt�� </summary>
	public float waveSpeed = 1.7f;
    /// <summary> [�ư�] �W�U�첾�T�� </summary>
	public float moveDis = 0.1f;
    /// <summary> [�ư�] ���k�\�ʴT�� </summary>
    public float LeftRight = 1.0f;


    // Use this for initialization
    void Start() {
        orgY = transform.localPosition.y;
    }


    // Update is called once per frame
    void Update() {
        //�W�U����
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            orgY + moveDis * Mathf.Sin(Time.time * waveSpeed),
            transform.localPosition.z);

        //���k�\��
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y,
            LeftRight * Mathf.Sin(Time.time * (waveSpeed) + 1.5f));
    }
}
