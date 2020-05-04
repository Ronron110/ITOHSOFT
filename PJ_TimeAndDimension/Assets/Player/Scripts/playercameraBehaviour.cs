using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercameraBehaviour : MonoBehaviour
{
    GameObject campos;
    GameObject cameraTarget;
    GameObject cameraLenz;

    [SerializeField]
    private float cameraSpeed=0.25f;

    private Vector3 offsetPos;
 
    private RaycastHit hit;//Raycastの当たり判定で使う。
    private float maxDistance = 0.5f;//Rayの長さを設定
    private Vector3 cameraAway;//カメラが上に離れる行先
    private Camera cameraself;//カメラのレンズをコントロールするためのコンポーネント

    //private float sideRayLength=0.1f;//横のRayの距離。最終的には使っていない。

   

    void Start()
    {
        campos = GameObject.Find("cameraPos");
        cameraTarget = GameObject.Find("cameraTarget");
        cameraLenz = GameObject.Find("Main Camera");
        cameraself = cameraLenz.GetComponent<Camera>();

    }
    private void Update()
    {
        cameraSpeed = 0.5f;
        
        //カメラが右の壁に0.3f以上寄りすぎたら
        //Raycastを右に0.3f出して検証
        /*if (Physics.Raycast(transform.position, transform.rotation * Vector3.right, out hit, sideRayLength))
        {
            //画角を狭くして調整してみたがダメ
            //cameraself.fieldOfView = Mathf.Lerp(cameraself.fieldOfView, 20, 0.5f);
            //カメラを上に逃がす処理
            cameraAway = new Vector3(transform.position.x - 0.1f, transform.position.y + 0.1f, transform.position.z);
            transform.position = Vector3.Lerp(cameraAway, transform.position, cameraSpeed);

        }
        //カメラが左の壁に0.3f以上寄りすぎたらカメラを上に逃がす処理
        //Raycastを左に0.3f出して検証
        if (Physics.Raycast(transform.position, transform.rotation * Vector3.left, out hit, sideRayLength))
        {
            //画角を狭くして調整してみたがダメ
            //cameraself.fieldOfView = Mathf.Lerp(cameraself.fieldOfView, hit.distance, 0.01f);
            //カメラを上に逃がす処理
            cameraAway = new Vector3(transform.position.x + 0.1f, transform.position.y + 0.1f, transform.position.z);
            transform.position = Vector3.Lerp(cameraAway,transform.position,cameraSpeed);

        }

        と色々やったけど結局下記の処理だけで一番いい結果がでた。まだまだ改善の余地あり。
        */



        //RaycastはRaycastHit型のメンバーに当たった場所を返すhit.pointやらRayの当たってるところまでの距離を返すhit.distanceなど色々使えるものがある。

        //プレイヤー（カメラポジション）の真後ろ後ろにRayを射出してカメラの当たり判定を行う
        if (Physics.Raycast(cameraTarget.transform.position, cameraTarget.transform.rotation*Vector3.back,out hit,maxDistance))
        {

           //横にくっつきそうになったらCameraをhitPointの位置で、ちょっと上に逃がす（当たった距離だけカメラを上に離していく感じ。
            cameraAway = new Vector3(hit.point.x , hit.point.y + 0.3f, hit.point.z);
            transform.position = Vector3.Lerp(transform.position, cameraAway, 1f);

            //Debug.Log(hit.distance+" "+ hit.point+" "+cameraAway);


        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, campos.transform.position, cameraSpeed);

            //画角を戻す
            //cameraself.fieldOfView = Mathf.Lerp(cameraself.fieldOfView, 60, 0.1f);
            //カメラのY軸を戻す


        }
        //Rayを横に出しているときに使った処理
        //Debug.DrawRay(cameraTarget.transform.position, cameraTarget.transform.rotation * Vector3.left * sideRayLength, Color.red, 0.1f, false);
        //Debug.DrawRay(cameraTarget.transform.position, cameraTarget.transform.rotation * Vector3.right * sideRayLength, Color.red, 0.1f, false);

        //Debug.DrawRay(cameraTarget.transform.position, cameraTarget.transform.rotation*Vector3.back*hit.distance, Color.red, 0.1f, false);


         
        //transform.rotation = cameraTarget.transform.rotation;

        //レンズが常にプレイヤーを見つめる処理
        cameraLenz.transform.LookAt(cameraTarget.transform.position);

    }



}
