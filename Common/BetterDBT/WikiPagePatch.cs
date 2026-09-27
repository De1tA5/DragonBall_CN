using DragonBall_CN;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace DragonBall_CN.Common.BetterDBT
{
    //史上最不极端、最不搞人心态的硬编码模组，看完不笑说明你不做硬编码汉化
    public class WikiPagePatch : ModSystem
    {
        
        private ILHook? addButtonHook;
        private ILHook? addTextHook;
        //private ILHook? showFistpageHook;
        //private ILHook? onInitializeHook;

        private static readonly Dictionary<string, string> ButtonsName = new()
        {
            //OnInitialize
            ["< Back"] = "返回",
            ["Close Book"] = "关闭书本",
            //FistButtons
            ["Basic Combo"] = "基础连段",
            ["Kaioken Combo"] = "界王拳",
            ["Star Fist"] = "星之拳",
            ["Blazing Gauntlet"] = "炽焰拳套",
            ["Mech Gauntlet"] = "机械拳套",
            ["Thunder Fist"] = "雷拳",
            ["Dragon Gauntlet"] = "龙拳",
            ["Radiant Gauntlet"] = "辐辉拳套",
            ["Legendary Fist"] = "传说之拳",
            ["Ultra Combo"] = "究极拳套",
            
        };

        public override void Load()
        {
            if (!ModLoader.TryGetMod("BetterDBT", out Mod mod))
                return;

            Type? type = mod.Code.GetType("BetterDBT.NewContent.WikiUIState");

            MethodInfo? addButtonMethod = type?.GetMethod("AddButton", BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo? addTextMethod = type?.GetMethod("AddText", BindingFlags.Instance | BindingFlags.NonPublic);

            //MethodInfo? onInitializeMethod = type?.GetMethod("OnInitialize", BindingFlags.Instance | BindingFlags.NonPublic);

            //FieldInfo? backButton = type?.GetField("")

            //MethodInfo? showFistpageMethod = type?.GetMethod("ShowFistpage", BindingFlags.Instance | BindingFlags.NonPublic);

            if (addTextMethod is null || addButtonMethod is null)
            {
                Mod.Logger.Warn("Method is null");
                return;
            }
            
            //传入参数时进行替换，导出的即为翻译后文本
            addButtonHook = new ILHook(addButtonMethod, il =>
            {
                var c = new ILCursor(il);

                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<string, string>>(text => 
                {
                    return ButtonsName.TryGetValue(text, out string translated) ? translated : text;
                });
                c.Emit(OpCodes.Starg_S, (byte)1);
            });

            addTextHook = new ILHook(addTextMethod, il =>
            {
                var c = new ILCursor(il);

                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<string, string>>(text => 
                {
                    return ButtonsName.TryGetValue(text, out string translated) ? translated : text;
                });
                c.Emit(OpCodes.Starg_S, (byte)1);
            });

            
            
            //UIState? uiState = mod.GetUIState(type, "wikiUI");

            //if (uiState is null)
            //    return;

            //int count = UIPatchHelper.LocalizeWikiButtonList(uiState, fistButtonTranslations);

            //FieldInfo? uiListField = uiState.GetType().GetField("buttonList", BindingFlags.Instance | BindingFlags.NonPublic);

            //UIList? uiList = uiListField?.GetValue(uiState) as UIList;

            //if (uiList is null)
            //    return;

            //uiList.LocalizeUIList(fistButtonTranslations);


            //showFistSubClassWeaponsHook = new ILHook(fistSubClassMethod, il =>
            //{
            //    int replacedCount = 0;
            //    var c = new ILCursor(il);

            //    foreach (var pair in fistButtons) 
            //    {
            //        if (!c.TryGotoNext(MoveType.Before, instruction => instruction.MatchLdstr(pair.Key)))
            //        {
            //            Mod.Logger.Warn($"未找到IL字符串：{pair.Key}");
            //            continue;
            //        }

            //        c.Next.Operand = pair.Value;
            //        replacedCount++;
            //        c.Index++;
            //    }
            //    Mod.Logger.Info($"ShowFistSubClassWeapons IL 字符串替换数：{replacedCount}/{fistButtons.Count}");
            //});

            //showFistPageHook = new ILHook(fistPageMethod, il =>
            //{
            //    int replacedCount = 0;
            //    var c = new ILCursor(il);

            //    foreach (var pair in fistButtons)
            //    {
            //        if (!c.TryGotoNext(MoveType.Before, instruction => instruction.MatchLdstr(pair.Key)))
            //        {
            //            Mod.Logger.Warn($"未找到IL字符串：{pair.Key}");
            //            continue;
            //        }

            //        c.Next.Operand = pair.Value;
            //        replacedCount++;
            //        c.Index++;
            //    }
            //    Mod.Logger.Info($"ShowFist IL 字符串替换数：{replacedCount}/{fistButtons.Count}");
            //});
        }

        public override void Unload()
        {
            addButtonHook?.Dispose();
            addButtonHook = null;
            addTextHook?.Dispose();
            addTextHook = null;
        }
    }
}
