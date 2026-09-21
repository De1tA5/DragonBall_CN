using DBZGoatLib.Model;
using DragonBall_CN.Common.DBZGoatLib;
using System.Collections.Generic;
using Terraria.ModLoader;
using TigerForceLocalizationLib;
using TigerForceLocalizationLib.Filters;

namespace DragonBall_CN.Common.AwakeningPower
{
    public class AwakeningPowerPatch : ModSystem
    {
        private static readonly string AP = "AwakeningPower";

        private static Dictionary<string, string> FormNames = new()
        {
            ["FullPowerBuff"] = "全功率",
            ["ShatteredLimitsBuff"] = "突破极限",
        };

        private static Dictionary<string, string> NewUnlockHints = new()
        {
            ["FullPowerBuff"] = "只有驾驭自身的能量才能释放潜能（制作全功率物品）\n[C/959595:译者补充：制作并使用宁静气结晶即可解锁]",
            ["ShatteredLimitsBuff"] = "击杀一只恶魔野兽\n[C/959595:译者补充：击败血肉墙即可解锁]",
        };

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(AP))
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, AP, false, filters: new() 
                {
                    MethodFilter = MethodFilter.MatchNames("UseItem", "BuildFixedTooltip", "OnKill", "ProcessTriggers")
                });
            }
        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod(AP, out Mod mod))
                return;

            //形态名称
            foreach (var form in FormNames)
            {
                if (!ModelHelper.TryModifyFormName(mod, "AwakeningPower.Content.Transformations.", form.Key, form.Value))
                    Mod.Logger.Info("Replace FAILED");
            }

            if (!ModelHelper.TryGetNodes(mod, "AwakeningPower.Content.Transformations.FullPowerPanel", out Node[] nodes))
                return;

            //解锁条件
            for (int i = 0; i < nodes.Length; i++)
            {
                if (NewUnlockHints.TryGetValue(nodes[i].BuffKeyName, out string newUnlockHint))
                    if (ModelHelper.TryModifyNodes(mod, "AwakeningPower.Content.Transformations.FullPowerPanel", i, newUnlockHint))
                        Mod.Logger.Info("Replace Success");
            }
        }
    }
}
