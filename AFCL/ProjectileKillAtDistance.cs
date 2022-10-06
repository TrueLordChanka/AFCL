using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;
using System.Collections;


namespace AndrewFTW
{ 
    public class ProjectileKillAtDistance : MonoBehaviour
    {
        public BallisticProjectile ParentRound;
        public float DistanceToKillAt; 


#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)

		public void Awake()
        {
            Hook();
        } 

        public void Hook()
        {
            Debug.Log("Hooking Ballistic Projectile");
            On.FistVR.BallisticProjectile.FixedUpdate += BallisticProjectile_FixedUpdate;
            
        }

		public void OnDestroy()
		{
			Unhook();
		}

		public void Unhook()
		{
			On.FistVR.BallisticProjectile.FixedUpdate -= BallisticProjectile_FixedUpdate;
		}

		private void BallisticProjectile_FixedUpdate(On.FistVR.BallisticProjectile.orig_FixedUpdate orig, BallisticProjectile self)
        {
            orig(self);//run the normal "fixed Update" thing
            if(self == ParentRound) //This is where all the code will go to ensure that the round is the correct one being hooked.
            {
                ///parentRound.m_distanceTraveled  //This is the variable I need
                if(ParentRound.m_distanceTraveled >= DistanceToKillAt)//if it has traveled further than the distanec we want, fire the subproj.
                {
                    ParentRound.m_velocity = new Vector3(0,0,0);
                    ParentRound.m_isMoving = false;
                    ParentRound.TickDownToDeath();
					
                }
            }           
        }

#endif

    }
}



