using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    public GameObject MyThrowObject;
   public GameObject OtherThrowObject; 
    public GameObject NetworkController;
    public Camera mainCamera;
    GameObject nextBall;
    Rigidbody nextBall_rigitBody;
    public int initBallWait_ms = 0;
    int initBallWaitCounter = 0;
    ClientNetwork cn;
    
    // Start is called before the first frame update
    void Start()
    {
        cn = NetworkController.GetComponent<ClientNetwork>();
        initNextBall();
    }

    public GameObject getBall(){
        return MyThrowObject;
    }

    void initNextBall(){
        if(mainCamera.transform.childCount == 1){
            nextBall = Instantiate(MyThrowObject);
            nextBall_rigitBody = nextBall.GetComponent<Rigidbody>();
            nextBall.transform.parent = mainCamera.transform;
            nextBall.transform.position = mainCamera.transform.position +  (mainCamera.transform.forward * 300 +  new Vector3(0,0,-1) * 70);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("initBallWaitCounter = " + initBallWaitCounter);
       if(initBallWaitCounter != 0){
           initBallWaitCounter--;
       }else if(initBallWaitCounter == 0){
           initNextBall();
       }
    }

    public void ThrowThing(){
        if(mainCamera.transform.childCount == 2){
            nextBall.transform.parent = null;
            var thrPosition = nextBall.transform.position;
            var thrForce = mainCamera.transform.forward * 20000 * 0.7f + new Vector3(0, 0, 1) * 28000 * 0.9f;
            nextBall_rigitBody.useGravity = true;
            nextBall_rigitBody.AddForce(thrForce,ForceMode.Impulse);

            cn.SendBallData(thrPosition,thrForce);
            initBallWaitCounter += initBallWait_ms * 60;
        }

    }


    public void throwRecevedBall(Vector3 pos, Vector3 way)
    {
        GameObject ThrowThing = Instantiate(OtherThrowObject);
        Rigidbody thR = ThrowThing.GetComponent<Rigidbody>();
        ThrowThing.transform.position = pos;
        thR.useGravity = true;
        thR.AddForce(way,ForceMode.Impulse);
    } 
}

// class cn : ClientNetwork{
//     public GameObject ThrowBall;
//     protected override void OnReceiveBallData(Vector3 pos, Vector3 way)
//     {
//         GameObject ThrowThing = Instantiate(ThrowBall);
//         Rigidbody thR = ThrowThing.GetComponent<Rigidbody>();
//         ThrowThing.transform.position = pos;
//         thR.useGravity = true;
//         thR.AddForce(way,ForceMode.Impulse);
//     }
// }
