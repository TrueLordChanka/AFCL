using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using UnityEngine;
using BepInEx.Configuration;

namespace AndrewFTW
{
#if !DEBUG
    [BepInPlugin("h3vr.andrew_ftw.afcl", "Another Fucken Code Library", "2.25.0")] //used to be Liberary if this breaks shit
    
    class AFCL_BepInEx : BaseUnityPlugin
    {
        public ConfigEntry<bool> DoesUseArmingDistance;
        public ConfigEntry<float> StrobeFlashTime;
        
         
        public AFCL_BepInEx()
        {
            Logger.LogInfo("AFCL loaded!");

            //Config stuff

            DoesUseArmingDistance = Config.Bind<bool>("Ammunition Settings", "Does_Use_ArmingDistance", true, "Used by ammunition which can require an arming distance, such as 40mm Grenades");
            StrobeFlashTime = Config.Bind<float>("Strobe Flashlight Flash time.", "StrobeFlashTime", 0.02f, "Used by flashlights that have a strobe option.");
            

            ProjectileArmingDistance.DoesUseArmingDistance = DoesUseArmingDistance.Value;
            StrobeController.StrobeFlashTime = StrobeFlashTime.Value;
           

            //end config stuff


            Logger.LogInfo("Arming Distance set to " + DoesUseArmingDistance.Value);
            Logger.LogInfo("Strobe time set to " + StrobeFlashTime.Value);
            

        }
       
    }
#endif
}
