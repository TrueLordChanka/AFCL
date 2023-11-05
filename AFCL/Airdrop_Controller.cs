using FistVR;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

namespace AndrewFTW
{ 
    public class Airdrop_Controller : MonoBehaviour
    {



        public string FileName;
        public string PluginDictionaryString;
        public Transform Call_Pt;
        public GameObject Airdropvehicle;
        public Airdrop_Vehicle DropVehicle;


        private VaultFile FileToSpawn = new VaultFile();
        private string path;

        private string DVPrefabName;
        private string DVFileName;
        private string DVPluginDictString;
        private AssetBundle bundle;
        private GameObject Prefab;


#if !(UNITY_EDITOR || UNITY_5)

        public Airdrop_Controller(
            string fileName, string pluginDictionaryString, string dvPrefabName, string dvFileName,
            string dvPluginDictString, Transform ntransform, GameObject prefab = null)
        {
            FileName = fileName;
            PluginDictionaryString = pluginDictionaryString;
            Call_Pt = ntransform;
            DVPrefabName = dvPrefabName;
            DVFileName = dvFileName;
            DVPluginDictString = dvPluginDictString;
            if(prefab != null)
            {
                Prefab = prefab;
            }

            Debug.Log("Alpha");

        }
        
        public void LoadVaultFile()
        {
            
            Debug.Log("Load vault fiel");
            BepInEx.PluginInfo pluginInfo = new BepInEx.PluginInfo();
            BepInEx.Bootstrap.Chainloader.PluginInfos.TryGetValue(PluginDictionaryString, out pluginInfo);
            string PluginPath = pluginInfo.Location;
            path = Path.GetDirectoryName(PluginPath);

            string json = string.Empty;
            Debug.Log(Path.Combine(path, FileName));
            json = File.ReadAllText(Path.Combine(path, FileName));
            JsonUtility.FromJsonOverwrite(json, FileToSpawn);
            
            

        }

        public void InitAirdropVehicle()
        {
   
            Debug.Log("Init airdrovheasdioads");
            BepInEx.PluginInfo pluginInfo = new BepInEx.PluginInfo();
            BepInEx.Bootstrap.Chainloader.PluginInfos.TryGetValue(DVPluginDictString, out pluginInfo);
            string dvpluginpath = pluginInfo.Location;
            string dvpath = Path.GetDirectoryName(dvpluginpath); //get the path to the directory for the plugin that has our vehicle asset bundle in it
            bundle = AssetBundle.LoadFromFile(Path.Combine(dvpath, DVFileName));
            
             
            Debug.Log("Echo");
            Airdropvehicle = bundle.LoadAsset<GameObject>(DVPrefabName);
            GameObject adv = Instantiate(Airdropvehicle);
            bundle.Unload(false);
            DropVehicle = adv.GetComponent<Airdrop_Vehicle>();
            DropVehicle.Controller = this;
            DropVehicle.transform.position = new Vector3(2000, -100, 2000);
            DropVehicle.CallTransform = Call_Pt;
            DropVehicle.Init();

        }

        public void DropPayload(Transform ptToSpawnAt)
        {
            if(Prefab == null)
            {
                string s;
                VaultSystem.SpawnObjects(ItemSpawnerV2.VaultFileDisplayMode.SingleObjects, FileToSpawn, out s, ptToSpawnAt, Vector3.up);
                if (s != null)
                {
                    Debug.LogError(s);
                }
            }
            else
            {
                Instantiate(Prefab, ptToSpawnAt);
            }

            



        }



        /*///////////////////
         *                  ToDo
         *                  
         *   This class will control the spawning of all parts, vehicle, airdrop, ordinance, etc. 
         *   It has to make sure the vehicles path is clear
         *   It has to calculate when to drop the package so that it hits the target
         *                  
         *   1: Spawn the drop vehicle prefab from file. (look at how I did blindness)
         *   2: The drop vehicle does its thing and calls the Drop method (part of the controller) The controller then spawns the vault file (somehow) 
         *   3: Play the audio event              
         *                  
         *                  
         * 
         * 
         * 
         *////////////////////




#endif
    }
}



