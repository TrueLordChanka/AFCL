using FistVR;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AndrewFTW
{
    public class AdvancedLaserController : MonoBehaviour
    {
        public FVRInteractiveObject AttachmentInterface; //the interface were watching
        private FVRFireArmAttachment Attachment;
        [Header("Stuff for displaying the Modes and Emitters:")]
        public Transform DisplayRotPoint; //used to rotate the display text irt the rotation of the attachment.
        public GameObject Canvas;
        public List<Text> ListOfEmitterTexts;
        public List<string> ListOfEmitterNames;
        public List<GameObject> ListOfEmitterObjects;
        public List<GameObject> ListOfEmittersTandemItems;
        public List<Types> ListOfEmitterTypes;
        public Setting[] EmitterMode;
        public GameObject TextFrame; //This is the frame for the text
        public GameObject FrameLArrow; //These are the arrows that will be childs of the frame
        public GameObject FrameRArrow;

        [Header("On/Off Audio")]
        public AudioEvent EmitterOnAudio;
        public AudioEvent EmitterOffAudio;

        
        private int _currentEmitterIndex = 0;
        private int _numberOfEmitters;

        public enum Types
        {
            Other,
            Vislaser,
            IrLaser,
            Light,
            IrLight,
            StrobeController,
            Rangefinder
        }
        public enum Setting
        {
            Off, //0
            On //1
        }

#if !(UNITY_EDITOR || UNITY_5)

        public void Awake()
        {
            _numberOfEmitters = ListOfEmitterObjects.Count; // sets the number of emmitters to be the number
            //Debug.Log("Num of emitters: " + _numberOfEmitters);

            FrameLArrow.SetActive(false);
            FrameRArrow.SetActive(true);
            Attachment = (AttachmentInterface as FVRFireArmAttachmentInterface).Attachment;

            
            if(ListOfEmitterTypes.Count != ListOfEmitterObjects.Count)
            {
                //THis is a state which breaks citys thing, kill it
                Debug.LogError("The list of emmitter types is not equal to the list of emitter objects. This will cause other things to break. \n Go fix it now.");
                Destroy(this);
            }
            
#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)
            Hook();
#endif
        }
#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)
        public void Hook()
        {
            //Debug.Log("Hooking the attachment to find when its attached or detached");
            On.FistVR.FVRFireArmAttachment.AttachToMount += FVRFireArmAttachment_AttachToMount;
        }

        private void FVRFireArmAttachment_AttachToMount(On.FistVR.FVRFireArmAttachment.orig_AttachToMount orig, FVRFireArmAttachment self, FVRFireArmAttachmentMount m, bool playSound)
        {
            orig(self, m, playSound);
            if (self == Attachment)
            {
                DisplayRotPoint.rotation = m.GetRootMount().MyObject.transform.rotation;
            }
            
        }
        
        public void OnDestroy()
        {
            Unhook();
        }

        public void Unhook()
        {
            On.FistVR.FVRFireArmAttachment.AttachToMount -= FVRFireArmAttachment_AttachToMount;
        }

#endif

        public void Update()
        {
            FVRViveHand hand = AttachmentInterface.m_hand;
            if(hand != null) //when the hand on the interface is not null
            {
                Canvas.SetActive(true); //sets the canvas to on so you see it
                if(hand.Input.TouchpadDown && Vector2.Angle(hand.Input.TouchpadAxes, Vector2.up) < 45f)
                { //Go up to the next menu
                    NextMenu();
                } else if (hand.Input.TouchpadDown && Vector2.Angle(hand.Input.TouchpadAxes, Vector2.right) < 45f)
                { // Toggle right on the selected menu
                    ToggleRight();
                } else if(hand.Input.TouchpadDown && Vector2.Angle(hand.Input.TouchpadAxes, Vector2.left) < 45f)
                { // Toggle left of the selected menu
                    ToggleLeft();
                }
            } else
            {
                Canvas.SetActive(false);
            }
        }


        

        public void NextMenu()
        {
            //Debug.Log("NM1 " + _currentEmitterIndex);
            //Do the next menue stuff
            if (_currentEmitterIndex + 1 >= _numberOfEmitters )
            {
                //Debug.Log("NM2 " + _currentEmitterIndex);
                _currentEmitterIndex = 0;
            } else
            {
                //Debug.Log("NM3 " + _currentEmitterIndex);
                _currentEmitterIndex++;
            }
           // Debug.Log("NM4 " + _currentEmitterIndex);
            //Move the frame to its propper position
            TextFrame.transform.localPosition = ListOfEmitterTexts[_currentEmitterIndex].transform.localPosition;
            //Debug.Log("NM5 " + _currentEmitterIndex);
            //Now we need to display the propper arrow
            if (EmitterMode[_currentEmitterIndex] == Setting.Off) //The arrow should be right
            {
                FrameLArrow.SetActive(false);
                FrameRArrow.SetActive(true);
            } else 
            { //If the setting is not off, it it on, which means the left arrow should be on
                FrameLArrow.SetActive(true);
                FrameRArrow.SetActive(false);
            }
        }

        public void ToggleRight()
        {
            //First off, lets see if we can go right. (on)
            if(EmitterMode[_currentEmitterIndex] == Setting.Off)
            { //The current emmitter is off, we need to turn it on.
                //First off, swap the arrows.
                FrameLArrow.SetActive(true);
                FrameRArrow.SetActive(false);

                //Now we need to set the text to be "On"
                ListOfEmitterTexts[_currentEmitterIndex].text = ListOfEmitterNames[_currentEmitterIndex]+ ": On"  ;

                //Now we need to set the emmitter to be on.
                ListOfEmitterObjects[_currentEmitterIndex].SetActive(true);
                if(ListOfEmittersTandemItems[_currentEmitterIndex] != null)
                {
                    ListOfEmittersTandemItems[_currentEmitterIndex].SetActive(true);
                }

                SM.PlayCoreSound(FVRPooledAudioType.GenericClose, this.EmitterOnAudio, base.transform.position);
                EmitterMode[_currentEmitterIndex] = Setting.On;
            } // else do nothing cause you cant go more on than on. we need On2.0


        }

        public void ToggleLeft()
        {
            //First off, lets see if we can go left (off)
            if (EmitterMode[_currentEmitterIndex] == Setting.On)
            { //The current emmitter is on, we need to turn it off.
                //First off, swap the arrows.
                FrameLArrow.SetActive(false);
                FrameRArrow.SetActive(true);

                //Now we need to set the text to be "Off"
                ListOfEmitterTexts[_currentEmitterIndex].text = ListOfEmitterNames[_currentEmitterIndex] + ": Off";

                //Now we need to set the emmitter to be on.
                ListOfEmitterObjects[_currentEmitterIndex].SetActive(false);
                if (ListOfEmittersTandemItems[_currentEmitterIndex] != null)
                {
                    ListOfEmittersTandemItems[_currentEmitterIndex].SetActive(false);
                }

                SM.PlayCoreSound(FVRPooledAudioType.GenericClose, this.EmitterOffAudio, base.transform.position);
                EmitterMode[_currentEmitterIndex] = Setting.Off;
            } // else do nothing cause you cant go more on than off

        }

        public int PreviousEmitter() //why did i write this. you cant go down??
        {
            int _prevEmitter = -1;

            if(_currentEmitterIndex == 0)
            {
                _prevEmitter = _numberOfEmitters;
            } else
            {
                _prevEmitter = _currentEmitterIndex--;
            }
            if(_prevEmitter == -1)
            {
                Debug.LogError("Previous emitter is gonna make a index out of range exception, giving it 0 to not do that");
                _prevEmitter = 0;
            }

            return _prevEmitter;
        }


#endif

        }
    }



