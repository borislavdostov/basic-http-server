﻿using System;

namespace BasicHttpServer.MvcFramework
{
    public interface IServiceCollection
    {
        void Add<TSource, TDestination>();

        object CreateInstance(Type type);
    }
}
