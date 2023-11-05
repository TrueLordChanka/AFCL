using FistVR;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace AndrewFTW
{ 
    public class Airdrop_Marker : MonoBehaviour
    {
        [Header("Vault data")]
        public string FileName;
        public string PluginDictionaryString = "Andrew_FTW.Airdrop_Test";
        [Header("Drop Prefab")]
        public GameObject Prefab;

        [Header("Drop vehicle data")]
        public string DVPrefabName;
        public string DVFileName;
        public string DVPluginDictString = "Andrew_FTW.Airdrop_Test";




#if !(UNITY_EDITOR || UNITY_5)

        //Spawn the controller, passing it the id of vehicle and a list of ids it can spawn
        //This class is what gets implemented into a grenade, allowing all the required data to be passed on

        public void Awake()
        {
            Airdrop_Controller controller = new Airdrop_Controller(
                FileName, PluginDictionaryString, DVPrefabName, DVFileName,
                DVPluginDictString, transform, Prefab);
            if(Prefab == null)
            {
                controller.LoadVaultFile();
            } 

            
            controller.InitAirdropVehicle();
        }



#endif
    }
}



