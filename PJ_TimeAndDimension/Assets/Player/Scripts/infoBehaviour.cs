using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class infoBehaviour : MonoBehaviour
{
    private GameObject playerObj;
    private GameObject animSpeedText;
    private GameObject slowTimeText;
    private GameObject playerHPText;



    private Text animSpeed;   
    private Text slowTime;    
    private Text playerHP;    

    void Start()
    {

        playerObj = GameObject.Find("Player");
        animSpeedText = GameObject.Find("animSpeedText");
        slowTimeText = GameObject.Find("SlowTimeRemain");
        playerHPText = GameObject.Find("playerHP");


        animSpeed = animSpeedText.GetComponent<Text>();//UIのテキストの取得の仕方
        slowTime = slowTimeText.GetComponent<Text>();//UIのテキストの取得の仕方
        playerHP = playerHPText.GetComponent<Text>();//UIのテキストの取得の仕方

    }

    // Update is called once per frame
    void Update()
    {
        animSpeed.text = "amin.speed:"+ playerObj.GetComponent<masamoveBehaviour>().anim.speed.ToString();
        slowTime.text = "SlowTime:" + playerObj.GetComponent<masamoveBehaviour>().slowTimeRemain.ToString();
        playerHP.text = "HP:" + playerObj.GetComponent<masamoveBehaviour>().PlayerHP.ToString();

    }

}
