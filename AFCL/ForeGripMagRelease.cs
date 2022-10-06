using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;
using System.Collections;

namespace Plugin.src
{
    public class ForeGripMagRelease : MonoBehaviour
    {

        public ClosedBoltWeapon gun;
        public FVRAlternateGrip grip;
        public Transform button;
        public enum Axis
        {
            X,
            Y,
            Z
        } 
        public Axis axis;
        public float pressed;
        public float unpressed;
        public float InteractDelay2 = 0.75f;
        public GameObject InteractionCollider;
        private FVRViveHand m_hand;

        private bool isInteractHidden = false;



#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)


        public void Awake()
        {
            Hook();
        }

        public void Update()
        {
            if(m_hand != null && m_hand.OtherHand.m_currentInteractable == gun && m_hand.CurrentInteractable == null)
            {
                m_hand = null;

            } else if(m_hand != null && m_hand.OtherHand.m_currentInteractable != gun && m_hand.CurrentInteractable == null)
            {
                m_hand = null;
            }
            //Debug.Log("grabbed from attachable grip: " + grip.GetLastGrabbedGrip().name);
            if (m_hand != null && grip.GetLastGrabbedGrip() == null) //if your holding it
            {
                if (!isInteractHidden) //if the things not hidden
                {
                    InteractionCollider.SetActive(false); //Sets the interaction collider to not exist when your holding it
                    isInteractHidden = true;
                }
                if (m_hand.IsInStreamlinedMode) //if its in streamlined
                {   //This is for the mag release
                    if (m_hand.Input.AXButtonDown) //if the AX button is pressed
                    {
                        gun.EjectMag();
                    }
                    if (m_hand.Input.AXButtonPressed) //if the AX button is held
                    {
                        Buttonpressed(true);
                    }
                    else
                    {
                        Buttonpressed(false);
                    }
                }
                else //If in normal
                {   //This is for the mag release
                    if (m_hand.Input.TouchpadDown && Vector2.Angle(m_hand.Input.TouchpadAxes, Vector2.down) < 45f) //button clicked
                    {
                        gun.EjectMag();
                    }
                    if (m_hand.Input.TouchpadPressed && Vector2.Angle(m_hand.Input.TouchpadAxes, Vector2.down) < 45f) //button held
                    {
                        Buttonpressed(true);
                    }
                    else
                    {
                        Buttonpressed(false);
                    }
                    //THis is for the bolt Catch
                    if (gun.Bolt.IsBoltLocked()) //if the bolt is locked
                    {
                        if (m_hand.Input.TouchpadPressed && Vector2.Angle(m_hand.Input.TouchpadAxes, Vector2.right) < 45f) //button clicked
                        {
                            gun.Bolt.ReleaseBolt();
                        }
                    }
                    else //if the bolt is not locked
                    {
                        if (gun.Bolt.CurPos >= ClosedBolt.BoltPos.Locked) //if the bolt is behind the lock point
                        {
                            if (m_hand.Input.TouchpadPressed && Vector2.Angle(m_hand.Input.TouchpadAxes, Vector2.up) < 45f) //button clicked
                            {
                                gun.Bolt.LockBolt();
                            }
                        }
                    }
                }
            }
        }

        public void Hook()
        {
            Debug.Log("Hooking Foregrip Attachment");
            On.FistVR.FVRAlternateGrip.UpdateInteraction += FVRAlternateGrip_UpdateInteraction;
            On.FistVR.FVRAlternateGrip.EndInteraction += FVRAlternateGrip_EndInteraction;
        }

        private void FVRAlternateGrip_EndInteraction(On.FistVR.FVRAlternateGrip.orig_EndInteraction orig, FVRAlternateGrip self, FVRViveHand hand)
        {
            if (self == grip)
            {
                StartCoroutine(ReactivateButton());
                
            }
            orig(self, hand);
        }

        private void FVRAlternateGrip_UpdateInteraction(On.FistVR.FVRAlternateGrip.orig_UpdateInteraction orig, FVRAlternateGrip self, FVRViveHand hand)
        {
            if (self == grip)
            {
                m_hand = hand;
            }
            orig(self, hand); //runs the origional updateinteractions shit
        }

        IEnumerator ReactivateButton()
        {
            yield return new WaitForSeconds(InteractDelay2);
            InteractionCollider.SetActive(true);
            isInteractHidden = false;
        }

        private void Buttonpressed(bool ispressed) //do the move thing
        {
            Vector3 pos = button.transform.localPosition;

            if (ispressed)
            {
                pos[(int)axis] = pressed;
            }
            else
            {
                pos[(int)axis] = unpressed;
            }
            button.transform.localPosition = pos;
        }
#endif
    }
}



