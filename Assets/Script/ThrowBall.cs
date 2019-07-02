using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : ClientNetwork
{
    public GameObject ThrowObject;
    public GameObject NetworkController;
    GameObject nextBall;
    Rigidbody nextBall_rigitBody;
    ClientNetwork cn;
    Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        cn = NetworkController.GetComponent<ClientNetwork>();

        initNextBall();

    }

    void initNextBall(){
        nextBall = Instantiate(ThrowObject);
        nextBall_rigitBody = nextBall.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       mainCamera = Camera.main;
       nextBall.transform.position = mainCamera.transform.position + (mainCamera.transform.forward  +  new Vector3(0,0,-1) * 200);
    }

    public void ThrowThing(){
        var thrPosition = nextBall.transform.position;
        var thrForce = mainCamera.transform.forward * 10000 + new Vector3(0,0,1) * 30000;
        nextBall_rigitBody.useGravity = true;
        nextBall_rigitBody.AddForce(thrForce,ForceMode.Impulse);

        //cn.SendBallData(thrPosition,thrForce);
        initNextBall();
    }

    protected override void OnReceiveBallData(Vector3 pos, Vector3 way)
    {
        GameObject ThrowThing = Instantiate(ThrowObject);
        Rigidbody thR = ThrowThing.GetComponent<Rigidbody>();
        ThrowThing.transform.position = pos;
        nextBall_rigitBody.useGravity = true;
        thR.AddForce(way,ForceMode.Impulse);
    }

 




}
