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


        public override void Start()
        {
            foreach(GameObject items in ItemsToToggle) //make sure everythings actually disabled on spawn
            {
                items.SetActive(false);
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
                } else
                {
                    foreach (GameObject item in ItemsToToggle) //for each gameobject turn them on (theyre currently off)
                    {
                        item.SetActive(true);
                    }
                    SM.PlayCoreSound(FVRPooledAudioType.GenericClose, this.AudEvent_OnClip, base.transform.position); //play the on sound
                }
            }
        }

#endif
    }
}



