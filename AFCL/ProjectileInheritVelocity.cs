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
    public class ProjectileInheritVelocity : MonoBehaviour
    {
        public BallisticProjectile parentRound;

		public List<bool> usesParentSpeed;
		public float ParentSpeedMultiplier = 1f;
		public float InitialSpeed;
		private float _parentRoundVel;

#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)

		public void Awake()
        {
			_parentRoundVel = InitialSpeed;
            Hook(); 
        } 

        public void Hook()
        {
            //Debug.Log("Hooking Ballistic Projectile");
            On.FistVR.BallisticProjectile.FireSubmunitions += BallisticProjectile_FireSubmunitions;
        }

		public void Update()
		{
			if (!parentRound.m_hasFiredSubmunitions)
			{
				//Debug.Log("perentRndvel:" + parentRound.m_velocity.magnitude);
				if (parentRound.m_velocity.magnitude > 1)
				{
					_parentRoundVel = parentRound.m_velocity.magnitude;
				}
			}
		}

		private void BallisticProjectile_FireSubmunitions(On.FistVR.BallisticProjectile.orig_FireSubmunitions orig, BallisticProjectile self, Vector3 shatterRicochetDir, Vector3 velNorm, Vector3 hitPoint, float VelocityOverride)
        {
			if (self == parentRound)
			{
				if (self.m_usesSubmunitions && !self.m_hasFiredSubmunitions)
				{
					self.m_hasFiredSubmunitions = true;
					for (int i = 0; i < self.Submunitions.Count; i++)
					{
						BallisticProjectile.Submunition submunition = self.Submunitions[i];
						Vector3 vector = shatterRicochetDir;
						Vector3 vector2 = hitPoint;
						for (int j = 0; j < submunition.NumToSpawn; j++)
						{
							GameObject original = submunition.Prefabs[UnityEngine.Random.Range(0, submunition.Prefabs.Count)];
							
							float launchVel;
							if (usesParentSpeed[i])
							{
								if (_parentRoundVel < 10f)
								{
									launchVel = 10f;
								}
								else
								{
									launchVel = _parentRoundVel * ParentSpeedMultiplier;
								}
							}
							else
							{
								launchVel = UnityEngine.Random.Range(submunition.Speed.x, submunition.Speed.y);
							}
							//Debug.Log("launchvel:" + launchVel);


							switch (submunition.Trajectory)
							{
								case BallisticProjectile.Submunition.SubmunitionTrajectoryType.Random:
									vector = UnityEngine.Random.onUnitSphere;
									break;
								case BallisticProjectile.Submunition.SubmunitionTrajectoryType.Backwards:
									vector = Vector3.Lerp(-velNorm, shatterRicochetDir, 0.5f);
									break;
								case BallisticProjectile.Submunition.SubmunitionTrajectoryType.Forwards:
									vector = velNorm;
									break;
								case BallisticProjectile.Submunition.SubmunitionTrajectoryType.ForwardsCone:
									vector = Vector3.Lerp(UnityEngine.Random.onUnitSphere, velNorm, submunition.ConeLerp);
									break;
							}
							BallisticProjectile.Submunition.SubmunitionSpawnLogic spawnLogic = submunition.SpawnLogic;
							if (spawnLogic != BallisticProjectile.Submunition.SubmunitionSpawnLogic.Inside)
							{
								if (spawnLogic == BallisticProjectile.Submunition.SubmunitionSpawnLogic.Outside)
								{
									vector2 += self.m_hit.normal * 0.001f;
								}
							}
							else
							{
								vector2 -= self.m_hit.normal * 0.001f;
							}
							GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original, vector2, Quaternion.LookRotation(vector));
							switch (submunition.Type)
							{
								case BallisticProjectile.Submunition.SubmunitionType.GameObject:
									{
										Explosion component = gameObject.GetComponent<Explosion>();
										if (component != null)
										{
											component.IFF = self.Source_IFF;
										}
										ExplosionSound component2 = gameObject.GetComponent<ExplosionSound>();
										if (component2 != null)
										{
											component2.IFF = self.Source_IFF;
										}
										break;
									}
								case BallisticProjectile.Submunition.SubmunitionType.Projectile:
									{
										BallisticProjectile component3 = gameObject.GetComponent<BallisticProjectile>();
										component3.Source_IFF = self.Source_IFF;
										component3.Fire(launchVel, gameObject.transform.forward, null, true);
										break;
									}
								case BallisticProjectile.Submunition.SubmunitionType.Rigidbody:
									gameObject.GetComponent<Rigidbody>().velocity = vector * launchVel;
									break;
								case BallisticProjectile.Submunition.SubmunitionType.StickyBomb:
									gameObject.GetComponent<Rigidbody>().velocity = vector * VelocityOverride;
									if (self.PassesFirearmReferenceToSubmunitions)
									{
										MF2_StickyBomb component4 = gameObject.GetComponent<MF2_StickyBomb>();
										component4.SetIFF(self.Source_IFF);
										if (component4 != null && self.tempFA != null && (self.tempFA as ClosedBoltWeapon).UsesStickyDetonation)
										{
											(self.tempFA as ClosedBoltWeapon).RegisterStickyBomb(component4);
										}
									}
									break;
								case BallisticProjectile.Submunition.SubmunitionType.MeleeThrown:
									gameObject.GetComponent<Rigidbody>().velocity = vector * Mathf.Max(launchVel, self.m_initialMuzzleVelocity);
									break;
								case BallisticProjectile.Submunition.SubmunitionType.Demonade:
									gameObject.GetComponent<Rigidbody>().velocity = vector * launchVel;
									gameObject.GetComponent<MF2_Demonade>().SetIFF(self.Source_IFF);
									break;
							}
						}
					}
				}
            }
            else
            {
				orig(self, shatterRicochetDir, velNorm, hitPoint, VelocityOverride);
            }
		}

        public void OnDestroy()
		{
			Unhook();
		}

		public void Unhook()
		{
			On.FistVR.BallisticProjectile.FireSubmunitions -= BallisticProjectile_FireSubmunitions;
		}

#endif

    }
}



