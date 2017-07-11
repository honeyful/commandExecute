using System;
using System.Diagnostics;
using System.Security.Principal;

namespace CommandExecute
{
    static class Program
    {
        static void Main()
        {

            string c = string.Empty;

            while (true)
            {
                try
                {
                    if (isAdministrator() == false)
                    {
                        Console.Title = Environment.UserName;
                        Console.Write("main$");
                        c = Console.ReadLine(); 
                    }

                    else
                    {
                        Console.Title = Environment.UserName + "@Administrator";
                        Console.Write("main#");
                        c = Console.ReadLine();
                    }

                    if (c == "su")
                    {
                        superUser(Process.GetCurrentProcess().MainModule.FileName);
                    }

                    else if (c == "exit")
                    {
                        Environment.Exit(0);
                    }

                    exec(c);

                }

                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        static void exec(string c)
        {

            try
            {

                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = "/c" + c,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                };

                Process p = Process.Start(psi);

                p.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                    Console.WriteLine(e.Data);
                p.BeginOutputReadLine();

                p.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                    Console.WriteLine(e.Data);
                p.BeginErrorReadLine();

                p.WaitForExit();

                Console.WriteLine("ExitCode:" + p.ExitCode);
                Console.WriteLine("ExitTime:" + p.ExitTime);
                Console.WriteLine();

                p.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("err: " + ex.Data);
            }
        }

        static bool isAdministrator()
        {

            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal pr = new WindowsPrincipal(id);
            return pr.IsInRole(WindowsBuiltInRole.Administrator);

        }

        static void superUser(string p)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = p,
                Verb = "runas",
            };

            Process.Start(psi);

            Environment.Exit(0);
        }
    }
}