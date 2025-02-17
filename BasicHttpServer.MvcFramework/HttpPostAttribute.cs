﻿using BasicHttpServer.HTTP;

namespace BasicHttpServer.MvcFramework
{
    public class HttpPostAttribute : BaseHttpAttribute
        {
            public HttpPostAttribute()
            {

            }

            public HttpPostAttribute(string url)
            {
                Url = url;
            }

            public override HttpMethod Method => HttpMethod.Post;
        }
}