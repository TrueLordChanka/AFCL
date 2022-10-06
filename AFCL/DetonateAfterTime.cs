using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class DetonateAfterTime : MonoBehaviour
    {
        public float FuseTime;
        public List<GameObject> SpawnOnSplode;

        private float _timeElapsed = 0;
        private bool _hasExploded = false;
#if !(UNITY_EDITOR || UNITY_5)
        
        public void FixedUpdate()
        { 
            if(_timeElapsed < FuseTime)
            {
                //if the time elapsed is less than the fuse time then increase it by the time since last check (1 frame)
                _timeElapsed += Time.fixedDeltaTime;
            } else
            {
                if (!_hasExploded)
                {
                    _hasExploded = true;

                    //Its time to detonate! WOOOOOOOOOOOOO
                    for (int j = 0; j < this.SpawnOnSplode.Count; j++)
                    {
                        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.SpawnOnSplode[j], base.transform.position, Quaternion.identity);
                        Explosion component = gameObject.GetComponent<Explosion>();
                        //get the player iff
                        int PIFF = GM.CurrentPlayerBody.GetPlayerIFF();

                        if (component != null)
                        {
                            component.IFF = PIFF;
                        }
                        ExplosionSound component2 = gameObject.GetComponent<ExplosionSound>();
                        if (component2 != null)
                        {
                            component2.IFF = PIFF;
                        }
                        GrenadeExplosion component3 = gameObject.GetComponent<GrenadeExplosion>();
                        if (component3 != null)
                        {
                            component3.IFF = PIFF;
                        }
                    }
                    UnityEngine.Object.Destroy(base.gameObject);
                }
            }





        }
        
#endif
    }
}



