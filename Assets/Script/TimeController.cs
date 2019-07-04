using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TimeController : MonoBehaviour
{
    public float endTime_s;
    public Text timeText;
    public UnityEvent onTimerEnded;
    float countTime_s;
    bool startFlag = false;
    


    // Start is called before the first frame update
    void Start()
    {
        countTime_s = endTime_s;
    }

    // Update is called once per frame
    void Update()
    {
        if(countTime_s <= 0){
            onTimerEnded.Invoke();
            countTime_s = endTime_s;
        }

        if(startFlag){
            countTime_s -= Time.deltaTime; 
        }
        timeText.text = (int)countTime_s + "";
        

    }

    public void gameStart(){
        startFlag = true;        
    }
}
