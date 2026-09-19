using DBZMODPORT.Items.Consumables.KiFragments;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using TigerForceLocalizationLib;
using TigerForceLocalizationLib.Filters;

namespace DragonBall_CN.Common.DBZMODPORT
{
    public class DBZMODPORTPatch:ModSystem
    {
        private static readonly string dragonBall = "DBZMODPORT";
        
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

        public override void PostAddRecipes()
        {
            //recipeGroup翻译
            if (RecipeGroup.recipeGroupIDs.TryGetValue($"DBZMODPORT:KiFragment", out int groupId)) 
            {
                RecipeGroup.recipeGroups[groupId].GetText = () => $"{Language.GetTextValue("LegacyMisc.37")} 气碎片";
            }
        }

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(dragonBall))
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, dragonBall, false);
                //TigerForceLocalizationHelper.LocalizeAll(Mod.Name, dragonBall, false, filters: new() 
                //{
                //    MethodFilter = MethodFilter.MatchNames(
                //        "AssembleTransBuffDescription", "UpdateArmorSet", "UseItem", "OnPickup", 
                //        "SelectButtonAwakening", "SelectButtonGenetics", "SelectButtonImmortality", "SelectButtonWealth", "SelectButtonPower", "OnInitialize", 
                //        "TrySelectingSSJR", "TrySelectingSSJB", "TrySelectingSSJG", "TrySelectingLSSJ3", "TrySelectingLSSJ2", "TrySelectingLSSJ", "TrySelectingSSJ3", "TrySelectingSSJ2", "TrySelectingSSJ1",
                //        "Update", "PostUpdate", "PostDrawFullScreenMap", "ModifyTooltips", "OnKilledNPC", "PreKill" , "OnKill", "AwakeningFormUnlock" , "LSSJ2TextSelect", "ProcessTriggers"),
                //    TypeFilter = TypeFilter.MismatchFullName("DBZMODPORT.DBZWorld")
                //});
            }
        }
    }
}
