using FistVR;
using System.Collections.Generic;
using UnityEngine;


namespace AndrewFTW
{
    public class SubprojInRichochetDirection : MonoBehaviour
    {
        public BallisticProjectile parentRound;
        
        [Header("Submunitions")]
		public List<BallisticProjectile.Submunition> TangentMunitions;
		public List<bool> usesParentSpeed;
		public float ParentSpeedMultiplier = 1f;

		private bool m_hasFiredTangentMunitions;
		private float _parentRoundVel;


		
		

		public void Update()
		{
            if (!m_hasFiredTangentMunitions)
            {
				Debug.Log("perentRndvel:" + parentRound.m_velocity.magnitude);
				if (parentRound.m_velocity.magnitude > 1) {
					_parentRoundVel = parentRound.m_velocity.magnitude;
				}
			}
		}


#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)

		public void Awake()
		{
			_parentRoundVel = 120f;
			Hook();
		}

		public void Hook()
        {
            Debug.Log("Hooking Ballistic Projectile");
            On.FistVR.BallisticProjectile.FireSubmunitions += BallisticProjectile_FireSubmunitions;
        }
		

        private void BallisticProjectile_FireSubmunitions(On.FistVR.BallisticProjectile.orig_FireSubmunitions orig, BallisticProjectile self, Vector3 shatterRicochetDir, Vector3 velNorm, Vector3 hitPoint, float VelocityOverride)
        {

			//Debug.Log("into the hook");
            if(parentRound == self)
            {
				//Debug.Log("parent round is indeed self");
				if (!m_hasFiredTangentMunitions)
				{
					//Debug.Log("Has not fired tangent munitin");
					this.m_hasFiredTangentMunitions = true;
					for (int i = 0; i < this.TangentMunitions.Count; i++)
					{
						//Debug.Log("loop 1: " + i);
						//BallisticProjectile.Submunition submunition = this.TangentMunitions[i];
						BallisticProjectile.Submunition submunition = TangentMunitions[i];
						Vector3 vector = shatterRicochetDir;
						Vector3 vector2 = hitPoint;
						for (int j = 0; j < submunition.NumToSpawn; j++)
						{
							//Debug.Log("loop 2: " + j);
							GameObject original = submunition.Prefabs[UnityEngine.Random.Range(0, submunition.Prefabs.Count)];
							float launchVel;

							if (usesParentSpeed[i])
							{
								if(_parentRoundVel < 10f)
                                {
									launchVel = 10f;
                                } else
                                {
									launchVel = _parentRoundVel * ParentSpeedMultiplier;
								}
								
								Debug.Log("Launch vel:" + launchVel);
								Debug.Log("Parent round vel:" + parentRound.m_velocity.magnitude);
							}
							else
							{
								launchVel = UnityEngine.Random.Range(submunition.Speed.x, submunition.Speed.y);
								//Debug.Log("Doesnt use parent speed" );

							}

							/* This has to be replaced with the stuff for bouncing...
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
							*/
							//Set the new trajectory to be the richochet
							//Get the "normal" vector to the impact surface
							Vector3 NormVector = parentRound.m_hit.normal;
								//Vector3.Lerp(-velNorm, shatterRicochetDir, 0.5f);
							//Set the submunition vector to be that calculated angle from the norm
							vector = Vector3.Reflect(velNorm, NormVector);

							BallisticProjectile.Submunition.SubmunitionSpawnLogic spawnLogic = submunition.SpawnLogic;
							if (spawnLogic != BallisticProjectile.Submunition.SubmunitionSpawnLogic.Inside)
							{
								if (spawnLogic == BallisticProjectile.Submunition.SubmunitionSpawnLogic.Outside)
								{
									vector2 += parentRound.m_hit.normal * 0.001f;
								}
							}
							else
							{
								vector2 -= parentRound.m_hit.normal * 0.001f;
							}


							GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original, vector2, Quaternion.LookRotation(vector));
							switch (submunition.Type)
							{
								case BallisticProjectile.Submunition.SubmunitionType.GameObject:
									{
										Explosion component = gameObject.GetComponent<Explosion>();
										if (component != null)
										{
											component.IFF = parentRound.Source_IFF;
										}
										ExplosionSound component2 = gameObject.GetComponent<ExplosionSound>();
										if (component2 != null)
										{
											component2.IFF = parentRound.Source_IFF;
										}
										break;
									}
								case BallisticProjectile.Submunition.SubmunitionType.Projectile:
									{
										BallisticProjectile component3 = gameObject.GetComponent<BallisticProjectile>();
										component3.Source_IFF = parentRound.Source_IFF;
										component3.Fire(launchVel, gameObject.transform.forward, null, true);
										break;
									}
								case BallisticProjectile.Submunition.SubmunitionType.Rigidbody:
									gameObject.GetComponent<Rigidbody>().velocity = vector * launchVel;
									break;
								case BallisticProjectile.Submunition.SubmunitionType.StickyBomb:
									gameObject.GetComponent<Rigidbody>().velocity = vector * VelocityOverride;
									if (parentRound.PassesFirearmReferenceToSubmunitions)
									{
										MF2_StickyBomb component4 = gameObject.GetComponent<MF2_StickyBomb>();
										component4.SetIFF(parentRound.Source_IFF);
										if (component4 != null && parentRound.tempFA != null && (parentRound.tempFA as ClosedBoltWeapon).UsesStickyDetonation)
										{
											(parentRound.tempFA as ClosedBoltWeapon).RegisterStickyBomb(component4);
										}
									}
									break;
								case BallisticProjectile.Submunition.SubmunitionType.MeleeThrown:
									gameObject.GetComponent<Rigidbody>().velocity = vector * Mathf.Max(launchVel, parentRound.m_initialMuzzleVelocity);
									break;
								case BallisticProjectile.Submunition.SubmunitionType.Demonade:
									gameObject.GetComponent<Rigidbody>().velocity = vector * launchVel;
									gameObject.GetComponent<MF2_Demonade>().SetIFF(parentRound.Source_IFF);
									break;
							}
						}
					}
				}

			} else
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



