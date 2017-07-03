using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serialTester
{
    class Program
    {
        static bool active = false;
        static bool closing = false;
        static void Main(string[] args)
        {
            var ports = SerialPort.GetPortNames();
            SerialPort port;
            while (true)
            {
                Console.WriteLine("Ports:");
                for (int i = 0; i < ports.Length; i++)
                {
                    Console.WriteLine($"{i}: {ports[i]}");
                }
                Console.WriteLine("\nSelect port by number: ");
                var input = Console.ReadLine();
                int number;
                if (int.TryParse(input, out number))
                {
                    port = new SerialPort(ports[number]);
                    break;
                }
                Console.WriteLine("Invalid port!");
            }
            while (true)
            {
                try
                {
                    port.Open();
                    break;
                }
                catch
                {
                    Task.Delay(100).Wait();
                }
            }
            Task.Run(consoleWatcher);
            while (true)
            {
                if (closing)
                {
                    break;
                }
                if (port.BytesToRead == 0)
                {
                    Task.Delay(500).Wait();
                }
                else
                {
                    string line = port.ReadLine().Trim();
                    switch (line)
                    {
                        case "ack":
                            port.WriteLine("acked");
                            break;
                        case "id":
                            port.WriteLine("UCRoboticsFieldController");
                            break;
                        case "status":
                            port.WriteLine(active ? "rec" : "norec");
                            break;
                    }

                }
            }
        }

        static async Task consoleWatcher()
        {
            while (true)
            {
                Console.WriteLine("Enter input (0 = set to stopped, 1 = set to started, 2 = exit):");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "0":
                        active = false;
                        break;
                    case "1":
                        active = true;
                        break;
                    case "2":
                        closing = true;
                        break;
                }
                if (closing)
                {
                    break;
                }
            }
        }
    }
}
