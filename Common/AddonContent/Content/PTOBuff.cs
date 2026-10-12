using DBZGoatLib;
using DBZGoatLib.Model;
using DBZMODPORT;
using DragonBall_CN.Common.AddonContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DragonBall_CN.Common.AddonContent.Content
{
    public class PTOBuff : Transformation
    {
        public override Gradient KiBarGradient()
        {
            return new Gradient(Color.White).AddStop(1f, new Color(0, 198, 118));
        }

        public override string FormName()
        {
            return "攻克之力";
        }

        public override AuraData AuraData()
        {
            //颜色为发光颜色，只有7帧因为我是蓝狗
            return new AuraData("DragonBall_CN/Assets/PTO/PTOAura", 7, BlendState.AlphaBlend, new Color(58, 240, 200));
        }

        public override string HairTexturePath()
        {
            return "";
        }

        //变身一瞬间时
        public override void OnTransform(Player player)
        {
            player.GetModPlayer<AddonPlayer>().PTOActive = true;
        }

        //即变身结束后
        public override void PostTransform(Player player)
        {
            player.GetModPlayer<AddonPlayer>().PTOActive = false;
        }

        public override bool CanTransform(Player player)
        {
            return player.GetModPlayer<AddonPlayer>().PTOAchieved;
        }

        public override bool SaiyanSparks()
        {
            //额外增加闪电特效
            return true;
        }

        public override bool Stackable()
        {
            return true;
        }

        public override SoundData SoundData()
        {
            return new SoundData("DBZMODPORT/Sounds/SSJAscension", "DBZMODPORT/Sounds/SSJ3", 200);
        }

        public override Color TextColor()
        {
            //变身跳出文字
            return new Color(58, 240, 200);
        }

        public override void SetStaticDefaults()
        {
            kiDrainRate = 0.35f;
            kiDrainRateWithMastery = 0.25f;
            attackDrainMulti = 0.4f;//气消耗
            baseDefenceBonus = 50;//防御
            damageMulti = 5f;//倍率乘算
            speedMulti = 6f;
            //加上base.SetStaticDefaults()，否则时间不会隐藏
            base.SetStaticDefaults();
        }

        private const float awakenDamageMulti = 21f;

        public override void Update(Player player, ref int buffIndex)
        {
            AddonPlayer addonPlayer = player.GetModPlayer<AddonPlayer>();

            //使伤害回归正常值
            SetStaticDefaults();

            //statLifeMax2 受到装备影响故更新发生在PostUpdateEquips() 后
            if (addonPlayer.PTOAwaken)
            {
                // ！？超模模超？！
                player.GetDamage(DamageClass.Generic) *= awakenDamageMulti;
                damageMulti = 1f;//这里手动改GetDamage，故原倍率需调为1
                attackDrainMulti = 0f;
                kiDrainRateWithMastery = 0f;
                kiDrainRate = 0f;
                baseDefenceBonus = 0;
            }

            base.Update(player, ref buffIndex);

        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            Player player = Main.LocalPlayer;
            AddonPlayer addonPlayer = player.GetModPlayer<AddonPlayer>();

            string description = player.GetModPlayer<GPlayer>().GetMastery(ModContent.BuffType<PTOBuff>()) >= 1f
               ? this.GetLocalizedValue("MasterDescription")
               : this.GetLocalizedValue("Description");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(MyBuildTooltip());

            if (addonPlayer.PTOAwaken)
            {
                sb.Append($"暴击 [C/F54219:+100.00%]");
                buffName = "觉醒 · " + buffName;
            }

            tip = sb.ToString().Trim() + $"\n{description}"; 

        }

        public string MyBuildTooltip()
        {
            Player player = Main.LocalPlayer;

            float activeDamageMulti = player.GetModPlayer<AddonPlayer>().PTOAwaken ? awakenDamageMulti : damageMulti;

            float dmg = activeDamageMulti - 1f;
            float speed = speedMulti - 1f;
           

            float num1 = 60f * kiDrainRate;
            float num2 = 60f * kiDrainRateWithMastery;

            StringBuilder sb = new StringBuilder();

            if (dmg != 0f)
                sb.Append($"伤害 {(dmg > 0 ? '+' : "")}{dmg:P2} "); //P2 百分比 且保留小数点后2位
            if (speed != 0f)
                if (dmg != 0)
                    sb.AppendLine($" | 移动速度 {(speed > 0 ? '+' : "")}{speed:P2}");
                else
                    sb.AppendLine($"移动速度 {(speed > 0 ? '+' : "")}{speed:P2}");
            if (baseDefenceBonus != 0)
                sb.Append($"防御 {(baseDefenceBonus > 0 ? '+' : "")}{baseDefenceBonus:N0}");//N0 数字 不保留小数点数字 带千位分隔符
            if (attackDrainMulti != 0f)
                if (baseDefenceBonus != 0)
                    sb.AppendLine($" | 气消耗 {(attackDrainMulti > 0 ? '+' : '-')}{attackDrainMulti:P2}");
                else
                    sb.AppendLine($"气消耗 {(attackDrainMulti > 0 ? '+' : '-')}{attackDrainMulti:P2}");
            if (kiDrainRate != 0)
                sb.Append($"气损失 {MathF.Round(num1):N0}/s （精通时{MathF.Round(num2):N0}/s）");

            return sb.ToString().Trim();//加上Trim，移除因气消耗为0出现的空行
        }
    }
}
