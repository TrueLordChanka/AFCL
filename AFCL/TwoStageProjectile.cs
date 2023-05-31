
using UnityEngine;
using FistVR;
using Unity;



namespace AndrewFTW
{
	public class TwoStageProjectile : MonoBehaviour
	{
		public BallisticProjectile ParentRound;
		
		public float Stage1Distance;
		public float Stage2Acceleration;
		public float Stage2AccelerationTime;
		public GameObject Stage2DisplayItem;
		

#if !(UNITY_EDITOR || UNITY_5)
		public void Update()
        {
			if(ParentRound.m_distanceTraveled >= Stage1Distance && Stage2AccelerationTime > 0)
            { //time to begine the acceleration as the staging distance is 
				ParentRound.m_velocity += ParentRound.m_velocity.normalized * Stage2Acceleration * Time.deltaTime;
				//add the acceleration of the projectile to the velocity
				Stage2AccelerationTime -= Time.deltaTime;
				//accounting for the motors fuel
				Stage2DisplayItem.SetActive(true);
            } else
            {
				Stage2DisplayItem.SetActive(false);
            }
        }
#endif
	}
}



