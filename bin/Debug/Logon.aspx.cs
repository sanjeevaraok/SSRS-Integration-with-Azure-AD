#region
// Copyright (c) 2016 Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License (MIT)
/*============================================================================
  File:     Logon.aspx.cs
  Summary:  The code-behind for a logon page that supports Forms
            Authentication in a custom security extension    
--------------------------------------------------------------------
  This file is part of Microsoft SQL Server Code Samples.
    
 This source code is intended only as a supplement to Microsoft
 Development Tools and/or on-line documentation. See these other
 materials for detailed information regarding Microsoft code 
 samples.

 THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF 
 ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
 THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
 PARTICULAR PURPOSE.
===========================================================================*/
#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.Security;
using Microsoft.ReportingServices.Interfaces;
using Microsoft.Samples.ReportingServices.CustomSecurity.App_LocalResources;
using System.Globalization;
using System.Collections.Generic;

namespace Microsoft.Samples.ReportingServices.CustomSecurity
{
    public class Logon : System.Web.UI.Page
    {
        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Request.Form["id_token"] != null)
            {
                // ClaimsPrincipal token = Validate(Request.Form["id_token"]);

                FormsAuthentication.RedirectFromLoginPage(
                        "", false);
                Response.Write("Success");
            }
            else
            {
                LoginAzure();

            }
        }

        void LoginAzure()
        {
            var authority = "https://login.microsoftonline.com";
            var tenant = "common";
            var authorizeSuffix = "oauth2";

            var EndPointUrl = String.Format("{0}/{1}/{2}/authorize?", authority, tenant, authorizeSuffix);

            var clientId = "15d8583e-84e2-4f05-851d-5c78a9f6337c"; ;
            var redirectURL = "http://ggku3dell1097/ReportServer/Logon.aspx";
            var parameters = new Dictionary<string, string>
            {
                { "response_type", "id_token" },
                { "client_id", clientId },
                { "redirect_uri", redirectURL },
                {"response_mode","form_post" },
                {"nonce","678910" },
                { "state", "12345"}
            };

            var list = new List<string>();

            foreach (var parameter in parameters)
            {
                if (!string.IsNullOrEmpty(parameter.Value))
                    list.Add(string.Format("{0}={1}", parameter.Key, HttpUtility.UrlEncode(parameter.Value)));
            }
            var strParameters = string.Join("&", list);
            var requestURL = String.Concat(EndPointUrl, strParameters);

            Response.Redirect(requestURL);
        }
    }
}
