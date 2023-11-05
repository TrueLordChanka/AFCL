using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class SpringLoadedStockFirearmControl : MonoBehaviour
    {
        public FVRFireArm Firearm;
        public SpringLoadedStockButton StockButton;
        bool IsDian;

        

#if !(UNITY_EDITOR || UNITY_5)

        public void Update()
        {
            if(Firearm.m_hand != null)
            {
                FVRViveHand hand = Firearm.m_hand;
                if (!IsDian)
                {
                    if (!hand.IsInStreamlinedMode)
                    {
                        if (hand.Input.TouchpadDown && Vector2.Angle(hand.Input.TouchpadAxes, Vector2.right) < 45f) //button clicked
                        {
                            StockButton.EjectStock(false);
                        }
                    }
                }
                if(IsDian)
                {
                    ClosedBoltWeapon wep = (ClosedBoltWeapon)Firearm;
                    if(wep.FireSelector_Modes[wep.m_fireSelectorMode].ModeType == ClosedBoltWeapon.FireSelectorModeType.Safe)
                    {
                        if (hand.IsInStreamlinedMode)
                        {
                            if (hand.Input.BYButtonDown )
                            {
                                StockButton.EjectStock(false);
                            }
                        } else
                        {
                            if(hand.Input.TouchpadDown && Vector2.Angle(hand.Input.TouchpadAxes, Vector2.left) < 45f)
                            {
                                StockButton.EjectStock(false);
                            }
                        }
                    }
                     
                    
                    
                }
                
            }
        }
#endif
    }
}



