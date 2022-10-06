using FistVR;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AndrewFTW
{
    public class SpinnyHoloSight : MonoBehaviour
    {
        public FVRFireArmAttachment ParentAttachment;
        public GameObject Arms;
        public GameObject Spindle;
        public GameObject FrontDot;
        public GameObject RearDot;

        public float SpinAccel;
        public float SpinSpeedMax;

        public float XJiggleMax;
        public float YJiggleMax;

        private float SpinCurrentSpeed;
        private Vector3 FrontDotPos;
        private Vector3 RearDotPos;


#if !(UNITY_EDITOR || UNITY_5)

        public void Start()
        {
            FrontDotPos = FrontDot.transform.localPosition; 
            RearDotPos = RearDot.transform.localPosition;
        }

       public void FixedUpdate()
       {
            if(ParentAttachment.curMount != null)
            {
                //If current speed isnt at max, accelerate it by one acceleration unit.
                if(SpinCurrentSpeed < SpinSpeedMax)
                {
                    SpinCurrentSpeed += SpinAccel;
                    //actually spins the thing
                    Spindle.transform.localEulerAngles = new Vector3(Spindle.transform.localEulerAngles.x, Spindle.transform.localEulerAngles.y + SpinCurrentSpeed, Spindle.transform.localEulerAngles.z);

                } else
                {

                    //now that it is at max speed, we can actually hide the stuff and display the things
                    Arms.SetActive(false);
                    FrontDot.SetActive(true);
                    RearDot.SetActive(true);

                    //Spinn thing
                    Spindle.transform.localEulerAngles = new Vector3(Spindle.transform.localEulerAngles.x, Spindle.transform.localEulerAngles.y + SpinCurrentSpeed, Spindle.transform.localEulerAngles.z);

                    //need to jiggle the reticle

                    FrontDot.transform.localPosition = new Vector3(FrontDotPos.x + Random.Range(-XJiggleMax, XJiggleMax), FrontDotPos.y + Random.Range(-YJiggleMax, YJiggleMax), FrontDotPos.z);
                    RearDot.transform.localPosition = new Vector3(RearDotPos.x + Random.Range(-XJiggleMax, XJiggleMax), RearDotPos.y + Random.Range(-YJiggleMax, YJiggleMax), RearDotPos.z);
                }
            } else
            {
                if(SpinCurrentSpeed > 0)
                {
                    SpinCurrentSpeed -= SpinAccel;
                    //actually spins the thing
                    Spindle.transform.localEulerAngles = new Vector3(Spindle.transform.localEulerAngles.x, Spindle.transform.localEulerAngles.y + SpinCurrentSpeed, Spindle.transform.localEulerAngles.z);
                } else
                {
                    SpinCurrentSpeed = 0f;
                    Arms.SetActive(true);
                    FrontDot.SetActive(false);
                    RearDot.SetActive(false);
                }
            }
        }
#endif

    }
}



