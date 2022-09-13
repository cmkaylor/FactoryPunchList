using FactoryPunchList.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FactoryPunchList.Models
{
    //using this to pass punchlist and image file from form to controller
    public class PunchlistImageModel
    {
        public Punchlist Punchlists { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}