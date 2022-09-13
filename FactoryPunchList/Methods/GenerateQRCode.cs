using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace FactoryPunchList.Methods
{
    public static class GenerateQRCode
    {
        public static string GetCodeUrl(int id)
        {
            string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            string url = $"{domainName}/Punchlists/Details/{id.ToString()}";

            return "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=" + url;
        }

        public static void Bulk()
        {

        }
    }
}