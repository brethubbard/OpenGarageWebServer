using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;
using System;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Web.Http;

namespace OpenGarageWebServer
{
    [RestController(InstanceCreationType.Singleton)]
    public sealed class GarageController
    {
        private bool _GarageOpening;
        private object myLock = new object();

        private const int SIG_PIN = 26;

        private GpioPin sigPin;

        private GpioPinValue sigPinValue = GpioPinValue.Low;

        public bool GarageOpening
        {
            get
            {
                lock (myLock)
                {
                    return _GarageOpening;
                }
            }
            private set
            {
                lock (myLock)
                {
                    _GarageOpening = value;
                }
            }
        }

        public GarageController()
        {
            var gpio = GpioController.GetDefault();

            if (gpio != null)
            {
                sigPin = gpio.OpenPin(SIG_PIN);

                sigPinValue = GpioPinValue.Low;
                sigPin.Write(sigPinValue);
                sigPin.SetDriveMode(GpioPinDriveMode.Output);
            }
        }
        
        [UriFormat("/garage/openclose")]
        public IAsyncOperation<IPostResponse> OpenClose()
        {
            try
            {
                if (!GarageOpening)
                {
                    GarageOpening = true;
                    sigPinValue = GpioPinValue.High;
                    sigPin.Write(sigPinValue);
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                    sigPinValue = GpioPinValue.Low;
                    sigPin.Write(sigPinValue);
                    GarageOpening = false;
                }
                return Task.FromResult<IPostResponse>(new PostResponse(PostResponse.ResponseStatus.Created, "", new { openedGarageDoor = true })).AsAsyncOperation();
            }
            catch
            {
                return Task.FromResult<IPostResponse>(new PostResponse(PostResponse.ResponseStatus.Conflict, "", new { openedGarageDoor = false })).AsAsyncOperation();
            }
        }
    }
}