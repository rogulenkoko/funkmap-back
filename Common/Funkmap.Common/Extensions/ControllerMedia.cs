using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Funkmap.Common.Owin.Extensions
{
    public static class ControllerMedia
    {
        public static HttpResponseMessage MediaResponse(byte[] image)
        {
            var response = new HttpResponseMessage();
            if (image == null || image.Length == 0)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new ByteArrayContent(image);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            }
            return response;
        }

        public static string ToAbsoluteMediaUrl(this HttpRequestMessage request, string url, string urlPrefix)
        {
            var isUrl = Uri.TryCreate(url, UriKind.Absolute, out var u);
            if (!isUrl && !String.IsNullOrEmpty(url))
            {
                var currentHost = $"{request.RequestUri.Scheme}://{request.RequestUri.Host}:{request.RequestUri.Port}";
                return $"{currentHost}/{urlPrefix}";
            }
            return url;
        }
    }
}
