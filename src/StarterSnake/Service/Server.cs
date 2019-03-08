using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

using StarterSnake.ApiModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace StarterSnake.Service {

    /// <summary>
    /// Contract resolver used for JSON deserialization by the HTTP server. Required to enforce
    /// all properties.
    /// </summary>
    internal sealed class ServerContractResolver : DefaultContractResolver
    {
        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var contract = base.CreateObjectContract(objectType);
            // Always require all properties
            contract.ItemRequired = Required.Always;
            return contract;
        }
    }

    /// <summary>
    /// Exception treated specially by the HTTP server to stop serving with a specific error code
    /// </summary>
    internal sealed class StopServingException : Exception {
        public readonly int StatusCode;

        public StopServingException(int statusCode) {
            StatusCode = statusCode;
        }
    }

    /// <summary>
    /// Crude HTTP REST server for serving snake API requests
    /// </summary>
    public sealed class Server : IDisposable {

        private HttpListener listener;

        private JsonSerializerSettings serializerSettings;

        private ISnakeServiceController controller;

        /// <summary>
        /// Create a new HTTP snake server on the given root URL with the specified instance controller.
        /// To start the server, call <see cref="Run()"/>
        /// </summary>
        /// <param name="root">Root path. Should end with a slash and may contain wildcards.</param>
        /// <param name="controller">Controller determining snake action</param>
        public Server(string root, ISnakeServiceController controller) {
            if (controller == null) {
                throw new ArgumentNullException("controller");
            }

            listener = new HttpListener();
            listener.Prefixes.Add(root);

            this.controller = controller;

            // Configure serializer
            serializerSettings = DefaultSerializerSettings;
        }

        /// <summary>
        /// Default (de)serializer settings for the server. Missing members are an error
        /// and require all items.
        /// </summary>
        public static JsonSerializerSettings DefaultSerializerSettings {
            get {
                var opt = new JsonSerializerSettings();
                opt.MissingMemberHandling = MissingMemberHandling.Error;
                opt.ContractResolver = new ServerContractResolver();
                return opt;
            }
        }

        /// <summary>
        /// Handle single HTTP request
        /// </summary>
        private void HandleRequest(HttpListenerContext ctx) {
            try {
                // Initially assume internal error
                ctx.Response.StatusCode = 503;

                var path = ctx.Request.Url.AbsolutePath;

                // TODO: Limit HTTP methods
                if (path == "/ping") {
                    ctx.Response.StatusCode = 200;
                } else if (path == "/start") {
                    var state = ParseGameState(ctx.Request);
                    var json = JsonConvert.SerializeObject(new ResponseStart(controller.Start(state)));
                    ReplyJson(ctx.Response, 200, json);
                } else if (path == "/move") {
                    var state = ParseGameState(ctx.Request);
                    var json = JsonConvert.SerializeObject(new ResponseMove(controller.Move(state)));
                    ReplyJson(ctx.Response, 200, json);
                } else if (path == "/end") {
                    var state = ParseGameState(ctx.Request);
                    controller.End(state);
                    ctx.Response.StatusCode = 200;
                } else {
                    throw new StopServingException(404);
                }

            } catch (StopServingException sse) {
                ctx.Response.StatusCode = sse.StatusCode;
            } catch (Exception e) {
                Console.Error.WriteLine($"[SERVER] Uncaught exception while handling HTTP request: {e}");
            } finally {
                ctx.Response.OutputStream.Close();
            }
        }

        /// <summary>
        /// Run this server, servicing each request via thread pooling
        /// </summary>
        public void Run() {
            listener.Start();

            while (listener.IsListening) {
                var ctx = listener.GetContext();
                ThreadPool.QueueUserWorkItem(c => HandleRequest(c), ctx, false);
            }
        }

        /// <summary>
        /// Parse game state from http request body
        /// </summary>
        private GameState ParseGameState(HttpListenerRequest r) {
            try {
                // Always parse as UTF-8
                // TODO: Should also allow other valid encodings for JSON
                var reader = new StreamReader(r.InputStream, Encoding.UTF8);

                // Deserialize game object by reading request payload stream to end
                var value = JsonConvert.DeserializeObject<GameState>(reader.ReadToEnd(), serializerSettings);

                // Empty is also invalid
                if (value == null) throw new IOException("Request must not be empty or otherwise evaluate to null");

                return value;
            } catch (Exception e) {
                // On any error, just return bad request
                Console.Error.WriteLine($"[SERVER] Bad request: {e}");
                throw new StopServingException(400);
            }
        }

        /// <summary>
        /// Write JSON reply with correct length, content type and specified error code
        /// </summary>
        private static void ReplyJson(HttpListenerResponse r, int code, string content) {
            r.StatusCode = code;
            r.ContentType = "application/json";
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            r.ContentLength64 = buffer.Length;
            r.OutputStream.Write(buffer, 0, buffer.Length);
        }

        #region Disposable
        // Initially not disposed
        bool disposed = false;

        public void Dispose() {
            Dispose(true);
            // Manually disposing, do not finalize
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool manuallyDisposing) {
            // Cannot dispose more than once
            if (disposed) return;

            if (manuallyDisposing) {
                // If manually disposing, GC has not disposed any managed child members yet,
                // we are disposing top-down so we are responsible for disposing managed children

                // DISPOSE MANAGED MEMBERS HERE
                if (listener.IsListening) {
                    listener.Stop();
                }
                listener.Close();
            }

            // DIPOSE UNMANAGED MEMBERS HERE

            // Done disposing
            disposed = true;
        }

        ~Server() {
            // Automatically disposing
            Dispose(false);
        }
        #endregion
    }
}