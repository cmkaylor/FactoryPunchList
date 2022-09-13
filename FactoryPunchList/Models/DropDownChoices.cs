using FactoryPunchList.App_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FactoryPunchList.Models
{
    //keeping these values in once place for easier updates... maybe can assign them to actual database values
    public static class DropDownChoices
    {
        public static List<SelectListItem> StatusDropDownList = new List<SelectListItem>()
        {
            new SelectListItem() {Text = "Completed", Value = "Completed"},
            new SelectListItem() {Text = "Not Completed", Value = "Not Completed"}
        };

        public static List<SelectListItem> sortChoices = GetPunchProperties();

        private static List<SelectListItem> GetPunchProperties()
        {
            List<SelectListItem> returnable = new List<SelectListItem>();
            foreach(var prop in typeof(Punchlist).GetProperties())
            {
                if(prop.Name.ToString() != "id" && !prop.GetGetMethod().IsVirtual)
                {
                    returnable.Add(new SelectListItem() { Text = prop.Name.ToString(), Value = prop.Name.ToString() });
                }
            }

            returnable.Add(new SelectListItem() { Text = "Title", Value = "Title"});
            return returnable;
        }

    }
}