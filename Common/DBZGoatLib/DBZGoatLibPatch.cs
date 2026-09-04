using DBZGoatLib.Model;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using TigerForceLocalizationLib;

namespace DragonBall_CN.Common.DBZGoatLib
{
    [JITWhenModsEnabled("DBZGoatLib")]
    public class DBZGoatLibPatch : ModSystem
    {
        private readonly string dragonBallLib = "DBZGoatLib";
        private static Dictionary<string, string> NewUnlockHints = new()
        {
            ["SSJ1Buff"] = "唯有在强敌面前经历失败，真正的力量才会觉醒\n[C/959595:译者补充：击败骷髅王后Boss战死亡4次，必定变身，死亡次数可积攒]",
            ["SSJ2Buff"] = "在升华状态下，唯有承受极端的压力，才能唤醒真正的力量\n[C/959595:译者补充：击败任意机械Boss后，处于100%掌握度的超级赛亚人，在Boss战中死亡]",
            ["SSJ3Buff"] = "古老敌人的力量，或许正是解锁更强大力量的关键\n[C/959595:译者补充：处于100%掌握度的超级赛亚人2，在与石巨人战斗中死亡]",
            ["SSJGBuff"] = "夜明恒星的神力，或许能唤醒超越凡人理解的存在\n[C/959595:译者补充：超级赛亚人3的掌握度为100%时，捡起天界柱掉落的超级赛亚人之神解锁物品]",
            ["LSSJBuff"] = "最稀有的赛亚人或许才能达到一种超越普通赛亚人所能获得的形态\n[C/959595:译者补充：击败任意机械Boss后，处于100%掌握度的超级赛亚人，在Boss战中死亡]",
            ["LSSJ2Buff"] = "传说中的赛亚人有时在被逼入危急状况时，会彻底失去控制\n[C/959595:译者补充：击败猪龙鱼公爵后，玩家当前生命值低于最大生命值10%，持续5秒后有12.5%概率变身]",
            ["LSSJ3Buff"] = "神秘学之士掌握着这种形态的秘密\n[C/959595:译者补充：传说超级赛亚人2的掌握度为100%时，击败拜月教邪教徒]",
            ["SSJBBuff"] = "与银河级生物的战斗经历，或许能唤醒这股力量\n[C/959595:译者补充：超级赛亚人之神的掌握度为100%时，击败月亮领主]",
            ["SSJRBuff"] = "与银河级生物的战斗经历，或许能唤醒这股力量\n[C/959595:译者补充：超级赛亚人之神的掌握度为100%时，击败月亮领主]"
        };
        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(dragonBallLib))
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, dragonBallLib, false);
            }
        }

        public override void Load()
        {
            //界面文字汉化
            Defaults.FormNames = new()
            {
                { "SSJ1Buff", "超级赛亚人" },
                { "SuperKaiokenBuff", "超级界王拳"},
                { "SSJ2Buff", "超级赛亚人2" },
                { "SSJ3Buff", "超级赛亚人3" },
                { "ASSJBuff", "升华超级赛亚人"},
                { "USSJBuff", "究极超级赛亚人" },
                { "SSJGBuff", "超级赛亚人之神" },
                { "SSJBBuff", "超级赛亚人蓝" },
                { "SSJRBuff", "超级赛亚人桃红" },
                { "LSSJBuff", "传说超级赛亚人" },
                { "LSSJ2Buff", "传说超级赛亚人2" },
                { "LSSJ3Buff", "传说超级赛亚人3" }
            };

            //解锁方式汉化
            var nodes = Defaults.DefaultNodes;
            for (int i = 0; i < Defaults.DefaultNodes.Length; i++)
            {
                if (NewUnlockHints.TryGetValue(nodes[i].BuffKeyName, out string newUnlockHint))
                {
                   if(!ModelHelper.TryReplaceUnlockHint(nodes, i ,newUnlockHint))
                        Mod.Logger.Info("Replace FAILED"); 
                }

            }
        }
    }

    [JITWhenModsEnabled("DBZGoatLib")]
    public static class ModelHelper
    {
        private static BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
        public static bool TryReplaceUnlockHint(Node[] nodes, int index, string newUnlockHint)
        {

            if (nodes is null || index < 0 || index >= nodes.Length)
                return false;

            Node node = nodes[index];
            FieldInfo? unlockHintField = typeof(Node)?.GetField("UnlockHint", flags);

            if (unlockHintField is null)
                return false;

            object boxedNode = node;
            unlockHintField.SetValue(boxedNode, newUnlockHint);
            nodes[index] = (Node)boxedNode;
            
            return true;
        }
    }
}
