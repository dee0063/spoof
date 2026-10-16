using System;
using Microsoft.Win32;

namespace TTY_SPOOFER_DAYZ
{
    class Settings
    {
        // --- Ключи реестра (HWID) ---
        public static readonly string CPU_KEY = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\Processor";
        public static readonly string BASEBOARD_KEY = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\PCI";
        public static readonly string CHASSIS_KEY = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\PCI";
        public static readonly string BIOS_KEY = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\PCI";
        public static readonly string NETWORK_KEY = @"H