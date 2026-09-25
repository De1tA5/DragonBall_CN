using Terraria.ModLoader;
using TigerForceLocalizationLib;
using TigerForceLocalizationLib.Filters;

namespace DragonBall_CN.Common.BetterDBT
{
    public class BetterDBTPatch : ModSystem
    {
        private readonly static string BDBP = "BetterDBT";

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(BDBP))
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, BDBP, false);
                //其他方法
                //TigerForceLocalizationHelper.LocalizeAll(Mod.Name, BDBP, false, filters: new()
                //{
                //    MethodFilter = MethodFilter.MatchNames(
                //    "ModifyTooltips", "PostUpdateWorld", "PreUpdate", "OnEnterWorld","OnKill",
                //    "UseItem")
                //});
                //整个wikiUI
                //TigerForceLocalizationHelper.LocalizeAll(Mod.Name, BDBP, false, filters: new()
                //{
                //    TypeFilter = TypeFilter.MatchFullNames("BetterDBT.NewContent.WikiUIState")
                //});
                //整个BossRush
                //TigerForceLocalizationHelper.LocalizeAll(Mod.Name, BDBP, false, filters: new()
                //{
                //    TypeFilter = TypeFilter.MatchFullNames("BetterDBT.NewContent.BossRush.BossRushSystem")
                //});
            }
        }
    }
}
