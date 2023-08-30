using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;
using OpenScripts2;


namespace AndrewFTW
{ 
    public class RPG76 : FVRFireArm
    {
        [Header("RPG67 Params")]
        public float StockOpenRot;
        public FVRFireArmChamber Chamber;
        public MovableObjectPart Stock;
        private bool m_isCocked = false;
        private bool m_hasBeenUncocked = true;
        private bool m_isSafe = true;
        public e_StockCockPose StockCockPose;

        [Header("Trigger Params")]
        public Transform Trigger;
        public FVRPhysicalObject.Axis Trigger_Axis;
        public FVRPhysicalObject.InterpStyle Trigger_Interp;
        public Vector2 Trigger_ValRange;
        [NonSerialized]
        [HideInInspector]
        public float m_triggerVal;

        [Header("Safety Params")]
        public Transform Safety;
        public FVRPhysicalObject.Axis Safety_Axis;
        public FVRPhysicalObject.InterpStyle Safety_Interp;
        public Vector2 Safety_ValRange;

        public enum e_StockCockPose
        {
            Open,
            Close
        }


#if !(UNITY_EDITOR || UNITY_5|| DEBUG == true)

        public override void Awake()
        {
            base.Awake();
            FChambers.Add(Chamber);
        }

        public void ToggleSafety()
        {
            m_isSafe =! m_isSafe; //toggle the value
            if (m_isSafe) //do the animation for the part
            {
                base.SetAnimatedComponent(Safety, Safety_ValRange.x, Safety_Interp, Safety_Axis);
                Debug.Log("a");
            } else
            {
                base.SetAnimatedComponent(Safety, Safety_ValRange.y, Safety_Interp, Safety_Axis);
                Debug.Log("b");
            }
            base.PlayAudioEvent(FirearmAudioEventType.FireSelector, 1f); //play click click audio
        }

        public override void UpdateInteraction(FVRViveHand hand)
        {
            base.UpdateInteraction(hand);
            //Debug.Log(IsAltHeld + " " + hand.Input.TriggerDown + " " + m_hasTriggeredUpSinceBegin + " " + m_isSafe + " " + m_isCocked);
            //Debug.Log("false true true false true desired");
            if(!IsAltHeld && hand.Input.TriggerDown && m_hasTriggeredUpSinceBegin && !m_isSafe && m_isCocked) //all the parameters for firing is checked //
            {

                Fire();
                m_isCocked = false;
                base.PlayAudioEvent(FirearmAudioEventType.HammerHit, 1f);
            }
            if(m_triggerVal != hand.Input.TriggerFloat && !m_isSafe) //if the trigger val isnt equal to the controller value, make it so
            {
                m_triggerVal = hand.Input.TriggerFloat;
                base.SetAnimatedComponent(Trigger, Mathf.Lerp(Trigger_ValRange.x, Trigger_ValRange.y, m_triggerVal), Trigger_Interp, Trigger_Axis);
            }
            //TODO check the stocks position

            if (Stock.IsOpen && m_hasBeenUncocked && !m_isCocked)
            {
                m_isCocked = true;
                m_hasBeenUncocked = false;
            }
            else if (Stock.IsClosed || Stock.IsMid)
            {
                m_hasBeenUncocked = true;
            }

            
            switch (StockCockPose) //determine if the gun should be cocked
            {
                case e_StockCockPose.Open:
                    if (Stock.IsClosed && m_hasBeenUncocked && !m_isCocked)
                    {
                        m_isCocked = true;
                        m_hasBeenUncocked = false;
                    }
                    else if (Stock.IsOpen || Stock.IsMid)
                    {
                        m_hasBeenUncocked = true;
                    }
                    break;
                case e_StockCockPose.Close:
                    
                    if (Stock.IsOpen && m_hasBeenUncocked && !m_isCocked)
                    {
                        m_isCocked = true;
                        m_hasBeenUncocked = false;
                    }
                    else if (Stock.IsClosed || Stock.IsMid)
                    {
                        m_hasBeenUncocked = true;
                    }
                    break;
                default:
                    
                    break;
            }
            
            

            //Debug.Log(Stock.IsOpen + " is open and is cocked? " + m_isCocked);
        }

        public void Fire() //gun should be good to fire
        {
            //Debug.Log("into fire");
            if(Chamber.IsFull && !Chamber.IsSpent)
            {
                //Debug.Log("chamber full and not spent");
                Chamber.Fire(); //fire proj
                Fire(Chamber, GetMuzzle(), true, 1f, -1f); //do gun firing stuff like buzzing
                FireMuzzleSmoke(); //self explanatory
                bool twoHandStabilized = IsTwoHandStabilized();
                bool foregripStabilized = base.AltGrip != null;
                bool shoulderStabilized = IsShoulderStabilized();
                Recoil(twoHandStabilized, foregripStabilized, shoulderStabilized, null, 1f);
                base.PlayAudioGunShot(this.Chamber.GetRound(), GM.CurrentPlayerBody.GetCurrentSoundEnvironment(), 1f);
                if (GM.CurrentSceneSettings.IsAmmoInfinite || GM.CurrentPlayerBody.IsInfiniteAmmo)
                {
                    Chamber.IsSpent = false;
                    Chamber.UpdateProxyDisplay();
                }
                Chamber.SetRound(null, false); //clear the chamber so it can be reloaded
            }
        }



#endif
    }
}



