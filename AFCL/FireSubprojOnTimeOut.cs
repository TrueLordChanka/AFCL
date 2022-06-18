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
    public class FireSubprojOnTimeOut : MonoBehaviour
    {
		
		public BallisticProjectile ParentRound;
		public Vector2 FuseTimer;
		private float _setFuseTimer;
		private float _fuseElapsedTime = 0;

#if !(UNITY_EDITOR || UNITY_5 )

		public void Awake()
        {
			_setFuseTimer = UnityEngine.Random.Range(FuseTimer[0], FuseTimer[1]);
        }

		public void FixedUpdate()
        {
			if (_fuseElapsedTime >= _setFuseTimer - Time.fixedDeltaTime)
			{
				ParentRound.MaxRange = 0;
			}
			else
			{
				_fuseElapsedTime += Time.fixedDeltaTime;
			}
		}



#endif

    }
}



