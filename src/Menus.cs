using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

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
                    case "7": Spoof.ExecuteAllProtections(); break;
                    case "8": RestoreOriginalHWIDs(); break;
                    case "9": Environment.Exit(0);
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
            Console.WriteLine("    cpu       - Spoof CPU Serial Number");
            Console.WriteLine("    baseboard - Spoof Baseboard Serial Number");
            Console.WriteLine("    chassis   - Spoof Chassis Serial Number");
            Console.WriteLine("    bios      - Spoof BIOS Version & Serial");
            Console.WriteLine("    mac       - Spoof MAC Addresses (All Cards)");
            Console.WriteLine("    uuid      - Spoof UUID / MachineGuid");
            Console.WriteLine("    all       - Full HWID Spoof (All components)");
            Console.WriteLine("    full      - Full HWID Spoof (All components)");
            Console.WriteLine("    menu      - Interactive Menu Mode");
            Console.WriteLine("    help      - Show this help message\n");

            Console.WriteLine("[*] Run without arguments to enter interactive menu mode.\n");
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

        private static string ReadInput(string prompt)
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

        private static void SpoofCPU()
        {
            try
            {
                string currentSerial = GetWmiValue("SELECT SerialNumber FROM Win32_Processor");
                if (string.IsNullOrEmpty(currentSerial)) currentSerial = "UNKNOWN";
                Console.WriteLine($"\n[*] CPU Serial Number: {currentSerial}");

                string newSerial = GenerateRandomString(16);
                Console.WriteLine($"[+] Spoofing to: {newSerial}");

                string cpuKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\Processor";
                IntPtr hKey;
                if (!RegOpenKeyEx((IntPtr)-1, cpuKey, 0, 26, out hKey))
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

        private static void SpoofBaseboardSerial()
        {
            try
            {
                string currentSerial = GetWmiValue("SELECT SerialNumber FROM Win32_BaseBoard");
                if (string.IsNullOrEmpty(currentSerial)) currentSerial = "UNKNOWN";
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

        private static void SpoofChassisSerial()
        {
            try
            {
                string currentSerial = GetWmiValue("SELECT SerialNumber FROM Win32_Chassis");
                if (string.IsNullOrEmpty(currentSerial)) currentSerial = "UNKNOWN";
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

        private static void SpoofBIOSVersionAndSerial()
        {
            try
            {
                string currentVer = GetWmiValue("SELECT Version FROM Win32_BIOS");
                if (string.IsNullOrEmpty(currentVer)) currentVer = "UNKNOWN";
                Console.WriteLine($"\n[*] BIOS Version: {currentVer}");

                string newVer = GenerateRandomString(16);
                Console.WriteLine($"[+] Spoofing to: {newVer}");

                string biosKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\PCI";
                IntPtr hKey;
                if (!RegOpenKeyEx((IntPtr)-1, biosKey, 0, 26, out hKey))
                {
                    byte[] data = Encoding.UTF8.GetBytes(newVer);
                    RegSetValueEx(biosKey, "SerialNumber", 0, REG_SZ, data, (int)data.Length);
                    Console.WriteLine("[+] BIOS Version spoofed successfully!");
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
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error spoofing MACs: {ex.Message}");
            }
        }

        private static void SpoofUUID_GUID()
        {
            try
            {
                string currentGuid = Guid.NewGuid().ToString();
                Console.WriteLine($"\n[*] Current GUID: {currentGuid}");

                string newGuid = GenerateRandomString(36);
                Console.WriteLine($"[+] Spoofing to: {newGuid}");

                RegistryKey key = Registry.CurrentUser.OpenSubkey(@"SOFTWARE\Microsoft\Cryptography", true);
                if (key != null)
                {
                    key.SetValue("MachineGuid", newGuid, RegistryValueKind.String);
                    Console.WriteLine("[+] UUID / MachineGuid spoofed successfully!");
                    key.Close();
                }
                else
                {
                    Console.WriteLine("[-] Failed to open registry key for GUID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error spoofing UUID: {ex.Message}");
            }
        }

        private static void RestoreOriginalHWIDs()
        {
            try
            {
                SpoofSpoofCPU();
                SpoofSpoofBaseboard();
                SpoofSpoofChassis();
                SpoofSpoofBIOS();
                SpoofSpoofMACs();
                SpoofSpoofUUID();
                Console.WriteLine("\n[*] All original HWIDs restored!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error restoring HWIDs: {ex.Message}");
            }
        }

        private static void SpoofSpoofCPU()
        {
            try
            {
                string cpuKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\Processor";
                IntPtr hKey;
                if (!RegOpenKeyEx((IntPtr)-1, cpuKey, 0, 26, out hKey))
                {
                    byte[] data = Encoding.UTF8.GetBytes("ORIGINAL");
                    RegSetValueEx(cpuKey, "SerialNumber", 0, REG_SZ, data, (int)data.Length);
                    Console.WriteLine("[+] CPU Serial restored!");
                }
                CloseHandle(hKey);
            }
            catch { /* ignore */ }
        }

        private static void SpoofSpoofBaseboard()
        {
            try
            {
                string baseboardKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\PCI";
                IntPtr hKey;
                if (!RegOpenKeyEx((IntPtr)-1, baseboardKey, 0, 26, out hKey))
                {
                    byte[] data = Encoding.UTF8.GetBytes("ORIGINAL");
                    RegSetValueEx(baseboardKey, "SerialNumber", 0, REG_SZ, data, (int)data.Length);
                    Console.WriteLine("[+] Baseboard Serial restored!");
                }
                CloseHandle(hKey);
            }
            catch { /* ignore */ }
        }

        private static void SpoofSpoofChassis()
        {
            try
            {
                string chassisKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\PCI";
                IntPtr hKey;
                if (!RegOpenKeyEx((IntPtr)-1, chassisKey, 0, 26, out hKey))
                {
                    byte[] data = Encoding.UTF8.GetBytes("ORIGINAL");
                    RegSetValueEx(chassisKey, "SerialNumber", 0, REG_SZ, data, (int)data.Length);
                    Console.WriteLine("[+] Chassis Serial restored!");
                }
                CloseHandle(hKey);
            }
            catch { /* ignore */ }
        }

        private static void SpoofSpoofBIOS()
        {
            try
            {
                string biosKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\PCI";
                IntPtr hKey;
                if (!RegOpenKeyEx((IntPtr)-1, biosKey, 0, 26, out hKey))
                {
                    byte[] data = Encoding.UTF8.GetBytes("ORIGINAL");
                    RegSetValueEx(biosKey, "SerialNumber", 0, REG_SZ, data, (int)data.Length);
                    Console.WriteLine("[+] BIOS Serial restored!");
                }
                CloseHandle(hKey);
            }
            catch { /* ignore */ }
        }

        private static void SpoofSpoofMACs()
        {
            try
            {
                var adapters = NetworkInterface.GetAllNetworkInterfaces();
                foreach (var adapter in adapters)
                {
                    if (adapter.OperationalStatus != OperationalStatus.Up) continue;
                    SpoofSpoofMAC(adapter, "ORIGINAL");
                }
            }
            catch { /* ignore */ }
        }

        private static void SpoofSpoofUUID()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubkey(@"SOFTWARE\Microsoft\Cryptography", true);
                if (key != null)
                {
                    key.SetValue("MachineGuid", Guid.NewGuid().ToString(), RegistryValueKind.String);
                    Console.WriteLine("[+] UUID restored!");
                    key.Close();
                }
            }
            catch { /* ignore */ }
        }

        private static void SpoofSpoofMAC(NetworkInterface adapter, string mac)
        {
            try
            {
                var props = adapter.GetPhysicalProperties();
                byte[] bytes = new byte[6];
                for (int i = 0; i < 6; i++) bytes[i] = Convert.ToByte(mac.Substring(i * 2, 2), 16);

                using (var ipProps = adapter.IPProperties)
                {
                    if (ipProps.UnicastAddresses.Length > 0)
                        ipProps.UnicastAddresses[0].PhysicalAddress = new System.Net.NetworkInformation.PhysicalAddress(bytes);
                    else
                        ipProps.AddUnicastIPAddress(new System.Net.NetworkInformation.IpAddressInfo
                        {
                            IPAddress = adapter.GetIPProperties().GetIPv4Properties()?.First()?.Address ??
                                       adapter.GetIPProperties().GetIPv6Properties()?.First()?.Address,
                            PhysicalAddress = new System.Net.NetworkInformation.PhysicalAddress(bytes)
                        });

                    Console.WriteLine($"[+] MAC spoofed: {mac}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[-] Failed to spoof MAC on {adapter.Name}: {ex.Message}");
            }
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        private static string GenerateRandomMAC()
        {
            var rnd = new Random();
            byte[] bytes = new byte[6];
            for (int i = 0; i < 6; i++) bytes[i] = (byte)rnd.Next(256);
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

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int RegOpenKeyEx(IntPtr hKey, string subkey, uint ulOptions, int samDesired, out IntPtr phkResult);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int RegSetValueEx(IntPtr hKey, string name, uint reserved, int type, byte[] data, int size);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        private const int REG_SZ = 1;
    }
}
