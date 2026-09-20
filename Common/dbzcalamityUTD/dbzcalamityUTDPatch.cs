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

namespace DragonBall_CN.Common.dbzcalamityUTD
{
    public class dbzcalamityUTDPatch:ModSystem
    {
        private static readonly string dragonBallCalamity = "dbzcalamityUTD";

        private static Dictionary<string, string> FormNames = new()
        {
            ["PUIBuff"] = "自在极意功",
            ["UEBuff"] = "自我极意功",
            ["UIBuff"] = "自在极意兆"
        };

        private static Dictionary<string, string> NewUnlockHints = new()
        {
            ["UIBuff"] = "通过击败硫磺海的妖虫，便能获得更敏锐的战斗本能\n[C/959595:译者补充：击败渊海灾虫即可解锁]",
            ["UEBuff"] = "通过战胜神明吞噬者的护卫，便能为这股力量铺平道路\n[C/959595:译者补充：击败风暴编制者、西格纳斯和无尽虚空之中的任意一个即可解锁]",
            ["PUIBuff"] = "与愚钝化身的对峙，能够觉醒神明的本能\n[C/959595:译者补充：击败痴愚金龙即可解锁]"
        };
        public override void PostSetupContent()
        {
            if (ModLoader.HasMod(dragonBallCalamity)) 
            {
                TigerForceLocalizationHelper.LocalizeAll(Mod.Name, dragonBallCalamity, false, filters: new()
                {
                    MethodFilter = MethodFilter.MatchNames(
                        "UpdateCinematic", "PreKill", "OnKill", "ProcessTriggers", "PostUpdate", "FreeDodge", "GetChat", "SetChatButtons", "SetBestiary",
                        "SetNPCNameList", "ReceiveFormUnlock", "UpdateArmorSet", "ModifyTooltips", "ProjectileInitialize", "AssembleHeatDesc", "ModifyBuffText", "BuildUIDescription")
                });
            }
        }

        public override void Load()
        {
            if (!ModLoader.TryGetMod(dragonBallCalamity, out Mod mod))
                return;

            //形态名称
            foreach (var form in FormNames)
            {
                if (!ModelHelper.TryModifyFormName(mod, "dbzcalamityUTD.Buffs.SSJForms.", form.Key, form.Value))
                    Mod.Logger.Info("Replace FAILED");
            }

            if (!ModelHelper.TryGetNodes(mod, "dbzcalamityUTD.Util.DBCATree", out Node[] nodes))
                return;

            //解锁条件
            for (int i = 0; i < nodes.Length; i++)
            {
                if (NewUnlockHints.TryGetValue(nodes[i].BuffKeyName, out string newUnlockHint))
                    if (ModelHelper.TryModifyNodes(mod, "dbzcalamityUTD.Util.DBCATree", i, newUnlockHint))
                        Mod.Logger.Info("Replace Success");
            }
        }
    }
}
