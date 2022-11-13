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

#if !(UNITY_EDITOR || UNITY_5)

        public void Update()
        {
            if(Firearm.m_hand != null)
            {
                FVRViveHand hand = Firearm.m_hand;
                if(!hand.IsInStreamlinedMode)
                {
                    if (hand.Input.TouchpadDown && Vector2.Angle(hand.Input.TouchpadAxes, Vector2.right) < 45f) //button clicked
                    {
                        StockButton.EjectStock();
                    }
                }
            }
        }
#endif
    }
}



