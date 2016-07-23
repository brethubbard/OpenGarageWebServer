using System.Threading.Tasks;

namespace OpenGarageWebServer.Handlers
{
    public abstract class RequestHandler
    {
        public abstract Task<HttpResponse> Handle(HttpRequest request);

    }
}