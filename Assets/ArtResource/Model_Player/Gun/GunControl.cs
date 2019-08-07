using System.Collections;
using UnityEngine;
namespace CYX
{




    public class GunControl : MonoBehaviour
    {

        // 搖晃類型
        public enum ShakeType
        {
            PositionShake,
            RotationShake,
            NoShake
        }

        public ShakeType st;

        public vp_MuzzleFlash mf;
        public AudioSource audioFire;
        public Animation anim;
        public Transform shellSpawnPoint;
        public GameObject shellPrefab;

        //public SteamVR_TrackedController lCtrler, rCtrler;

        // 槍cd計時器
        public float timer;

        // 後座力 之前槍的角度
        Vector3 s_pre_euler;
        // 後座力 之前槍的位置
        Vector3 s_pre_pos;

        public float gunSpeed = 0.1f;
        public float gun_end_force_r = 10f; // 槍後座力大小 （可以先調大些方便調試）
        public float gun_end_force_p = 0.53f; // 槍後座力大小 （可以先調大些方便調試）

        void Start()
        {
            st = ShakeType.RotationShake;

            s_pre_euler = transform.eulerAngles;
            s_pre_pos = transform.localPosition;
        }

        void Update()
        {

            timer += Time.deltaTime;
            if (Input.GetKey(KeyCode.A))
            {
                mf.Shoot();
                audioFire.Play();

                GameObject sp = Instantiate(shellPrefab, shellSpawnPoint.position, transform.parent.rotation) as GameObject;
                sp.GetComponent<Rigidbody>().AddForce(shellSpawnPoint.forward * 20);

                // 計時器清零
                timer = 0f;

                // 後座力
                // 角度抖動
                if (st == ShakeType.RotationShake)
                {
                    Quaternion yQuaternion2 = Quaternion.AngleAxis(gun_end_force_r, Vector3.left);

                    transform.localRotation *= yQuaternion2;
                }
                else if (st == ShakeType.PositionShake)
                {
                    Vector3 newPos = transform.localPosition;
                    newPos.y += gun_end_force_p;
                    newPos.z -= gun_end_force_p;
                    transform.localPosition = newPos;
                }
                else if (st == ShakeType.NoShake)
                {
                    anim.Play();
                }
            }
            recoverGun();

        }




            //    if (rCtrler.triggerPressed && timer >= gunSpeed)
            //{

            //    mf.Shoot();
            //    audioFire.Play();

            //    GameObject sp = Instantiate(shellPrefab, shellSpawnPoint.position, transform.parent.rotation) as GameObject;
            //    sp.GetComponent<Rigidbody>().AddForce(shellSpawnPoint.forward * 20);

            //    // 計時器清零
            //    timer = 0f;

            //    // 後座力
            //    // 角度抖動
            //    if (st == ShakeType.RotationShake)
            //    {
            //        Quaternion yQuaternion2 = Quaternion.AngleAxis(gun_end_force_r, Vector3.left);

            //        transform.localRotation *= yQuaternion2;
            //    }
            //    else if (st == ShakeType.PositionShake)
            //    {
            //        Vector3 newPos = transform.localPosition;
            //        newPos.y += gun_end_force_p;
            //        newPos.z -= gun_end_force_p;
            //        transform.localPosition = newPos;
            //    }
            //    else if (st == ShakeType.NoShake)
            //    {
            //        anim.Play();
            //    }
            //}
            //recoverGun();
     //   }

        // 恢復後座力以前的位置
        void recoverGun()
        {

            // 角度抖動
            if (st == ShakeType.RotationShake)
            {
                //s_pre_euler = rCtrler.transform.eulerAngles + new Vector3(45, 0, 0);
                Quaternion current_cam = Quaternion.Euler(transform.eulerAngles);
                Quaternion target_cam = Quaternion.Euler(s_pre_euler);
                transform.eulerAngles = Quaternion.Slerp(current_cam, target_cam, 10 * Time.deltaTime).eulerAngles;
            }
            else if (st == ShakeType.PositionShake)
            {
                transform.localPosition = Vector3.Slerp(transform.localPosition, s_pre_pos, 10 * Time.deltaTime);
            }
        }
    }
}