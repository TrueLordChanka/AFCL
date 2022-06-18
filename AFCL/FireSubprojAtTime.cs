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
    public class FireSubprojAtTime : MonoBehaviour
    {
        public BallisticProjectile parentRound;
        public float timeToFire;

        [Header("Tangent Munitions")]
        public List<c_TangentMunition> TangentMunitions; //List of class c_TangentMunitions


		
        [Serializable]
        public class c_TangentMunition //random shit for the tangent projectiles
        {
            public List<GameObject> Prefabs;
            public int NumToSpawn;
            public BallisticProjectile.Submunition.SubmunitionTrajectoryType Trajectory;
            public BallisticProjectile.Submunition.SubmunitionType Type;
            public BallisticProjectile.Submunition.SubmunitionSpawnLogic SpawnLogic;
            public Vector2 Speed = default(Vector2);
			public bool usesParentSpeed;
            public float ConeLerp = 0.85f;
        
            public enum SubmunitionType 
            {
                GameObject,
                Projectile,
                Rigidbody,
                StickyBomb,
                MeleeThrown,
                Demonade
            }
            public enum SubmunitionTrajectoryType
            {
                Random,
                RicochetDir,
                Backwards,
                Forwards,
                ForwardsCone
            }
            public enum SubmunitionSpawnLogic
            {
                Outside,
                Inside,
                On
            }
        }
        private bool m_hasFiredTangentMunitions;
		private float currLifeTime= 0;

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

		public void Update()
        {
			
		}

        private void BallisticProjectile_FixedUpdate(On.FistVR.BallisticProjectile.orig_FixedUpdate orig, BallisticProjectile self)
        {
            orig(self);//run the normal "fixed Update" thing
			
			if (self == parentRound) //This is where all the code will go to ensure that the round is the correct one being hooked.
            {

				
				///parentRound.m_distanceTraveled  //This is the variable I need
				if (parentRound.m_dieTimerTick <= 0.05f)//if it has traveled further than the distanec we want, fire the subproj.
                {
					Debug.Log("Time to die");
					Vector3 normalized = parentRound.m_velocity.normalized;
					FireTangentMunitions(normalized, normalized, base.transform.position,parentRound.m_velocity.magnitude);
                }
            }           
        }

        public void FireTangentMunitions(Vector3 shatterRicochetDir, Vector3 velNorm, Vector3 hitPoint, float VelocityOverride)
        {
            if (!m_hasFiredTangentMunitions)
            {
				this.m_hasFiredTangentMunitions = true;
				for (int i = 0; i < this.TangentMunitions.Count; i++)
				{
					//BallisticProjectile.Submunition submunition = this.TangentMunitions[i];
					c_TangentMunition submunition = TangentMunitions[i];
					Vector3 vector = shatterRicochetDir;
					Vector3 vector2 = hitPoint;
					for (int j = 0; j < submunition.NumToSpawn; j++)
					{
						GameObject original = submunition.Prefabs[UnityEngine.Random.Range(0, submunition.Prefabs.Count)];
						float launchVel;

						if (submunition.usesParentSpeed)
                        {
							launchVel = parentRound.m_velocity.magnitude;
                        }
                        else
                        {
							launchVel = UnityEngine.Random.Range(submunition.Speed.x, submunition.Speed.y);
						}
						
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
        }




#endif

    }
}



