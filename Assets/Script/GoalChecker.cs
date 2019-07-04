using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalChecker : MonoBehaviour
{
    public Text result;
    GameObject goal;

    static int score = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        goal = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int getResult(){
        return score;
    }
    void OnTriggerEnter(Collider t)
    {
        string layerName = LayerMask.LayerToName(t.gameObject.layer);
        if( layerName == "Ball")
        {
            score+=1;
            result.text = score + "";
        }
    }
}
