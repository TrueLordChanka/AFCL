using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class FirearmAuxMuzzleEffects : MonoBehaviour
    {
        public FVRFireArm Gun;


#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)
        
        public void Awake()
        {
            Hook();
        }

        public void Hook()
        {
            On.FistVR.FVRFireArm.Fire += FVRFireArm_Fire;
        }

        private void FVRFireArm_Fire(On.FistVR.FVRFireArm.orig_Fire orig, FVRFireArm self, FVRFireArmChamber chamber, Transform muzzle, bool doBuzz, float velMult, float rangeOverride)
        {
            orig(self, chamber, muzzle, doBuzz, velMult, rangeOverride); //Do all the origional stuff

            if(self == Gun) //If we have the gun we want, we gotta do the gas out shit
            {
                
                



            }



            
        }

        public void OnDestroy()
        {
            Unhook();
        }

        public void Unhook()
        {
            On.FistVR.FVRFireArm.Fire -= FVRFireArm_Fire;
        }







#endif
    }
}



