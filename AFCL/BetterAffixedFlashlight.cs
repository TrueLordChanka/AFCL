using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace FistVR
{
	public class BetterAffixedFlashlight : FVRInteractiveObject
	{
		public override void SimpleInteraction(FVRViveHand hand)
		{
			base.SimpleInteraction(hand);
			this.ToggleOn();
		}

		private void ToggleOn()
		{
			this.IsOn = !this.IsOn;
			this.LightParts.SetActive(this.IsOn);
			
			if (this.IsOn)
			{
				SM.PlayCoreSound(FVRPooledAudioType.GenericClose, this.AudEvent_LaserOnClip, base.transform.position);
			}
			else
			{
				SM.PlayCoreSound(FVRPooledAudioType.GenericClose, this.AudEvent_LaserOffClip, base.transform.position);
			}
		}

		private bool IsOn;

		public GameObject LightParts;

		public AudioEvent AudEvent_LaserOnClip;

		public AudioEvent AudEvent_LaserOffClip;

		
	}
}

