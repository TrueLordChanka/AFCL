using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class BlastJumpOnFire : MonoBehaviour
    {
        public FVRFireArm Firearm;
        public bool IsMinigun;
        public float lungeStrength = -2;

#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)
        
         public void Awake()
        {
            if (!IsMinigun)
            {
                GM.CurrentSceneSettings.ShotFiredEvent += CurrentSceneSettings_ShotFiredEvent;
            } else
            {
                On.FistVR.Minigun.Fire += Minigun_Fire;
            }
             
        }

        private void Minigun_Fire(On.FistVR.Minigun.orig_Fire orig, Minigun self)
        {
            orig(self);
            if (Firearm == self)
            {
                GM.CurrentMovementManager.Blast(Firearm.MuzzlePos.forward, lungeStrength, true);
            }
        }

        private void CurrentSceneSettings_ShotFiredEvent(FVRFireArm firearm)
        {
            if (Firearm == firearm)
            {
                GM.CurrentMovementManager.Blast(Firearm.MuzzlePos.forward, lungeStrength, true);
            }
        } 

        public void OnDestroy()
        {
            GM.CurrentSceneSettings.ShotFiredEvent -= CurrentSceneSettings_ShotFiredEvent;
        }

#endif
    }
}



