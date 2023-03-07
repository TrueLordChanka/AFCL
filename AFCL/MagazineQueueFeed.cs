using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;
using OpenScripts2;


namespace AndrewFTW
{
    public class MagazineQueueFeed : MonoBehaviour
    {
		public FVRFireArmMagazine Mag;

        public bool isDebug = false; //if (isDebug) { Debug.Log("test"); }

		private static Dictionary<FVRFireArmMagazine, MagazineQueueFeed> _magQueues = new Dictionary<FVRFireArmMagazine, MagazineQueueFeed>();

#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)

        static MagazineQueueFeed()
        {
            On.FistVR.FVRFireArmMagazine.RemoveRound_bool += FVRFireArmMagazine_RemoveRound_bool;
        }

		


        private static GameObject FVRFireArmMagazine_RemoveRound_bool(On.FistVR.FVRFireArmMagazine.orig_RemoveRound_bool orig, FVRFireArmMagazine self, bool b)
        {
			MagazineQueueFeed _magQueue;
            if(!_magQueues.TryGetValue(self, out _magQueue))
			{
				return orig(self, b);
            }

			return _magQueue.LoadRounds();
			
		}

		public GameObject LoadRounds()
        {
			GameObject gameObject = Mag.LoadedRounds[0].LR_ObjectWrapper.GetGameObject();
			if (isDebug) { Debug.Log("aaaaaaaaaaa"); }
			if ((!Mag.IsInfinite || !GM.CurrentSceneSettings.AllowsInfiniteAmmoMags) && !GM.CurrentSceneSettings.IsAmmoInfinite && !GM.CurrentPlayerBody.IsInfiniteAmmo)
			{ //if its not infinite, do this
				if (GM.CurrentPlayerBody.IsAmmoDrain) //no fucken clue
				{
					Mag.m_numRounds = 0;
				}
				else
				{
					if (Mag.m_numRounds > 0) //rounds are greater than zero, do this
					{
						Mag.LoadedRounds[0] = null;
						for (int i = 0; i < Mag.m_numRounds; i++)
						{
							if (Mag.LoadedRounds[i + 1] != null)
							{
								Mag.LoadedRounds[i] = Mag.LoadedRounds[i + 1];
							}
							else
							{
								Mag.LoadedRounds[i] = null;
							}
						}
						Mag.m_numRounds--;
					}
					Mag.UpdateBulletDisplay();
				}
			}

			return gameObject;
		}



		public void Awake()
		{
			_magQueues.Add(Mag, this);
		}

		public void OnDestroy()
        {
			_magQueues.Remove(Mag);
        }


#endif
	}
}



