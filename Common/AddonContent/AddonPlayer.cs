using DBZGoatLib;
using DragonBall_CN.Common.AddonContent.Content;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static DragonBall_CN.DragonBall_CN;

namespace DragonBall_CN.Common.AddonContent
{
    [JITWhenModsEnabled("DBZGoatLib")]
    [ExtendsFromMod("DBZGoatLib")]
    public class AddonPlayer : ModPlayer
    {
        
        public bool PTOAchieved;
        public bool PTOActive;
        public bool PTOAchievedMsg;
        public bool PTOMatserMsg;
        public bool PTOAwakenReadyText;
        public bool PTOAwakenText;
        public bool PTOAwaken;
        public float PTOCritBonus;

        public override void ResetEffects()
        {
            PTOCritBonus = 0f;
        }

        //记录击杀Boss
        private readonly HashSet<int> killedBossTypes = new();

        public override void SaveData(TagCompound tag)
        {
            //由于字符串多打一个空格导致无法sl数据，每次进出世界导致PTOAchievedMsg = false
            //这里特别记录错误
            tag["DragonBall_CN:PTOAchieved"] = PTOAchieved;
            tag["DragonBall_CN:PTOAchievedMsg"] = PTOAchievedMsg;
            tag["DragonBall_CN:PTOMatserMsg"] = PTOMatserMsg;
            tag["DragonBall_CN:killedBossTypes"] = new List<int>(killedBossTypes);
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("DragonBall_CN:PTOAchieved"))
                PTOAchieved = tag.GetBool("DragonBall_CN:PTOAchieved");

            if (tag.ContainsKey("DragonBall_CN:PTOAchievedMsg"))
                PTOAchievedMsg = tag.GetBool("DragonBall_CN:PTOAchievedMsg");

            if (tag.ContainsKey("DragonBall_CN:PTOMatserMsg"))
                PTOMatserMsg = tag.GetBool("DragonBall_CN:PTOMatserMsg");

            killedBossTypes.Clear();
            if (tag.ContainsKey("DragonBall_CN:killedBossTypes"))
            {
                foreach (int t in tag.GetList<int>("DragonBall_CN:killedBossTypes"))
                    killedBossTypes.Add(t);
            }
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            //眼睛绘制
            if (PTOActive)
                drawInfo.colorEyes = new Color(58, 240, 200);
        }

        public override void PostUpdate()
        {
            //粒子在玩家使用手上，至于为什么不做成超宇宙的左手，因为我是蓝狗
            PTODustTrail();
        }

        public override void PostUpdateEquips()
        {
            // 仅本地玩家显示
            if (!PTOAchievedMsg && Player.whoAmI == Main.myPlayer && PTOAchieved)
            {
                PTOAchievedMsg = true;
                Main.NewText("这个世界认可了你……", new Color(58, 240, 200));
            }

            //觉醒文本
            if (PTOActive && Player.statLife <= Player.statLifeMax2 * 0.75)
            {
                PTOAwaken = true;
                PTOAwakenText = true; //低于75%血以上
                
            }
            else if (PTOActive && Player.statLife > Player.statLifeMax2 * 0.75)
            {
                PTOAwaken = false;
                PTOAwakenReadyText = true;//回到75%血以上准备文本
                PTOAwakenText = false;
            }
            if (PTOAwakenText && PTOAwakenReadyText && Player.whoAmI == Main.myPlayer)
            {
                CombatText.NewText(Player.Hitbox, Color.OrangeRed, "你体内的力量已觉醒", false, false);
                PTOAwakenText = PTOAwakenReadyText = false;
            }

            //精通时
            if (Player.GetModPlayer<GPlayer>().GetMastery(ModContent.BuffType<PTOBuff>()) >= 1f)
            {
                //文本
                if (!PTOMatserMsg && Player.whoAmI == Main.myPlayer && PTOAchieved)
                {
                    PTOMatserMsg = true;
                    Main.NewText("世界的意志与你融合……", new Color(58, 240, 200));
                }

                //额外效果
                Player.lavaImmune = true;
                Player.fireWalk = true;

                //药水提供的水上行走
                Player.waterWalk2 = true;

                //免疫原版所有Buff
                for (int debuffIndex = 0; debuffIndex <= BuffID.Count; debuffIndex++)
                {
                    if (Main.debuff[debuffIndex])
                    {
                        Player.buffImmune[debuffIndex] = true;
                    }
                }
            }

            if (PTOAwaken)
            {
                PTOCritBonus = 100f;
                Player.GetCritChance(DamageClass.Generic) += PTOCritBonus;
            }
            else 
            {
                PTOCritBonus = 0f;
            }
        }

        // 服务端调用，记一次击杀返回是否真的有新增

        public bool RecordBossKill(params int[] npcTypes)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return false;

            bool changed = false;
            foreach (int type in npcTypes)
            {
                if (type > 0 && killedBossTypes.Add(type))
                    changed = true;
            }
            if (BossRegistryHelper.AllSatisfied(killedBossTypes))
                PTOAchieved = true;
            return changed;
        }

        //写包发送,新玩家加入时 tML 会自动调本方法
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();

            packet.Write((byte)PTOPacketType.UnlockSync);
            packet.Write((byte)Player.whoAmI);
            packet.Write(PTOAchieved);

            int count = System.Math.Min(killedBossTypes.Count, 255);
            packet.Write((byte)count);
            int written = 0;
            foreach (int type in killedBossTypes)
            {
                if (written++ >= count)
                    break;
                packet.Write((short)type);
            }

            packet.Send(toWho, fromWho);
        }

        //收包（由主类的 HandlePacket 调用）
        public void ReceivePlayerSync(BinaryReader reader)
        {
            int count = reader.ReadByte();
            killedBossTypes.Clear();
            //击杀表单包
            for (int i = 0; i < count; i++)
                killedBossTypes.Add(reader.ReadInt16());
            if (BossRegistryHelper.AllSatisfied(killedBossTypes))
                PTOAchieved = true;
        }

        //服务端把"这份玩家状态"拷进快照
        public override void CopyClientState(ModPlayer targetCopy)
        {
            AddonPlayer clone = (AddonPlayer)targetCopy;
            clone.PTOAchieved = PTOAchieved;
            clone.killedBossTypes.Clear();
            clone.killedBossTypes.UnionWith(killedBossTypes);
        }

        //服务端每 tick 拿快照比对，有差别发包
        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            AddonPlayer clone = (AddonPlayer)clientPlayer;
            if (PTOAchieved != clone.PTOAchieved || !killedBossTypes.SetEquals(clone.killedBossTypes))
            {
                SyncPlayer(toWho: -1, fromWho: Player.whoAmI, newPlayer: false);
            }
        }

        internal void PTODustTrail()
        {
            //更新速度快不会出现粒子速度跟不上玩家情况，测试下来15倍速也跟的上
            //不再PTOBuff中写，因为PTOBuff继承变身，后者再继承buff，内部Update绕了前置库导致更新速度没这个快
            if (!PTOActive)
                return;

            Vector2? hand = Player.HandPosition;

            if (!hand.HasValue)
                return;

            //拖尾位置,通过减去玩家速度实现粒子生成在玩家后面，形成拖尾
            Vector2 trailPositon = hand.Value - Player.velocity + Player.direction * new Vector2(1f, -1f);

            //决定每次粒子数量，一帧3个
            for (int i = 0; i < 3; i++)
            {
                Dust d = Main.dust[Dust.NewDust(trailPositon, 0, 0, DustID.UltraBrightTorch/*亮度高、饱和度较高的粒子*/ , 0f, 0f, 255, new Color(58, 240, 200), 0.7f)];

                d.position = trailPositon; //这里再设置因为原版还有随机偏移量
                d.noGravity = true;
                d.fadeIn = 1f;
                d.velocity = Player.velocity * 0.9f;

                if (Main.rand.Next(2) == 0)
                {
                    d.position += Utils.RandomVector2(Main.rand, -2f, 2f);//X Y 都会变成正方形
                    d.scale += Utils.NextFloat(Main.rand);
                }
            }
        }
    }
}

