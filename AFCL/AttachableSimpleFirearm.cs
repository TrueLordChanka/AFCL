using System;
using UnityEngine;
using FistVR;
using Unity;
using OpenScripts2;


namespace AndrewFTW
{
    public class AttachableSimpleFirearm : AttachableFirearm
    {
        public FVRFireArmChamber Chamber;



        [Header("Trigger Config")] //this is all yoinked from antons
        public Transform Trigger;
        public float TriggerFiringThreshold = 0.8f;
        public float TriggerResetThreshold = 0.4f;
        public float Trigger_ForwardValue;
        public float Trigger_RearwardValue;
        public FVRPhysicalObject.Axis TriggerAxis;
        public FVRPhysicalObject.InterpStyle TriggerInterpStyle = FVRPhysicalObject.InterpStyle.Rotation;

        [NonSerialized]
        [HideInInspector]
        public float m_triggerFloat;
        [NonSerialized]
        [HideInInspector]
        public bool m_hasTriggerReset;


        [Header("Fore Slide Stuff")]
        //public GameObject MoveableComponent;
        public MovableObjectPart MoveableComponent;

        public FVRFirearmAudioSet[] AudioSets;



        public bool isDebug = false; //if (isDebug) { Debug.Log("test"); }


#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)

        public override void Awake()
        {
            base.Awake();
        }


        public override void Update()
        {
            base.Update();
            UpdateComponents(); //update the moving parts

            


        }


        public override void ProcessInput(FVRViveHand hand, bool fromInterface, FVRInteractiveObject obj) //processes the input each frame
        {
            base.ProcessInput(hand, fromInterface, obj);
            if (obj.m_hasTriggeredUpSinceBegin)
            {
                this.m_triggerFloat = hand.Input.TriggerFloat;
            }
            else
            {
                this.m_triggerFloat = 0f;
            }

            if (!this.m_hasTriggerReset && this.m_triggerFloat <= this.TriggerResetThreshold)
            {
                this.m_hasTriggerReset = true;
                base.PlayAudioEvent(FirearmAudioEventType.TriggerReset, 1f);
            }


            if(m_triggerFloat >= TriggerFiringThreshold && obj.m_hasTriggeredUpSinceBegin && m_hasTriggerReset && MoveableComponent.IsOpen) //if the trigger is past the threshold, and "has trigger up since begin. ive got no clue what this is but
            {                                                                              // I assume its trigger reset shit. 
                m_hasTriggerReset = false;
                if (LoadChamber())
                {
                    PlayAudioEvent(FirearmAudioEventType.HammerHit, 1f);
                    SwapAudioSet();
                    Fire(fromInterface);
                }
                
            }
        }


        public void Fire(bool firedFromInterface)
        {
            if (isDebug) { Debug.Log("Into Fire Method"); }
            if (Chamber.Fire())
            {
                if (isDebug) { Debug.Log("past chamber fire check"); }
                FireMuzzleSmoke(); //poof

                FVRFireArm fvrfireArm = null;
                if (this.OverrideFA != null)
                {
                    fvrfireArm = this.OverrideFA;
                    this.Fire(this.Chamber, this.GetMuzzle(), true, fvrfireArm, 1f);
                }
                else if (firedFromInterface && this.Attachment.curMount != null) //if fired from interface and the attach mount doesnt exist, its handheld
                {
                    fvrfireArm = (this.Attachment.curMount.GetRootMount().MyObject as FVRFireArm); //set the gun its attached to as fvrfirearm
                    if (fvrfireArm != null)
                    {
                        this.Fire(this.Chamber, this.GetMuzzle(), true, fvrfireArm, 1f);
                    }
                    else
                    {
                        this.Fire(this.Chamber, this.GetMuzzle(), true, null, 1f);
                    }
                }
                else
                {
                    this.Fire(this.Chamber, this.GetMuzzle(), true, null, 1f); //this just seems to be a sort of insurance policy
                }
                Recoil(firedFromInterface, fvrfireArm); //do recoil
                base.PlayAudioGunShot(Chamber.GetRound(), GM.CurrentPlayerBody.GetCurrentSoundEnvironment(), 1f); //do play sound



                if (Chamber.IsSpent)
                {
                    Chamber.Unload();
                }
            }
        }


        public void UpdateComponents() //called every frame, used to update things like triggers
        {
            //Animate the trigger based on its current "float value"
            Attachment.SetAnimatedComponent(Trigger, Mathf.Lerp(Trigger_ForwardValue, Trigger_RearwardValue, m_triggerFloat), TriggerInterpStyle, TriggerAxis);
        }

        public bool LoadChamber()
        {
            if (isDebug) { Debug.Log("IntoLoadChamber chamberfull: " + Chamber.IsFull + " & MagHasRound: "+ Magazine.HasARound()); }
            //if the chamber is empty, and there are rounds in the mag, move one to the chamber
            GameObject fromPrefabReference = null;

            if (!Chamber.IsFull && Magazine.HasARound())//if the chamber is empty, and there are rounds in the mag,
            {
                fromPrefabReference = Magazine.RemoveRound(false); //remove round from mag
                FVRFireArmRound Round = fromPrefabReference.GetComponent<FVRFireArmRound>(); //reassure it that its a round and is loved <3
                Chamber.SetRound(Round , false); // move it to the chamber
                if (isDebug) { Debug.Log("True"); }
                return true;
            }
            if (isDebug) { Debug.Log("False"); }
            return false;
        }

        public void SwapAudioSet()
        {
            /**
            if(Magazine.m_capacity != AudioSets.Length)
            {
                Debug.Log("Mag capacity not equal to list of sets");
            }
            **/
            if (isDebug) { Debug.Log("AudioSet set to " + Magazine.m_numRounds); }
            AudioClipSet = AudioSets[Magazine.m_numRounds];
            

        }


#endif
    }
}



