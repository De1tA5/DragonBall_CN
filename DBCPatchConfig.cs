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

        [ReloadRequired]
        [DefaultValue(true)]
        public bool SSJ5FurRecipeFix { get; set; }


        [ReloadRequired]
        [DefaultValue(true)]
        public bool GokuItemsRecipeFix { get; set; }
        #endregion

        #region 夹带私货

        [Header("SpecialContent")]

        public SubConfig AutoTransformation = new SubConfig();

        #endregion

        #region 自定义数据
        [SeparatePage]
        public class SubConfig 
        {
            [DefaultValue(false)]
            public bool enableFullPowerBuff;

            [DefaultValue(false)]
            public bool enableShatteredLimitsBuff;

            [DefaultValue(false)]
            public bool enableUIBuff;

            [DefaultValue(false)]
            public bool enableVVSBuff;


            //下面内容用于调试
            //public override string ToString()
            //{
            //    return $"{enableFullPowerBuff} {enableShatteredLimitsBuff} {enableUIBuff} {enableVVSBuff}";
            //}

            public override bool Equals(object obj)
            {
                if (obj is SubConfig other)
                    return enableFullPowerBuff == other.enableFullPowerBuff &&
                        enableShatteredLimitsBuff == other.enableShatteredLimitsBuff &&
                        enableUIBuff == other.enableUIBuff &&
                        enableVVSBuff == other.enableVVSBuff;
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return new { enableFullPowerBuff, enableShatteredLimitsBuff, enableUIBuff, enableVVSBuff }.GetHashCode();
            }
        }
        #endregion
    }
}
