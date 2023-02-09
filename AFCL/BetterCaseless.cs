using System.Collections.Generic;
using UnityEngine;
using FistVR;


namespace AndrewFTW
{ 
    public class BetterCaseless : MonoBehaviour
    {
        private FVRFireArmRound Round;

#if !(UNITY_EDITOR || UNITY_5 )

        public void Awake()
        {
            Round = this.gameObject.GetComponent<FVRFireArmRound>();
            //Debug.Log(this.gameObject.name + " is a caseless round");
        }

        public void FixedUpdate()
        {
            //Debug.Log(Round.name + " is a caseless round and is spent? " + Round.m_isSpent);
            if (Round.m_isSpent) // the other one is m_isSpent, idk if it works different.
            {
                Destroy(Round.gameObject);
            }
        }



#endif
    }
}



