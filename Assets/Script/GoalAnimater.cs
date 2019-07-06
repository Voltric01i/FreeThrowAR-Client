using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAnimater : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject GoalRing;
    Animator gr;

    void Start()
    {
        gr = GoalRing.GetComponent<Animator>();
    }

// Update is called once per frame
    void Update()
    {
        
    }
    public void bigger(){
        Debug.Log("60 ");
        gr.SetTrigger("100to200");
    }

    public void smaller(){
        Debug.Log("120 ");
        gr.SetTrigger("70to100");
    }
}
