using DBZGoatLib.Handlers;
using DBZGoatLib.Model;
using DBZGoatLib.UI;
using DBZGoatLib.UI.Components;
using DBZMODPORT.Buffs.SSJBuffs;
using log4net;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace DragonBall_CN.Common.K7DBTRF
{
    public class ConflictFormPatch : ModSystem
    {
        /*
         *用于修复同名SSJ8变身冲突问题
         *由于K7DBTRF使用Player.GetModPlayer<GPlayer>().FetchTransformation()选取变身
         *而FetchTransformation()中返回值用的是TransformationHandler.GetTransformation(TransformationMenu.ActiveForm)
         *使用模糊的 string buffName 而非更为精确的 int buffType ，
         *且GetTransformation()用Transformations.First(x => x.buffKeyName == buffName)
         *因此先注册的SSJ8Buff会先变身，而后注册的同名变身不会变身
         *
         *
         *通过获取选取当前变身树的Node，来推断属于哪个模组的变身buff
         */

        private Hook transNodeHook;
        private Hook getTransformationHook;

        //SSJ8
        private readonly static FormFilter SSJ8 = new FormFilter("SSJ8Buff", new()
        {
            ["AF & others Forms"] = "dbztac",
            ["Alternative Future"] = "K7DBTRF"
        });
        //Limit Breaker
        private readonly static FormFilter LimitBreaker = new FormFilter("LimitBreakerBuff", new()
        {
            ["SEPBSSFPanel"] = "SSBETES",
            ["FSSJPanel"] = "SSBETES",
            ["Extra Forms"] = "XV2Forms"
        });

        internal static bool hasChangeNode;

        //记录最后选取变身形态的面板索引,由于前置库神秘代码导致实际显示面板和索引对应不正确,故采用面板名称
        internal static int currentFormPanelIndex = 0;
        internal static string currentFormPanelName = "";

        public override void Load()
        {
            if (!DBCPatchConfig.Instance.FixConflictBuffKey)
                return;

            MethodInfo? leftClickMethod = typeof(TransNode)?.GetMethod("LeftClick", BindingFlags.Public | BindingFlags.Instance);
            MethodInfo? getTransformationMethod = typeof(TransformationHandler)?.GetMethod("GetTransformation", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static, [typeof(string)]);

            if (leftClickMethod is null)
            {
                Mod.Logger.Info("ConflictFormPatch]: leftClickMethod is null");
                return;
            }

            if (getTransformationMethod is null)
            {
                Mod.Logger.Info("[ConflictFormPatch]: getTransformationMethod is null");
                return;
            }

            transNodeHook = new Hook(leftClickMethod, LeftClickRecordNodes);
            getTransformationHook = new Hook(getTransformationMethod, BetterGetTransformation);
        }

        public override void PostSetupContent()
        {
            if (!DBCPatchConfig.Instance.DebugMode)
                return;

            //获取所有面板信息

            foreach (var panel in UIHandler.Panels)
            {
                Mod.Logger.Info($"Panel Name:{panel.Name}, Panel Index:{UIHandler.Panels.FindIndex(p => p.Name == panel.Name)}");
            }
            Mod.Logger.Info("Complete Panel");
            foreach (var truePanel in UIHandler.Panels.Where(p => p.Complete).ToList())
            {
                Mod.Logger.Info($"Panel Name:{truePanel.Name}, Panel Index:{UIHandler.Panels.FindIndex(p => p.Name == truePanel.Name)}");
            }
        }

        public override void Unload()
        {
            transNodeHook?.Dispose();
            transNodeHook = null;
            getTransformationHook?.Dispose();
            getTransformationHook = null;
        }

        //静态方法无需实例，对应方法返回值为nullable，钩子方法返回值也必须为nullable
        private static TransformationInfo? BetterGetTransformation(Func<string, TransformationInfo?> orig, string buffName)
        {
            //这里加入FormFilter
            if (TransformationHandler.Transformations.Any(x => 
                x.buffKeyName == SSJ8.conflictBuffKey || 
                x.buffKeyName == LimitBreaker.conflictBuffKey)) 
            {
                FormFilter? filter = buffName switch
                {
                    var name when name == SSJ8.conflictBuffKey => SSJ8,
                    var name when name == LimitBreaker.conflictBuffKey => LimitBreaker,
                    _ => null
                };

                if (!filter.HasValue)
                    return orig.Invoke(buffName);

                if (DBCPatchConfig.Instance.DebugMode) 
                {
                    Main.NewText($"Has {filter.Value.conflictBuffKey}");
                    Main.NewText($"{UIHandler.Panels[currentFormPanelIndex].Name}");
                }

                if (filter.Value.modTransformationInfo.TryGetValue(currentFormPanelName, out string modName))
                {
                    if (ModLoader.TryGetMod(modName, out Mod mod))
                    {
                        //使用更精准的buffType进行搜索
                        mod.TryFind<ModBuff>(filter.Value.conflictBuffKey, out ModBuff buff);
                        var transformation = TransformationHandler.GetTransformation(buff.Type);
                        if (DBCPatchConfig.Instance.DebugMode)
                            Main.NewText($"transformation:{transformation.Value.buffKeyName}, modName:{mod.Name}");
                        return transformation;
                    }
                }
                else 
                {
                    if (DBCPatchConfig.Instance.DebugMode)
                        Main.NewText("Not Find Mod");
                }
            }

            return orig.Invoke(buffName);
        }


        private static void LeftClickRecordNodes(Action<TransNode, UIMouseEvent> orig, TransNode self, UIMouseEvent evt)
        {
            Node node = self.Node;
            hasChangeNode = false;
            if (!node.UnlockCondition(Main.CurrentPlayer) || !node.DiscoverCondition(Main.CurrentPlayer))
            {
                SoundHandler.PlayVanillaSound(SoundID.MenuTick, Main.CurrentPlayer.position);
                    
                try
                {
                    Main.NewText(node.UnlockHint);
                    return;
                }
                catch 
                {
                    Main.NewText($"{node.BuffKeyName} 该变身解锁文本有bug，可能是该解锁文本为空");
                    return;
                }
            }

            SoundHandler.PlayVanillaSound(SoundID.MenuTick, Main.CurrentPlayer.position);
    
            if (node.ViewOnly)
            {
                node.OnSelect?.Invoke(Main.CurrentPlayer);
                return;
            }

            if (TransformationMenu.ActiveForm != node.BuffKeyName)
            {
                hasChangeNode = true;
                //currentFormPanelIndex = UIHandler.ActivePanel;
                currentFormPanelName = TransformationMenu.transformationPanel.Name;
                if (DBCPatchConfig.Instance.DebugMode)
                    Main.NewText($"hasChangeNode:{hasChangeNode}, currentActivePanel:{currentFormPanelIndex}, Panel Name:{currentFormPanelName} , Node Name:{node.BuffKeyName}");
            }

            TransformationMenu.ActiveForm = node.BuffKeyName;
            node.OnSelect?.Invoke(Main.CurrentPlayer);
        }


        //筛选器
        public struct FormFilter
        {
            public readonly string conflictBuffKey;
            public readonly Dictionary<string, string> modTransformationInfo;

            public FormFilter(string _conflictBuffKey, Dictionary<string, string> _modTransformationInfo)
            {
                conflictBuffKey = _conflictBuffKey;
                modTransformationInfo = _modTransformationInfo;
            }
        }
    }
}
