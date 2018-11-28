using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace Funkmap.Common.Core.Extensions
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

        public static string ToAbsoluteMediaUrl(this HttpRequest request, string url, string urlPrefix)
        {
            var isUrl = Uri.TryCreate(url, UriKind.Absolute, out var u);
            if (isUrl || string.IsNullOrEmpty(url)) return url;
            
            var currentHost = $"{request.Scheme}://{request.Host}";
            return $"{currentHost}/{urlPrefix}";
        }
    }
}
