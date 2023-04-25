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
		public static bool DoesUseArmingDistance2;

		private static Dictionary<BallisticProjectile, ProjectileArmingDistance> _existingProjArmDists = new Dictionary<BallisticProjectile, ProjectileArmingDistance>();

#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)

		static ProjectileArmingDistance()
        {
			if (!DoesUseArmingDistance2)
			{
				On.FistVR.BallisticProjectile.MoveBullet += BallisticProjectile_MoveBullet;
			}
		}

		private static void BallisticProjectile_MoveBullet(On.FistVR.BallisticProjectile.orig_MoveBullet orig, BallisticProjectile self, float t)
		{
			orig(self, t); //run the origional part of move bullet
			ProjectileArmingDistance _projArmDist;

			if (_existingProjArmDists.TryGetValue(self, out _projArmDist))
			{
				if (_projArmDist.parentRound.m_distanceTraveled >= _projArmDist.armingDistance)
				{
					_projArmDist.parentRound.Submunitions = _projArmDist.ArmedMunitions;
				}
			}

		}

		public void Awake()
		{
			_existingProjArmDists.Add(parentRound, this);
		}

		public void OnDestroy()
        {
			_existingProjArmDists.Remove(parentRound);
		}

#endif

	}
}



