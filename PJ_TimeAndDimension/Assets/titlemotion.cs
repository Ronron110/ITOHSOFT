using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titlemotion : MonoBehaviour
{

    Quaternion titleZero;
    private bool isRotate = false;


    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.SetActive(false);
        titleZero = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Time.timeScale = 0.05f;
            transform.rotation = Quaternion.Lerp(transform.rotation, titleZero, 0.1f);

        }
        else
        {
            Quaternion rot = Quaternion.AngleAxis(313f * Time.deltaTime, Vector3.up);
            Quaternion q = this.transform.rotation;
            this.transform.rotation = q * rot;
            rot = Quaternion.AngleAxis(211f * Time.deltaTime, Vector3.forward);
            q = this.transform.rotation;
            this.transform.rotation = q * rot;
            rot = Quaternion.AngleAxis(99f * Time.deltaTime, Vector3.right);
            // 現在の自信の回転の情報を取得する。

            q = this.transform.rotation;
            // 合成して、自身に設定
            this.transform.rotation = q * rot;
           
            //Time.timeScale = 100f;
            
        }
        if (isRotate != true)
        {
            this.gameObject.SetActive(true);
            isRotate = true;

        }
    }
}
