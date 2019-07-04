using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : ClientNetwork
{
    public GameObject ThrowObject;
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
        return ThrowObject;
    }

    void initNextBall(){
        if(mainCamera.transform.childCount == 1){
            nextBall = Instantiate(ThrowObject);
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
            var thrForce = mainCamera.transform.forward * 20000 + new Vector3(0,0,1) * 28000;
            nextBall_rigitBody.useGravity = true;
            nextBall_rigitBody.AddForce(thrForce,ForceMode.Impulse);

            cn.SendBallData(thrPosition,thrForce);
            initBallWaitCounter += initBallWait_ms * 60;
        }

    }


    public void OnReceiveBallData()
    {
        Vector3 pos = new Vector3(0,0,0);
        Vector3 way = new Vector3(0,0,0);
        GameObject ThrowThing = Instantiate(ThrowObject);
        Rigidbody thR = ThrowThing.GetComponent<Rigidbody>();
        ThrowThing.transform.position = pos;
        nextBall_rigitBody.useGravity = true;
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
