﻿// using UnityEngine;
// using UnityEngine.Events;
// using Vuforia;
// using static DefaultTrackableEventHandler;

// public class CustomDefaultTrackableEventHandler : DefaultTrackableEventHandler {

//     public UnityEvent OnTrackingAction;
//     public UnityEvent OffTrackingAction;

//     protected override void OnTrackingFound ()
//     {
//         base.OnTrackingFound ();
//         OnTrackingAction.Invoke ();
//     }

//     protected override void OnTrackingLost()
//     {
//         base.OnTrackingLost ();
//         OffTrackingAction.Invoke ();
//     }
// }