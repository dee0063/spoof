using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TTY_SPOOFER_DAYZ
{
    enum SpoofMode { CPU, Baseboard, Motherboard, HDDSSD, RAM, GPU }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            if (args.Length == 0 || args[0].ToLower() == "menu")
            {
                Menus.ShowMenu();
            }
            else if (args.Length > 0 && Enum.TryParse(args[0], ignoreCase: true, out SpoofMode mode))
            {
                switch (mode)
                {
                    case SpoofMode.CPU:
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
                        break;

                    case SpoofMode.Baseboard:
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
                        break;

                    case SpoofMode.Motherboard:
                        try
                        {
                            string currentSerial = GetWmiValue("SELECT SerialNumber FROM Win32_BaseBoard");
                            if (string.IsNullOrEmpty(currentSerial)) currentSerial = "UNKNOWN";
                            Console.WriteLine($"\n[*] Motherboard Serial Number: {currentSerial}");

                            string newSerial = GenerateRandomString(16);
                            Console.WriteLine($"[+] Spoofing to: {newSerial}");

                            string motherboardKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\ROOTSYS";
                            IntPtr hKey;
                            if (!RegOpenKeyEx((IntPtr)-1, motherboardKey, 0, 26, out hKey))
                            {
                                byte[] data = Encoding.UTF8.GetBytes(newSerial);
                                RegSetValueEx(motherboardKey, "BoardSerialNumber", 0, REG_SZ, data, (int)data.Length);
                                Console.WriteLine("[+] Motherboard Serial Number spoofed successfully!");
                            }
                            else
                            {
                                Console.WriteLine("[-] Failed to open registry key for Motherboard Serial Number.");
                            }

                            CloseHandle(hKey);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[!] Error spoofing Motherboard: {ex.Message}");
                        }
                        break;

                    case SpoofMode.HDDSSD:
                        try
                        {
                            string currentSerial = GetWmiValue("SELECT SerialNumber FROM Win32_DiskDrive");
                            if (string.IsNullOrEmpty(currentSerial)) currentSerial = "UNKNOWN";
                            Console.WriteLine($"\n[*] HDD/SSD Serial Number: {currentSerial}");

                            string newSerial = GenerateRandomString(16);
                            Console.WriteLine($"[+] Spoofing to: {newSerial}");

                            string hddKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\DiskDrive";
                            IntPtr hKey;
                            if (!RegOpenKeyEx((IntPtr)-1, hddKey, 0, 26, out hKey))
                            {
                                byte[] data = Encoding.UTF8.GetBytes(newSerial);
                                RegSetValueEx(hddKey, "SerialNumber", 0, REG_SZ, data, (int)data.Length);
                                Console.WriteLine("[+] HDD/SSD Serial Number spoofed successfully!");
                            }
                            else
                            {
                                Console.WriteLine("[-] Failed to open registry key for HDD/SSD Serial Number.");
                            }

                            CloseHandle(hKey);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[!] Error spoofing HDD/SSD: {ex.Message}");
                        }
                        break;

                    case SpoofMode.RAM:
                        try
                        {
                            string currentSerial = GetWmiValue("SELECT SerialNumber FROM Win32_PhysicalMemory");
                            if (string.IsNullOrEmpty(currentSerial)) currentSerial = "UNKNOWN";
                            Console.WriteLine($"\n[*] RAM Serial Number: {currentSerial}");

                            string newSerial = GenerateRandomString(16);
                            Console.WriteLine($"[+] Spoofing to: {newSerial}");

                            string ramKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\PCI";
                            IntPtr hKey;
                            if (!RegOpenKeyEx((IntPtr)-1, ramKey, 0, 26, out hKey))
                            {
                                byte[] data = Encoding.UTF8.GetBytes(newSerial);
                                RegSetValueEx(ramKey, "SerialNumber", 0, REG_SZ, data, (int)data.Length);
                                Console.WriteLine("[+] RAM Serial Number spoofed successfully!");
                            }
                            else
                            {
                                Console.WriteLine("[-] Failed to open registry key for RAM Serial Number.");
                            }

                            CloseHandle(hKey);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[!] Error spoofing RAM: {ex.Message}");
                        }
                        break;

                    case SpoofMode.GPU:
                        try
                        {
                            string currentSerial = GetWmiValue("SELECT SerialNumber FROM Win32_VideoController");
                            if (string.IsNullOrEmpty(currentSerial)) currentSerial = "UNKNOWN";
                            Console.WriteLine($"\n[*] GPU Serial Number: {currentSerial}");

                            string newSerial = GenerateRandomString(16);
                            Console.WriteLine($"[+] Spoofing to: {newSerial}");

                            string gpuKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\PCI";
                            IntPtr hKey;
                            if (!RegOpenKeyEx((IntPtr)-1, gpuKey, 0, 26, out hKey))
                            {
                                byte[] data = Encoding.UTF8.GetBytes(newSerial);
                                RegSetValueEx(gpuKey, "SerialNumber", 0, REG_SZ, data, (int)data.Length);
                                Console.WriteLine("[+] GPU Serial Number spoofed successfully!");
                            }
                            else
                            {
                                Console.WriteLine("[-] Failed to open registry key for GPU Serial Number.");
                            }

                            CloseHandle(hKey);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[!] Error spoofing GPU: {ex.Message}");
                        }
                        break;
                }
            }
        }

        static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        static string GetWmiValue(string query)
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["SerialNumber"]?.ToString() ?? "UNKNOWN";
                    }
                }
                return "UNKNOWN";
            }
            catch
            {
                return "UNKNOWN";
            }
        }

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool RegSetValueEx(string subKey, string valueName, uint reserved, int dataType, byte[] data, int dataLength);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(int processHandle, bool inheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        const uint REG_SZ = 1;
    }
}
