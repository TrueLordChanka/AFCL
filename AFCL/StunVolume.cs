using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using System.Collections.Generic;

namespace AndrewFTW
{
    [RequireComponent(typeof(Collider))] //each volume needs a collider

    public class StunVolume : MonoBehaviour
    {
        public float StunValue;
        public float StunTick;
        public AudioEvent StunSound;

        private Dictionary<Collider, Coroutine> coroutines = new Dictionary<Collider, Coroutine>();
        

#if !(UNITY_EDITOR || UNITY_5)


        void OnTriggerEnter(Collider collider) //when somthing enters the collider
        {
            //Debug.Log("Collision detected");
            LayerMask layer;
            
            layer = collider.gameObject.layer;
            //Debug.Log("b "+ layer.ToString()); //this next check isnt worken for sosigs
            if (layer == LayerMask.NameToLayer("AgentBody") && collider.attachedRigidbody.gameObject.GetComponent<SosigLink>() != null)//collider.gameObject.GetComponent<SosigLink>() != null&& collider.gameObject.GetComponent<SosigLink>().BodyPart == SosigLink.SosigBodyPart.Head
            {
                //Debug.Log("ValidTarget");
                Coroutine coroutine = StartCoroutine(DamageTick(collider));
                coroutines.Add(collider, coroutine);
            }
        }

        void OnTriggerExit(Collider collider)
        {
            //Debug.Log("Collision Exit");
            Coroutine coroutineToKill;
            if (coroutines.TryGetValue(collider, out coroutineToKill))
            {
                StopCoroutine(coroutineToKill);
                coroutines.Remove(collider);
            }
        }

        IEnumerator DamageTick(Collider collider)
        {
            while (true)
            {
                //Debug.Log("volume tick dealing " + VolumeGasData.Damage + "Damage, " + VolumeGasData.Blindness + " Blindness, with a "+ VolumeGasData.EffectTick + " Tick"); ;

                //determine if its a sosig or player
                if (collider.gameObject.layer == LayerMask.NameToLayer("AgentBody") ) //its a sosig, is it their head? //&& collider.GetComponent<SosigLink>().BodyPart == SosigLink.SosigBodyPart.Head
                {
                    //Debug.Log("UwU2");
                    SosigLink component = collider.attachedRigidbody.gameObject.GetComponent<SosigLink>();
                    SM.PlayGenericSound(StunSound, transform.position);
                    component.Damage(new Damage //apply both blinding and blunt damage to the sosig based on the damage
                    {
                        Dam_Stunning = StunValue
                    });;
                    

                }
                yield return new WaitForSeconds(StunTick); //wait for tick
            }
        }

        


#endif
    }
}
