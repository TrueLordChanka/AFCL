using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class BackBlast : MonoBehaviour
    {
        public FVRFireArm Firearm;
        public List<GameObject> SpawnOnSplode;

#if !(UNITY_EDITOR || UNITY_5)

        public void Awake()
        {
            GM.CurrentSceneSettings.ShotFiredEvent += CurrentSceneSettings_ShotFiredEvent;
        }

        private void CurrentSceneSettings_ShotFiredEvent(FVRFireArm firearm)
        {
            if (Firearm == firearm)
            {
                for (int j = 0; j < this.SpawnOnSplode.Count; j++)
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.SpawnOnSplode[j], base.transform.position, Quaternion.identity);
                    gameObject.transform.eulerAngles = transform.eulerAngles; //This should hopefully solve the angle issues
                                        
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
                

            }
        }

        public void OnDestroy()
        {
            GM.CurrentSceneSettings.ShotFiredEvent -= CurrentSceneSettings_ShotFiredEvent;
        }

#endif
    }
}



