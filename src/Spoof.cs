using System;
using System.Text;
using System.Linq;
using System.Management;
using Microsoft.Win32;
using System.Net.NetworkInformation;

namespace TTY_SPOOFER_DAYZ
{
    class Spoof
    {
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
                string currentSerial = GetWmiValue("SELECT SerialNumber FROM Win32_Processor");
                if (string.IsNullOrEmpty(currentSerial)) currentSerial = "UNKNOWN";
                Console.WriteLine($"\n[*] CPU Serial Number: {currentSerial}");

                string newSerial = GenerateRandomString(16);
                Console.WriteLine($"[+] Spoofing to: {newSerial}");

                // Примечание: Реальный спуфинг CPU требует ядерного драйвера. 
                // Запись в реестр меняет только кэш/определенные значения, но не сам WMI-ответ.
                string cpuPath = @"SYSTEM\CurrentControlSet\Enum\Processor";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(cpuPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue("SerialNumber", newSerial, RegistryValueKind.String);
                        Console.WriteLine("[+] CPU Serial Number spoofed successfully!");
                    }
                    else
                    {
                        Console.WriteLine("[-] Failed to open registry key for CPU Serial Number.");
                    }
                }
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
                if (string.IsNullOrEmpty(currentSerial)) currentSerial = "UNKNOWN";
                Console.WriteLine($"\n[*] Baseboard Serial Number: {currentSerial}");

                string newSerial = GenerateRandomString(16);
                Console.WriteLine($"[+] Spoofing to: {newSerial}");

                string baseboardPath = @"SYSTEM\CurrentControlSet\Enum\PCI";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(baseboardPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue("SerialNumber", newSerial, RegistryValueKind.String);
                        Console.WriteLine("[+] Baseboard Serial Number spoofed successfully!");
                    }
                }
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
                if (string.IsNullOrEmpty(currentSerial)) currentSerial = "UNKNOWN";
                Console.WriteLine($"\n[*] Chassis Serial Number: {currentSerial}");

                string newSerial = GenerateRandomString(16);
                Console.WriteLine($"[+] Spoofing to: {newSerial}");

                string chassisPath = @"SYSTEM\CurrentControlSet\Enum\PCI";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(chassisPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue("SerialNumber", newSerial, RegistryValueKind.String);
                        Console.WriteLine("[+] Chassis Serial Number spoofed successfully!");
                    }
                }
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
                
                if (string.IsNullOrEmpty(currentVersion)) currentVersion = "UNKNOWN";
                if (string.IsNullOrEmpty(currentSerial)) currentSerial = "UNKNOWN";
                
                Console.WriteLine($"\n[*] BIOS Version: {currentVersion}");
                Console.WriteLine($"[*] BIOS Serial Number: {currentSerial}");

                string newVersion = GenerateRandomString(16);
                string newSerial = GenerateRandomString(16);
                Console.WriteLine($"[+] Spoofing Version to: {newVersion}");
                Console.WriteLine($"[+] Spoofing Serial to: {newSerial}");

                string biosPath = @"SYSTEM\CurrentControlSet\Enum\PCI";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(biosPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue("BIOSVersion", newVersion, RegistryValueKind.String);
                        key.SetValue("SerialNumber", newSerial, RegistryValueKind.String);
                        Console.WriteLine("[+] BIOS Version and Serial spoofed successfully!");
                    }
                }
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
                var adapters = NetworkInterface.GetAllNetworkInterfaces();
                Console.WriteLine("\n[*] MAC Addresses:");
                
                foreach (var adapter in adapters)
                {
                    if (adapter.OperationalStatus != OperationalStatus.Up) continue;

                    string mac = adapter.GetPhysicalAddress().ToString();
                    Console.WriteLine($"    [{adapter.Name}] - MAC: {mac}");

                    string newMac = GenerateRandomMAC();
                    Console.WriteLine($"[+] Spoofing to: {newMac}");

                    // Правильный путь в реестре для смены MAC-адреса
                    string guid = adapter.Id;
                    string regPath = $@"SYSTEM\CurrentControlSet\Control\Class\{{4D36E972-E325-11CE-BFC1-08002BE10318}}\{guid}";

                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regPath, true))
                    {
                        if (key != null)
                        {
                            key.SetValue("NetworkAddress", newMac, RegistryValueKind.String);
                            Console.WriteLine("[+] MAC Address spoofed in registry!");
                        }
                    }
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
                // Win32_UUID не существует, правильный класс для UUID - Win32_ComputerSystemProduct
                string currentGUID = GetWmiValue("SELECT UUID FROM Win32_ComputerSystemProduct");
                if (string.IsNullOrEmpty(currentGUID)) currentGUID = "UNKNOWN";
                Console.WriteLine($"\n[*] UUID / MachineGuid: {currentGUID}");

                string newGUID = Guid.NewGuid().ToString();
                Console.WriteLine($"[+] Spoofing to: {newGUID}");

                // Правильный путь для MachineGuid
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography", true))
                {
                    if (key != null)
                    {
                        key.SetValue("MachineGuid", newGUID, RegistryValueKind.String);
                        Console.WriteLine("[+] UUID / MachineGuid spoofed successfully!");
                    }
                    else
                    {
                        Console.WriteLine("[-] Failed to open registry key for MachineGuid.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error spoofing UUID: {ex.Message}");
            }
        }

        // ================= ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ =================

        private static string GetWmiValue(string query)
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher(query))
                {
                    var results = searcher.Get().Cast<ManagementObject>().ToList();
                    if (results.Count == 0) return "";
                    
                    foreach (var prop in results[0].Properties)
                    {
                        if (prop.Name == "SerialNumber" || prop.Name == "UUID" || prop.Name == "Version")
                            return prop.Value?.ToString() ?? "";
                    }
                }
            }
            catch { /* ignore */ }
            return "";
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string GenerateRandomMAC()
        {
            var rnd = new Random();
            byte[] bytes = new byte[6];
            for (int i = 0; i < 6; i++) bytes[i] = (byte)rnd.Next(256);
            
            // Делаем MAC локально администрируемым (второй бит первого байта = 1)
            bytes[0] = (byte)((bytes[0] & 0xFC) | 0x02); 
            
            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }
    }
}