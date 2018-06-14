using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Udp_Client1
{
    class UdpClient1
    {

        static string remoteAddress = "192.168.1.149";
        static int remotePort = 100;
        static int localPort = 101;

        private static void SendMessage()
        {
            // создаем UdpClient для отправки сообщений

            UdpClient sender = new UdpClient();
            //sender.Connect(remoteAddress, remotePort);
            //UdpClient sender = new UdpClient(new IPEndPoint(IPAddress.Parse(remoteAddress), remotePort));
            try
            {
                while (true)
                {
                    string message = Console.ReadLine(); // сообщение для отправки
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    sender.Send(data, data.Length, remoteAddress, remotePort); // отправка
                    //sender.Send(data, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }

        private static void ReceiveMessage()
        {
            UdpClient receiver = new UdpClient(localPort); // UdpClient для получения данных
            IPEndPoint remoteIp = null; // адрес входящего подключения
            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp); // получаем данные
                    string message = Encoding.Unicode.GetString(data);
                    Console.WriteLine($"{remoteIp.ToString()}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                receiver.Close();
            }
        }
        static void Main(string[] args)
        {
            Task task1 = Task.Run(() =>
            {
                Thread.CurrentThread.IsBackground = false; //For option 1
                UdpClient1.SendMessage();
            });

            Task task2 = Task.Run(() =>
            {
                UdpClient1.ReceiveMessage();
            });
            task2.Wait(); //For option2
        }
    }
}
