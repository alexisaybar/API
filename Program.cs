using System;
using System.Collections.Generic;

namespace DgiiEcff
{
    public class Request
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public List<Header> Headers { get; set; } = new();
        public Body Body { get; set; }
        public ResponseExample ResponseExample { get; set; }
    }

    public class Header
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class Body
    {
        public string Mode { get; set; }
        public string Raw { get; set; }
    }

    public class ResponseExample
    {
        public int Status { get; set; }
        public string Body { get; set; }
        public string SaveToFile { get; set; }
    }

    public class Collection
    {
        public string CollectionName { get; set; }
        public string Version { get; set; }
        public List<Request> Requests { get; set; } = new();
    }

    internal static class Program
    {
        private static void Main()
        {
            // Ejemplo de inicialización:
            var collection = new Collection
            {
                CollectionName = "DGII_E-CF_Flujo_Autorizacion",
                Version = "1.0",
                Requests = new List<Request>
                {
                    new Request
                    {
                        Id = "1",
                        Name = "1. Solicitar Archivo Semilla",
                        Method = "POST",
                        Url = "https://api.dgii.gov.do/ecf/semilla",
                        Headers = new List<Header>
                        {
                            new Header { Key = "Content-Type", Value = "application/json" }
                        },
                        Body = new Body
                        {
                            Mode = "raw",
                            Raw = string.Empty
                        },
                        ResponseExample = new ResponseExample
                        {
                            Status = 200,
                            Body = "{\"semilla\":\"xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx\"}",
                            SaveToFile = "Z:/sistema/freyes/krystal-siig/firma/semilla_response.txt"
                        }
                    },
                    new Request
                    {
                        Id = "2",
                        Name = "2. Solicitar Token (Autenticación y Firma)",
                        Method = "POST",
                        Url = "https://api.dgii.gov.do/ecf/token",
                        Headers = new List<Header>
                        {
                            new Header { Key = "Content-Type", Value = "application/json" }
                        },
                        Body = new Body
                        {
                            Mode = "json",
                            Raw = "{\"firmaBase64\": \"[RESULTADO_DE_LA_FIRMA]\"}"
                        },
                        ResponseExample = new ResponseExample
                        {
                            Status = 200,
                            Body = "{\"access_token\": \"[TOKEN_OBTENIDO]\", \"expires_in\": 3600}",
                            SaveToFile = "Z:/sistema/freyes/krystal-siig/firma/token_response.txt"
                        }
                    },
                    new Request
                    {
                        Id = "3",
                        Name = "3. Enviar XML (Comprobante)",
                        Method = "POST",
                        Url = "https://api.dgii.gov.do/ecf/envio",
                        Headers = new List<Header>
                        {
                            new Header { Key = "Content-Type", Value = "text/xml" },
                            new Header { Key = "Authorization", Value = "Bearer [TOKEN_OBTENIDO_DEL_PASO_2]" }
                        },
                        Body = new Body
                        {
                            Mode = "raw",
                            Raw = "Z:\\sistema\\freyes\\krystal-siig\\xml_a_enviar\\101028191E310000000820.xml"
                        },
                        ResponseExample = new ResponseExample
                        {
                            Status = 200,
                            Body = "{\"trackId\":\"123456789\", \"mensaje\":\"Aceptado\"}",
                            SaveToFile = "Z:/sistema/freyes/krystal-siig/firma/trackid_response.txt"
                        }
                    }
                }
            };

            Console.WriteLine($"Colección: {collection.CollectionName} (Requests: {collection.Requests.Count})");
        }
    }
}