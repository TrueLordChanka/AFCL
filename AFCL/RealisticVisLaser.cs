using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using FistVR;
using Unity;
using System.Collections;


namespace AndrewFTW
{
	public class RealisticVisLaser : MonoBehaviour
	{
		/*
		 * Laser has a "falloff" range in which the opacity goes from 1 to 0 (look at energy sword for details, use coroutines this time lol)
		 * Laser dot scales at distance, but only slightly, not as dramatic as antons.
		 * 
		 * 
		 */

		public Transform Aperature;
		[Header("Visible parts:")]
		public GameObject HitObject; //The visible part that hits the wall
		public LayerMask LayerMask; //what layers the laser can actually hit
		public float ScaleFactor = 0.2f;


		private RaycastHit _hit;
		//public float FalloffStartRange = 150f;
		//public float FalloffEndRange = 200f;
		//public List<Material> HitMaterialList;
		//private float FalloffRatio;


#if !(UNITY_EDITOR || UNITY_5)
		


		public void Update()
        {
            if (this.isActiveAndEnabled)//This could be a cause of issues since ive never used .isActiveAndEnabled before. just make sure its actie before doing this work every frame
            { //we know its on to be in here
			  //First we should cast our ray iot see where we are gonna hit

				//this moves the hit point really far out to start off
				Vector3 HitPos = Aperature.position + Aperature.forward * 2000f;

				//now we need to do the raycast IOT see where this should actally hit
				if(Physics.Raycast(Aperature.position, Aperature.forward, out _hit, 2300f, LayerMask, QueryTriggerInteraction.Ignore))
                { //from the aperature position pointing forward output the hit to _hit with a max range of 2300, hitting anything on Layermask and ignoring trigger colliders.
				  //when in this statement we know it actually hit somthing.
					HitPos = _hit.point;
					
					//Move the laser dot to the actual hit position
					HitObject.transform.position = HitPos;



                }
				HitObject.transform.position = HitPos;
				//Anton code im not 100% sure about
				float t = _hit.distance * 0.01f;
				float dotScale = Mathf.Lerp(0.01f, ScaleFactor,t);
				HitObject.transform.localScale = new Vector3(dotScale,dotScale,dotScale);

			} 

		}

		

		

#endif

	}
}



