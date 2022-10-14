using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class TubeFedSlamfireToggle : FVRInteractiveObject
    {
        public TubeFedShotgun Weapon;
        public GameObject SwitchGraphic;
        public Transform SwitchOff;
        public Transform SwitchOn;
        public AudioEvent SwitchSound;


#if !(UNITY_EDITOR || UNITY_5)

        public override void SimpleInteraction(FVRViveHand hand)
        {
            base.SimpleInteraction(hand);

            if (Weapon.UsesSlamFireTrigger)
            {
                Weapon.UsesSlamFireTrigger = false;
                SwitchGraphic.transform.localEulerAngles = SwitchOff.localEulerAngles;
            } else
            {
                Weapon.UsesSlamFireTrigger = true;
                SwitchGraphic.transform.localEulerAngles = SwitchOn.localEulerAngles;
            }
            SM.PlayGenericSound(SwitchSound, transform.position);
        }

#endif
    }
}



