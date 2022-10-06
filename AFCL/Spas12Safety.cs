using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class Spas12Safety : MonoBehaviour
    {
        public TubeFedShotgun Spas12;
        public float DischargeChance = 0.2f;


#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)
        
        public void Awake()
        {
            Hook();
        }

        public void Hook()
        {
            On.FistVR.TubeFedShotgun.ToggleSafety += TubeFedShotgun_ToggleSafety;
        }

        private void TubeFedShotgun_ToggleSafety(On.FistVR.TubeFedShotgun.orig_ToggleSafety orig, TubeFedShotgun self)
        {
            orig(self); //do the important safety stuff
            if (self == Spas12 && !Spas12.m_isSafetyEngaged) //If the gun is the one imputted, and the safty is not engaged, it must have just been set to off
            {
                //Pick a random number between 0-1
                
                if (UnityEngine.Random.Range(0.0f, 1.0f) > DischargeChance)
                {
                    Spas12.ReleaseHammer(); //release the guns hammer, as if pulled by the trigger
                }
            }
        }

        public void OnDestroy()
        {
            Unhook();
        }

        public void Unhook()
        {
            On.FistVR.TubeFedShotgun.ToggleSafety -= TubeFedShotgun_ToggleSafety;
        }







#endif
    }
}



