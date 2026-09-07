using DBZGoatLib.Model;
using DragonBall_CN.Common.DBZGoatLib;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TigerForceLocalizationLib;
using TigerForceLocalizationLib.Filters;

namespace DragonBall_CN.Common.Oozaru144port
{
    public class Oozaru144portPatch:ModSystem
    {
        private static readonly string Oozaru = "oozaru144port";

        private static Dictionary<string, string> FormNames = new()
        {
            //传说
            ["LSSJ4Buff"] = "传说超级赛亚人4",
            ["LSSJ4LBBuff"] = "传说超级赛亚人4极限突破", 
            //常规
            ["SSJ4Buff"] = "超级赛亚人4",
            ["SSJ4FPBuff"] = "超级赛亚人4全功率",
            ["SSJ4LBBuff"] = "超级赛亚人4极限突破",
            ["SSJ5Buff"] = "超级赛亚人5",
            ["SSJ5FPBuff"] = "超级赛亚人5全功率"
        };

        private static Dictionary<string, string> NewUnlockHints = new()
        {
            //传说
            ["LSSJ4Buff"] = "唯有击败宇宙级的敌人，才能解锁这股力量\n[C/959595:译者补充：传说超级赛亚人3掌握度100%时，击败月亮领主解锁，或直接使用月亮领主掉落的道具解锁]",
            //常规
            ["SSJ4Buff"] = "邪教徒会将你逼至极限，而能否打破这些极限，全凭你自己\n[C/959595:译者补充：超级赛亚人3掌握度100%时，击败拜月教邪教徒，或者直接使用拜月邪教徒掉落的解锁道具]",
            ["SSJ4FPBuff"] = "来自太阳的天界柱将赐予你全功率\n[C/959595:译者补充：超级赛亚人4掌握度100%时，实际上击败任意天界柱即可解锁，或直接使用仅日耀柱掉落的道具解锁]",
            ["SSJ4LBBuff"] = "月亮领主正静候你的到来\n[C/959595:译者补充：超级赛亚人4全功率掌握度100%时，击败月亮领主解锁，或直接使用月亮领主掉落的道具解锁]",
            ["SSJ5Buff"] = "处于超级赛亚人4极限突破形态时，死亡有20%概率解锁该形态\n[C/959595:译者补充：实际上是25%概率，解锁时回满全部血量]",
            ["SSJ5FPBuff"] = "处于超级赛亚人5形态时，死亡有20%概率解锁该形态\n[C/959595:译者补充：实际上是25%概率，解锁时回满全部血量]",
        };

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(Oozaru)) 
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, Oozaru, false);
            }
        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod(Oozaru, out Mod mod))
                return;

            foreach (var form in FormNames)
            {
                if (!ModelHelper.TryModifyFormName(mod, "oozaru144port.Transformations.", form.Key, form.Value))
                    Mod.Logger.Info("Replace FAILED");
            }

            if (!ModelHelper.TryGetNodes(mod, "oozaru144port.Assets.OPlayer+SSJ4Panel", out Node[] nodes))
                return;

            for (int i = 0; i < nodes.Length; i++)
            {
                if (NewUnlockHints.TryGetValue(nodes[i].BuffKeyName, out string newUnlockHint))
                    if (ModelHelper.TryModifyNodes(mod, "oozaru144port.Assets.OPlayer+SSJ4Panel", i, newUnlockHint))
                        Mod.Logger.Info("Replace Success");
            }
        }

        public override void AddRecipes()
        {
            if (!ModLoader.TryGetMod(Oozaru, out Mod mod))
                return;

            if (!mod.TryFind<ModItem>("SSJ5Fur", out ModItem SSJ5Fur) ||
                !mod.TryFind<ModItem>("SSJ4Fur", out ModItem SSJ4Fur))
                return;

            //修复超级赛亚人5毛发合成配方缺失
            Recipe recipe = Recipe.Create(SSJ5Fur.Type)
                .AddIngredient(SSJ4Fur, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
