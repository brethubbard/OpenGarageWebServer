using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Rest;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace OpenGarageWebServer
{
    public sealed class StartupTask : IBackgroundTask
    {
        private const uint BufferSize = 8192;

        private HttpServer _httpServer;

        private BackgroundTaskDeferral _deferral;


        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            //This is grabbing the defferal in order to allow the code to continue to run after the Run method is complete.
            _deferral = taskInstance.GetDeferral();

            //Create a new instance of the webserver and set the port to 8800
            var server = new HttpServer(8800);
            _httpServer = server;

            //Create an instance of the route handler
            var restRouteHander = new RestRouteHandler();

            //Register the routes with the route handler
            restRouteHander.RegisterController<GarageController>();

            //Set up the web server with the prefix and the route handler
            server.RegisterRoute("api", restRouteHander);

            //Start the web server.
            await server.StartServerAsync();
            
        }
    }
}
