using FistVR;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AndrewFTW
{
    public class StrobeController : MonoBehaviour
    {
        public GameObject LightToStrobe;

        public static float StrobeFlashTime = 0.02f;

        public LayerMask LM_TargetMask;
        public LayerMask LM_Blockers;
        public float BlindingFOV = 3f;
        public float BlindingRange = 11f;
        public float BlindingAtOneMeter;

        private Collider[] _targetArray = new Collider[32];
        private float _overlapCapsulRadius;

#if !(UNITY_EDITOR || UNITY_5)

        public void Awake()
        {
            _overlapCapsulRadius = Mathf.Tan(BlindingFOV * Mathf.Deg2Rad) * BlindingRange;
        }

        public void OnEnable()
        {
            StartCoroutine(StrobeLight());
            //Debug.Log("flash time is "+ StrobeFlashTime);
        }


        public void OnDisable()
        {
            //Debug.Log("Disabling");
            LightToStrobe.SetActive(true); //Sets it true so it doesnt conflict with the light itself
            StopCoroutine(StrobeLight()); //Stops the strobing
        }

        IEnumerator StrobeLight()
        {
            while (true)
            {

                //Debug.Log("Blink " + LightToStrobe.activeSelf);
                //set the lights activness to the opposite of what it is now.
                LightToStrobe.SetActive(!LightToStrobe.activeSelf);
                if (LightToStrobe.activeInHierarchy)
                {
                    //If its being turned on, we need to do the flash thing for sosigs
                    
                    //New method thanks to City
                    //makes a capsul between two points, with the layer maks n shit
                    int NumTargets = Physics.OverlapCapsuleNonAlloc(LightToStrobe.transform.position, LightToStrobe.transform.position + BlindingRange * LightToStrobe.transform.forward, _overlapCapsulRadius, _targetArray, LM_TargetMask, QueryTriggerInteraction.Collide);

                    Vector3 direction;

                    for (int i = 0; i < NumTargets; i++)
                    {
                        //get the direction from the strobe to the target
                        direction = _targetArray[i].transform.position - LightToStrobe.transform.position;

                        if (Vector3.Angle(direction, transform.forward) < BlindingFOV && !Physics.Linecast(LightToStrobe.transform.position, _targetArray[i].transform.position - LightToStrobe.transform.forward * 0.2f, LM_Blockers))
                        {
                            //If the thing is a sosig link
                            if (_targetArray[i].attachedRigidbody.gameObject.GetComponent<SosigLink>())
                            {
                                SosigLink component = _targetArray[i].attachedRigidbody.gameObject.GetComponent<SosigLink>();
                                if (component.BodyPart == SosigLink.SosigBodyPart.Head)
                                {
                                    if (direction.magnitude > 1f)
                                    {
                                        component.Damage(new Damage
                                        {
                                            Dam_Blinding = BlindingAtOneMeter / (Mathf.Pow(direction.magnitude, 2))
                                        });
                                    }
                                    else
                                    {
                                        component.Damage(new Damage
                                        {
                                            Dam_Blinding = BlindingAtOneMeter
                                        });
                                    }
                                }
                            }
                            
                        }
                    }
                }

                //Debug.Log("Is now " + LightToStrobe.activeSelf);
                //Wait the strobe time
                yield return new WaitForSeconds(StrobeFlashTime);
            }
        }

#endif
    }
}



