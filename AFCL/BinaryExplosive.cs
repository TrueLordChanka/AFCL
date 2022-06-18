using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class BinaryExplosive : MonoBehaviour, IFVRDamageable
    {

        public FVRPhysicalObject BaseObj;
        public float Durability;
        public GameObject[] SpawnOnDet;
        public bool IsDebug = false;

        private float _damageTaken;

#if !(UNITY_EDITOR || UNITY_5)
        

        public void Update()
        {
            
            if(_damageTaken >= Durability)
            {
                for(int i = 0; i < SpawnOnDet.Length; i++)
                {
                    //Spawn the game object
                    GameObject _gameObject = UnityEngine.Object.Instantiate<GameObject>(SpawnOnDet[i], this.transform.position, Quaternion.identity);
                    //correctly set its IFF
                    Explosion _component = _gameObject.GetComponent<Explosion>();
                    if (_component != null)
                    {
                        _component.IFF = GM.CurrentPlayerBody.GetPlayerIFF();
                    }

                    ExplosionSound _component2 = _gameObject.GetComponent<ExplosionSound>();
                    if(_component2 != null)
                    {
                        _component2.IFF = GM.CurrentPlayerBody.GetPlayerIFF();
                    }
                    GrenadeExplosion _component3 = _gameObject.GetComponent<GrenadeExplosion>();
                    if( _component3 != null)
                    {
                        _component3.IFF = GM.CurrentPlayerBody.GetPlayerIFF();
                    }

                }

                //If the object is held make it not held
                if (BaseObj.IsHeld)
                {
                    FVRViveHand _hand = BaseObj.m_hand;
                    _hand.ForceSetInteractable(null);
                    BaseObj.EndInteraction(_hand);
                }

                //Destroy the object itself
                Destroy(BaseObj.gameObject);

            }
        }
		
        public void Damage(Damage d)
        {
            _damageTaken = _damageTaken + d.Dam_TotalKinetic;
            if (IsDebug)
            {
                Debug.Log("Damange taken: " + d.Dam_TotalKinetic);
                Debug.Log("Total Damange taken: " + _damageTaken);
            }
            
        }
#endif
    }
}



