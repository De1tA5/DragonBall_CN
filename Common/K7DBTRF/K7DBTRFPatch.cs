using DBZGoatLib.Model;
using DragonBall_CN.Common.DBZGoatLib;
using System.Collections.Generic;
using Terraria.ModLoader;
using TigerForceLocalizationLib;
using TigerForceLocalizationLib.Filters;

namespace DragonBall_CN.Common.K7DBTRF
{
    public class K7DBTRFPatch : ModSystem
    {
        private static readonly string Custom = "K7DBTRF";
        private static Dictionary<string, string> FormNames = new()
        {
            //传说
            ["LSSJ5Buff"] = "传说超级赛亚5",
            ["LSSJ6Buff"] = "传说超级赛亚6",
            ["LSSJ7Buff"] = "传说超级赛亚7",
            ["LSSJ8Buff"] = "传说超级赛亚8",
            ["LSSJ9Buff"] = "传说超级赛亚9",
            ["LSSJ9FPBuff"] = "传说超级赛亚9全功率",
            ["LSSJ10FPBuff"] = "传说超级赛亚人10禁忌之力",
            //常规
            ["K7SSJ5Buff"] = "超级赛亚人5",
            ["K7SSJ6Buff"] = "超级赛亚人6",
            ["K7SSJ7Buff"] = "超级赛亚人7",
            ["SSJ8Buff"] = "超级赛亚人8",
            ["SSJ9Buff"] = "超级赛亚人9",
            ["SSJ9FPBuff"] = "超级赛亚人9全功率",
            ["SSJ10FPBuff"] = "超级赛亚人10禁忌之力",
        };

        private static Dictionary<string, string> NewUnlockHints = new()
        {
            //传说
            ["LSSJ5Buff"] = "你做得不错，但是你还会再次赌上世界的命运来精进自己的能力吗？（击败月亮领主）",
            ["LSSJ6Buff"] = "使用与邪恶傲慢精华融合的四星龙珠（制作邪能灌注龙珠）",
            ["LSSJ7Buff"] = "我觉得你应该在新形态更加努力地修炼（精通超级赛亚人6）",
            ["LSSJ8Buff"] = "在掌握众多形态之后，你能击败疯狂的邪教徒吗？（在解锁超级赛亚人6后击败拜月教邪教徒）\n或者使用拜月教邪教徒掉落的解锁物品",
            ["LSSJ9Buff"] = "地狱守护者的徽章可以与纯粹的气融合，从而解锁新的力量（制作纯粹徽章）",
            ["LSSJ9FPBuff"] = "和传说超级赛亚人9解锁条件相同",
            ["LSSJ10FPBuff"] = "吃掉你第一个朋友的巫毒娃娃，即可获得黑暗力量（制作盘中巫毒玩偶）\n[C/959595:译者补充：请确保解锁传说超级赛亚人9后使用]",
            //常规
            ["K7SSJ5Buff"] = "再次击败月亮领主，即可强化你的新力量\n若仍未解锁，请使用月亮领主掉落的解锁物品",
            ["K7SSJ6Buff"] = "使用与邪恶傲慢精华融合的四星龙珠（制作邪能灌注龙珠）",
            ["K7SSJ7Buff"] = "我觉得你应该在新形态更加努力地修炼（精通超级赛亚人6）",
            ["SSJ8Buff"] = "在掌握众多形态之后，你能击败疯狂的邪教徒吗？（在解锁超级赛亚人6后击败拜月教邪教徒）\n或者使用拜月教邪教徒掉落的解锁物品",
            ["SSJ9Buff"] = "地狱守护者的徽章可以与纯粹的气融合，从而解锁新的力量（制作纯粹徽章）",
            ["SSJ9FPBuff"] = "和超级赛亚人9解锁条件相同",
            ["SSJ10FPBuff"] = "吃掉你第一个朋友的巫毒娃娃，即可获得黑暗力量（制作盘中巫毒玩偶）\n[C/959595:译者补充：请确保解锁超级赛亚人9后使用]",
        };

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(Custom))
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, Custom, false, filters: new()
                {
                    MethodFilter = MethodFilter.MatchNames("OnConsumeItem", "ModifyBuffText", "Update", "PostUpdate")
                });
            }

        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod(Custom, out Mod mod))
                return;

            //形态名称
            foreach (var form in FormNames)
            {
                if (!ModelHelper.TryModifyFormName(mod, "K7DBTRF.Buffs.", form.Key, form.Value))
                    Mod.Logger.Info("Replace FAILED");
            }

            //存在2个变身树，一个传说资质，一个常规

            //传说超级赛亚人
            if (ModelHelper.TryGetNodes(mod, "K7DBTRF.Assets.LSSJAFPanel", out Node[] nodesLSSJAF)) 
            {
                for (int i = 0; i < nodesLSSJAF.Length; i++)
                {
                    if (NewUnlockHints.TryGetValue(nodesLSSJAF[i].BuffKeyName, out string newUnlockHint))
                        if (ModelHelper.TryModifyNodes(mod, "K7DBTRF.Assets.LSSJAFPanel", i, newUnlockHint))
                            Mod.Logger.Info("Replace Success");
                }
            }

            //超级赛亚人
            if (ModelHelper.TryGetNodes(mod, "K7DBTRF.Assets.SSJAFPanel", out Node[] nodesSSJAF))
            {
                for (int i = 0; i < nodesSSJAF.Length; i++)
                {
                    if (NewUnlockHints.TryGetValue(nodesSSJAF[i].BuffKeyName, out string newUnlockHint))
                        if (ModelHelper.TryModifyNodes(mod, "K7DBTRF.Assets.SSJAFPanel", i, newUnlockHint))
                            Mod.Logger.Info("Replace Success");
                }
            }

        }
    }
}
