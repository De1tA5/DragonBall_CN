using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DragonBall_CN.Common.AddonContent
{
    [JITWhenModsEnabled("DBZGoatLib")]
    [ExtendsFromMod("DBZGoatLib")]
    public class BossKilledTracker : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if (!BossRegistryHelper.ShouldRecord(npc))
                return;

            // 找Boss本体，多段Boss用主体，realLife
            NPC creditSource = npc;
            if (npc.realLife >= 0 && npc.realLife != npc.whoAmI && npc.realLife < Main.maxNPCs)
            {
                NPC body = Main.npc[npc.realLife];
                if (body.active)
                    creditSource = body;
            }

            //记录整体Boss还是多段Boss
            int[] types = creditSource == npc ? [npc.type] : [npc.type, creditSource.type];

            // 给每个打过的玩家记录
            int credited = 0;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (!creditSource.playerInteraction[i])
                    continue;

                Player player = Main.player[i];
                if (!player.active)
                    continue;

                player.GetModPlayer<AddonPlayer>().RecordBossKill(types);
                credited++;
            }

            //一个人都没被标记 ，记录最后一个人
            int lastPlyer = creditSource.lastInteraction;
            if (credited == 0 && lastPlyer >= 0 && lastPlyer < Main.maxPlayers && Main.player[lastPlyer].active)
                Main.player[lastPlyer].GetModPlayer<AddonPlayer>().RecordBossKill(types);
        }
    }

    public static class BossRegistryHelper
    {
        //部分Boss算没被标记为 npc.boss = true
        public static readonly HashSet<int> ExtraBossTypes = [
            NPCID.Mothron,// 日食：蛾怪
            NPCID.GoblinSummoner,// 哥布林入侵：哥布林召唤师
            NPCID.BloodNautilus
        ];

        //是否为Boss
        public static bool ShouldRecord(NPC npc) => npc.boss || ExtraBossTypes.Contains(npc.type);

        //解锁清单
        public static readonly int[][][] Required =
        {
            // 常规
            [[NPCID.KingSlime]],
            [[NPCID.EyeofCthulhu]],
            [[NPCID.EaterofWorldsHead], [ NPCID.BrainofCthulhu]],//邪恶Boss2选1
            [[NPCID.QueenBee]],
            [[NPCID.SkeletronHead]],
            [[NPCID.Deerclops]],
            [[NPCID.WallofFlesh]],
            [[NPCID.QueenSlimeBoss]],
            [[NPCID.Retinazer, NPCID.Spazmatism]],// 双子：两只都要
            [[NPCID.TheDestroyer]],
            [[NPCID.SkeletronPrime]],
            [[NPCID.Plantera]],
            [[NPCID.Golem]],
            [[NPCID.DukeFishron]],
            [[NPCID.HallowBoss]],
            [[NPCID.CultistBoss]],
            [[NPCID.MoonLordCore]],

            // 事件
            [[NPCID.MourningWood]],// 南瓜月
            [[NPCID.Pumpking]],// 南瓜月
            [[NPCID.Everscream]],// 霜月
            [[NPCID.SantaNK1]],// 霜月
            [[NPCID.IceQueen]],// 霜月
            [[NPCID.DD2DarkMageT1], [ NPCID.DD2DarkMageT3]],// 旧日军团：黑暗法师（任一难度）
            [[NPCID.DD2OgreT2], [ NPCID.DD2OgreT3]],// 旧日军团：食人魔（任一难度）
            [[NPCID.DD2Betsy]],// 旧日军团：贝茜
            [[NPCID.MartianSaucerCore]], // 火星暴乱
            [[NPCID.PirateShip]],// 海盗入侵
            [[NPCID.GoblinSummoner]],// 哥布林入侵
            [[NPCID.BloodNautilus]]//恐惧鹦鹉螺
        };

        //已打过的类型集合是否满足整份清单
        public static bool AllSatisfied(ICollection<int> killed)
        {
            foreach (int[][] entry in Required)
            {
                bool entryOk = false;
                foreach (int[] plan in entry)
                {
                    bool planOk = true;
                    foreach (int type in plan)
                    {
                        if (!killed.Contains(type))
                        {
                            planOk = false;
                            break;
                        }
                    }
                    if (planOk)
                    {
                        entryOk = true;
                        break;
                    }
                }
                if (!entryOk)
                    return false;
            }
            return true;
        }
    }
}
