using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 

    [RequireComponent (typeof(PMat))]
    public class ShatterableArmourPiece : MonoBehaviour, IFVRDamageable
    {
        public ShatterableArmour ParentArmour; 

#if !(UNITY_EDITOR || UNITY_5)
        
        public void Damage(Damage d)
        {
            ParentArmour._damageTaken += d.Dam_TotalKinetic;
            Debug.Log("Damage: " + d.Dam_TotalKinetic + "was just taken");
        }

        public void Initialize(MatDef DefaultMatDef)
        {
            GetComponent<PMat>().MatDef = DefaultMatDef;
        }

        public void Shatter(MatDef BrokenMatDef)
        {
            GetComponent<PMat>().MatDef = BrokenMatDef;
        }
#endif
    }
}



