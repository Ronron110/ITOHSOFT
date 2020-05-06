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
        animSpeedText = GameObject.Find("animSpeedText");//アニメーションスピード（プレイヤーの動き)
        slowTimeText = GameObject.Find("SlowTimeRemain");//スローモーションの残り時間
        playerHPText = GameObject.Find("playerHP");//プレイヤーの体力

        //テキストコンポーネントの取得
        animSpeed = animSpeedText.GetComponent<Text>();
        slowTime = slowTimeText.GetComponent<Text>();
        playerHP = playerHPText.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        //それぞれの状態をUIに表示
        animSpeed.text = "amin.speed:"+ playerObj.GetComponent<masamoveBehaviour>().anim.speed.ToString();
        slowTime.text = "SlowTime:" + playerObj.GetComponent<masamoveBehaviour>().slowTimeRemain.ToString();
        playerHP.text = "HP:" + playerObj.GetComponent<masamoveBehaviour>().PlayerHP.ToString();

    }

}
