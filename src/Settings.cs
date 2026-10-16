using System;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace TTY_SPOOFER_DAYZ
{
    class Settings
    {
        // --- Ключи реестра (HWID) ---
        public static readonly string CPU_KEY = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\Processor";
        public static readonly string BASEBOARD_KEY = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\PCI";
        public static readonly string CHASSIS_KEY = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\ROOTSYS";
        public static readonly string BIOS_KEY = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\BIOS";
        public static readonly string NETWORK_KEY = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VBoxNetAdp";

        // --- Путь к драйверу (если используется) ---
        public static readonly string DRIVER_PATH = @"C:\Windows\System32\drivers\tty-spoof.sys";

        // --- Константы для работы с реестром ---
        const uint REG_SZ = 1;
        const int KEY_READ = 0x20019;

        // --- Вспомогательные методы ---
        public static bool RegOpenKeyEx(string subKey, out IntPtr hKey)
        {
            hKey = IntPtr.Zero;
            return !RegOpenKeyEx((IntPtr)-1, subKey, 0, KEY_READ, out hKey);
        }

        public static bool RegSetValueEx(string subKey, string valueName, uint reserved, int dataType, byte[] data, int dataLength)
        {
            return RegSetValueEx(subKey, valueName, reserved, dataType, data, dataLength);
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool RegOpenKeyEx(IntPtr hKey, string subKey, uint ulOptions, int samDesired, out IntPtr phkResult);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool RegSetValueEx(string subKey, string valueName, uint reserved, int dataType, byte[] data, int dataLength);

        public static void CloseHandle(IntPtr hObject)
        {
            if (hObject != IntPtr.Zero)
                Marshal.Release(hObject);
        }
    }
}
