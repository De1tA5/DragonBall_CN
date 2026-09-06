using DBZGoatLib.Model;
using DragonBall_CN.Common.DBZGoatLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using TigerForceLocalizationLib;

namespace DragonBall_CN.Common.GohanForms
{
    public class GohanFormsPatch:ModSystem
    {
        private static readonly string gohanForms = "GohanForms";

        private static Dictionary<string, string> FormNames = new()
        {
            ["MysticBuff"] = "究极",
            ["BeastBuff"] = "野兽",
        };

        private static Dictionary<string, string> NewUnlockHints = new()
        {
            
            ["MysticBuff"] = "用Z神剑攻击石巨人并与释放的老界王神对话\n[C/959595:译者补充：非传奇资质且已解锁超级赛亚人2才能解锁该形态]",
            ["BeastBuff"] = "月亮领主有着另一半解放潜能的关键（仅限天才资质）\n[C/959595:译者补充：究极形态掌握度100%时，且为天才资质击败月亮领主解锁]",
        };

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(gohanForms)) 
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, gohanForms, false);
            }
        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod(gohanForms, out Mod mod))
                return;

            foreach (var form in FormNames)
            {
                if (!ModelHelper.TryModifyFormName(mod, "GohanForms.Transformations.", form.Key, form.Value))
                    Mod.Logger.Info("Replace FAILED");
            }

            if (!ModelHelper.TryGetNodes(mod, "GohanForms.Assets.GOHPlayer+BSSFPanel", out Node[] nodes))
                return;

            for (int i = 0; i < nodes.Length; i++) 
            {
                if (NewUnlockHints.TryGetValue(nodes[i].BuffKeyName, out string newUnlockHint))
                    if (ModelHelper.TryModifyNodes(mod, "GohanForms.Assets.GOHPlayer+BSSFPanel", i, newUnlockHint))
                        Mod.Logger.Info("Replace Success");
            }
        }
    }
}
