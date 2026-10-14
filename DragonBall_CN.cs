using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DragonBall_CN.Common.AddonContent;

namespace DragonBall_CN
{
    [JITWhenModsEnabled("DBZGoatLib")]
    [ExtendsFromMod("DBZGoatLib")]
    public class DragonBall_CN : Mod
    {
        public enum PTOPacketType : byte
        {
            //解锁进度
            UnlockSync = 1,
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            PTOPacketType msgType = (PTOPacketType)reader.ReadByte();

            switch (msgType)
            {
                case PTOPacketType.UnlockSync:
                    {
                        byte playerNumber = reader.ReadByte();
                        AddonPlayer modPlayer = Main.player[playerNumber].GetModPlayer<AddonPlayer>();

                        modPlayer.ReceivePlayerSync(reader);

                        if (Main.netMode == NetmodeID.Server)
                        {
                            // 服务端收到客户端的包 → 转发给其余客户端，保持全网一致
                            modPlayer.SyncPlayer(-1, whoAmI, false);
                        }
                        break;
                    }

                default:
                    Logger.Warn("Unknown packet type: " + msgType);
                    break;
            }
        }
    }
}
