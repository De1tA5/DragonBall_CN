using DragonBall_CN.Common.DBZGoatLib;
using Terraria.ModLoader;

namespace DragonBall_CN.Common.AddonContent
{
    [JITWhenModsEnabled("DBZGoatLib")]
    [ExtendsFromMod("DBZGoatLib")]
    public class PTOBuffSystem : ModSystem
    {
        public override void Load()
        {
            if (DBCPatchConfig.Instance.EnablePTOForm)
                return;

            //手动删除变身和界面
            ModelHelper.TryRemoveTransformationBuff(Mod, "PTOBuff");

            ModelHelper.TryRemovePanel("Addon Panel");

        }
    }
}
