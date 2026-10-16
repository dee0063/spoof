using System;
using System.Diagnostics;
using System.Linq;

namespace TTY_SPOOFER_DAYZ
{
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
                                RegSetValueEx(baseboardKey, "