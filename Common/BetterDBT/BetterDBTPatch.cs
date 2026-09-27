using System;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using TigerForceLocalizationLib;
using TigerForceLocalizationLib.Filters;

namespace DragonBall_CN.Common.BetterDBT
{
    public class BetterDBTPatch : ModSystem
    {
        private readonly static string BDBP = "BetterDBT";

        //Buff效果
        private readonly static string[] newBuffTooltips = 
            new string[8] {
                "邪恶： \n增加5%伤害\n提升5%移动速度\n减少10点防御\n减少1点生命再生速度\n可以与任意超级赛亚人/传说超级赛亚人叠加",
                "超级赛亚人蓝界王拳： \n增加25%伤害\n增加5点防御\n减少18点生命再生速度\n可叠加于超级赛亚人蓝形态",
                "超级赛亚人蓝进化： \n增加10%伤害\n增加15点防御\n你的生命值越低，获得的防御越高\n可叠加于超级赛亚人蓝形态",
                "超级赛亚人桃红2： \n增加10%伤害\n增加5点防御\n提升2点生命再生速度\n可叠加于超级赛亚人桃红形态",
                "狂怒： \n增加10%伤害\n增加10点防御\n提升15%移动速度\n受到伤害越多，你会变得更强大（最高30%伤害加成）\n仅限传说资质可叠加",
                "自在极意兆： \n提升20%移动速度\n自动回避敌怪\n可叠加任意形态",
                "超级赛亚人暴怒： \n增加15%伤害\n增加8点防御\n可叠加任意超级赛亚人形态",
                "自我极意功： \n你的生命值越低，你就越强大"
            };
        //Buff解锁方式
        private readonly static string[] newLockedTooltips =
            new string[8] {
                "无伤击败血肉墙",
                "全程使用界王拳通过Boss Rush",
                "未死亡情况下通过Boss Rush",
                "10分钟内速通Boss Rush",
                "通过Boss Rush时受到的总伤害不超过2000",
                "无伤通过Boss Rush",
                "无伤击败任意机械Boss",
                "未完成，无法通过正常手段获取"
            };
        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(BDBP))
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, BDBP, false);
                //其他方法
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, BDBP, false, filters: new()
                {
                    MethodFilter = MethodFilter.MatchNames(
                    "ModifyTooltips", "PostUpdateWorld", "PreUpdate", "OnEnterWorld", "OnKill",
                    "UseItem", "BuildTooltip_Hook","Update", "OnInitialize", "GetDustLabel","Draw")
                });
                //整个wikiUI
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, BDBP, false, filters: new()
                {
                    TypeFilter = TigerForceLocalizationLib.Filters.TypeFilter.MatchFullNames("BetterDBT.NewContent.WikiUIState")
                });
                //整个BossRush
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, BDBP, false, filters: new()
                {
                    TypeFilter = TigerForceLocalizationLib.Filters.TypeFilter.MatchFullNames("BetterDBT.NewContent.BossRush.BossRushSystem")
                });
                //悟空
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, BDBP, false, filters: new()
                {
                    TypeFilter = TigerForceLocalizationLib.Filters.TypeFilter.MatchFullNames("BetterDBT.NewContent.SonGoku.Goku")
                });

            }

        }

        //额外变身UI修复
        public override void PostSetupRecipes()
        {
            //由于BuffSelectorUIState实例化在PostSetupContent阶段,而且由于build没有sortAfter
            //因此要在该阶段更后面进行替换处理,PostAddRecipes()或者PostSetupRecipes()阶段均可
            if (ModLoader.TryGetMod(BDBP, out Mod mod))
            {
                //Buff效果
                ReflectionString(mod, "buffTooltips", newBuffTooltips);
                //Buff解锁条件
                ReflectionString(mod, "lockedTooltips", newLockedTooltips);
            }
        }

        private static bool ReflectionString(Mod mod, string fieldName ,string[] newString)
        {
            Type? systemType = mod.Code.GetType("BetterDBT.NewContent.FormTechs.BuffSelectorUISystem");

            Type? uiType = mod.Code.GetType("BetterDBT.NewContent.FormTechs.BuffSelectorUIState");

            if (systemType is null || uiType is null)
                return false;

            //获取 BuffSelectorUISystem 实例
            ModSystem? system = ModContent.GetContent<ModSystem>()
                .FirstOrDefault(s =>
                    s.Mod == mod &&
                    s.GetType() == systemType);

            FieldInfo? uiField = systemType?.GetField("buffUI", BindingFlags.Instance | BindingFlags.NonPublic);

            object? uiInstance = system is null ? null : uiField?.GetValue(system);

            FieldInfo? field = uiType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            if (uiInstance is not null && field is not null)
            {
                field.SetValue(uiInstance, newString);
                ModContent.GetInstance<DragonBall_CN>().Logger.Info("SetValue Success");
                return true;
            }

            return false;
        }
    }
}
