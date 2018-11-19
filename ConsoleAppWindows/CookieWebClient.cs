using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWindows
{
    class CookieWebClient : WebClient
    {
        private CookieContainer cookies = new CookieContainer();
        private string referer;
        public CookieContainer CookieContainer { get { return cookies; } }

        private sealed class AcceptAllCertificatePolicy : ICertificatePolicy
        {
            public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
            {
                return true;
            }
        }

        static CookieWebClient()
        {
            ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                HttpWebRequest httprequest = (HttpWebRequest)request;
                if (httprequest.Method == "POST")
                    httprequest.ContentType = "application/x-www-form-urlencoded";
                httprequest.Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, application/msword, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/x-shockwave-flash, */*";
                httprequest.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                httprequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0)";
                httprequest.Referer = referer;
                httprequest.CookieContainer = cookies;
            }
            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            if (request is HttpWebRequest)
                referer = request.RequestUri.ToString();
            return base.GetWebResponse(request);
        }
    }
}
