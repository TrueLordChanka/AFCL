using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class ChangePhysWhenSpent : MonoBehaviour
    {
        public FVRFireArmRound Round;
        public GameObject UnfiredPhys;
        public GameObject FiredPhys;

#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)

        public void Awake()
        {
            On.FistVR.FVRFireArmRound.UpdateInteraction += FVRFireArmRound_UpdateInteraction;
        }

        private void FVRFireArmRound_UpdateInteraction(On.FistVR.FVRFireArmRound.orig_UpdateInteraction orig, FVRFireArmRound self, FVRViveHand hand)
        {
            orig(self, hand);
            if(self == Round) //If the round is itself, and the round is spent, change its physics
            {
                if(Round.m_isSpent)
                {
                    UnfiredPhys.SetActive(false);
                    FiredPhys.SetActive(true);
                }
            }
        }

        public void OnDestroy()
        {
            On.FistVR.FVRFireArmRound.UpdateInteraction -= FVRFireArmRound_UpdateInteraction;
        }

#endif
    }
}



