﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRealty.Common
{
    public class CustomServerHeaderModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        public void Dispose() { }

        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            //HttpContext.Current.Response.Headers.Set("Server", "Apache 2000 Server");
            HttpContext.Current.Response.Headers.Remove("Server");
        }
    }
}