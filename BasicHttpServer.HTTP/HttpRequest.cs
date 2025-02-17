﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BasicHttpServer.HTTP
{
    public class HttpRequest
    {
        public static IDictionary<string, Dictionary<string, string>> Sessions =
            new Dictionary<string, Dictionary<string, string>>();

        public HttpRequest(string requestString)
        {
            Headers = new List<Header>();
            Cookies = new List<Cookie>();
            FormData = new Dictionary<string, string>();
            QueryData = new Dictionary<string, string>();

            var lines = requestString.Split(new[] { HttpConstants.NewLine }, StringSplitOptions.None);
            var headerLine = lines[0];
            var headerLineParts = headerLine.Split(' ');
            Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), headerLineParts[0], true);
            Path = headerLineParts[1];


            var lineIndex = 1;
            var isInHeaders = true;
            var bodyBuilder = new StringBuilder();

            while (lineIndex < lines.Length)
            {
                var line = lines[lineIndex];
                lineIndex++;

                if (string.IsNullOrWhiteSpace(line))
                {
                    isInHeaders = false;
                    continue;
                }

                if (isInHeaders)
                {
                    Headers.Add(new Header(line));
                }
                else
                {
                    bodyBuilder.AppendLine(line);
                }
            }

            if (Headers.Any(h => h.Name.Equals(HttpConstants.RequestCookieHeader)))
            {
                var cookiesAsString = Headers.
                    FirstOrDefault(h => h.Name.Equals(HttpConstants.RequestCookieHeader))?.Value;
                var cookies = cookiesAsString.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var cookieAsString in cookies)
                {
                    Cookies.Add(new Cookie(cookieAsString));
                }
            }

            var sessionCookie = Cookies.FirstOrDefault(c => c.Name.Equals(HttpConstants.SessionCookieName));
            if (sessionCookie == null)
            {
                var sessionId = Guid.NewGuid().ToString();
                Session = new Dictionary<string, string>();
                Sessions.Add(sessionId, new Dictionary<string, string>());
                Cookies.Add(new Cookie(HttpConstants.SessionCookieName, sessionId));
            }
            else if (!Sessions.ContainsKey(sessionCookie.Value))
            {
                Session = new Dictionary<string, string>();
                Sessions.Add(sessionCookie.Value, new Dictionary<string, string>());
            }
            else
            {
                Session = Sessions[sessionCookie.Value];
            }

            if (Path.Contains("?"))
            {
                var pathParts = Path.Split(new char[] { '?' }, 2);
                Path = pathParts[0];
                QueryString = pathParts[1];
            }
            else
            {
                QueryString = string.Empty;
            }

            Body = bodyBuilder.ToString().TrimEnd('\n', '\r');
            SplitParameters(Body, FormData);
            SplitParameters(QueryString, QueryData);
        }

        private static void SplitParameters(string parametersAsString, IDictionary<string, string> output)
        {
            var parameters = parametersAsString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var parameter in parameters)
            {
                var parameterParts = parameter.Split(new[] { '=' }, 2);
                var name = parameterParts[0];
                var value = WebUtility.UrlDecode(parameterParts[1]);

                if (!output.ContainsKey(name))
                {
                    output.Add(name, value);
                }
            }
        }

        public HttpMethod Method { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public ICollection<Header> Headers { get; set; }
        public ICollection<Cookie> Cookies { get; set; }
        public IDictionary<string, string> FormData { get; set; }
        public IDictionary<string, string> QueryData { get; set; }
        public Dictionary<string, string> Session { get; set; }
        public string Body { get; set; }
    }
}
