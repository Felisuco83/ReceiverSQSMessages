using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;
using MvcCoreSenderSQS.Models;
using ReceiverSQSMessages.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReceiverSQSMessages
{
    class Program
    {
        async static Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection().AddAWSService<IAmazonSQS>().AddTransient<ServiceSQS>().BuildServiceProvider();

            ServiceSQS service = serviceProvider.GetService<ServiceSQS>();

            List<MensajeUsuario> mensajes = await service.ReceiveMessagesAsync();
            if (mensajes == null)
            {
                Console.WriteLine("No existen mensajes en la cola");
            } else
            {
                Console.WriteLine("Numero de mensajes: " + mensajes.Count);
                foreach (var msj in mensajes)
                {
                    Console.WriteLine(msj.Asunto);
                    Console.WriteLine(msj.Email);
                    Console.WriteLine(msj.FechaMensaje);
                    Console.WriteLine(msj.Mensaje);
                    Console.WriteLine("-----------------------------------------------");
                }
                Console.WriteLine("Fin de lectura de mensajes");
                Console.WriteLine("¿Desea eliminar los mensajes leidos? (Y/N)");
                string respuesta = Console.ReadLine();
                if (respuesta.ToLower() == "y")
                {
                    bool dato = await service.DeleteMessageAsync();
                    Console.WriteLine("Mensajes eliminados(" + dato + ")");
                }
            }
            Console.WriteLine("FIN DE PROGRAMA");
        }
    }
}
