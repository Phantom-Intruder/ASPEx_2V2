using ECommerce.Tables.Active.HR;

namespace ASPEx_2.Models
{
    public class UserModel
    {
        #region Properties
        private string Name { get; set; }
        private string ShippingAddress { get; set; }
        private string ContactNumber { get; set; }
        private string Email { get; set; }
        #endregion

        #region Class members
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

        #region Getters
        public string GetName()
        {
            return this.Name;
        }

        public string GetShippingAddress()
        {
            return this.ShippingAddress;
        }

        public string GetContactNumber()
        {
            return this.ContactNumber;
        }

        public string GetEmail()
        {
            return this.Email;
        }
        #endregion
    }
}