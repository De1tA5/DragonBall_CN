using DBZGoatLib.Model;
using DragonBall_CN.Common.DBZGoatLib;
using System.Collections.Generic;
using Terraria.ModLoader;
using TigerForceLocalizationLib;
using TigerForceLocalizationLib.Filters;


namespace DragonBall_CN.Common.SatanicDBT
{
    public class SatanicDBTPatch : ModSystem
    {
        private static readonly string Sin = "SatanicDBT";
        private static Dictionary<string, string> FormNames = new()
        {
            ["Envy"] = "嫉妒",
            ["Gluttony"] = "暴食",
            ["Greed"] = "贪婪",
            ["Lust"] = "色欲",
            ["Pride"] = "傲慢",
            ["Sloth"] = "懒惰",
            ["Wrath"] = "暴怒",
        };

        private static Dictionary<string, string> NewUnlockHints = new()
        {
            ["Envy"] = "有随机概率抽取\n[C/959595:译者补充：详细请见思想抽取器的译者补充文本或变身文档]",
            ["Gluttony"] = "有随机概率抽取\n[C/959595:译者补充：详细请见思想抽取器的译者补充文本或变身文档]",
            ["Greed"] = "有随机概率抽取\n[C/959595:译者补充：详细请见思想抽取器的译者补充文本或变身文档]",
            ["Lust"] = "有随机概率抽取\n[C/959595:译者补充：详细请见思想抽取器的译者补充文本或变身文档]",
            ["Pride"] = "有随机概率抽取\n[C/959595:译者补充：详细请见思想抽取器的译者补充文本或变身文档]",
            ["Sloth"] = "有随机概率抽取\n[C/959595:译者补充：详细请见思想抽取器的译者补充文本或变身文档]",
            ["Wrath"] = "有随机概率抽取\n[C/959595:译者补充：详细请见思想抽取器的译者补充文本或变身文档]",

        };

        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(Sin))
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, Sin, false, filters: new()
                {
                    MethodFilter = MethodFilter.MatchNames("PostUpdate", "OnConsumeItem")
                });
            }
        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod(Sin, out Mod mod))
                return;

            //形态名称
            foreach (var form in FormNames)
            {
                ModelHelper.TryModifyFormName(mod, "SatanicDBT.Content.Forms.", form.Key, form.Value);
            }

            if (!ModelHelper.TryGetNodes(mod, "SatanicDBT.SatanicTree", out Node[] nodes))
                return;

            //解锁条件
            for (int i = 0; i < nodes.Length; i++)
            {
                if (NewUnlockHints.TryGetValue(nodes[i].BuffKeyName, out string newUnlockHint))
                    ModelHelper.TryModifyNodes(mod, "SatanicDBT.SatanicTree", i, newUnlockHint);
            }
        }
    }
}
