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
	public class ProjectileArmingDistance : MonoBehaviour
	{
		public BallisticProjectile parentRound;
		public float armingDistance;


		[Header("Armed Subprojectiles")]
		public List<BallisticProjectile.Submunition> ArmedMunitions;

		//[HideInInspector]
		public static bool DoesUseArmingDistance;



#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)

		public void Awake()
		{
			//Debug.Log("Does use Arming Distance? " + DoesUseArmingDistance);

            if (!DoesUseArmingDistance)
            {
				parentRound.Submunitions = ArmedMunitions;
			}
			Hook();

			

		}

		public void Hook()
		{
			Debug.Log("Hooking Ballistic Projectile");
            On.FistVR.BallisticProjectile.MoveBullet += BallisticProjectile_MoveBullet;
		}

        private void BallisticProjectile_MoveBullet(On.FistVR.BallisticProjectile.orig_MoveBullet orig, BallisticProjectile self, float t)
        {
			orig(self, t); //run the origional part of move bullet
			if (parentRound == self)
			{
				if (self.m_distanceTraveled >= armingDistance)
				{
					self.Submunitions = ArmedMunitions;
				}
			}
            
        }

		public void OnDestroy()
        {
			Unhook();
        }

		public void Unhook()
        {
			On.FistVR.BallisticProjectile.MoveBullet -= BallisticProjectile_MoveBullet;
		}

#endif

	}
}



