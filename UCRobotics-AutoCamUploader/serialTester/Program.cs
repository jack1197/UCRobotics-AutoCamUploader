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
                if(int.TryParse(input, out number))
                {
                    port = new SerialPort(ports[number]);
                    break;
                }
                Console.WriteLine("Invalid port!");
            }
            port.Open();
            while(true)
            {
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
                            port.WriteLine("norec");
                            break;
                    }

                }
            }
        }
    }
}
