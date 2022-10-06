using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class TubeFedShotgunInteractableSelector : FVRInteractiveObject
    {
        public TubeFedShotgun Weapon;



#if !(UNITY_EDITOR || UNITY_5)

        public override void SimpleInteraction(FVRViveHand hand)
        {
            base.SimpleInteraction(hand);
            Weapon.ToggleSafety(); //toggle the safety
        }

#endif
    }
}



