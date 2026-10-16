using System;
using System.Diagnostics;
using System.Linq;
using TTY_SPOOFER_DAYZ;

namespace TTY_SPOOFER_DAYZ
{
    class Program
    {
        static void Main(string[] args)
        {
            // Проверка прав администратора
            if (!IsAdmin())
            {
                Console.WriteLine("\n[!] HWID spoofing requires administrator privileges!");
                Console.WriteLine("[*] Please run this program as Administrator.\n");
                return;
            }

            // Парсинг аргументов командной строки
            string mode = args.Length > 0 ? args[0].ToLower() : "menu";

            if (mode == "help")
            {
                PrintHelp();
                return;
            }

            switch (mode)
            {
                case "cpu":
                    Spoof.SpoofCPU();
                    break;
                case "baseboard":
                    Spoof.SpoofBaseboardSerial();
                    break;
                case "chassis":
                    Spoof.SpoofChassisSerial();
                    break;
                case "bios":
                    Spoof.SpoofBIOSVersionAndSerial();
                    break;
                case "mac":
                    Spoof.SpoofMACAddresses();
                    break;
                case "uuid":
                    Spoof.SpoofUUID_GUID();
                    break;
                case "all":
                case "full":
                    Spoof.ExecuteAllProtections();
                    break;
                default:
                    Console.WriteLine($"\n[!] Unknown mode '{mode}'. Use 'help' for available modes.\n");
                    PrintHelp();
                    return;
            }

            Console.WriteLine("\n[*] HWID spoofing complete. Press any key to exit...");
            Console.ReadKey();
        }

        static bool IsAdmin()
        {
            var startInfo = new ProcessStartInfo("powershell", "-Command \"[Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent().IsInRole([Security.Principal.WindowsBuiltInRole] 'Administrator')\"")
            {
                UseShellExecute = true,
                RedirectStandardOutput = true
            };

            using var process = Process.Start(startInfo);
            if (process == null) return false;

            string output = process.StandardOutput.ReadToEnd();
            return output.Contains("True");
        }

        static void PrintHelp()
        {
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
            Console.WriteLine("    help      - Show this help message");
            Console.WriteLine("\n[*] Run without arguments to enter interactive menu mode.\n");
        }
    }
}