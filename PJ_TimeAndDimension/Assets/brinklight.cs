using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brinklight : MonoBehaviour
{

    Light BrokenLight;
    double accumulatedDeltaTime;// デルタタイムをたしこむためのもの
    // Start is called before the first frame update
    void Start()
    {
        BrokenLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {

         //ーーーーーーーー  次回ここから
        //スローになったときにブリンクの中が100分1なので、100秒ごとに乱数を発生。

        if ((Time.unscaledTime-lastUpdateTime)*Time.timeScale>  )
        if (Random.Range(1, 10) == 1)
        {
            BrokenLight.intensity = Random.Range(1, 10);
        }
        else
        {
            BrokenLight.intensity = 0;
        }
    }
}
