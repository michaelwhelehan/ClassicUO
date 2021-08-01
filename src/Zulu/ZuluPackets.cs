using System;
using System.Reflection;
using ClassicUO.Game;
using ClassicUO.IO;

namespace ClassicUO.Network
{
    internal static class ZuluPackets
    {
        public static void SetupPackets()
        {
            // Set our custom packet length so we avoid merge conflicts with upstream
            FieldInfo field = typeof(PacketsTable).GetField("_packetsTable", BindingFlags.NonPublic | BindingFlags.Static);

            if (field?.GetValue(null) is Array packetTable)
            {
                packetTable.SetValue((short)36, 0xF9);
                PacketHandlers.Handlers.Add(0xF9, ReceiveZuluPacket);
            }
        }

        private static void ReceiveZuluPacket(ref StackDataReader p)
        {
            byte type = p.ReadUInt8();

            switch (type)
            {
                case 0x1: HandleStatusUpdate(ref p);
                    break;
            }
        }

        private static void HandleStatusUpdate(ref StackDataReader p)
        {
            if (World.Player == null)
                return;

            var serial = p.ReadUInt32BE(); // In-case we decide to send this packet to describe other mobiles

            World.Player.Hunger = (short)p.ReadUInt16BE();
            World.Player.HealBonus = (short) p.ReadUInt16BE();
            World.Player.MagicImmunity = (short) p.ReadUInt16BE();
            World.Player.MagicReflect = (short) p.ReadUInt16BE();
            World.Player.PhysicalProtection = (short) p.ReadUInt16BE();
            World.Player.PoisonProtection = (short) p.ReadUInt16BE();
            World.Player.FireProtection = (short) p.ReadUInt16BE();
            World.Player.WaterProtection = (short) p.ReadUInt16BE();
            World.Player.AirProtection = (short) p.ReadUInt16BE();
            World.Player.EarthProtection = (short) p.ReadUInt16BE();
            World.Player.NecroProtection = (short) p.ReadUInt16BE();
            World.Player.CriminalTimer = (short) p.ReadUInt16BE();
            World.Player.MurderTimer = (short) p.ReadUInt16BE();
            World.Player.DamageMin = (short) p.ReadUInt16BE();
            World.Player.DamageMax = (short) p.ReadUInt16BE();
        }
    }
} 