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
    public class AirstrikeOnInterval : MonoBehaviour
    {
        public ProjectileDataList[] Projectiles;
        public int ListRepetitionNum;
        public float SpawnAltitude = 1500;
        public bool CalledByPlayer = false;

        [Serializable]
        public class ProjectileDataList
        {
            public GameObject Projectile;
            public float InitVel;
            public float WaitBeforFire;
            public float ConeLerpAccuracy;
        }

        private int ListIndex =0;
        private Vector3 Position;
        private Vector3 Vector;

#if !(UNITY_EDITOR || UNITY_5)

       


        public void Start()
        {
            Position = transform.position;
            Debug.Log("StartingAirstrike");

            StartCoroutine(IDKWhatToCallIt());
        }

        IEnumerator IDKWhatToCallIt()
        {
            while(ListRepetitionNum >= 0){
                //Debug.Log("Starting Shit");
                float Wait = Projectiles[ListIndex].WaitBeforFire;
                float ConeLerpAccuracy = Projectiles[ListIndex].ConeLerpAccuracy;
                GameObject CurProjectile = Projectiles[ListIndex].Projectile; //Get all the data from the current bit
                float Vel = Projectiles[ListIndex].InitVel;
                Vector = transform.position + Vector3.up * SpawnAltitude + Vector3.Lerp(UnityEngine.Random.onUnitSphere, Vector3.down, ConeLerpAccuracy);



                yield return new WaitForSeconds(Wait); //wait the amount of time
                //Debug.Log("Just waited " + Wait);
                Vector3 forward = Position - Vector;
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(CurProjectile, Vector, Quaternion.LookRotation(Vector));
                BallisticProjectile ballisticProjectile = gameObject.GetComponent<BallisticProjectile>();
                ballisticProjectile.Fire(Vel, forward.normalized, null, true);
                if (CalledByPlayer)
                {
                    ballisticProjectile.SetSource_IFF(GM.CurrentPlayerBody.GetPlayerIFF());
                }
                ListIndex++;
                //Debug.Log(ListIndex);
                if(ListIndex >= Projectiles.Length)
                {
                    ListIndex = 0;
                }

                ListRepetitionNum--;
            }
        }





#endif

    }
}



