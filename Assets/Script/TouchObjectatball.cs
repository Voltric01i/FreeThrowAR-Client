using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class TouchObjectatball : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent onTouchAtball;
    GameObject goal;
    void Start()
    {
        goal = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider t)
    {
        string layerName = LayerMask.LayerToName(t.gameObject.layer);
        if( layerName == "Ball")
        {
            onTouchAtball.Invoke();
        }
    }
}
