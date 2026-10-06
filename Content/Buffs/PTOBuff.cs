//using DBZGoatLib.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;
//using DragonBall_CN.Common.AddonContent;

//namespace DragonBall_CN.Content.Buffs
//{
//    public class PTOBuff : Transformation
//    {
//        public override Gradient KiBarGradient()
//        {
//            return new Gradient();
//        }

//        public override string FormName()
//        {
//            return "攻克之力";
//        }

//        public override AuraData AuraData()
//        {
//            throw new NotImplementedException();
//        }

//        public override string HairTexturePath()
//        {
//            return "";
//        }

//        public override void OnTransform(Player player)
//        {
            
//        }

//        public override void PostTransform(Player player)
//        {
//        }

//        public override bool CanTransform(Player player)
//        {
//            return player.GetModPlayer<AddonPlayer>().PTOAchieved;
//        }

//        public override bool SaiyanSparks()
//        {
//            return false;
//        }

//        public override bool Stackable()
//        {
//            return false;
//        }

//        public override SoundData SoundData()
//        {
//            return new SoundData("DBZMODPORT/Sounds/SSJAscension", "DBZMODPORT/Sounds/SSJAura", 120);
//        }

//        public override Color TextColor()
//        {
//            return new Color(160, 220, 255);
//        }

//        public override void SetStaticDefaults()
//        {
//            this.kiDrainRate = 0.35f;
//            this.kiDrainRateWithMastery = 0.25f;
//            this.attackDrainMulti = 0.1f;
//            this.baseDefenceBonus = 20;
//            this.damageMulti = 1.2f;
//            this.speedMulti = 1.4f;
//            base.SetStaticDefaults();
//        }
//    }
//}
