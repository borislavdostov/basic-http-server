﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BasicHttpServer.HTTP
{
    public class HttpResponse
    {
        public HttpResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            Headers = new List<Header>();
            Cookies = new List<Cookie>();
        }

        public HttpResponse(string contentType, byte[] body, HttpStatusCode statusCode = HttpStatusCode.Ok)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            StatusCode = statusCode;
            Body = body;
            Headers = new List<Header>
            {
                new Header("Content-Type", contentType),
                new Header("Content-Length", body.Length.ToString())
            };
            Cookies = new List<Cookie>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public ICollection<Header> Headers { get; set; }
        public byte[] Body { get; set; }
        public ICollection<Cookie> Cookies { get; set; }

        public override string ToString()
        {
            var responseBuilder = new StringBuilder();
            responseBuilder.Append($"HTTP/1.1 {(int)StatusCode} {StatusCode}" + HttpConstants.NewLine);
            foreach (var header in Headers)
            {
                responseBuilder.Append(header.ToString() + HttpConstants.NewLine);
            }

            foreach (var cookie in Cookies)
            {
                responseBuilder.Append("Set-Cookie: " + cookie.ToString() + HttpConstants.NewLine);
            }

            responseBuilder.Append(HttpConstants.NewLine);

            return responseBuilder.ToString();
        }
    }
}