using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fanBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject playerObj;
    
    void Start()
    {
       playerObj = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {

        Quaternion rot = Quaternion.AngleAxis(5 * Time.timeScale, Vector3.forward);
        // 現在の自信の回転の情報を取得する。
        Quaternion q = this.transform.rotation;
        // 合成して、自身に設定
        this.transform.rotation = q * rot;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Player")
        {
            //ファンに当たったらダメージを300食らわせる
            playerObj.GetComponent<masamoveBehaviour>().Damage += 300.0f;
        }
    }
}
