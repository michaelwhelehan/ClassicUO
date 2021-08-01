using ClassicUO.Configuration;
using ClassicUO.Network;

namespace ClassicUO
{
    public static class ZuluLoader
    {
        public static void Load()
        {
            // Settings.GlobalSettings.ClientVersion += " zuluhotel";
            ZuluPackets.SetupPackets();
        }
    }
}