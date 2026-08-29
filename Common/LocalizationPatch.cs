using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using TigerForceLocalizationLib;

namespace DragonBall_CN.Common
{
    public class LocalizationPatch:ModSystem
    {
        private readonly string dragonBall = "DBZMODPORT";
        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(dragonBall)) 
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, dragonBall, false);
            }
        }
    }
}
