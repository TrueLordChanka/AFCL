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
    public class SpringLoadedStockButton : FVRInteractiveObject
    {
        [Header("This only works for z axis movement")]
        public GameObject Stock;
        public float CollapseVal;
        public float ExtendedVal;
        public float TimeToExtend = 0.5f;
        public AudioEvent SwitchSound;
        

#if !(UNITY_EDITOR || UNITY_5)

        public override void SimpleInteraction(FVRViveHand hand)
        {
            base.SimpleInteraction(hand);
            //Start a lerp to the open position.
            EjectStock();
           
        }

        public void EjectStock()
        {
            //used to call it from the outside
            if (Mathf.Abs(Stock.transform.localPosition.z - CollapseVal) <= 0.02) //get the absolute difference between the two points
            {
                StartCoroutine(LerpStock());
                SM.PlayGenericSound(SwitchSound, transform.position);
            }
        }



        IEnumerator LerpStock()
        {
            float _timeEllapsed = 0f;
            bool _StockExtended = false;
            while(!_StockExtended) //while the stocks not extended
            {
                /*
                 * https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/#right_way_to_use_lerp
                 * Very usefull link wrt lerp 
                 */

                _timeEllapsed += Time.deltaTime;
                float _percentComplete = _timeEllapsed / TimeToExtend;
                if(_percentComplete > 0.9) //If its 90% there, just put it all the way there.
                {
                    _percentComplete = 1f;
                    _StockExtended = true;
                }

                float lerpValue = Mathf.Lerp(CollapseVal, ExtendedVal, _percentComplete); 

                //Move the local pos to the new spot
                Stock.transform.localPosition = new Vector3(Stock.transform.localPosition.x,Stock.transform.localPosition.y, lerpValue);

                yield return null;
            }

            yield return null;
        }





#endif
    }
}



