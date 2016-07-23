using OpenGarageWebServer.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace OpenGarageWebServer
{
    public sealed class HttpServer : IDisposable
    {
        private readonly StreamSocketListener listener;
        private readonly int port;
        public RESTHandler restHandler { get; } = new RESTHandler();

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
