using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using UnityEngine;
using BepInEx.Configuration;
using System.IO;

namespace AndrewFTW
{
#if !DEBUG
    [BepInPlugin("h3vr.andrew_ftw.afcl", "Another Fucken Code Library", "2.34.1")] //used to be Liberary if this breaks shit
    
    class AFCL_BepInEx : BaseUnityPlugin
    {
        public ConfigEntry<bool> DoesUseArmingDistance2;
        public ConfigEntry<float> StrobeFlashTime;
        public static string PluginPath;
         
        public AFCL_BepInEx()
        {
            string pluginPath = Path.GetDirectoryName(Info.Location);
            string youLikeKissingSosigsDontYou = File.ReadAllText(Path.Combine(pluginPath, "boykisser.txt"));
            Logger.LogInfo(youLikeKissingSosigsDontYou);

            Logger.LogInfo("AFCL loaded!");

            //Config stuff

            DoesUseArmingDistance2 = Config.Bind<bool>("Ammunition Settings", "Does_Use_ArmingDistance", false, "Used by ammunition which can require an arming distance, such as 40mm Grenades");
            StrobeFlashTime = Config.Bind<float>("Strobe Flashlight Flash time.", "StrobeFlashTime", 0.02f, "Used by flashlights that have a strobe option.");
            

            ProjectileArmingDistance.DoesUseArmingDistance2 = DoesUseArmingDistance2.Value;
            StrobeController.StrobeFlashTime = StrobeFlashTime.Value;
           

            //end config stuff


            Logger.LogInfo("Arming Distance set to " + DoesUseArmingDistance2.Value);
            Logger.LogInfo("Strobe time set to " + StrobeFlashTime.Value);

            PluginPath = Path.GetDirectoryName(Info.Location);
        }
       
        


    }
#endif
}
