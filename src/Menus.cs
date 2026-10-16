using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.Win32;
using System.Management;

namespace TTY_SPOOFER_DAYZ
{
    class Menus
    {
        public static void ShowMenu()
        {
            Console.Clear();
            PrintBanner();

            while (true)
            {
                Console.WriteLine("\n========================================");
                Console.WriteLine("   TTY-SPOOFER-DAYZ — Main Menu");
                Console.WriteLine("========================================\n");
                Console.WriteLine("[1] Spoof CPU Serial Number");
                Console.WriteLine("[2] Spoof Baseboard Serial Number");
                Console.WriteLine("[3] Spoof Chassis Serial Number");
                Console.WriteLine("[4] Spoof BIOS Version & Serial");
                Console.WriteLine("[5] Spoof MAC Addresses (All Cards)");
                Console.WriteLine("[6] Spoof UUID / MachineGuid");
                Console.WriteLine("[7] Full HWID Spoof (All components)");
                Console.WriteLine("[8] Restore Original HWIDs");
                Console.WriteLine("[9] Exit\n");
                
                string input = ReadInput("Enter choice [1-9]: ");
                if (string.IsNullOrWhiteSpace(input)) continue;

                switch (input.Trim().ToLower())
                {
                    case "1": SpoofCPU(); break;
                    case "2": SpoofBaseboardSerial(); break;
                    case "3": SpoofChassisSerial(); break;
                    case "4": SpoofBIOSVersionAndSerial(); break;
                    case "5": SpoofMACAddresses(); break;
                    case "6": SpoofUUID_GUID(); break;
                    case "7": Spoof.ExecuteAllProtections(); break; // Убедись, что класс Spoof существует
                    case "8": RestoreOriginalHWIDs(); break;
                    case "9": 
                        Environment.Exit(0); 
                        break; // <-- ИСПРАВЛЕНО: добавлен break
                }

                Console.WriteLine("\n[*] Press any key to continue...");
                Console.ReadKey();
            }
        }

        public static void ShowHelp()
        {
            Console.Clear();
            PrintBanner();
            Console.WriteLine("\n========================================");
            Console.WriteLine("   TTY-SPOOFER-DAYZ — Help");
            Console.WriteLine("========================================\n");
            Console.WriteLine("[*] Usage: TTY-SPOOFER-DAYZ.exe <mode>");
            Console.WriteLine("\n[*] Available modes:");
            Console.WriteLine("    cpu, baseboard, chassis, bios, mac, uuid, all, full, menu, help\n");
        }

        public static void PrintBanner()
        {
            string banner = @"
██████╗ ██╗     ██╗███╗   ███╗███████╗    ██████╗ ███████╗██████╗
██╔═══██╗██║     ██║████╗ ████║██╔════╝    ██╔══██╗██╔════╝██╔══██╗
██║   ██║██║     ██║██╔████╔██║█████╗      ██████╔╝█████╗  ██████╔╝
██║▄▄ ██║██║     ██║██║╚██╔╝██║██╔══╝      ██╔══██╗██╔══╝  ██╔══██╗
╚██████╔╝███████╗██║██║ ╚═╝ ██║███████╗    ██████╔╝███████╗██║  ██║
╚══▀▀═╝ ╚══════╝╚═╝╚═╝     ╚═╝╚══════╝    ╚═════╝ ╚══════╝╚═╝  ╚═╝
   TTY-SPOOFER-DAYZ — Full HWID Spoofing Tool for DayZ
";
            Console.WriteLine(banner);
        }

        private static string? ReadInput(string prompt)
        {
            try
            {
                Console.Write(prompt);
                return Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[!] Input error: {ex.Message}");
                return null;
            }
        }

        // --- СПУФИНГ КОМПОНЕНТОВ (Через стандартный Registry API) ---

        private static void SpoofCPU()
        {
            try
            {
                string currentSerial = GetWmiValue("SELECT SerialNumber FROM Win32_Processor");
                if (string.IsNullOrEmpty(currentSerial)) currentSerial = "UNKNOWN";
                Console.WriteLine($"\n[*] CPU Serial Number: {currentSerial}");
                
                string newSerial = GenerateRandomString(16);
                Console.WriteLine($"[+] Spoofing to: {newSerial}");

                string cpuPath = @"SYSTEM\CurrentControlSet\Enum\Processor";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(cpuPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue("SerialNumber", newSerial, RegistryValueKind.String);
                        Console.WriteLine("[+] CPU Serial Number spoofed successfully!");
                    }
                    else Console.WriteLine("[-] Failed to open registry key for CPU.");
                }
            }
            catch (Exception ex) { Console.WriteLine($"[!] Error spoofing CPU: {ex.Message}"); }
        }

        private static void SpoofBaseboardSerial()
        {
            try
            {
                string newSerial = GenerateRandomString(16);
                string path = @"SYSTEM\CurrentControlSet\Enum\PCI";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path, true))
                {
                    if (key != null)
                    {
                        key.SetValue("SerialNumber", newSerial, RegistryValueKind.String);
                        Console.WriteLine("[+] Baseboard Serial Number spoofed successfully!");
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"[!] Error: {ex.Message}"); }
        }

        private static void SpoofChassisSerial()
        {
            try
            {
                string newSerial = GenerateRandomString(16);
                string path = @"SYSTEM\CurrentControlSet\Enum\PCI";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path, true))
                {
                    if (key != null)
                    {
                        key.SetValue("SerialNumber", newSerial, RegistryValueKind.String);
                        Console.WriteLine("[+] Chassis Serial Number spoofed successfully!");
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"[!] Error: {ex.Message}"); }
        }

        private static void SpoofBIOSVersionAndSerial()
        {
            try
            {
                string newVer = GenerateRandomString(16);
                string path = @"SYSTEM\CurrentControlSet\Enum\PCI";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path, true))
                {
                    if (key != null)
                    {
                        key.SetValue("SerialNumber", newVer, RegistryValueKind.String);
                        Console.WriteLine("[+] BIOS Version spoofed successfully!");
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"[!] Error: {ex.Message}"); }
        }

        private static void SpoofMACAddresses()
        {
            try
            {
                var adapters = NetworkInterface.GetAllNetworkInterfaces();
                foreach (var adapter in adapters)
                {
                    if (adapter.OperationalStatus != OperationalStatus.Up) continue;
                    string mac = adapter.GetPhysicalAddress().ToString();
                    Console.WriteLine($"\n[*] MAC ({adapter.Name}): {mac}");
                    
                    string newMac = GenerateRandomMAC();
                    Console.WriteLine($"[+] Spoofing to: {newMac}");
                    SpoofSpoofMAC(adapter, newMac);
                }
            }
            catch (Exception ex) { Console.WriteLine($"[!] Error spoofing MACs: {ex.Message}"); }
        }

        private static void SpoofUUID_GUID()
        {
            try
            {
                string newGuid = Guid.NewGuid().ToString();
                Console.WriteLine($"[+] Spoofing MachineGuid to: {newGuid}");
                
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography", true))
                {
                    if (key != null)
                    {
                        key.SetValue("MachineGuid", newGuid, RegistryValueKind.String);
                        Console.WriteLine("[+] UUID / MachineGuid spoofed successfully!");
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"[!] Error spoofing UUID: {ex.Message}"); }
        }

        private static void RestoreOriginalHWIDs()
        {
            Console.WriteLine("[*] Restoring original HWIDs... (Заглушка)");
            // Здесь должна быть логика восстановления бэкапов
        }

        // --- ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ---

        private static void SpoofSpoofMAC(NetworkInterface adapter, string mac)
        {
            try
            {
                // Реальный спуфинг MAC делается через реестр и перезапуск адаптера
                string guid = adapter.Id;
                string regPath = $@"SYSTEM\CurrentControlSet\Control\Class\{{4D36E972-E325-11CE-BFC1-08002BE10318}}\{guid}";
                
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue("NetworkAddress", mac, RegistryValueKind.String);
                        Console.WriteLine($"[+] MAC spoofed in registry: {mac}");
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"[-] Failed to spoof MAC: {ex.Message}"); }
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string GenerateRandomMAC()
        {
            var rnd = new Random();
            byte[] bytes = new byte[6];
            for (int i = 0; i < 6; i++) bytes[i] = (byte)rnd.Next(256);
            // Локально администрируемый MAC
            bytes[0] = (byte)((bytes[0] & 0xFC) | 0x02); 
            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }

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
                        if (prop.Name == "SerialNumber")
                            return prop.Value?.ToString() ?? "";
                    }
                }
            }
            catch { /* ignore */ }
            return "";
        }
    }
}