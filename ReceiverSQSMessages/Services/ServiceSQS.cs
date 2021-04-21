using Amazon.SQS;
using Amazon.SQS.Model;
using MvcCoreSenderSQS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReceiverSQSMessages.Services
{
    public class ServiceSQS
    {
        private IAmazonSQS clientSQS;
        private string queueUrl;

        public ServiceSQS(IAmazonSQS clientsqs)
        {
            this.queueUrl = "https://sqs.eu-west-3.amazonaws.com/161635109687/queue-consultas-usuarios";
            this.clientSQS = clientsqs;
        }

        public async Task<List<MensajeUsuario>> ReceiveMessagesAsync()
        {
            ReceiveMessageRequest request = new ReceiveMessageRequest()
            {
                QueueUrl = this.queueUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 5
            };

            ReceiveMessageResponse response = await this.clientSQS.ReceiveMessageAsync(request);
            if(response.HttpStatusCode == HttpStatusCode.OK)
            {
                if(response.Messages.Count == 0)
                {
                    return null;
                }else
                {
                    List<Message> messages = response.Messages;
                    List<MensajeUsuario> mensajesUsuarios = messages.Select(x => JsonConvert.DeserializeObject<MensajeUsuario>(x.Body)).ToList();
                    return mensajesUsuarios;
                }
            } else
            {
                return null;
            }
        }

        public async Task<bool> DeleteMessageAsync()
        {
            DeleteMessageRequest request = new DeleteMessageRequest()
            {
                QueueUrl = this.queueUrl,
                ReceiptHandle = "OK"
            };

            DeleteMessageResponse response = await this.clientSQS.DeleteMessageAsync(request);
            if(response.HttpStatusCode == HttpStatusCode.OK)
            {
                return true;
            }else
            {
                return false;
            }
        }
    }
}
