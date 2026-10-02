using DBZGoatLib.Handlers;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace DragonBall_CN.Common.AdditionalContent
{
    //可叠加形态自动变身
    public class AutoTransformation : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            //AwakeningPower - 全功率&突破极限
            var fullPower = TransformationHandler.GetTransformation("FullPowerBuff");
            var shatteredLimits = TransformationHandler.GetTransformation("ShatteredLimitsBuff");
            //dbzcalamityUTD - 自在极意兆
            var uI = TransformationHandler.GetTransformation("UIBuff");
            //dbztac - 紫色面纱
            var vVS = TransformationHandler.GetTransformation("VVSBuff");
            //XV2Forms - 究极
            var ultimate = TransformationHandler.GetTransformation("StackableUltimateBuff");

            if (TransformationHandler.TransformKey.JustPressed)
            {
                if (fullPower is not null && DBCPatchConfig.Instance.AutoTransformation.enableFullPowerBuff)
                    TransformationHandler.Transform(Player, fullPower.Value);

                if (shatteredLimits is not null && DBCPatchConfig.Instance.AutoTransformation.enableShatteredLimitsBuff)
                    TransformationHandler.Transform(Player, shatteredLimits.Value);

                if (uI is not null && DBCPatchConfig.Instance.AutoTransformation.enableUIBuff)
                    TransformationHandler.Transform(Player, uI.Value);

                if (vVS is not null && DBCPatchConfig.Instance.AutoTransformation.enableVVSBuff)
                    TransformationHandler.Transform(Player, vVS.Value);
                //其实还需要判断是否为天才资质的，但我懒得加了就是这么超模
                if (ultimate is not null && DBCPatchConfig.Instance.AutoTransformation.enableStackableUltimate)
                    TransformationHandler.Transform(Player, ultimate.Value);
            }
        }
    }
}
