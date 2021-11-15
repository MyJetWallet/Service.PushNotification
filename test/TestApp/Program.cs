using System;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Client;
using Service.PushNotification.Client;
using Service.PushNotification.Grpc.Models;
using Service.PushNotification.Grpc.Models.Requests;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Task.Delay(10);
            GrpcClientFactory.AllowUnencryptedHttp2 = true;

            Console.Write("Press enter to start");
            Console.ReadLine();


            var factory = new PushNotificationClientFactory("http://localhost:5001");


            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
