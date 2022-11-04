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

		[Header("Smart Ricochet")]
		public bool IsSmartRicochet;
		public float TargetingFOV = 5f;
		public float TargetingRange = 20f;
		public LayerMask LM_TargetMask;
		public LayerMask LM_Blockers;


		private bool m_hasFiredTangentMunitions;
		private float _parentRoundVel;

		private Collider[] _targetArray = new Collider[32];
		private float _overlapCapsulRadius;

		public void Update()
		{
            if (!m_hasFiredTangentMunitions)
            {
				//Debug.Log("perentRndvel:" + parentRound.m_velocity.magnitude);
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
			_overlapCapsulRadius = Mathf.Abs(Mathf.Tan(TargetingFOV * Mathf.Deg2Rad) * TargetingRange);

			//Debug.Log("Radius = " + _overlapCapsulRadius + " Target FOV w/ math is" + TargetingFOV * Mathf.Deg2Rad + " range = " + TargetingRange);
			
		}

		public void Hook()
        {
            //Debug.Log("Hooking Ballistic Projectile");
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
						Vector3 _richochetVector = shatterRicochetDir;
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
								
								//Debug.Log("Launch vel:" + launchVel);
								//Debug.Log("Parent round vel:" + parentRound.m_velocity.magnitude);
							}
							else
							{
								launchVel = UnityEngine.Random.Range(submunition.Speed.x, submunition.Speed.y);
								//Debug.Log("Doesnt use parent speed" );

							}

							
							//Set the new trajectory to be the richochet
							//Get the "normal" vector to the impact surface
							Vector3 NormVector = parentRound.m_hit.normal;
								//Vector3.Lerp(-velNorm, shatterRicochetDir, 0.5f);
							//Set the submunition vector to be that calculated angle from the norm
							_richochetVector = Vector3.Reflect(velNorm, NormVector);

                            //Now, if its a smart richochet, we need to do that stuff
                            if (IsSmartRicochet)
                            {
								//makes a capsul between two points, with the layer maks n shit
								//Debug.Log(_overlapCapsulRadius);
								int NumTargets = Physics.OverlapCapsuleNonAlloc(transform.position + 0.1f * _richochetVector, transform.position + TargetingRange * _richochetVector, _overlapCapsulRadius, _targetArray, LM_TargetMask, QueryTriggerInteraction.Collide);
								Debug.Log(NumTargets);

								if(_targetArray.Length > 0) //if there arnt any targets dont do anything
                                {
									//Debug.Log("Target");
									Vector3 _aimedRichochetVector;
									int _validTarget;

									int _SelectedLink = UnityEngine.Random.Range(0, 2);

									for (int k = 0; k < NumTargets; k++)
                                    {
										int _SelectedTarget = UnityEngine.Random.Range(0, NumTargets); //select a random target

										_aimedRichochetVector = _targetArray[_SelectedTarget].GetComponent<Rigidbody>().GetComponent<Sosig>().Links[_SelectedLink].transform.position;

										//_aimedRichochetVector = _targetArray[_SelectedTarget].transform.position - transform.position;

										if (Vector3.Angle(_aimedRichochetVector, _richochetVector) < TargetingFOV && !Physics.Linecast(transform.position, _targetArray[_SelectedTarget].transform.position - transform.forward * 0.2f, LM_Blockers))
										{
											//Aim the actual vector to the aimed vector
											_richochetVector = _aimedRichochetVector;
											break;
										}
									}


									/*
									//int _SelectedTarget = UnityEngine.Random.Range(0, NumTargets);
									
									//The aimed vector is the position between the target and the transform
									_aimedRichochetVector = _targetArray[_SelectedTarget].transform.position - transform.position;

									//If the line is withing the cone and it doesnt hit any blockers...
									if (Vector3.Angle(_aimedRichochetVector, _richochetVector) < TargetingFOV && !Physics.Linecast(transform.position, _targetArray[_SelectedTarget].transform.position - transform.forward * 0.2f, LM_Blockers))
									{
										//Aim the actual vector to the aimed vector
										_richochetVector = _aimedRichochetVector;
									}
									else //If the check doesnt succeed on the first target, 
									{
										for (int k = 0; k < NumTargets; k++)
										{
											_aimedRichochetVector = _targetArray[k].transform.position - transform.position;
											if (Vector3.Angle(_aimedRichochetVector, _richochetVector) < TargetingFOV && !Physics.Linecast(transform.position, _targetArray[k].transform.position - transform.forward * 0.2f, LM_Blockers))
											{
												_richochetVector = _aimedRichochetVector;

												k = 40;


											}
										}
									}
									*/
								}
							}


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

							//Spawn the richochet proj
                            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original, vector2, Quaternion.LookRotation(_richochetVector));

							//Do case by case stuff for differnet types
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
									gameObject.GetComponent<Rigidbody>().velocity = _richochetVector * launchVel;
									break;
								case BallisticProjectile.Submunition.SubmunitionType.StickyBomb:
									gameObject.GetComponent<Rigidbody>().velocity = _richochetVector * VelocityOverride;
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
									gameObject.GetComponent<Rigidbody>().velocity = _richochetVector * Mathf.Max(launchVel, parentRound.m_initialMuzzleVelocity);
									break;
								case BallisticProjectile.Submunition.SubmunitionType.Demonade:
									gameObject.GetComponent<Rigidbody>().velocity = _richochetVector * launchVel;
									gameObject.GetComponent<MF2_Demonade>().SetIFF(parentRound.Source_IFF);
									break;
							}


							//Delete self after a few cm so it doesnt keep going on a Y
							//itll go ~10cm more then die
							parentRound.MaxRange = parentRound.m_distanceTraveled + 0.1f;


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



