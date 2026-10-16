using System;

namespace TTY_SPOOFER_DAYZ
{
    class Settings
    {
        // --- Ключи реестра (HWID) ---
        public static readonly string CPU_KEY = @"SYSTEM\CurrentControlSet\Enum\Processor";
        public static readonly string BASEBOARD_KEY = @"SYSTEM\CurrentControlSet\Enum\PCI";
        public static readonly string CHASSIS_KEY = @"SYSTEM\CurrentControlSet\Enum\ROOTSYS";
        public static readonly string BIOS_KEY = @"SYSTEM\CurrentControlSet\Enum\BIOS";
        public static readonly string NETWORK_KEY = @"SYSTEM\CurrentControlSet\Services\VBoxNetAdp";

        // --- Путь к драйверу (если используется) ---
        public static readonly string DRIVER_PATH = @"C:\Windows\System32\drivers\tty-spoof.sys";
    }
}