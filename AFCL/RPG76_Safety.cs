using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class RPG76_Safety : FVRInteractiveObject
    {
        public RPG76 RPG76;

#if !(UNITY_EDITOR || UNITY_5|| DEBUG == true)

        public override void SimpleInteraction(FVRViveHand hand)
        {
            //Debug.Log("fak");
            RPG76.ToggleSafety();
            base.SimpleInteraction(hand);
        }

#endif
    }
}



