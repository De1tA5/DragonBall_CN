using Terraria.ModLoader;
using TigerForceLocalizationLib;
using TigerForceLocalizationLib.Filters;

namespace DragonBall_CN.Common.dbzmodfargos
{
    public class dbzmodfargosPatch : ModSystem
    {
        private readonly static string DBZF = "dbzmodfargos";

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(DBZF))
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, DBZF, false, filters: new() 
                {
                    MethodFilter = MethodFilter.MatchNames("OnKill", "UpdateArmorSet")
                });
            }
        }
    }
}
