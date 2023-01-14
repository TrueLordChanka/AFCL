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
    public class SimpleSwitch : FVRInteractiveObject
    {

        public bool OnOff = true;
        public GameObject[] ItemsToToggle;

        public AudioEvent AudEvent_OnClip;
        public AudioEvent AudEvent_OffClip;


        private bool _isItemToggled = false;


#if !(UNITY_EDITOR || UNITY_5)


        public void Start()
        {
            base.Start();
            if (ItemsToToggle[0].activeSelf) //if its active, set _isItemToggled to true
            {
                _isItemToggled=true;
            } else
            {
                _isItemToggled=false;
            }
        }

        public override void SimpleInteraction(FVRViveHand hand)
        {
            base.SimpleInteraction(hand);
            if (OnOff) //Check the mode, currently only OnOff is implemented
            {
                if (_isItemToggled)
                {
                    foreach(GameObject item in ItemsToToggle) //for each gameobject turn them off (theyre currently on)
                    {
                        item.SetActive(false);
                    }
                    SM.PlayCoreSound(FVRPooledAudioType.GenericClose, this.AudEvent_OffClip, base.transform.position); //play the off sound
                    _isItemToggled = false;
                } else
                {
                    foreach (GameObject item in ItemsToToggle) //for each gameobject turn them on (theyre currently off)
                    {
                        item.SetActive(true);
                    }
                    SM.PlayCoreSound(FVRPooledAudioType.GenericClose, this.AudEvent_OnClip, base.transform.position); //play the on sound
                    _isItemToggled = true;
                }
            }
        }

#endif
    }
}



