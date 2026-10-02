using DBZGoatLib.Model;
using DragonBall_CN.Common.DBZGoatLib;
using System.Collections.Generic;
using Terraria.ModLoader;
using TigerForceLocalizationLib;
using TigerForceLocalizationLib.Filters;

namespace DragonBall_CN.Common.XV2Forms
{
    public class XV2FormsPatch : ModSystem
    {
        private static readonly string XV2 = "XV2Forms";
        private static Dictionary<string, string> FormNames = new()
        {
            //天才
            ["DivineBeastBuff"] = "野兽",
            ["StackableUltimateBuff"] = "究极",
            ["LimitBreakerBuff"] = "极限突破",
            //常规
            ["SupervillainBuff"] = "极恶化",
            ["UltraSupervillainBuff"] = "超级极恶化",
            ["VillainousBuff"] = "邪恶化",
            
        };

        private static Dictionary<string, string> NewUnlockHints = new()
        {
            //天才
            ["DivineBeastBuff"] = "究极形态掌握度达到100%时变身自动变为该形态",
            ["StackableUltimateBuff"] = "天才资质自动解锁\n[C/959595:译者补充：该形态掌握度达到100%时能变身为野兽形态，但有bug导致无法变身成野兽]",
            ["LimitBreakerBuff"] = "血肉墙的血量低于25点时解锁",
            //常规
            ["SupervillainBuff"] = "让黑暗魔法支配你（使用极恶化物品）",
            ["UltraSupervillainBuff"] = "你的极恶化形态的到强化（精通极恶化形态）",
            ["VillainousBuff"] = "让邪恶支配你，做出最后一搏（在骷髅王Boss战斗中，将生命值降至低血量）\n[C/959595:译者补充：玩家生命值要低于最大的25%]",
     
        };

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(XV2))
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, XV2, false, filters: new()
                {
                    MethodFilter = MethodFilter.MatchName("PostUpdate")
                });
            }
        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod(XV2, out Mod mod))
                return;

            //形态名称
            foreach (var form in FormNames)
            {
                if (!ModelHelper.TryModifyFormName(mod, "XV2Forms.Transformations.", form.Key, form.Value))
                    Mod.Logger.Info("Replace FAILED");
            }


            //超宇宙2形态
            if (ModelHelper.TryGetNodes(mod, "XV2Forms.Assets.WPlayer+XV2TranformationTree", out Node[] nodesLSSJAF))
            {
                for (int i = 0; i < nodesLSSJAF.Length; i++)
                {
                    if (NewUnlockHints.TryGetValue(nodesLSSJAF[i].BuffKeyName, out string newUnlockHint))
                        if (ModelHelper.TryModifyNodes(mod, "XV2Forms.Assets.WPlayer+XV2TranformationTree", i, newUnlockHint))
                            Mod.Logger.Info("Replace Success");
                }
            }

            //额外形态
            if (ModelHelper.TryGetNodes(mod, "XV2Forms.Assets.WPlayer+ExtraFormsTransformationTree", out Node[] nodesSSJAF))
            {
                for (int i = 0; i < nodesSSJAF.Length; i++)
                {
                    if (NewUnlockHints.TryGetValue(nodesSSJAF[i].BuffKeyName, out string newUnlockHint))
                        if (ModelHelper.TryModifyNodes(mod, "XV2Forms.Assets.WPlayer+ExtraFormsTransformationTree", i, newUnlockHint))
                            Mod.Logger.Info("Replace Success");
                }
            }
        }

    }
}
