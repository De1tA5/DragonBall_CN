using DBTBalanceRevived.Buffs;
using DBZGoatLib.Handlers;
using DBZGoatLib.Model;
using DragonBall_CN.Common.DBZGoatLib;
using DragonBall_CN.Common.Oozaru144port;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria.ModLoader;
using TigerForceLocalizationLib;

namespace DragonBall_CN.Common.DBTBalanceRevived
{
    public class DBTBalanceRevivedPatch : ModSystem
    {
        private static readonly string Balance = "DBTBalanceRevived";

        public override void PostSetupContent()
        {
            if (!ModLoader.TryGetMod(Balance, out Mod mod))
                return;

            //TigerForceLocalizationHelper.LocalizeAll(Mod.Name, Balance, true);


            //开启Oozaru，将禁用DBTrebalance的传说超级赛亚人4的变身BUFF
            //由于Oozaru和DBTrebalance的传说超级赛亚人4的类名相同
            //又因为模组加载顺序，及先加载DBTrebalance后加载Oozaru，会导致系统错误变身选择前者
            if (DBCPatchConfig.Instance.HideDBTBalanceTransformationTree) 
            {
                if(ModelHelper.TryRemoveTransformationBuff(mod, "LSSJ4Buff"))
                {

                    Mod.Logger.Info("Remove Buff Succes");
                }
            }
        }

        public override void SetStaticDefaults()
        {
            if (!ModLoader.TryGetMod(Balance, out Mod mod))
                return;

            //开启Oozaru，将禁用DBTrebalance的传说超级赛亚人4的变身树
            if (DBCPatchConfig.Instance.HideDBTBalanceTransformationTree)
            {
                if (ModelHelper.TryRemovePanel("LSSJ Partial Tree")) 
                {
                    Mod.Logger.Info("Remove Panel Succes");
                }
            }
        }
        public override void Load()
        {

            if (!ModLoader.TryGetMod(Balance, out Mod mod))
                return;

            //变身文本翻译
            if (!ModelHelper.TryModifyFormName(mod, "DBTBalanceRevived.Buffs.", "LSSJ4Buff", "传说超级赛亚人4"))
                Mod.Logger.Info("Replace FAILED");

            if (!ModelHelper.TryGetNodes(mod, "DBTBalanceRevived.Buffs.LSSJ4Panel", out Node[] nodes))
                return;

            //解锁文本 - TODO禁用配置选项后导致显示Oozaru的解锁条件
            if (ModelHelper.TryModifyNodes(mod, "DBTBalanceRevived.Buffs.LSSJ4Panel", 0, "唯有击败宇宙级的敌人，才能解锁这股力量\n[C/959595:译者补充：传说超级赛亚人3掌握度100%时，击败月亮领主解锁]"))
                Mod.Logger.Info("Replace Success");

        }
    }
}
