using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;

namespace AFCL
{
    public class ClosedBoltFireAttachedFirearmWithFiremode : ClosedBoltWeapon
    {
		[NonSerialized]
		[HideInInspector]
		public float m_engagementDelay;

		public new enum FireSelectorModeType
		{
			Safe,
			Single,
			Burst,
			FullAuto,
			FireAttached
		}

		[Serializable]
		public new class FireSelectorMode
		{
			public float SelectorPosition;

			public ClosedBoltWeapon.FireSelectorModeType ModeType;

			public int BurstAmount = 3;

			public bool ARStyleBurst;

			public float EngagementDelay;
		}

		public new void Awake()
        {
			base.Awake();
            On.FistVR.ClosedBoltWeapon.Fire += ClosedBoltWeapon_Fire;
        }

        private bool ClosedBoltWeapon_Fire(On.FistVR.ClosedBoltWeapon.orig_Fire orig, ClosedBoltWeapon self)
        {
			if (!this.Chamber.Fire())
			{
				return false;
			}
			this.m_timeSinceFiredShot = 0f;
			float velMult = 1f;
			if (this.UsesStickyDetonation)
			{
				velMult = 1f + Mathf.Lerp(0f, this.StickyMaxMultBonus, this.m_stickyChargeUp);
			}
			base.Fire(this.Chamber, this.GetMuzzle(), true, velMult, -1f);
			bool twoHandStabilized = this.IsTwoHandStabilized();
			bool foregripStabilized = base.AltGrip != null;
			bool shoulderStabilized = this.IsShoulderStabilized();
			this.Recoil(twoHandStabilized, foregripStabilized, shoulderStabilized, null, 1f);
			bool flag = false;
			ClosedBoltWeapon.FireSelectorMode fireSelectorMode = this.FireSelector_Modes[this.m_fireSelectorMode];
			if (fireSelectorMode.ModeType == ClosedBoltWeapon.FireSelectorModeType.SuperFastBurst)
			{
				for (int i = 0; i < fireSelectorMode.BurstAmount - 1; i++)
				{
					if (this.Magazine.HasARound())
					{
						this.Magazine.RemoveRound();
						base.Fire(this.Chamber, this.GetMuzzle(), false, 1f, -1f);
						flag = true;
						this.Recoil(twoHandStabilized, foregripStabilized, shoulderStabilized, null, 1f);
					}
				}
			}
			this.FireMuzzleSmoke();
			if (this.UsesDelinker && this.HasBelt)
			{
				this.DelinkerSystem.Emit(1);
			}
			if (this.HasBelt)
			{
				this.BeltDD.AddJitter();
			}
			if (flag)
			{
				base.PlayAudioGunShot(false, this.Chamber.GetRound().TailClass, this.Chamber.GetRound().TailClassSuppressed, GM.CurrentPlayerBody.GetCurrentSoundEnvironment());
			}
			else
			{
				base.PlayAudioGunShot(this.Chamber.GetRound(), GM.CurrentPlayerBody.GetCurrentSoundEnvironment(), 1f);
			}
			if (this.ReciprocatesOnShot && (!this.HasForeHandle || this.ForeHandle.Mode == ClosedBoltForeHandle.ForeHandleMode.Locked))
			{
				this.Bolt.ImpartFiringImpulse();
			}
			return true;
		}

        public new void OnDestroy()
        {
			base.OnDestroy();
			On.FistVR.ClosedBoltWeapon.Fire -= ClosedBoltWeapon_Fire;
		}

    }
}
