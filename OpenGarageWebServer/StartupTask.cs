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
        //class WebServer
        //{
        //    const string HelloWorld = "<html><body>Hello, World!</body></html>";

        //    private const int SIG_PIN = 26;

        //    private bool _GarageOpening;

        //    private object myLock = new object();

        //    private GpioPin sigPin;

        //    private GpioPinValue sigPinValue = GpioPinValue.Low;

        //    public WebServer()
        //    {
        //        var gpio = GpioController.GetDefault();

        //        if (gpio != null)
        //        {
        //            sigPin = gpio.OpenPin(SIG_PIN);

        //            sigPinValue = GpioPinValue.Low;
        //            sigPin.Write(sigPinValue);
        //            sigPin.SetDriveMode(GpioPinDriveMode.Output);
        //        }
        //    }
        //    public bool GarageOpening
        //    {
        //        get
        //        {
        //            lock (myLock)
        //            {
        //                return _GarageOpening;
        //            }
        //        }
        //        private set
        //        {
        //            lock (myLock)
        //            {
        //                _GarageOpening = value;
        //            }
        //        }
        //    }
        //    public bool Start()
        //    {
        //        try
        //        {
        //            StreamSocketListener listener = new StreamSocketListener();
        //            listener.BindServiceNameAsync("80").AsTask();
        //            listener.ConnectionReceived += async(sender, args) =>
        //            {
        //                string responseHTML = HelloWorld;
        //                StringBuilder request = new StringBuilder();
        //                using (IInputStream input = args.Socket.InputStream)
        //                {
        //                    byte[] data = new byte[BufferSize];
        //                    IBuffer buffer = data.AsBuffer();
        //                    uint dataRead = BufferSize;
        //                    while (dataRead == BufferSize)
        //                    {
        //                        await input.ReadAsync(buffer, BufferSize, InputStreamOptions.Partial);
        //                        request.Append(Encoding.UTF8.GetString(data, 0, data.Length));
        //                        dataRead = buffer.Length;
        //                    }

        //                    responseHTML = PrepareResponse(ProcessRequest(request.ToString()));

        //                }

        //                using (IOutputStream output = args.Socket.OutputStream)
        //                {
        //                    using (Stream response = output.AsStreamForWrite())
        //                    {
        //                        byte[] bodyArray = Encoding.UTF8.GetBytes(responseHTML);
        //                        var bodyStream = new MemoryStream(bodyArray);
        //                        var header = "HTTP/1.1 200 OK\r\n" +
        //                                     $"Content-Length: {bodyStream.Length}\r\n" +
        //                                     "Connection: close\r\n\r\n";
        //                        byte[] headerArray = Encoding.UTF8.GetBytes(header);
        //                        await response.WriteAsync(headerArray, 0, headerArray.Length);
        //                        await bodyStream.CopyToAsync(response);
        //                        await response.FlushAsync();
        //                    }
        //                }
        //            };
        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //            return false;
        //        }
        //    }

        //    private async void OpenGarage()
        //    {
        //        if (!GarageOpening)
        //        {
        //            GarageOpening = true;
        //            sigPinValue = GpioPinValue.High;
        //            sigPin.Write(sigPinValue);
        //            await Task.Delay(TimeSpan.FromSeconds(1));
        //            sigPinValue = GpioPinValue.Low;
        //            sigPin.Write(sigPinValue);
        //            GarageOpening = false;
        //        }
        //    }

        //    private string PrepareResponse(string request)
        //    {
        //        string response = "ERROR";
        //        if(request == "opengarage")
        //        {
        //            Task.Run(() => OpenGarage());
        //            response = "<html><body>Garage Opening</body></html>";
        //        }
        //        else
        //        {
        //            response = HelloWorld;
        //        }
        //        return response;
        //    }

        //    //private async void ProcessRequestAsync(StreamSocket socket)
        //    //{
        //    //    HttpRequest request;
        //    //}

        //    private string ProcessRequest(string buffer)
        //    {
        //        string request = "ERROR";
        //        try
        //        {
        //            string[] tokens = buffer.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

        //            if (tokens[0] == "GET")
        //            {
        //                request = tokens[1];
        //                request = request.Replace("/", "");
        //                request = request.ToLower();
        //            }
        //            return request;
        //        }
        //        catch (Exception)
        //        {
        //            return "ERROR";
        //        }
        //    }
        //}
    }
}
