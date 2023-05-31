using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 

    public class ReactiveTarget : ReactiveSteelTarget
    {
        public Vector2 BulletHoleSizeRange2 = new Vector2(0.075f, 0.12f);
		public Vector2 DamageScaleRange = new Vector2(400f, 2000f);



#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)

        public new void Awake()
        {
            base.Awake();
            base.BulletHoleSizeRange = BulletHoleSizeRange2;
            On.FistVR.ReactiveSteelTarget.Damage += ReactiveSteelTarget_Damage;
        }

        private void ReactiveSteelTarget_Damage(On.FistVR.ReactiveSteelTarget.orig_Damage orig, ReactiveSteelTarget self, Damage dam) //makes my thing work. this could be done more efficently using a dictionary or list but I cant be assed
        {
			Type selftype = self.GetType();
			Type reactiveType = typeof(ReactiveTarget);
            if(selftype == reactiveType)
            {
				//Debug.Log("UwU");
				Damage(dam, self);
            } else
            {
				orig(self, dam);
            }
        }

        public void Damage(Damage dam, ReactiveSteelTarget self)
        {
			if (dam.Class != FistVR.Damage.DamageClass.Projectile)
			{
				return;
			}
			Vector3 position = dam.point + dam.hitNormal * UnityEngine.Random.Range(0.001f, 0.005f);
			if (self.BulletHolePrefabs.Length > 0 && self.m_useHoles)
			{
				float t = Mathf.InverseLerp(DamageScaleRange.x, DamageScaleRange.y, dam.Dam_TotalKinetic);
				float num = Mathf.Lerp(self.BulletHoleSizeRange.x, self.BulletHoleSizeRange.y, t);
				if (self.m_currentHoles.Count > self.MaxHoles)
				{
					self.holeindex++;
					if (self.holeindex > self.MaxHoles - 1)
					{
						self.holeindex = 0;
					}
					self.m_currentHoles[self.holeindex].transform.position = position;
					self.m_currentHoles[self.holeindex].transform.rotation = Quaternion.LookRotation(dam.hitNormal, UnityEngine.Random.onUnitSphere);
					self.m_currentHoles[self.holeindex].transform.localScale = new Vector3(num, num, num);
				}
				else
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(self.BulletHolePrefabs[UnityEngine.Random.Range(0, self.BulletHolePrefabs.Length)], position, Quaternion.LookRotation(dam.hitNormal, UnityEngine.Random.onUnitSphere));
					gameObject.transform.localScale  = new Vector3(num, num, num);
					//gameObject.transform.SetParent(transform.parent);
					gameObject.transform.SetParent(self.transform);
					//Debug.Log(transform.parent.name + " Transform name");
					self.m_currentHoles.Add(gameObject);
				}
			}
			if (self.m_hasRB && self.AddForceJuice)
			{
				float d = Mathf.Clamp(dam.Dam_Blunt * 0.01f, 0f, self.MaxForceImpulse);
				Debug.DrawLine(dam.point, dam.point + dam.strikeDir * d, Color.red, 40f);
				self.rb.AddForceAtPosition(dam.strikeDir * d, dam.point, ForceMode.Impulse);
			}
			self.PlayHitSound(Mathf.Clamp(dam.Dam_TotalKinetic * 0.0025f, 0.05f, 1f));
		}





#endif
	}
}



