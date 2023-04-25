using System.Collections.Generic;
using UnityEngine;
using FistVR;


namespace AndrewFTW
{ 
    public class BipodExtras : MonoBehaviour
    {
        public FVRFireArmBipod Bipod;
        public bool Rotatelegs;
        public GameObject LegHolder1;
        public GameObject LegHolder2;
        public Vector3 LegOpenAngles1;
        public Vector3 LegOpenAngles2;

        public bool MoveObj;
        public GameObject ObjToMove;
        public Vector3 ObjOpenLocalPos;


        private bool m_beenUpdated = false;

#if !(UNITY_EDITOR || UNITY_5 )

        

        public void Update()
        {
            
            if (Bipod.m_isBipodExpanded && !m_beenUpdated) //if its expanded and this is the first time its seeing this 
            {
                if (Rotatelegs)
                {
                    LegHolder1.transform.localEulerAngles = LegOpenAngles1;
                    LegHolder2.transform.localEulerAngles = LegOpenAngles2;
                }
                if (MoveObj)
                {
                    ObjToMove.transform.localPosition = ObjOpenLocalPos;
                }

                m_beenUpdated = true;
            }

            if (!Bipod.m_isBipodExpanded && m_beenUpdated)
            {
                if (Rotatelegs)
                {
                    LegHolder1.transform.localEulerAngles = new Vector3 (0,0,0);
                    LegHolder2.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
                if (MoveObj)
                {
                    ObjToMove.transform.localPosition = new Vector3(0, 0, 0);
                }

                m_beenUpdated = false;
            }

        }



#endif
    }
}



