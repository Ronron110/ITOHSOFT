using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class infoBehaviour : MonoBehaviour
{
    private Text myText;    // Start is called before the first frame update
    void Start()
    {

        GameObject obj = GameObject.Find("shadow");

        myText = GetComponentInChildren<Text>();//UIのテキストの取得の仕方

    }

    // Update is called once per frame
    void Update()
    {
        myText.text = Time.fixedTime.ToString();
    }
}
