using DragonBall_CN.Common.DBZGoatLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.UI;
using TigerForceLocalizationLib;
using TigerForceLocalizationLib.Filters;

namespace DragonBall_CN.Common.Terrariangene
{
    public class TerrariangenePatch:ModSystem
    {
        private static readonly string TDB = "Terrariangene";

        private static Dictionary<string, string> FormNames = new()
        {
            ["AwakenedBuff"] = "觉醒",
            ["ResolvedWillBuff"] = "决意",
            ["ReinforcedSoulBuff"] = "锻魄",
            ["TrueAwakeningBuff"] = "真·觉醒",
            ["AuricInfusedBuff"] = "金源灌注",
            ["ApexOfEternityBuff"] = "永恒之巅",
        };

        private static readonly string[] NewFormNames = [
            "觉醒",
            "决意",
            "锻魄",
            "真·觉醒",
            "金源灌注",
            "永恒之巅",
            ];
        private static readonly string[] NewUnlockHints = [
            "许多人能够发现的力量，但很少有人能准备好承受它",
            "若让愤怒驱使你前行，你多半命不久矣",
            "历经一切考验而存货之物，将变得难以摧毁",
            "一个明白自身使命的泰拉人，不再只为自己而战",
            "丛林巨龙放下了无尽的火焰，\n以便另一人能挺身对抗暴君",
            "最高的使命并非拥有永恒，\n而是当永恒托付于你时，仍能保持忠诚"
            ];

        private static readonly string[] NewCombatMasteryHints = [
            "不留破绽，自会精通",
            "不断其势，自会精通",
            ];

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(TDB)) 
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, TDB, false);
                //TigerForceLocalizationHelper.LocalizeAll(Mod.Name, TDB, false, filters: new() 
                //{
                //    MethodFilter = MethodFilter.MatchNames("UseItem")
                //});
            }
        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod(TDB, out Mod mod))
                return;

            //形态名称
            foreach (var form in FormNames)
            {
                ModelHelper.TryModifyFormName(mod, "Terrariangene.Buffs.Transformations.", form.Key, form.Value);
            }
        }

        public override void PostAddRecipes()
        {
            if (!ModLoader.TryGetMod(TDB, out Mod mod))
                return;

            ReflectionString(mod, "formNames", NewFormNames);
            ReflectionString(mod, "formHints", NewUnlockHints);
            ReflectionString(mod, "combatMasteryHints", NewCombatMasteryHints);
        }

        private static void ReflectionString(Mod mod, string fieldName, string[] newString)
        {
            Type? systemType = mod.Code.GetType("Terrariangene.Systems.TerrarianUISystem");

            Type? uiStateType = mod.Code.GetType("Terrariangene.UI.TerrarianGeneUI");

            if (systemType is null || uiStateType is null)
                return;

            //获取 TerrarianUISystem 实例
            ModSystem? system = ModContent.GetContent<ModSystem>()
                .FirstOrDefault(s =>
                    s.Mod == mod &&
                    s.GetType() == systemType);

            //System里UIState实例字段
            FieldInfo? uiField = systemType?.GetField("terrarianUI", BindingFlags.Instance | BindingFlags.NonPublic);

            object? uiInstance = system is null ? null : uiField?.GetValue(system);

            FieldInfo? field = uiStateType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (uiInstance is not null && field is not null)
            {
                field.SetValue(uiInstance, newString);
                if (DBCPatchConfig.Instance.DebugMode)    
                    ModContent.GetInstance<DragonBall_CN>().Logger.Info($"[TerrariangenePatch]: {fieldName} SetValue Success");
                return;
            }

            return;
        }
    }
}
