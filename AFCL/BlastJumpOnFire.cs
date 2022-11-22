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
        public float lungeStrength = -2;

#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)
        
         public void Awake()
        {
            GM.CurrentSceneSettings.ShotFiredEvent += CurrentSceneSettings_ShotFiredEvent;
        }

        private void CurrentSceneSettings_ShotFiredEvent(FVRFireArm firearm)
        {
            if (Firearm == firearm)
            {
                GM.CurrentMovementManager.Blast(GM.CurrentPlayerBody.Head.forward, lungeStrength, true);
            }
        }

        public void OnDestroy()
        {
            GM.CurrentSceneSettings.ShotFiredEvent -= CurrentSceneSettings_ShotFiredEvent;
        }

#endif
    }
}



