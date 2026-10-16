using System;
using System.Diagnostics;
using System.Linq;

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
                    Console.WriteLine