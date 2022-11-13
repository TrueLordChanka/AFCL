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
    public class AttachableMagWell : FVRFireArmReloadTriggerWell
    {
        public FVRFireArmAttachment Attachment;
        public UniversalMagazineGrabTrigger MagGrabTrigger;
        public Transform MagMountPos;
        public Transform MagEjectPos;
        float EjectDelay = 0f;


        private static Dictionary<FVRFireArm, List<AttachableMagWell>> _attachMagwelllDict = new Dictionary<FVRFireArm, List<AttachableMagWell>>();
        private int _index;
        private FVRFireArm.SecondaryMagazineSlot _mySlot;


#if !(UNITY_EDITOR || UNITY_5)

        public void OnEnable()
        {
            //get the gun, add our mag to its list of secondary mags
            FireArm = Attachment.curMount.GetRootMount().MyObject as FVRFireArm;
            _index = FireArm.SecondaryMagazineSlots.Length;
            SecondaryMagSlotIndex = _index;

            _mySlot = new FVRFireArm.SecondaryMagazineSlot();
            _mySlot.MagazineMountPos = MagMountPos;
            _mySlot.MagazineEjectPos = MagEjectPos;
            _mySlot.m_ejectDelay = EjectDelay;
            

            Array.Resize(ref FireArm.SecondaryMagazineSlots, FireArm.SecondaryMagazineSlots.Length +1 ); //add the index
            FireArm.SecondaryMagazineSlots[_index] = _mySlot;

            //Add firearm ref to mag eject trigger

            MagGrabTrigger.FireArm = FireArm;
            MagGrabTrigger.SecondaryGrabSlot = _index;

            List<AttachableMagWell> list;
            if(_attachMagwelllDict.TryGetValue(FireArm, out list)) //check for the firearm in the dictionary
            {
                list.Add(this); //if its there, add this component to the list of attachamblemagwells on that gun
            } else
            {
                list = new List<AttachableMagWell> //if not make a new entry for the firarm, and then add the entry to it.
                {
                    this
                };
                _attachMagwelllDict.Add(FireArm, list);
            }

            MagGrabTrigger.gameObject.SetActive(true);
            
        }

        public void OnDisable()
        {
            //Remove our entry from the list
            MagGrabTrigger.gameObject.SetActive(false);
            FireArm.EjectSecondaryMagFromSlot(_index);


            List<AttachableMagWell> list;
            if (_attachMagwelllDict.TryGetValue(FireArm, out list)) //check for the firearm in the dictionary
            {
                if(list.Count == 1)
                {
                    //remove it from the firearm and list
                    Array.Resize(ref FireArm.SecondaryMagazineSlots, FireArm.SecondaryMagazineSlots.Length - 1);
                    MagGrabTrigger.FireArm = null;
                    MagGrabTrigger.SecondaryGrabSlot = 0;
                    _attachMagwelllDict.Remove(FireArm);

                } else //the list is more than one which means the gun has multiple mag attachments on it. We must only remove ours.
                { //figure out what index we have, and see if there are other attachemnts in the list with a higher index than ours
                    
                    foreach (AttachableMagWell magWell in list)
                    {
                        if(magWell._index > _index)
                        {
                            magWell._index -= 1;
                            magWell.SecondaryMagSlotIndex -= 1;
                            magWell.MagGrabTrigger.SecondaryGrabSlot -= 1;
                        }
                    }
                    //Move everthing past the index over by 1
                    for(int i = _index; i < FireArm.SecondaryMagazineSlots.Length-1; i++)
                    {
                        FireArm.SecondaryMagazineSlots[i] = FireArm.SecondaryMagazineSlots[i + 1];
                    } //resize to remove the last value cause its junk now

                    Array.Resize(ref FireArm.SecondaryMagazineSlots, FireArm.SecondaryMagazineSlots.Length - 1);
                    list.Remove(this);

                    MagGrabTrigger.FireArm = null;
                    MagGrabTrigger.SecondaryGrabSlot = 0;

                }
            }
            else
            { //if the firearms not in the list then somthings fucked
                Debug.Log("fuck somthing broke tell Andrew_FTW you broke his mag script: " + this);
            }
        }

#endif
    }
}



