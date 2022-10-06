using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class RotateUp : MonoBehaviour
    {

#if !(UNITY_EDITOR || UNITY_5)
        
        public void Start()
        { 
            this.gameObject.transform.eulerAngles = Vector3.up;
        }
        
#endif
    }
}



