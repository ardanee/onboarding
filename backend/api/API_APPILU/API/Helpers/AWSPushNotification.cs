using Amazon;
using Amazon.Pinpoint;
using Amazon.Pinpoint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Helpers
{
    public class AWSPushNotification
    {
        public static dynamic enviarNotificacionAEndpoints(int[] tokensIds, string body, string title, string url)
        {
            //string body = "Este es el cuerpo :D";
            //string title = "Este es el titulo";
            //string url = "appilu://perfil";
            string priority = "10";
            bool responseBool = false;
            List<int> fallidos = new List<int>();
            List<int> entregados = new List<int>();
            

            IDictionary<string, dynamic> response = new Dictionary<string, dynamic>();

            if (tokensIds.Length > 0)
            {
                responseBool = true;
                var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(Configuration.getVar("AWS_ACCESS_KEY"), Configuration.getVar("AWS_SECRET_KEY"));

                using (var client = new AmazonPinpointClient(awsCredentials, RegionEndpoint.GetBySystemName(Configuration.getVar("AWS_REGION"))))
                {
                    foreach (int token in tokensIds)
                    {
                    Dictionary<string, EndpointSendConfiguration> endpoint = new Dictionary<string, EndpointSendConfiguration>();
                    endpoint.Add(token.ToString(), new EndpointSendConfiguration());
                    
                        try
                        {
                        
                            SendMessagesRequest sumr = new SendMessagesRequest()
                            {
                                ApplicationId = Configuration.getVar("AWS_PINPOINT_APP_ID"),
                                MessageRequest = new MessageRequest()
                                {

                                    Context = new Dictionary<string, string>() { { "deeplink", url } },

                                    MessageConfiguration = new DirectMessageConfiguration()
                                    {
                                        APNSMessage = new APNSMessage()
                                        {
                                            Action = new Amazon.Pinpoint.Action("DEEP_LINK"),
                                            Body = body,
                                            Title = title,
                                            Url = url,
                                            Priority = priority,
                                            SilentPush = false,
                                            Sound = "Marimba"
                                        },
                                        GCMMessage = new GCMMessage()
                                        {
                                            Action = new Amazon.Pinpoint.Action("DEEP_LINK"),
                                            Body = body,
                                            Title = title,
                                            Url = url,
                                            Priority = priority,
                                            SilentPush = false,
                                        }
                                    },
                                    TraceId = "1",
                                    Endpoints = endpoint
                                }
                            };

                            client.SendMessages(sumr);

                            entregados.Add(token);

                        
                    }
                    catch (Exception e)
                    {
                        fallidos.Add(token);
                    }
                }

            }
            }

            response.Add("response", responseBool);
            response.Add("fallidos", fallidos);
            response.Add("entregados", entregados);

            return response;
        }
    }
}