using FactoryPunchList.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryPunchList.Methods
{
    public class Authenticate
    {
        private MyDatabaseEntities sDb = new MyDatabaseEntities();
        public bool CookieExists(HttpRequestBase request)
        {
            HttpCookie myCookie = request.Cookies["UserIdentity"];
            
            if(myCookie != null)
            {
                if (sDb.Coins.FirstOrDefault(o => o.session == myCookie.Value) != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}