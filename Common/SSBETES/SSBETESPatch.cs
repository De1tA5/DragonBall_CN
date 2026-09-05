using DBZGoatLib.Model;
using DragonBall_CN.Common.DBZGoatLib;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using TigerForceLocalizationLib;

namespace DragonBall_CN.Common.SSBETES
{
    public class SSBETESPatch:ModSystem
    {
        private readonly string blueAddon = "SSBETES";

        private static Dictionary<string, string> FormNames = new()
        {
            ["DBEnchancedBuff"] = "龙珠附魔",
            //远古
            ["BERSERKERBuff"] = "暴走",
            ["EvilSaiyanBuff"] = "邪恶赛亚人",
            ["SSJ3FPBuff"] = "超级赛亚人3全功率",
            //传说
            ["IKARIBuff"] = "狂暴",
            ["FPSSJBuff"] = "传说超级赛亚人全功率",
            ["LimitBreakerBuff"] = "传说极限突破",
            //常规
            ["FSSJBuff"] = "拟态超级赛亚人",
            ["SSJFPBuff"] = "超级赛亚人全功率",
            ["SSJRGBuff"] = "超级赛亚人暴怒",
            ["PSSBBuff"] = "完美超级赛亚人蓝",
            ["SSBEBuff"] = "超级赛亚人蓝进化",
            ["SSBKKBuff"] = "超级赛亚人蓝10倍界王拳",
            ["SSBKKx20Buff"] = "超级赛亚人蓝20倍界王拳",
        };

        private static Dictionary<string, string> NewUnlockHints = new()
        {
            //远古
            ["EvilSaiyanBuff"] = "克苏鲁生物或许会揭示远古的力量\n[C/959595:译者补充：使用击败克苏鲁之眼掉落的力量之果即可解锁]",
            ["SSJ3FPBuff"] = "宇宙之物将逼迫你至极限\n[C/959595:译者补充：超级赛亚人3掌握度100%时，击败天界柱拾取掉落的邪恶之灵，即可解锁该形态]",
            //传说
            ["IKARIBuff"] = "粉碎光暗之间的封印，将赐予你狂野的力量\n[C/959595:译者补充：击败血肉墙解锁]",
            ["LimitBreakerBuff"] = "在失去控制之后，新的力量将苏醒\n[C/959595:译者补充：解锁传说超级赛亚人2后解锁]",
            //常规
            ["FSSJBuff"] = "当你与世界的邪恶抗争，怒火将化为你的新机遇\n[C/959595:译者补充：在克苏鲁之脑或世界吞噬者Boss战中，低于最大生命值25%即可解锁]",
            ["SSJFPBuff"] = "精通超级赛亚人\n[C/959595:译者补充：超级赛亚人掌握度100%时解锁]",
            ["SSJRGBuff"] = "击败皇家史莱姆之后，战胜夜明邪教将为你揭示新的道路\n[C/959595:译者补充：击败史莱姆皇后，出现提示文本后，在超级赛亚人2掌握度100%时击败拜月教邪教即可解锁]",
            ["PSSBBuff"] = "完善你最强大的形态\n[C/959595:译者补充：超级赛亚人蓝掌握度100%时解锁]",
            ["SSBEBuff"] = "掌握神之形态后，你将面临抉择\n[C/959595:译者补充：超级赛亚人蓝掌握度100%时解锁，只能选择一个，可通过交换卷轴重选]",
            ["SSBKKBuff"] = "掌握神之形态后，你将面临抉择\n[C/959595:译者补充：超级赛亚人蓝掌握度100%时解锁，只能选择一个，可通过交换卷轴重选]",
        };

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(blueAddon)) 
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, blueAddon, false);
            }
        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod(blueAddon, out Mod mod))
                return;

            //对变身名称的翻译
            foreach (var form in FormNames)
            {
                if(!ModelHelper.TryModifyFormName(mod, "SSBETES.Buffs.Transformations.", form.Key, form.Value))
                    Mod.Logger.Info("Replace FAILED");
            }


            //超蓝附属有2个变身树，一个不包含“拟态超级赛亚人”（SEPBSSF），一个包含(FSSJ)，因此2个均需要进行替换

            //无“拟态超级赛亚人”
            if (ModelHelper.TryGetNodes(mod, "SSBETES.Assets.BUPPlayer+SEPBSSFPanel", out Node[] nodesSEPBSSF)) 
            {
                for (int i = 0; i < nodesSEPBSSF.Length; i++) 
                {
                    if (NewUnlockHints.TryGetValue(nodesSEPBSSF[i].BuffKeyName, out string newUnlockHint)) 
                    {
                        Mod.Logger.Info($"{nodesSEPBSSF[i].BuffKeyName} - {newUnlockHint}");
                        if (ModelHelper.TryModifyNodes(mod, "SSBETES.Assets.BUPPlayer+SEPBSSFPanel", i, newUnlockHint))
                            Mod.Logger.Info("Replace Success");
                    }
                
                }
            }

            //有“拟态超级赛亚人”
            if (ModelHelper.TryGetNodes(mod, "SSBETES.Assets.BUPPlayer+FSSJPanel", out Node[] nodesFSSJ)) 
            {
                for (int i = 0; i < nodesFSSJ.Length; i++)
                {
                    if (NewUnlockHints.TryGetValue(nodesFSSJ[i].BuffKeyName, out string newUnlockHint))
                    {
                        Mod.Logger.Info($"{nodesFSSJ[i].BuffKeyName} - {newUnlockHint}");
                        if (ModelHelper.TryModifyNodes(mod, "SSBETES.Assets.BUPPlayer+FSSJPanel", i, newUnlockHint))
                            Mod.Logger.Info("Replace Success");
                    }

                }

            }
        }
    }
}
