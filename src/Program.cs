using System;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Management;

namespace TTY_SPOOFER_DAYZ
{
    enum SpoofMode { CPU, Baseboard, Motherboard, HDDSSD, RAM, GPU }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            if (args.Length == 0 || args[0].ToLower() == "menu")
            {
                Menus.ShowMenu();
            }
            else if (args.Length > 0 && Enum.TryParse(args[0], ignoreCase: true, out SpoofMode mode))
            {
                switch (mode)
                {
                    case SpoofMode.CPU:
                        SpoofComponent(@"SYSTEM\CurrentControlSet\Enum\Processor", "SerialNumber", "CPU");
                        break;
                    case SpoofMode.Baseboard:
                    case SpoofMode.Motherboard:
                        SpoofComponent(@"SYSTEM\CurrentControlSet\Enum\PCI", "SerialNumber", "Baseboard/Motherboard");
                        break;
                    case SpoofMode.HDDSSD:
                        SpoofComponent(@"SYSTEM\CurrentControlSet\Enum\DiskDrive", "SerialNumber", "HDD/SSD");
                        break;
                    case SpoofMode.RAM:
                    case SpoofMode.GPU:
                        SpoofComponent(@"SYSTEM\CurrentControlSet\Enum\PCI", "SerialNumber", mode.ToString());
                        break;
                }
            }
        }

        static void SpoofComponent(string registryPath, string valueName, string componentName)
        {
            try
            {
                string newSerial = GenerateRandomString(16);
                Console.WriteLine($"[+] Spoofing {componentName} to: {newSerial}");

                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, true))
                {
                    if (key != null)
                    {
                        key.SetValue(valueName, newSerial, RegistryValueKind.String);
                        Console.WriteLine($"[+] {componentName} Serial Number spoofed successfully!");
                    }
                    else
                    {
                        Console.WriteLine($"[-] Failed to open registry key for {componentName}.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error spoofing {componentName}: {ex.Message}");
            }
        }

        static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
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
            catch { return "UNKNOWN"; }
        }
    }
}