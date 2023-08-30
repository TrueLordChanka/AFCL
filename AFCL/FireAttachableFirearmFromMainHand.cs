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
    public class FireAttachableFirearmFromMainHand : MonoBehaviour
    {
        public FVRFireArm FireArm;
        public AttachableFirearm AttachableFirearm;
        public firearmActionMode FirearmActionMode = firearmActionMode.ClosedBolt;
        public FireSelectorModeType MainWeponFireMode = FireSelectorModeType.Single;


        public enum FireSelectorModeType
        {
            Single,
            FullAuto,
        }
        public enum firearmActionMode
        {
            ClosedBolt,
            OpenBolt
        }
        private bool selectorOnMain = true;
        private static Dictionary<FVRPhysicalObject, FireAttachableFirearmFromMainHand> fireAttachables = new Dictionary<FVRPhysicalObject,FireAttachableFirearmFromMainHand>();



#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)

        static FireAttachableFirearmFromMainHand()
        {
            On.FistVR.FVRPhysicalObject.UpdateInteraction += FVRPhysicalObject_UpdateInteraction;
        }

        public void Awake()
        {
            fireAttachables.Add(FireArm, this);
        }

        public void OnDestroy()
        {
            fireAttachables.Remove(FireArm);
        }

        private static void FVRPhysicalObject_UpdateInteraction(On.FistVR.FVRPhysicalObject.orig_UpdateInteraction orig, FVRPhysicalObject self, FVRViveHand hand)
        {
            
            FireAttachableFirearmFromMainHand _firfromhand;
            if(!fireAttachables.TryGetValue(self, out _firfromhand)) //if its not on the list, just do normal stuff and return.
            {
                orig(self, hand);
                return; 
            }

            if (!_firfromhand.selectorOnMain) //if the selector is on the attachgun, we 
            {
                _firfromhand.AttachableFirearm.ProcessInput(hand, false, _firfromhand.FireArm as FVRInteractiveObject);
            }
            else
            {
                //Debug.Log("test");
                orig(self, hand);
            }

            if (hand.IsInStreamlinedMode) //if on streamlined use the toggle 
            {
                if (hand.Input.BYButtonDown)
                {
                    _firfromhand.ToggleFireSelector(self); 
                }
            }
            else
            {
                Vector2 touchpadAxes = hand.Input.TouchpadAxes;
                if (hand.Input.TouchpadDown) //if not on streamlined use the toggle
                {
                    if (touchpadAxes.magnitude > 0.2f)
                    {
                        if (Vector2.Angle(touchpadAxes, Vector2.left) <= 45f)
                        {
                            _firfromhand.ToggleFireSelector(self);
                        }
                    }
                }
            }

 
        }

        public void ToggleFireSelector(FVRPhysicalObject self)
        {
            //Now I need to make it so when the player fires it goes to the secondary gun
            //Debug.Log("set to" + selectorOnMain);
            selectorOnMain = !selectorOnMain;
            if (!selectorOnMain) //if the gun is not targeted
            {
                if(FirearmActionMode == firearmActionMode.ClosedBolt)
                {
                    ClosedBoltWeapon wep = (ClosedBoltWeapon)self;  
                    wep.FireSelector_Modes[wep.m_fireSelectorMode].ModeType = ClosedBoltWeapon.FireSelectorModeType.Safe;

                } else if( FirearmActionMode == firearmActionMode.OpenBolt)
                {
                    OpenBoltReceiver wep = (OpenBoltReceiver)self;
                    wep.FireSelector_Modes[wep.m_fireSelectorMode].ModeType = OpenBoltReceiver.FireSelectorModeType.Safe;
                }
                

            } else
            {
                if (FirearmActionMode == firearmActionMode.ClosedBolt)
                {
                    ClosedBoltWeapon wep = (ClosedBoltWeapon)self;
                    if(MainWeponFireMode == FireSelectorModeType.Single)
                    {
                        wep.FireSelector_Modes[wep.m_fireSelectorMode].ModeType = ClosedBoltWeapon.FireSelectorModeType.Single;
                    } else
                    {
                        wep.FireSelector_Modes[wep.m_fireSelectorMode].ModeType = ClosedBoltWeapon.FireSelectorModeType.FullAuto;
                    }
                    
                }
                else if (FirearmActionMode == firearmActionMode.OpenBolt)
                {
                    OpenBoltReceiver wep = (OpenBoltReceiver)self;
                    if (MainWeponFireMode == FireSelectorModeType.Single)
                    {
                        wep.FireSelector_Modes[wep.m_fireSelectorMode].ModeType = OpenBoltReceiver.FireSelectorModeType.Single;
                    }
                    else
                    {
                        wep.FireSelector_Modes[wep.m_fireSelectorMode].ModeType = OpenBoltReceiver.FireSelectorModeType.FullAuto;
                    }
                    
                }
            }
        }




#endif
    }
}



