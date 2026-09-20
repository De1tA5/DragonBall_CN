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

        #region bug修复
        [Header("ExtraPatches")]

        [ReloadRequired]
        [DefaultValue(true)]
        public bool DBZMODPORTMapEntryFix { get; set; }

        [ReloadRequired]
        [DefaultValue(true)]
        public bool HideDBTBalanceTransformationTree { get; set; }
        #endregion

        #region 夹带私货

        //[Header("SpecialContent")]

        #endregion
    }
}
