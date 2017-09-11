using ECommerce.Tables.Active.HR;

namespace ASPEx_2.Models
{
    public class UserModel
    {
        #region Class properties
        public string Name { get; set; }
        public string ShippingAddress { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        #endregion

        #region Class field
        public static      int         ID      = 0;
        #endregion

        #region Class constructor
        public UserModel()
        {
            Account         account     = Account.ExecuteCreate(ID);

            this.Name                   = account.FirstName;
            this.ShippingAddress        = account.ShippingAddress;
            this.ContactNumber          = account.ContactNo;
            this.Email                  = account.Email;
        }
        #endregion        
    }
}