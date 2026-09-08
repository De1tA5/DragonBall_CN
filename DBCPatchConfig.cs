using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace DragonBall_CN
{
    public class DBCPatchConfig:ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static DBCPatchConfig Instance;

        [ReloadRequired]
        [DefaultValue(true)]
        public bool HideDBTBalanceTransformationTree { get; set; }
    }
}
