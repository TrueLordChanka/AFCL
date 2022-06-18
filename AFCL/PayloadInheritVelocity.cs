using FistVR;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AndrewFTW
{
    public class PayloadInheritVelocity : MonoBehaviour
    {
        public PinnedGrenade ParentGrenade;

        public GameObject[] SpawnOnExplode;
        public bool[] SplodeObjsToInheritVelocity;


        public void Update()
        {
            
            if (ParentGrenade.m_fuseTime <= 0.1 && !ParentGrenade.m_hasSploded)
            {
                Debug.Log("HasSploded!");
                for (int i = 0; i < SpawnOnExplode.Length; i++)
                {
                    Debug.Log("iteration " + i);
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(SpawnOnExplode[i], base.transform.position, Quaternion.identity);
                    if (gameObject.GetComponent<Explosion>() != null)
                    {
                        Explosion component = gameObject.GetComponent<Explosion>();
                        int IFF = GM.CurrentPlayerBody.GetPlayerIFF();
                        if (component != null)
                        {
                            component.IFF = IFF;
                        }
                        ExplosionSound component2 = gameObject.GetComponent<ExplosionSound>();
                        if (component2 != null)
                        {
                            component2.IFF = IFF;
                        }
                        GrenadeExplosion component3 = gameObject.GetComponent<GrenadeExplosion>();
                        if (component3 != null)
                        {
                            component3.IFF = IFF;
                        }
                    }
                    else
                    {
                        Debug.Log("FuckFuckFuckFuckFuckFuck");
                    }


                    if (SplodeObjsToInheritVelocity[i] == true)
                    {
                        //Give the new 'gameObject' the velocity of the parent, if it has the thing.
                        //This gets the velocity of the parent grenade.
                        Vector3 ParentVelocity = ParentGrenade.GetComponent<Rigidbody>().velocity;
                        //Now we need to apply this velocity.
                        gameObject.GetComponent<Rigidbody>().velocity = ParentVelocity;
                    }
                }
            }
        }
#if !(UNITY_EDITOR || UNITY_5)





#endif

    }
}



