//using CalamityMod;
//using CalamityMod.CalPlayer.Dashes;
//using CalamityMod.Items.Armor.GodSlayer;
//using DBZMODPORT;
//using System.Collections.Generic;
//using Terraria;
//using Terraria.Localization;
//using Terraria.ModLoader;

//namespace DragonBall_CN.Common.dbzcalamityUTD
//{
//    [JITWhenModsEnabled("CalamityMod", "dbzcalamityUTD")]
//    public class SetBonusPatch : GlobalItem
//    {
//        //头盔和护甲对应名
//        private static Dictionary<string, string[]> SetBonus = new()
//        {
//            ["AerospecHood"] = ["AerospecBreastplate", "AerospecLeggings"],
//            ["AuricTeslaBattleBand"] = ["AuricTeslaBodyArmor", "AuricTeslaCuisses"],
//            ["BloodflareDemonHead"] = ["BloodflareBodyArmor", "BloodflareCuisses"],
//            //["DaedalusBand"] = ["BloodflareBodyArmor", "BloodflareCuisses"],
//            ["GodSlayerHood"] = ["GodSlayerChestplate", "GodSlayerLeggings"],
//            ["HydrothermicBand"] = ["AtaxiaArmor", "AtaxiaSubligar"],
//            //["ReaverHood"] = ["ReaverScaleMail", "ReaverCuisses"],
//            ["SilvaHeadgear"] = ["SilvaArmor", "SilvaLeggings"],
//            //["StatigelBand"] = ["StatigelArmor", "StatigelGreaves"],
//            ["TarragonHood"] = ["TarragonBreastplate", "TarragonLeggings"],
//            ["VictideHood"] = ["VictideBreastplate", "VictideLeggings"],
//            //["WulfrumHood"] = ["WulfrumArmor", "WulfrumLeggings"],
//        };

//        private LocalizedText localizedText;

//        public override string IsArmorSet(Item head, Item body, Item legs)
//        {
//            if (!ModLoader.TryGetMod("dbzcalamityUTD", out Mod cal))
//                return "";

//            if (!ModLoader.TryGetMod("CalamityMod", out Mod mod))
//                return "";

//            foreach (var armor in SetBonus) 
//            {
//                if (!mod.TryFind<ModItem>(armor.Key, out ModItem modHead) ||
//                    !cal.TryFind<ModItem>(armor.Value[0], out ModItem modBody) ||
//                    !cal.TryFind<ModItem>(armor.Value[1], out ModItem modLegs))
//                    return "";

//                if (head.type == modHead.Type &&
//                    body.type == modBody.Type &&
//                    legs.type == modLegs.Type)
//                    localizedText = Language.GetText($"Mods.dbzcalamityUTD.Items.{modHead.Name}.SetBonus");
//                    return localizedText.Value;
//            }

//            return "";
//        }

//        public override void UpdateArmorSet(Player player, string set)
//        {
//            if (!ModLoader.TryGetMod("dbzcalamityUTD", out Mod cal))
//                return;

//            if (!ModLoader.TryGetMod("CalamityMod", out Mod mod))
//                return;

//            if (set == "")
//                return;

//            var calPlayer = player.Calamity();

//            //天蓝
//            if (localizedText.Key.Contains("Aerospec"))
//            {
//                calPlayer.aeroSet = true;
//            }
//            //古圣金源
//            if (localizedText.Key.Contains("AuricTesla"))
//            {
//                calPlayer.tarraSet = true;
//                calPlayer.bloodflareSet = true;
//                calPlayer.godSlayer = true;
//                calPlayer.auricSet = true;

//                if (calPlayer.godSlayerDashHotKeyPressed || (player.dashDelay != 0 && calPlayer.LastUsedDashID == GodslayerArmorDash.ID))
//                {
//                    calPlayer.DeferredDashID = GodslayerArmorDash.ID;
//                    player.dash = 0;
//                }
//            }
//            //血炎
//            if (localizedText.Key.Contains("Bloodflare"))
//            {
//                calPlayer.bloodflareSet = true;
//            }
//            //弑神者
//            if (localizedText.Key.Contains("GodSlayer"))
//            {
//                calPlayer.godSlayer = true;
//                var hotkey = CalamityKeybinds.GodSlayerDashHotKey.TooltipHotkeyString();
//                //player.setBonus = GetLocalization("SetBonus").Format(CalamityUtils.SecondsToFrames(2.5f).FramesToSeconds(), hotkey, GodSlayerChestplate.DashCooldown.FramesToSeconds());
//                if (calPlayer.godSlayerDashHotKeyPressed || (player.dashDelay != 0 && calPlayer.LastUsedDashID == GodslayerArmorDash.ID))
//                {
//                    calPlayer.DeferredDashID = GodslayerArmorDash.ID;
//                    player.dash = 0;
//                }
//            }
//            //渊泉
//            if (localizedText.Key.Contains("Hydrothermic"))
//            {
//                calPlayer.ataxiaBlaze = true;
//                calPlayer.ataxiaVolley = true;
//            }
//            //始源林海
//            if (localizedText.Key.Contains("Silva"))
//            {
//                calPlayer.silvaSet = true;
//            }
//            //龙蒿
//            if (localizedText.Key.Contains("Tarragon"))
//            {
//                calPlayer.tarraSet = true;
//            }
//            //胜潮水
//            if (localizedText.Key.Contains("Victide"))
//            {
//                calPlayer.victideSet = true;
//            }

            
//        }
//    }
//}
