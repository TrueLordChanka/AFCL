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
    public class ProjectileAirburstDetonation : MonoBehaviour
    {
        public BallisticProjectile parentRound;

		public float StandoffDistance = 5f;
		public Transform SensorDirectionOverride;
		public Transform SubProjDirectionOverride;


		private Vector3 _sensorDirection;
		private Vector3 _subProjDirection;
		private RaycastHit _raycastHit;

#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)

		public void Awake()
        {
			
			Hook();

		}

		public void Hook()
        {
            On.FistVR.BallisticProjectile.MoveBullet += BallisticProjectile_MoveBullet;
		}

        private void BallisticProjectile_MoveBullet(On.FistVR.BallisticProjectile.orig_MoveBullet orig, BallisticProjectile self, float t)
        {
			if(parentRound == self)
            {
                if (!CheckAirburst())
                {
					orig(self, t);
				}
            }
            else
            {
				orig(self, t);
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

		public bool CheckAirburst()
		{
			if (!parentRound.m_hasFiredSubmunitions)
			{
				//Debug.Log("hasnt Raycast");


				//Set the sensor direction to be that of the override if not null
				if (SensorDirectionOverride != null)
				{
					_sensorDirection = SensorDirectionOverride.forward;
				}
				else
				{
					_sensorDirection = parentRound.transform.forward;
				}
				//Set the fire direction to be that of the override if not null
				if (SubProjDirectionOverride != null)
				{
					_subProjDirection = SubProjDirectionOverride.forward;
				}
				else
				{
					_subProjDirection = parentRound.transform.forward;
				}


				float _frameDistance;

				if (parentRound.m_velocity.magnitude == 0)
				{
					_frameDistance = Time.fixedDeltaTime * parentRound.m_initialMuzzleVelocity + StandoffDistance;
				}
				else
				{
					_frameDistance = Time.fixedDeltaTime * parentRound.m_velocity.magnitude + StandoffDistance;
				}

				//If the raycast hits somthing thats on its layer mask, then its time to fire!
				if (Physics.Raycast(parentRound.transform.position, _sensorDirection, out _raycastHit, _frameDistance, parentRound.LM, QueryTriggerInteraction.Collide))
				{

					//Debug.Log("Raycast");
					//set the max range to 0 so the next check will always return that its gotta blow up
					parentRound.MaxRange = 0f;

					//move the bullet where I want it to be when it fires the submunitions
					float _distance = _raycastHit.distance - StandoffDistance;
					//Debug.Log("Distance is " + _distance + " Raycast distance is " + _raycastHit.distance);

					if (_distance > 0f)
					{
						//the position is the normalized vector - the standoff distance
						parentRound.transform.position = parentRound.transform.position + parentRound.m_velocity.normalized * _distance;
					}

					//parentRound.transform.position = parentRound.transform.position + parentRound.m_velocity.normalized * _distance;

					//Set the rotation of the bullet to be angled in the direction that its gonna shoot
					parentRound.transform.rotation = Quaternion.LookRotation(_subProjDirection);

					//fire the submunitions
					parentRound.FireSubmunitions(parentRound.m_velocity.normalized, parentRound.m_velocity.normalized, parentRound.transform.position, parentRound.m_velocity.magnitude);

					return true;
				}
			}
			return false;
		}

		

#endif

    }
}



