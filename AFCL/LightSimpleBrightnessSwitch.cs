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
    public class LightSimpleBrightnessSwitch : FVRInteractiveObject
    {

        public Light Light;
        public float[] LightIntensityVals;
        public GameObject LightEffectGameObject;

        private int _lightIndexVal = 0;

#if !(UNITY_EDITOR || UNITY_5)

        public override void SimpleInteraction(FVRViveHand hand)
        {
            if(_lightIndexVal+1 >= LightIntensityVals.Length)
            {
                _lightIndexVal = 0;
                Debug.Log("Index goin back to 0");
            }
            else
            {
                _lightIndexVal++;
                Debug.Log("indexing the index");
            }

            Light.intensity = LightIntensityVals[_lightIndexVal];
            Debug.Log("current light val is " + LightIntensityVals[_lightIndexVal]);

            if (Light.intensity == 0)
            {
                LightEffectGameObject.SetActive(false);
            }
            else
            {
                LightEffectGameObject.SetActive(true);
            }

        }

#endif
    }
}



