using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;

namespace AndrewFTW
{
#if !DEBUG
    [BepInPlugin("h3vr.andrew_ftw.afcl", "Another Fucken Code Liberary", "1.0.0")]
    class AFCL_BepInEx : BaseUnityPlugin
    {
        public AFCL_BepInEx()
        {
            Logger.LogInfo("AFCL loaded!");
        }
    }
#endif
}
