using DBZGoatLib.Model;
using Microsoft.Build.Framework;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace DragonBall_CN.Common.AddonContent
{
    [JITWhenModsEnabled("DBZGoatLib")]
    [ExtendsFromMod("DBZGoatLib")]
    public class AddonPanel : TransformationTree
    {
        public override bool Complete()
        {
            return false;
        }

        public override string Name()
        {
            return "Addon Panel";
        }

        public override bool Condition(Player player)
        {
            return true;
        }

        public override Connection[] Connections()
        {
            //不要直接返回null,即便是一个形态也可能报错
            return [
                new Connection(4f,4f, 0f, false, new Gradient(Color.MediumVioletRed).AddStop(0.6f, new Color(255, 242, 251)))
                ];

            //return Array.Empty<Connection>();
        }

        public override Node[] Nodes()
        {
            return [
                new Node(4f,4f, "PTOBuff","DragonBall_CN/Common/AddonContent/Content/PTOBuff" , "仅献给配得上的人……（击败原版所有Boss、迷你Boss以及事件Boss）" , UnlockConditionPTO , DiscoverConditionPTO , false , null)
                ];
        }

        public bool UnlockConditionPTO(Player player)
        {
            AddonPlayer addonPlayer = player.GetModPlayer<AddonPlayer>();
            return addonPlayer.PTOAchieved;
        }

        public bool DiscoverConditionPTO(Player player)
        {
            AddonPlayer addonPlayer = player.GetModPlayer<AddonPlayer>();
            return addonPlayer.PTOAchieved;
        }
    }
}
