using DBZGoatLib.Model;
using DragonBall_CN.Common.DBZGoatLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using TigerForceLocalizationLib;
using TigerForceLocalizationLib.Filters;

namespace DragonBall_CN.Common.GFAE
{
    public class GFAEPatch:ModSystem
    {
        private readonly static string GFAE = "GFAE";

        private static Dictionary<string, string> FormNames = new()
        {
            //天使
            ["SSJ2RBuff"] = "超级赛亚人桃红2",
            ["SSJ3RBuff"] = "超级赛亚人桃红3",
            ["BOFLBuff"] = "光之壁",
        };

        private static Dictionary<string, string> NewUnlockHints = new()
        {
            //天使
            ["SSJ2RBuff"] = "适应你最强的形态是获取力量的关键（仅限天使资质）\n[C/959595:译者补充：拥有天使资质，且超级赛亚人桃红掌握度100%时解锁]",
            ["SSJ3RBuff"] = "与亵渎天神的战斗，或许能让人突破极限（仅限天使资质）\n[C/959595:译者补充：拥有天使资质，且超级赛亚人桃红2掌握度达到100%时，在亵渎天神Boss战中即可解锁]",
            ["BOFLBuff"] = "地牢中的灵气生物将助你达到凡人无法企及的高度（仅限天使资质）\n[C/959595:译者补充：拥有天使资质，且超级赛亚人桃3红掌握度100%时，击败噬魂幽花即可解锁]",
        };

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(GFAE)) 
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, GFAE, false, filters: new() 
                {
                    MethodFilter = MethodFilter.MatchNames("Kill", "PostUpdate", "ProcessTriggers", "ModifyBuffText")
                });
            }
        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod(GFAE, out Mod mod))
                return;

            //形态名称
            foreach (var form in FormNames)
            {
                if (!ModelHelper.TryModifyFormName(mod, "GFAE.Transformations.", form.Key, form.Value))
                    Mod.Logger.Info("Replace FAILED");
            }

            if (!ModelHelper.TryGetNodes(mod, "GFAE.Assets.AEPlayer+BSSFPanel", out Node[] nodes))
                return;

            //解锁条件
            for (int i = 0; i < nodes.Length; i++)
            {
                if (NewUnlockHints.TryGetValue(nodes[i].BuffKeyName, out string newUnlockHint))
                    if (ModelHelper.TryModifyNodes(mod, "GFAE.Assets.AEPlayer+BSSFPanel", i, newUnlockHint))
                        Mod.Logger.Info("Replace Success");
            }
        }
    }
}
