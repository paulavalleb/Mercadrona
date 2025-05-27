using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace csDronLink
{
    public class PedidoListener
    {
        private HttpListener listener;
        private bool escuchando = false;

        // Recibimos PedidoWeb (JSON plano)
        public Action<PedidoWeb> OnPedidoRecibido;

        public void Start()
        {
            if (escuchando) return;
            escuchando = true;

            listener = new HttpListener();
            listener.Prefixes.Add("http://127.0.0.1:8080/pedido/");
            listener.Start();
            Console.WriteLine("PedidoListener iniciado y escuchando en " + listener.Prefixes.First()); // <-- Añade esto

            Task.Run(() =>
            {
                while (escuchando)
                {
                    HttpListenerContext context;
                    try
                    {
                        context = listener.GetContext();
                        Console.WriteLine("Petición recibida."); // <-- Añade esto
                    }
                    catch
                    {
                        break; // listener detenido
                    }

                    var req = context.Request;
                    var resp = context.Response;

                    // ******************************************************
                    // CRITICAL: ADD CORS HEADERS HERE, BEFORE ANY 'IF' STATEMENTS
                    // This ensures they are on all responses, including OPTIONS and POST.
                    // For local development, '*' is easiest.
                    resp.AddHeader("Access-Control-Allow-Origin", "*");
                    resp.AddHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                    resp.AddHeader("Access-Control-Allow-Headers", "Content-Type");
                    // If your JS ever includes credentials (cookies, auth headers):
                    // resp.AddHeader("Access-Control-Allow-Credentials", "true");
                    // ******************************************************

                    if (req.HttpMethod == "OPTIONS")
                    {
                        // For preflight requests, just send OK status and headers, then close.
                        // The headers are already added above.
                        resp.StatusCode = 200;
                        resp.Close();
                        continue; // Go to the next request
                    }
                    else if (req.HttpMethod == "POST")
                    {
                        try
                        {
                            // Leer JSON
                            string json;
                            using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
                                json = reader.ReadToEnd();
                            Console.WriteLine("JSON recibido: " + json); // <--- ESTA LÍNEA ES VITAL
                            // Deserializar
                            PedidoWeb pedidoWeb = null;
                            try { pedidoWeb = JsonConvert.DeserializeObject<PedidoWeb>(json); }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error deserializando JSON: " + ex.Message);
                            }

                            if (pedidoWeb != null)
                                OnPedidoRecibido?.Invoke(pedidoWeb);

                            // Responder OK
                            var buffer = Encoding.UTF8.GetBytes("OK");
                            resp.ContentLength64 = buffer.Length;
                            resp.StatusCode = 200;
                            resp.OutputStream.Write(buffer, 0, buffer.Length);
                            resp.Close();
                            continue;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error procesando petición POST: " + ex.Message);
                            resp.StatusCode = 500;
                            resp.Close();
                            continue;
                        }
                    }
                    else
                    {
                        // Método no permitido
                        resp.StatusCode = 405;
                        resp.Close();
                        continue;
                    }
                }
            });
        }

        public void Stop()
        {
            escuchando = false;
            try { listener?.Stop(); } catch { }
        }
    }
}