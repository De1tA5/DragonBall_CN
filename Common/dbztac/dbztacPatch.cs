using DBZGoatLib.Model;
using DragonBall_CN.Common.DBZGoatLib;
using System.Collections.Generic;
using Terraria.ModLoader;
using TigerForceLocalizationLib;
using TigerForceLocalizationLib.Filters;

namespace DragonBall_CN.Common.dbztac
{
    public class dbztacPatch : ModSystem
    {
        private readonly static string DBZF = "dbztac";

        private static Dictionary<string, string> FormNames = new()
        {
            ["xSSJ4Buff"] = "超级赛亚人4",
            ["xSSJ4FPBuff"] = "超级赛亚人4全功率",
            ["xSSJ4LBBuff"] = "超级赛亚人4限界突破",
            ["xSSJ5Buff"] = "超级赛亚人5",
            ["xSSJ5FPBuff"] = "超级赛亚人5全功率",
            ["SSJ6Buff"] = "超级赛亚人6",
            ["SSJ7Buff"] = "超级赛亚人7",
            ["SSJ8Buff"] = "超级赛亚人8",
            ["VVSBuff"] = "紫色面纱",
            ["SSJ4INBuff"] = "超级赛亚人4黄金",
        };

        private static Dictionary<string, string> NewUnlockHints = new()
        {
            ["xSSJ4Buff"] = "某种剧毒生物藏着什么……（瘟疫使者歌莉娅）\n[C/959595:译者补充：击败瘟疫使者歌莉娅，使用掉落道具即可解锁]",
            ["xSSJ4FPBuff"] = "在超级赛亚人4形态下努力训练\n[C/959595:译者补充：超级赛亚人4掌握度达到100%时解锁]",
            ["xSSJ4LBBuff"] = "与银河之神战斗，会让你突破自身的极限（月亮领主）\n[C/959595:译者补充：击败月亮领主，且超级赛亚人4掌握度达到100%，使用掉落道具即可解锁]",
            ["xSSJ5Buff"] = "这确定是鸟？（痴愚金龙）\n[C/959595:译者补充：击败痴愚金龙，且超级赛亚人4限界突破掌握度达到100%，使用掉落道具即可解锁]",
            ["xSSJ5FPBuff"] = "在超级赛亚人5形态下努力训练\n[C/959595:译者补充：超级赛亚人5掌握度达到100%时解锁]",
            ["SSJ6Buff"] = "4星龙珠和夜之魂\n[C/959595:译者补充：超级赛亚人5掌握度达到100%，使用制作的腐化4星龙珠即可解锁]",
            ["SSJ7Buff"] = " 奇异晶球能将你带到无归的境地\n[C/959595:译者补充：超级赛亚人6掌握度达到100%，使用制作的奇异晶球即可解锁]",
            ["SSJ8Buff"] = "制作压缩暗离子体\n[C/959595:译者补充：超级赛亚人7掌握度达到100%，使用制作的压缩暗离子体即可解锁]",
            ["VVSBuff"] = "蘑菇巨兽（菌生蟹）\n[C/959595:译者补充：击败菌生蟹，使用掉落道具即可解锁]",
            ["SSJ4INBuff"] = "制作纯净圣光\n[C/959595:译者补充：超级赛亚人7掌握度达到100%，使用制作的纯净圣光即可解锁]",
        };

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(DBZF))
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, DBZF, false, filters: new() 
                {
                    MethodFilter = MethodFilter.MatchNames("PostUpdate", "ProcessTriggers")
                });
            }
        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod(DBZF, out Mod mod))
                return;

            //形态名称
            foreach (var form in FormNames)
            {
                if (!ModelHelper.TryModifyFormName(mod, "dbztac.Transformations.", form.Key, form.Value))
                    Mod.Logger.Info("Replace FAILED");
            }

            if (!ModelHelper.TryGetNodes(mod, "dbztac.Assets.DACPlayer+xSSJ4Panel", out Node[] nodes))
                return;

            //解锁条件
            for (int i = 0; i < nodes.Length; i++)
            {
                if (NewUnlockHints.TryGetValue(nodes[i].BuffKeyName, out string newUnlockHint))
                    if (ModelHelper.TryModifyNodes(mod, "dbztac.Assets.DACPlayer+xSSJ4Panel", i, newUnlockHint))
                        Mod.Logger.Info("Replace Success");
            }
        }
    }
}
