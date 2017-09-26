using ECommerce.Tables.Active.HR;
using System;
using System.Web;

namespace ASPEx_2.Helpers
{
    [Serializable]
    public sealed class SessionSingleton
    {
        #region Singleton
        private SessionSingleton()
        {

        }

        public static SessionSingleton Current
        {
            get
            {
                if (HttpContext.Current.Session[Constants.SESSION_SINGLETON_NAME] == null)
                {
                    HttpContext.Current.Session[Constants.SESSION_SINGLETON_NAME] = new SessionSingleton();
                }

                return HttpContext.Current.Session[Constants.SESSION_SINGLETON_NAME] as SessionSingleton;
            }
        }
        #endregion

    #region Session variables
        public Account CurrentUserSession { get; set; }
        public int CurrentUserRole { get; set; }
        #endregion
    }
}