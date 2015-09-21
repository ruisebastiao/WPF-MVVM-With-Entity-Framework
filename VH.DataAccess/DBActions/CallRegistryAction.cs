using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VH.ErrorLog;
using VH.Model;
using VH.Resources;

namespace VH.DataAccess
{
    public class CallRegistryAction
    {
        #region Add Call Log

        public static bool AddCallLog(string connectionString, CallRegistry callRegistry)
        {
            try
            {
                using (var context = new CallRegistryDBContext(connectionString))
                {
                    context.CallRegistries.Add(callRegistry);
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

        #region Delete Call Log
        public static bool DeleteCallRegistry(string connectionString, CallRegistry callRegistry)
        {
            try
            {
                using (var context = new CallRegistryDBContext(connectionString))
                {
                    context.CallRegistries.Attach(new CallRegistry() {ID = callRegistry.ID});
                    context.CallRegistries.Remove(callRegistry);
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

        public static bool DeleteCallRegistries(string connectionString, IEnumerable<CallRegistry> callRegistries)
        {
            try
            {
                using (var context = new CallRegistryDBContext(connectionString))
                {
                    foreach (var customer in callRegistries.Select(callReg => new CallRegistry() {ID = callReg.ID}))
                    {
                        context.CallRegistries.Attach(customer);
                        context.CallRegistries.Remove(customer);
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

        #region Get Call Logs
        public static CallRegistryCollection GetCallRegistries(string connectionString, DateTime? callDate)
        {
            try
            {
                using (var context = new CallRegistryDBContext(connectionString))
                {
                    var callRegCollection = new CallRegistryCollection();
                    var items = context.CallRegistries.Where(x => EntityFunctions.TruncateTime(x.CallDate) == EntityFunctions.TruncateTime(callDate.Value.Date));
                    foreach (var callReg in items)
                        callRegCollection.Add(callReg);

                    return callRegCollection;
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public static CallRegistryCollection GetCallRegistries(string connectionString, string searchName, DateTime? callDate)
        {
            try
            {
                using (var context = new CallRegistryDBContext(connectionString))
                {
                    var callRegCollection = new CallRegistryCollection();
                    IQueryable<CallRegistry> items = null;
                    if (callDate.HasValue && !string.IsNullOrEmpty(searchName))
                        items =
                            context.CallRegistries.Where(
                                x =>
                                EntityFunctions.TruncateTime(x.CallDate) == EntityFunctions.TruncateTime(callDate.Value) &&
                                x.Name.Contains(searchName));
                    else if (!string.IsNullOrEmpty(searchName))
                        items = context.CallRegistries.Where(x => x.Name.Contains(searchName));
                    else if (callDate.HasValue)
                    {
                        items =
                            context.CallRegistries.Where(
                                x =>
                                EntityFunctions.TruncateTime(x.CallDate) == EntityFunctions.TruncateTime(callDate.Value));
                    }
                    else
                    {
                        items = context.CallRegistries;
                    }
                    if (items != null)
                        foreach (var callReg in items)
                            callRegCollection.Add(callReg);

                    return new CallRegistryCollection(callRegCollection.ObservableList.OrderByDescending(x => x.CallDate).ToList());
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public static CallRegistryCollection GetCallRegistries(string connectionString)
        {
            try
            {
                using (var context = new CallRegistryDBContext(connectionString))
                {
                    var callRegCollection = new CallRegistryCollection();

                    foreach (var callReg in context.CallRegistries)
                        callRegCollection.Add(callReg);

                    return callRegCollection;
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

        #region Update Call Log

        public static bool UpdateCallRegistry(string connectionString, CallRegistry callRegistry)
        {
            try
            {
                using (var context = new CallRegistryDBContext(connectionString))
                {
                    context.CallRegistries.Add(callRegistry);
                    var entry = context.Entry(callRegistry);
                    entry.State = EntityState.Modified;
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

        public static bool UpdateCallRegistries(string connectionString, IEnumerable<CallRegistry> callRegistries)
        {
            try
            {
                using (var context = new CallRegistryDBContext(connectionString))
                {
                    foreach (var callReg in callRegistries)
                    {
                        context.CallRegistries.Add(callReg);
                        var entry = context.Entry(callReg);
                        entry.State = EntityState.Modified;
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
    }
}
