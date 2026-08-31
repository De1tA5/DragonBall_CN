using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader;
using TigerForceLocalizationLib;

namespace DragonBall_CN.Common
{
    public class LocalizationPatch : ModSystem
    {
        private readonly string dragonBall = "DBZMODPORT";
        private readonly string dragonBallLib = "DBZGoatLib";
        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(dragonBall))
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, dragonBall, false);
            }

            if (ModLoader.HasMod(dragonBallLib)) 
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, dragonBallLib, false);
            }
        }
    }


    public class TileEntryPatch : ModSystem
    {
        //仅修复制作站tile
        public override void Load()
        {
            if (!ModLoader.TryGetMod("DBZMODPORT", out Mod mod))
                return;

            ModTile? zTableTile = mod.Find<ModTile>("ZTable");
            ModTile? kaiTableTile = mod.Find<ModTile>("KaiTable");

            if (zTableTile is not null)
            {
                LocalizedText name = zTableTile.CreateMapEntryName();
                zTableTile.AddMapEntry(new Color(255, 250, 34), name);
            }

            if (kaiTableTile is not null)
            {
                LocalizedText name = kaiTableTile.CreateMapEntryName();
                kaiTableTile.AddMapEntry(new Color(115, 204, 32), name);
            }
        }
    }
}
