using System;
using System.Runtime.InteropServices;
using System.Management;
using Microsoft.Win32;

namespace TTY_SPOOFER_DAYZ
{
    class Spoof
    {
        // --- P/Invoke для работы с реестром и WMI ---
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool RegSetValueEx(string subKey, string valueName, uint reserved, int dwType, byte[] data, int cbData);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool RegOpenKeyEx(IntPtr hKey, string subKey, uint ulOptions, int samDesired, out IntPtr phkResult);

        // Типы реестра
        private const int REG_SZ = 1;
        private const int REG_BINARY = 0;

        /// <summary>
        /// Выполняет все подмены HWID.
        /// </summary>
        public static void ExecuteAllProtections()
        {
            SpoofCPU();
            SpoofBaseboardSerial();
            SpoofChassisSerial();
            SpoofBIOSVersionAndSerial();
            SpoofMACAddresses();
            SpoofUUID_GUID();
        }

        /// <summary>
        /// Подмена CPU Serial Number.
        /// </summary>
        public static void SpoofCPU()
        {
            try
            {
                // Получаем текущий серийный номер процессора через WMI
                string currentSerial = GetWmiValue("SELECT SerialNumber FROM Win32_Processor");

                if (string.IsNullOrEmpty(currentSerial))
                    currentSerial = "UNKNOWN";

                Console.WriteLine($"\n[*] CPU Serial Number: {currentSerial}");

                // Генерируем случайный серийный номер
                string newSerial = GenerateRandomString(16);

                Console.WriteLine($"[+] Spoofing to: {newSerial}");

                // Записываем в реестр (CPUID HWID)
                string cpuKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\Processor";
                IntPtr hKey;
                if (!RegOpenKeyEx((IntPtr)-1, cpuKey, 0, 26, out hKey)) // KEY_READ | KEY_WOW64_64KEY
                {
                    byte[] data = Encoding.UTF8.GetBytes(newSerial);
                    RegSetValueEx(cpuKey, "SerialNumber", 0, REG_SZ, data, (int)data.Length);
                    Console.WriteLine("[+] CPU Serial Number spoofed successfully!");
                }
                else
                {
                    Console.WriteLine("[-] Failed to open registry key for CPU Serial Number.");
                }

                CloseHandle(hKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error spoofing CPU: {ex.Message}");
            }
        }

        /// <summary>
        /// Подмена Baseboard Serial Number.
        /// </summary>
        public static void SpoofBaseboardSerial()
        {
            try
            {
                string currentSerial = GetWmiValue("SELECT SerialNumber FROM Win32_BaseBoard");

                if (string.IsNullOrEmpty(currentSerial))
                    currentSerial = "UNKNOWN";

                Console.WriteLine($"\n[*] Baseboard Serial Number: {currentSerial}");

                string newSerial = GenerateRandomString(16);

                Console.WriteLine($"[+] Spoofing to: {newSerial}");

                string baseboardKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\PCI";
                IntPtr hKey;
                if (!RegOpenKeyEx((IntPtr)-1, baseboardKey, 0, 26, out hKey))
                {
                    byte[] data = Encoding.UTF8.GetBytes(newSerial);
                    RegSetValueEx(baseboardKey, "SerialNumber", 0, REG_SZ, data, (int)data.Length);
                    Console.WriteLine("[+] Baseboard Serial Number spoofed successfully!");
                }
                else
                {
                    Console.WriteLine("[-] Failed to open registry key for Baseboard Serial Number.");
                }

                CloseHandle(hKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error spoofing Baseboard: {ex.Message}");
            }
        }

        /// <summary>
        /// Подмена Chassis Serial Number.
        /// </summary>
        public static void SpoofChassisSerial()
        {
            try
            {
                string currentSerial = GetWmiValue("SELECT SerialNumber FROM Win32_Chassis");

                if (string.IsNullOrEmpty(currentSerial))
                    currentSerial = "UNKNOWN";

                Console.WriteLine($"\n[*] Chassis Serial Number: {currentSerial}");

                string newSerial = GenerateRandomString(16);

                Console.WriteLine($"[+] Spoofing to: {newSerial}");

                string chassisKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\PCI";
                IntPtr hKey;
                if (!RegOpenKeyEx((IntPtr)-1, chassisKey, 0, 26, out hKey))
                {
                    byte[] data = Encoding.UTF8.GetBytes(newSerial);
                    RegSetValueEx(chassisKey, "SerialNumber", 0, REG_SZ, data, (int)data.Length);
                    Console.WriteLine("[+] Chassis Serial Number spoofed successfully!");
                }
                else
                {
                    Console.WriteLine("[-] Failed to open registry key for Chassis Serial Number.");
                }

                CloseHandle(hKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error spoofing Chassis: {ex.Message}");
            }
        }

        /// <summary>
        /// Подмена BIOS Version и Serial.
        /// </summary>
        public static void SpoofBIOSVersionAndSerial()
        {
            try
            {
                string currentVersion = GetWmiValue("SELECT Version FROM Win32_BIOS");
                string currentSerial = GetWmiValue("SELECT SerialNumber FROM Win32_BIOS");

                if (string.IsNullOrEmpty(currentVersion))
                    currentVersion = "UNKNOWN";
                if (string.IsNullOrEmpty(currentSerial))
                    currentSerial = "UNKNOWN";

                Console.WriteLine($"\n[*] BIOS Version: {currentVersion}");
                Console.WriteLine($"[*] BIOS Serial Number: {currentSerial}");

                string newVersion = GenerateRandomString(16);
                string newSerial = GenerateRandomString(16);

                Console.WriteLine($"[+] Spoofing Version to: {newVersion}");
                Console.WriteLine($"[+] Spoofing Serial to: {newSerial}");

                string biosKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\PCI";
                IntPtr hKey;
                if (!RegOpenKeyEx((IntPtr)-1, biosKey, 0, 26, out hKey))
                {
                    byte[] versionData = Encoding.UTF8.GetBytes(newVersion);
                    RegSetValueEx(biosKey, "BIOSVersion", 0, REG_SZ, versionData, (int)versionData.Length);

                    byte[] serialData = Encoding.UTF8.GetBytes(newSerial);
                    RegSetValueEx(biosKey, "SerialNumber", 0, REG_SZ, serialData, (int)serialData.Length);

                    Console.WriteLine("[+] BIOS Version and Serial spoofed successfully!");
                }
                else
                {
                    Console.WriteLine("[-] Failed to open registry key for BIOS.");
                }

                CloseHandle(hKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error spoofing BIOS: {ex.Message}");
            }
        }

        /// <summary>
        /// Подмена MAC Addresses всех сетевых карт.
        /// </summary>
        public static void SpoofMACAddresses()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True");

                Console.WriteLine("\n[*] MAC Addresses:");

                foreach (ManagementObject obj in searcher.Get())
                {
                    string mac = obj["MACAddress"]?.ToString() ?? "UNKNOWN";
                    string currentIP = obj["IPAddress"]?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(currentIP))
                        Console.WriteLine($"    [{mac}] - IP: {currentIP}");

                    // Генерируем случайный MAC
                    byte[] newMacBytes = GenerateRandomMAC();
                    string newMac = BitConverter.ToString(newMacBytes).Replace("-", ":");

                    Console.WriteLine($"[+] Spoofing to: {newMac}");

                    // Записываем в реестр (NetworkCard HWID)
                    string networkKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\PCI";
                    IntPtr hKey;
                    if (!RegOpenKeyEx((IntPtr)-1, networkKey, 0, 26, out hKey))
                    {
                        byte[] macData = Encoding.UTF8.GetBytes(newMac);
                        RegSetValueEx(networkKey, "MACAddress", 0, REG_SZ, macData, (int)macData.Length);
                        Console.WriteLine("[+] MAC Address spoofed successfully!");
                    }
                    else
                    {
                        Console.WriteLine("[-] Failed to open registry key for MAC Address.");
                    }

                    CloseHandle(hKey);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error spoofing MAC: {ex.Message}");
            }
        }

        /// <summary>
        /// Подмена UUID / MachineGuid.
        /// </summary>
        public static void SpoofUUID_GUID()
        {
            try
            {
                string currentGUID = GetWmiValue("SELECT Value FROM Win32_UUID");

                if (string.IsNullOrEmpty(currentGUID))
                    currentGUID = "UNKNOWN";

                Console.WriteLine($"\n[*] UUID / MachineGuid: {currentGUID}");

                string newGUID = GenerateRandomString(36); // GUID format

                Console.WriteLine($"[+] Spoofing to: {newGUID}");

                // Записываем в реестр (MachineGuid HWID)
                string machineKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\ComputerName\ComputerDescription";
                IntPtr hKey;
                if (!RegOpenKeyEx((IntPtr)-1, machineKey, 0, 26, out hKey))
                {
                    byte[] data = Encoding.UTF8.GetBytes(newGUID);
                    RegSetValueEx(machineKey, "MachineGuid", 0, REG_SZ, data, (int)data.Length);
                    Console.WriteLine("[+] UUID / MachineGuid spoofed successfully!");
                }
                else
                {
                    Console.WriteLine("[-] Failed to open registry key for MachineGuid.");
                }

                CloseHandle(hKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error spoofing UUID: {ex.Message}");
            }
        }

        // --- Вспомогательные методы ---

        private static string GetWmiValue(string query)
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                foreach (ManagementObject obj in searcher.Get())
                {
                    return obj["SerialNumber"]?.ToString() ?? obj["Value"]?.ToString();
                }
            }
            catch { }
            return null;
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
                result[i] = chars[random.Next(chars.Length)];
            return new string(result);
        }

        private static byte[] GenerateRandomMAC()
        {
            var random = new Random();
            byte[] mac = new byte[6];
            for (int i = 0; i < 6; i++)
                mac[i] = (byte)random.Next(256);
            return mac;
        }

        private static void CloseHandle(IntPtr handle)
        {
            if (handle != IntPtr.Zero)
            {
                Marshal.Release(handle);
            }
        }
    }
}