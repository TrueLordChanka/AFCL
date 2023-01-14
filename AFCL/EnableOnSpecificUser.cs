using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using FistVR;


public class EnableOnSpecificUser : MonoBehaviour
{
	public action Action;
	public System.Collections.Generic.List<CSteamID> listofUsers;

	[Header("Object Enable")]
	public GameObject objectToEnable;

	[Header("Firearm Modify")]
	public Handgun Handgun;
	public bool HasMagReleaseButton;
	public enum action
    {
		Enable,
		Disable,
		ModifyFirearm
    }

#if !(UNITY_EDITOR || UNITY_5)
	// Use this for initialization
	void Start()
	{
		CSteamID userID = SteamUser.GetSteamID();
		
		if (listofUsers.Contains(userID))
		{
            switch (Action)
            {
				case action.Enable:
                {
					objectToEnable.SetActive(true);
				}
                break;
              
				case action.Disable:
                {
					objectToEnable.SetActive(false);
				}
				break;

				case action.ModifyFirearm:
                {
					Handgun.HasMagReleaseInput = true;
                }
				break;
				default: break;
            }

		}
	}
#endif
}
