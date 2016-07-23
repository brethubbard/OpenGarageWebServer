using System;
using System.Threading.Tasks;

namespace OpenGarageWebServer
{
    public class Controller
    {
        public string Prefix { get; internal set; }

        internal Task<HttpResponse> Handle(HttpRequest request)
        {
            throw new NotImplementedException();
        }
    }
}