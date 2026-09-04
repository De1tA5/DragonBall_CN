using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader;
using TigerForceLocalizationLib;

namespace DragonBall_CN.Common.DBZMODPORT
{
    public class DBZMODPORTPatch:ModSystem
    {
        private readonly string dragonBall = "DBZMODPORT";
        
        public override void Load()
        {
            //仅修复制作站tile
            if (!ModLoader.TryGetMod(dragonBall, out Mod mod))
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

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(dragonBall))
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, dragonBall, false);
            }
        }
    }
}
