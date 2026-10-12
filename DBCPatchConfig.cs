using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace DragonBall_CN
{
    public class DBCPatchConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static DBCPatchConfig Instance;

        #region bug修复
        [Header("ExtraPatches")]

        [ReloadRequired]
        [DefaultValue(true)]
        public bool DBZMODPORTMapEntryFix { get; set; }

        [ReloadRequired]
        [DefaultValue(false)]
        public bool HideDBTBalanceTransformationTree { get; set; }

        [ReloadRequired]
        [DefaultValue(true)]
        public bool SSJ5FurRecipeFix { get; set; }


        [ReloadRequired]
        [DefaultValue(true)]
        public bool GokuItemsRecipeFix { get; set; }

        [ReloadRequired]
        [DefaultValue(false)]
        public bool FixConflictBuffKey { get; set; }

        [DefaultValue(false)]
        public bool DebugMode { get; set; }
        #endregion

        #region 夹带私货

        [Header("SpecialContent")]

        [ReloadRequired]
        [DefaultValue(false)]
        public bool EnablePTOForm { get; set; }
        //[Header("SpecialContent")]
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

            [DefaultValue(false)]
            public bool enableStackableUltimate;

            [DefaultValue(false)]
            public bool enablePTOBuff;

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
                        enableVVSBuff == other.enableVVSBuff &&
                        enableStackableUltimate == other.enableStackableUltimate &&
                        enablePTOBuff == other.enablePTOBuff;
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return new { enableFullPowerBuff, enableShatteredLimitsBuff, enableUIBuff, enableVVSBuff , enableStackableUltimate , enablePTOBuff}.GetHashCode();
            }
        }
        #endregion
    }
}
