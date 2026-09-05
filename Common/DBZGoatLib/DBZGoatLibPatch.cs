using DBZGoatLib.Model;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;
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
            //菜单形态+变身文字汉化
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
            for (int i = 0; i < nodes.Length; i++)
            {
                if (NewUnlockHints.TryGetValue(nodes[i].BuffKeyName, out string newUnlockHint))
                {
                    if (!ModelHelper.TryReplaceUnlockHint(nodes, i, newUnlockHint))
                        Mod.Logger.Info("Replace FAILED");
                }
            }
            Defaults.DefaultNodes = nodes;
        }
    }

    [JITWhenModsEnabled("DBZGoatLib")]
    public class ModelHelper : ModSystem
    {
        private static readonly BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
        private static List<ILHook> ILHooks = new();

        /// <summary>
        /// Node为Lib常用的显示解锁条件数据结构，但标记为readonly
        /// 填入Node[]，对应索引以及翻译文本即可实现替换
        /// </summary>
        /// <param name="nodes">传入的Node数据</param>
        /// <param name="index">Node的索引填入遍历数据即可</param>
        /// <param name="newUnlockHint">翻译后的解锁文本</param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取TransformationTree的Nodes
        /// </summary>
        /// <param name="mod">对应附属模组</param>
        /// <param name="typeFullName">TransformationTree路径</param>
        /// <param name="nodes">获取的Nodes</param>
        /// <returns></returns>
        public static bool TryGetNodes(Mod mod, string typeFullName, out Node[] nodes)
        {
            nodes = null;

            Type? type = mod?.Code.GetType(typeFullName);

            MethodInfo? methodInfo = type?.GetMethod("Nodes", flags);

            object? instance = Activator.CreateInstance(type);

            if (instance is null)
                return false;

            if (methodInfo is null || methodInfo.ReturnType != typeof(Node[]))
                return false;

            nodes = methodInfo?.Invoke(instance, null) as Node[];

            return true;
        }

        /// <summary>
        /// TransformationTree为Lib用于储存变身菜单界面数据的数据结构
        /// 其中Nodes()包含形态解锁条件等，因此专门处理该方法
        /// </summary>
        /// <param name="mod">对应附属模组</param>
        /// <param name="typeFullName">TransformationTree路径</param>
        /// <param name="index">>Node的索引</param>
        /// <param name="newUnlockHint">翻译后的解锁文本</param>
        /// <returns></returns>
        public static bool TryModifyNodes(Mod mod, string typeFullName, int index, string newUnlockHint)
        {
            Type? type = mod?.Code.GetType(typeFullName);

            MethodInfo? methodInfo = type?.GetMethod("Nodes", flags);

            if (methodInfo is null)
                return false;

            ILHook hook = new(methodInfo, il =>
            {
                ILCursor c = new ILCursor(il);
                if (!c.TryGotoNext(MoveType.Before, instruction => instruction.MatchRet()))
                    return;
                c.EmitDelegate<Func<Node[], Node[]>>(nodes =>
                    {
                        TryReplaceUnlockHint(nodes, index, newUnlockHint);

                        return nodes;
                    });
                //if (!TryGetNodes(mod, typeFullName, out Node[] modifiedNodes))
                //    return;
                //if (!TryReplaceUnlockHint(modifiedNodes, index, newUnlockHint))
                //    return;
                //c.Goto(0);
                //c.EmitDelegate<Func<Node[]>>(() => modifiedNodes);
                //c.Emit(OpCodes.Ret);

            });
            ILHooks.Add(hook);

            return true;
        }

        /// <summary>
        /// 替换变身菜单，鼠标悬浮图标显示的变身名称以及变身时显示名称
        /// </summary>
        /// <param name="mod">对应附属模组</param>
        /// <param name="typeFullName">变身Buff类全部名/param>
        /// <param name="buffKeyName">变身Buff内部名</param>
        /// <param name="newName">翻译后名称</param>
        /// <returns></returns>
        public static bool TryModifyFormName(Mod mod, string typeFullName, string buffKeyName, string newName)
        {
            if (mod is null)
                return false;

            Type? type = mod?.Code.GetType(typeFullName + buffKeyName);

            MethodInfo? methodInfo = type?.GetMethod("FormName", flags);

            ILHook hook = new(methodInfo, il =>
            {
                ILCursor c = new(il);

                c.Goto(0);
                c.Emit(OpCodes.Ldstr, newName);
                c.Emit(OpCodes.Ret);
            });
            ILHooks.Add(hook);

            return true;
        }

        public override void Unload()
        {
            foreach (var hook in ILHooks)
            {
                hook?.Dispose();
            }
            ILHooks.Clear();
        }

    }
}
