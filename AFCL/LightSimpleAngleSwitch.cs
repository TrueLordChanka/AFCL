using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;
using System.Collections;

namespace AndrewFTW
{ 
    public class LightSimpleAngleSwitch : FVRInteractiveObject
    {

        public Light Light;
        public float[] LightAngleVals;

        private int _lightIndexVal = 0;

#if !(UNITY_EDITOR || UNITY_5)

        public override void SimpleInteraction(FVRViveHand hand)
        {
            if(_lightIndexVal+1 >= LightAngleVals.Length)
            {
                _lightIndexVal = 0;
                Debug.Log("Index goin back to 0");
            }
            else
            {
                Debug.Log("indexing the index");
                _lightIndexVal++;
            }

            Light.spotAngle = LightAngleVals[_lightIndexVal];

        }

#endif
    }
}



