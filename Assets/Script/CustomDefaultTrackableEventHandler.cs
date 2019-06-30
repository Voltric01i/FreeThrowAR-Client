using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class CustomDefaultTrackableEventHandler : DefaultTrackableEventHandler {


    protected override void OnTrackingFound ()
    {
        base.OnTrackingFound ();
        Debug.Log("見つけた！！！！");
        OnTrackingAction.Invoke ();
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost ();
        Debug.Log("外れた！！！！");
        OffTrackingAction.Invoke (); 
    }
}