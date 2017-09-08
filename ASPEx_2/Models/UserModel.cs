using ECommerce.Tables.Active.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPEx_2.Models
{
    public class UserModel
    {
        public static int ID = 0; 
        public UserModel()
        {
            Account     account         = Account.ExecuteCreate(ID);
            this.Name                   = account.FirstName;
            this.ShippingAddress        = account.ShippingAddress;
            this.contactNumber          = account.ContactNo;
            this.email                  = account.Email;
        }

        public string Name { get; set; }
        public string ShippingAddress { get; set; }
        public string contactNumber { get; set; }
        public string email { get; set; }
    }
}