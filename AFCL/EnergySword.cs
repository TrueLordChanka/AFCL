using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace Plugin.src
{ 
    public class EnergySword : FVRMeleeWeapon
    {
        //[SerializeField] 
        private Material bladeMat;
        public GameObject bladePhys;
        public AudioEvent igniteSound;
        public AudioEvent retractSound;
        [Header("Lerp Speed >1")]
        public float lerpSpeed = 0.5F;
        public bool isdebug = false;
        public float lungeStrength = 12;
        public float cooldown = 1;
        //public bool doesDamageResist = true;
        //public float damageResist = 1F;

        private bool isLit = false;
        private bool isLerping = false;
        private float startTime;
        private float journeyLength;
        private bool stable= true;
        private bool hasLunged = false;
        private float cooldownTime = 0;
        private float lastHealth;
        private bool firstRun = false;


#if !(UNITY_EDITOR || UNITY_5|| DEBUG == true)

        private void Start() //defaults the blade to off
        {
            Color color = bladeMat.color;
            color.a = 0;
            bladeMat.color = color;
            bladePhys.SetActive(false);
            lastHealth = GM.CurrentPlayerBody.GetPlayerHealth();
        }

        public override void UpdateInteraction(FVRViveHand hand)
        {
            base.UpdateInteraction(hand);
            if (base.IsHeld && this.m_hand.Input.TriggerDown && this.m_hasTriggeredUpSinceBegin)
            {
                //TODO TOGGLE BLADE STATE
                if (!isLit)
                {
                    if (isdebug) Debug.Log("turn on bitch");
                    Size = FVRPhysicalObjectSize.CantCarryBig;
                    isLit = true;
                    stable = false;
                }
                else
                {
                    if (isdebug) Debug.Log("turn off bitch");
                    Size = FVRPhysicalObjectSize.Medium;
                    isLit = false;
                    stable = false;
                }
            }else if (base.IsHeld && this.m_hand.Input.TouchpadDown && isLit)
            {
                if (isdebug) Debug.Log("YEET");
                Lunge();
            }

        }
        
        private void Update()
        {
            if (!stable)
            {
                if (isdebug) Debug.Log("now unstable");
                if (isdebug) Debug.Log("islerping"+ isLerping);
                if (isdebug) Debug.Log("islit" + isLit);
                if (isLit)
                {
                    if (!isLerping)  //lerping "open"
                    {
                        Color color = bladeMat.color;
                        color.a = 0;
                        bladeMat.color = color;
                        if (isdebug) Debug.Log("roatethinggoooo");
                        startTime = Time.time;
                        journeyLength = 1 - bladeMat.color.a ;
                        isLerping = true;
                        if (retractSound != null)
                        {
                            SM.PlayGenericSound(igniteSound, transform.position);
                        }
                    }
                    if (isLerping)
                    {
                        float distCovered = (Time.time - startTime) * lerpSpeed;
                        float fractionOfJourney = distCovered / journeyLength;
                        Color color = bladeMat.color;
                        color.a = Mathf.Lerp(bladeMat.color.a, 1, fractionOfJourney);
                        bladeMat.color = color;
                        if(fractionOfJourney >= .5)
                        {
                            bladePhys.SetActive(true);
                        }
                        if (fractionOfJourney >= 1)
                        {
                            isLerping = false;
                            isLit = true;
                            stable = true;
                        }
                    }
                }
                else
                {
                    if (!isLerping)  //lerping "closed"
                    {
                        Color color = bladeMat.color;
                        color.a = 1;
                        bladeMat.color = color;
                        if (isdebug) Debug.Log("roatethinggoooo");
                        startTime = Time.time;
                        journeyLength =  bladeMat.color.a;
                        isLerping = true;
                        if(retractSound != null)
                        {
                            SM.PlayGenericSound(retractSound, transform.position);
                        }
                    }
                    if (isLerping)
                    {
                        float distCovered = (Time.time - startTime) * lerpSpeed;
                        float fractionOfJourney = distCovered / journeyLength;
                        Color color = bladeMat.color;
                        color.a = Mathf.Lerp(bladeMat.color.a, 0, fractionOfJourney);
                        bladeMat.color = color;
                        if (fractionOfJourney >= .5)
                        {
                            bladePhys.SetActive(false);
                        }
                        if (fractionOfJourney >= 1)
                        {
                            isLerping = false;
                            isLit = false;
                            stable = true;
                        }
                    }
                }
            }

            if (hasLunged)
            {
                cooldownTime += Time.deltaTime;
                if (isdebug) Debug.Log("Cooldown is " + cooldownTime);
                if(cooldownTime >= cooldown)
                {
                    hasLunged = false;
                    cooldownTime = 0;
                }
            }

            /*
            if (doesDamageResist && base.IsHeld)
            {
                float currentHealth = GM.CurrentPlayerBody.GetPlayerHealth();
                if (currentHealth < lastHealth)
                {
                    float deltaHealth = lastHealth - currentHealth;
                    GM.CurrentPlayerBody.Health = currentHealth + (damageResist * deltaHealth);
                    if (isdebug) Debug.Log("took " +  deltaHealth);
                    if (isdebug) Debug.Log("Healed " + (damageResist * deltaHealth));
                }
                lastHealth = GM.CurrentPlayerBody.GetPlayerHealth();
            }
            */
        }

        private void Lunge()
        {
            if (!hasLunged)
            {
                GM.CurrentMovementManager.Blast(GM.CurrentPlayerBody.Head.forward, lungeStrength, true);
                hasLunged = true;
            }
        }








#endif

    }
}



