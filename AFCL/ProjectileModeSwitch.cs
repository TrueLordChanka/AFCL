using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using FistVR;
using Unity;
using System.Collections;


namespace AndrewFTW
{
	public class ProjectileModeSwitch : MonoBehaviour
	{
		public FVRFireArmRound ParentRound;

		[Header("Mode Control")]
		public List<string> ModeName; //A list created of the class "c_ModeList"
		public List<FireArmRoundClass> ModeClass;
		public List<FVRObject> AltObjWrapper; //For saving in the rig

		private int _currentMode = 0;
		

		[Header("Stuff for displaying the mode")]
		public GameObject Canvas;
		public Text DisplayText;

		[Header("Rotation parts")]
		public bool HasRotatingPart; //does it rotate?
		public Transform RotatingPart; //what part to actually rotate
		public RotAxis Axis;
		public enum RotAxis
        {
			x,
			y,
			z
        }
		public float[] RotationAngles;





#if !(UNITY_EDITOR || UNITY_5)



		public void Update()
        {
			FVRViveHand Hand = ParentRound.m_hand;
			if (Hand != null)
			{
				if (Hand.Input.TouchpadDown && Vector2.Angle(Hand.Input.TouchpadAxes, Vector2.right) < 45f)
				{
					SwapMode(1);
				}
				else if (Hand.Input.TouchpadDown && Vector2.Angle(Hand.Input.TouchpadAxes, Vector2.left) < 45f)
				{
					SwapMode(0);
				}

				Canvas.gameObject.SetActive(true);
			} else
            {
				Canvas.gameObject.SetActive(false);
            }

		}

		public void SwapMode(int direction)
		{ //1 is "right", 0 is "left"
			if (direction == 0) //decrementing the menue
			{
				if (_currentMode == 0)
				{
					Debug.Log("going down at bottom");
					return; //This is cause you cant loop around cause it doesnt make sense
				}
				else
				{
					Debug.Log("going down");
					_currentMode--;
					Debug.Log("going down2");
					UpdateMenue();
				} 
			}
			else if (direction == 1) //increment the menue
			{
				if (_currentMode == ModeName.Count)
				{
					Debug.Log("going up at top");
					return; //This is cause you cant loop around like above
				}
				else
				{
					Debug.Log("going up");
					_currentMode++;
					Debug.Log("going up2");
					UpdateMenue();
				}
			}
		}

		public void UpdateMenue()
        {
            if (HasRotatingPart) //If it has a rotating part, rotate it
            {
				UpdateRotation();
            }
			if(DisplayText != null) //If there is a display text, update it to the next one
            {
				DisplayText.text = ModeName[_currentMode]; 
            }

			ParentRound.RoundClass = ModeClass[_currentMode]; //sets the rounds class to the current selected class
			ParentRound.ObjectWrapper = AltObjWrapper[_currentMode]; //sets the object wrapper

		}

		public void UpdateRotation()
        {
            switch (Axis)
            {
				case RotAxis.x:
					RotatingPart.transform.localEulerAngles = new Vector3(RotationAngles[_currentMode],0,0);
                    break;
                case RotAxis.y:
					RotatingPart.transform.localEulerAngles = new Vector3(0,RotationAngles[_currentMode],0);
					break;
				case RotAxis.z:
					RotatingPart.transform.localEulerAngles = new Vector3(0, 0, RotationAngles[_currentMode]);
					break;
				default:
					RotatingPart.transform.localEulerAngles = new Vector3(0,0,0); //This is needed cause VS doesnt like without a default
					break;
            } //There should only be 3 cause its an enum. like, there literally cant be more
        }

#endif

	}
}



