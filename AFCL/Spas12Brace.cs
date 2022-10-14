using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class Spas12Brace : FVRInteractiveObject
    {
        public enum Axis
        {
            X,
            Y,
            Z
        }

        public TubeFedShotgun Weapon;

        public GameObject HookGeo; //to be used to rotate the hook
        public Axis HookAxis;
        public float HookMinAngle;
        public float HookMaxAngle;
        public float DegPerFrame;

        public AudioEvent ToggleSound; //Click Click

        public GameObject MainStock; //These two will be used to ensure the stocks unfolded 
        public Axis MainAxis;
        public GameObject SecondaryStock;
        public Axis SecondaryAxis;

       


        private bool _isStabalized = false;
        private bool _isHookOut = false;

#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)

        public override void SimpleInteraction(FVRViveHand hand) //When the box is interacted with
        {
            base.SimpleInteraction(hand); //Do base stuff

            //TODO Rotate the hook
            StartCoroutine(RotateHook());
            

            //TODO Engage the stabalization IF the stock is folded down




        }

        IEnumerator RotateHook()
        {

            if (_isHookOut) //The hooks out put it in
            {
                while (HookGeo.localEulerAngles.HookAxis >= HookMinAngle-HookMinAngle) //while the hook is greater the min with some variation
                {
                    HookGeo.localEulerAngles.HookAxis 



                }
            }
            
            

            




            yield return null
        }






#endif
    }
}



