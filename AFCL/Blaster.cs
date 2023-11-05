using System;
using UnityEngine;
using FistVR;
using Unity;
using OpenScripts2;
using System.Collections.Generic;

namespace AndrewFTW
{
    public class Blaster : FVRFireArm
    {
        [Header("Blaster Parts")]
        public FVRFireArmChamber Chamber;

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

        public bool UsesHeat = true;
        public float HeatPerShot = 0.1f;
        public float HeatDisipationRate = 0.3f;
        public float SecondsPerShot = 0.3f;
        public float TimeDisabledAfterOverheat = 2.0f;

        public bool UsesHeatingEffect;
        public List<Renderer> Renderers = new List<Renderer>();



        public bool isDebug = false;
        private float timeSinceLastShot = 0f;
        private bool isDisabled = false;
        private float heat = 0f;
        private float disipationTimer;
        private MaterialPropertyBlock PropertyBlock = new MaterialPropertyBlock();


#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)

        public override void Awake()
        {
            base.Awake();
            disipationTimer = TimeDisabledAfterOverheat;
        }

        public override void UpdateInteraction(FVRViveHand hand)
        {
            base.UpdateInteraction(hand);
            UpdateComponents(hand);
            //update the moving parts
            
        }

        public void Update()
        {
            if(timeSinceLastShot < 120)
            {
                timeSinceLastShot += Time.deltaTime;
            }
            //Debug.Log("heat " + heat);
            if (UsesHeat)
            {
                if (!isDisabled)
                {

                    if (heat > 0f)
                    {
                        heat -= HeatDisipationRate * Time.deltaTime;
                    }

                    if (heat >= 1)
                    {
                        isDisabled = true;
                    }
                }
                else
                {
                    Debug.Log("Disipation " + disipationTimer);
                    disipationTimer -= Time.deltaTime;
                    heat = Mathf.Lerp(0, 1, (disipationTimer / TimeDisabledAfterOverheat));
                    if (disipationTimer <= 0)
                    {
                        disipationTimer = TimeDisabledAfterOverheat;
                        isDisabled = false;
                        heat = 0f;
                    }
                }

                if (UsesHeatingEffect)
                {
                    //Update heated part color
                    PropertyBlock.SetFloat("_EmissionWeight", heat);
                    foreach (Renderer r in Renderers) //apply all the renderers to the property block
                    {
                        r.SetPropertyBlock(PropertyBlock);
                    }


                }
            }
            
        }

        public void UpdateComponents(FVRViveHand hand)
        {
            //Used to animate things like triggers and hammers
            SetAnimatedComponent(Trigger, Mathf.Lerp(Trigger_ForwardValue, Trigger_RearwardValue, m_triggerFloat), TriggerInterpStyle, TriggerAxis);
            if (this.m_hasTriggeredUpSinceBegin)
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
            if (this.m_triggerFloat >= this.TriggerFiringThreshold && this.m_hasTriggerReset)
            {
                
                Prefire();
                this.m_hasTriggerReset = false;
            }
            

            
            

        }

        public bool Prefire()
        {
            Debug.Log("UwU " + timeSinceLastShot + isDisabled);
            if(timeSinceLastShot >= SecondsPerShot && !isDisabled)
            {
                bool twoHandStabilized = this.IsTwoHandStabilized();
                bool foregripStabilized = base.AltGrip != null;
                bool shoulderStabilized = this.IsShoulderStabilized();
                this.Recoil(twoHandStabilized, foregripStabilized, shoulderStabilized, null, 1f);
                this.FireMuzzleSmoke();
                base.PlayAudioGunShot(Chamber.GetRound(), GM.CurrentPlayerBody.GetCurrentSoundEnvironment(), 1f); //do play sound
                                                                                                                  //TODO Fire the damn thing out the muzzle

                if (Chamber.Fire())
                {
                    Fire(Chamber, MuzzlePos, true, 1);
                    timeSinceLastShot = 0f;
                    heat += HeatPerShot;
                }
                if (Chamber.IsSpent)
                {
                    Chamber.Unload();
                    LoadChamber();
                }


                return true;
            }


            return false;
        }

        public bool LoadChamber()
        {
            if (isDebug) { Debug.Log("IntoLoadChamber chamberfull: " + Chamber.IsFull + " & MagHasRound: " + Magazine.HasARound()); }
            //if the chamber is empty, and there are rounds in the mag, move one to the chamber
            GameObject fromPrefabReference = null;

            if (!Chamber.IsFull && Magazine.HasARound())//if the chamber is empty, and there are rounds in the mag,
            {
                fromPrefabReference = Magazine.RemoveRound(false); //remove round from mag
                FVRFireArmRound Round = fromPrefabReference.GetComponent<FVRFireArmRound>(); //reassure it that its a round and is loved <3
                Chamber.SetRound(Round, false); // move it to the chamber
                if (isDebug) { Debug.Log("True"); }
                return true;
            }
            if (isDebug) { Debug.Log("False"); }
            return false;
        }


#endif
    }
}
