using FistVR;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;



namespace AndrewFTW
{ 
    public class SpawnVaultFileOnAwake : MonoBehaviour
    {

        public string FileName;
        public string PluginDictionaryString = "Andrew_FTW.Airdrop_Test";

        private VaultFile FileToSpawn = new VaultFile();

#if !(UNITY_EDITOR || UNITY_5)

        
        public void Awake()
        {
            Debug.Log("UwU");
            BepInEx.PluginInfo pluginInfo = new BepInEx.PluginInfo();
            BepInEx.Bootstrap.Chainloader.PluginInfos.TryGetValue(PluginDictionaryString, out pluginInfo);
            string pluginpath = pluginInfo.Location;
            string path = Path.GetDirectoryName(pluginpath);
            Load<VaultFile>(Path.Combine(path, FileName), FileToSpawn);
            Debug.Log("UwU");
            Debug.Log(FileToSpawn.FileName);
            string s;
            VaultSystem.SpawnObjects(ItemSpawnerV2.VaultFileDisplayMode.SingleObjects, FileToSpawn, out s, transform, Vector3.up);
            if (s != null)
            {
                Debug.LogError(s);
            }


            /*
            foreach (KeyValuePair<string, BepInEx.PluginInfo> entry in BepInEx.Bootstrap.Chainloader.PluginInfos)
            {
                Debug.Log(entry.Value.ToString());
                Debug.Log(entry.Key);
            }
            */
        }

       
        public void Load<T>(string path, T objectToOverwrite)
        {
            string json = string.Empty;
            Debug.Log(path);
            json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, objectToOverwrite);

        }
        

        








#endif
    }
}



