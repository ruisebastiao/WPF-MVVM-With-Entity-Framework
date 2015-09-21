using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using VH.ErrorLog;
using VH.Model;
using VH.Resources;

namespace VH.DataAccess
{
    public class UserLoginAction
    {
        private static readonly Object ThisLock = new Object();
        private static string _connectionString;
        private static UserLoginDBContext _userLoginDBContext;

        public static UserLoginDBContext DBContextInstance
        {
            get
            {
                return _userLoginDBContext ?? (_userLoginDBContext = new UserLoginDBContext(_connectionString));
                
            }
        }

        #region Authenticate Login Credentials

        public UserLogin AuthenticateUserLogin(string connectionString, UserLogin userLogin)
        {
            try
            {
                using (var context = new UserLoginDBContext(connectionString))
                {
                    return
                        context.UserLogins.FirstOrDefault(
                            x => x.LoginName == userLogin.LoginName && x.LoginPassword == userLogin.LoginPassword);
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        #endregion

        #region ADD

        public static bool AddUserLogin(string connectionString, UserLogin userLogin)
        {
            try
            {
                using (var context = new UserLoginDBContext(connectionString))
                {
                    context.UserLogins.Add(userLogin);
                    var result = context.SaveChanges();
                    return result > 0;
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
        }

        #endregion

        #region DELETE

        public static bool DeleteUserLogin(string connectionString, UserLogin userLogin)
        {
            try
            {
                using (var context = new UserLoginDBContext(connectionString))
                {
                    context.UserLogins.Attach(new UserLogin() { ID = userLogin.ID });
                    context.UserLogins.Remove(userLogin);
                    var result = context.SaveChanges();
                    return result > 0;
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
        }

        public static bool DeleteUserLogins(string connectionString, IEnumerable<UserLogin> userLogins)
        {
            try
            {
                using (var context = new UserLoginDBContext(connectionString))
                {
                    foreach (var userLogin in userLogins.Select(userLogin => new UserLogin() { ID = userLogin.ID }))
                    {
                        context.UserLogins.Attach(userLogin);
                        context.UserLogins.Remove(userLogin);
                    }

                    var result = context.SaveChanges();
                    return result > 0;
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
        }

        #endregion

        #region GET

        public static UserLoginCollection GetUserLoginList(string connectionString)
        {
            try
            {
                lock (ThisLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.UserLogins.Select(x => x).ToList();
                    return new UserLoginCollection(items);
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }


        #endregion

        #region UPDATE

        public static bool UpdateUserLogin(string connectionString, UserLogin userLogin)
        {
            try
            {
                _connectionString = connectionString;
                var entry = DBContextInstance.Entry(userLogin);
                entry.State = EntityState.Modified;
                var result = DBContextInstance.SaveChanges();
                return result > 0;
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured, ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
        }

        public static bool UpdateUserLogin(string connectionString, IEnumerable<UserLogin> userLogins)
        {
            try
            {
                _connectionString = connectionString;

                foreach (var userLogin in userLogins)
                {
                    DBContextInstance.UserLogins.Add(userLogin);
                    var entry = DBContextInstance.Entry(userLogin);
                    entry.State = EntityState.Modified;
                }

                var result = DBContextInstance.SaveChanges();
                return result > 0;
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return false;
            }
        }

        #endregion
    }
}
