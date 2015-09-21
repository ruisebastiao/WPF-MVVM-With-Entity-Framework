using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using VH.ErrorLog;
using VH.Model;
using VH.Resources;

namespace VH.DataAccess
{
    public class CustomerAction
    {
        private static readonly Object thisLock = new Object();
        private static readonly Object thisHALock = new Object();
        private static readonly Object thisEMLock = new Object();
        private static readonly Object thisRepairLock = new Object();
        private static CustomerDBContext _customerDBContext;
        private static string _connectionString;
        public static CustomerDBContext DBContextInstance
        {
            get
            {
                return _customerDBContext ?? (_customerDBContext = new CustomerDBContext(_connectionString));

                //if (_customerDBContext == null)
                //{
                //    _customerDBContext = new CustomerDBContext(_connectionString);
                //    if(_customerDBContext.Database.Connection.State == ConnectionState.Closed)
                //        _customerDBContext.Database.Connection.Open();

                //    return _customerDBContext;
                //}

                //if (_customerDBContext.Database.Connection.State == ConnectionState.Closed)
                //    _customerDBContext.Database.Connection.Open();

                //return _customerDBContext;

            }
        }

        #region ADD

        public static bool AddCustomer(string connectionString, Customer customer)
        {
            try
            {
                using (var context = new CustomerDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    context.Customers.Add(customer);
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

        public static bool DeleteCustomer(string connectionString, Customer customer)
        {
            try
            {
                using (var context = new CustomerDBContext(connectionString))
                {
                    context.Customers.Attach(new Customer() {ID = customer.ID});
                    context.Customers.Remove(customer);
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

        public static bool DeleteCustomers(string connectionString, IEnumerable<Customer> customers)
        {
            try
            {
                using (var context = new CustomerDBContext(connectionString))
                {
                    foreach (var customer in customers.Select(customer => new Customer() {ID = customer.ID}))
                    {
                        context.Customers.Attach(customer);
                        context.Customers.Remove(customer);
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

        public static bool DeleteCustomerPhones(string connectionString, IEnumerable<CustomerPhone> customerPhones)
        {
            try
            {
                using (var context = new CustomerDBContext(connectionString))
                {
                    foreach (var customerPhone in customerPhones.Select(customerPhone => new CustomerPhone() { ID = customerPhone.ID }))
                    {
                        context.Entry(customerPhone).State = EntityState.Deleted;
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

        public static bool DeleteCustomerAddresses(string connectionString, IEnumerable<CustomerAddress> customerAddresses)
        {
            try
            {
                using (var context = new CustomerDBContext(connectionString))
                {
                    foreach (var customerAddress in customerAddresses.Select(customerAddress => new CustomerAddress() { ID = customerAddress.ID }))
                    {
                        context.Entry(customerAddress).State = EntityState.Deleted;
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
        public IList<String> GetCustomerSearchList(string connectionString, string strSearchText)
        {
            try
            {
               // using (var context = new CustomerDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    return DBContextInstance.Customers.Where(x => x.FirstName.StartsWith(strSearchText) || x.LastName.StartsWith(strSearchText)).ToList().Select(x => x.FormattedName).ToList();
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public Dictionary<string, string> GetCustomerSearchDictionary(string connectionString, string strSearchText)
        {
            try
            {
               using (var context = new CustomerDBContext(connectionString))
                lock (thisLock)
                {
                    _connectionString = connectionString;
                    return
                        context.Customers.Where(
                            x => x.FirstName.StartsWith(strSearchText) || x.LastName.StartsWith(strSearchText))
                               .ToList()
                               .Select(x => new {CustomerID = x.ID, Name = x.FormattedName})
                               .ToDictionary(y => y.CustomerID.Value.ToString(CultureInfo.InvariantCulture), y => y.Name);
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public static CustomerCollection GetCustomerList(string connectionString)
        {
            try
            {
                lock (thisLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.Customers.Select(x => x).ToList();
                    return new CustomerCollection(items);
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }

        public static Customer GetCustomerByID(string connectionString, string customerId)
        {
            try
            {
               // var context = new CustomerDBContext(connectionString);
                _connectionString = connectionString;
                var id = Convert.ToInt32(customerId);
                return DBContextInstance.Customers.FirstOrDefault(x => x.ID == id);
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

        public static bool UpdateCustomer(string connectionString, Customer customer)
        {
            try
            {
                _connectionString = connectionString;
                var entry = DBContextInstance.Entry(customer);
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

        public static bool UpdateCustomers(string connectionString, IEnumerable<Customer> customers)
        {
            try
            {
                _connectionString = connectionString;

                foreach (var customer in customers)
                {
                    DBContextInstance.Customers.Add(customer);
                    var entry = DBContextInstance.Entry(customer);
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

        #region Customer Repair

        #region ADD
        public static bool AddCustomerRepair(string connectionString, CustomerRepair customerRepair)
        {
            try
            {
                using (var context = new CustomerDBContext(connectionString))
                {
                    var cloneObject = customerRepair.CreateAClone();
                    cloneObject.ApplyCurrentDateTime();
                    context.CustomerRepairs.Add(cloneObject);

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
            return false;
        }
        #endregion

        #region DELETE
        public static bool DeleteCustomerRepair(string connectionString, CustomerRepair customerRepair)
        {
            try
            {
                //using (var context = new CustomerDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    DBContextInstance.CustomerRepairs.Attach(new CustomerRepair() { ID = customerRepair.ID });
                    DBContextInstance.CustomerRepairs.Remove(customerRepair);
                    var result = DBContextInstance.SaveChanges();
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

        public static bool DeleteCustomerRepairs(string connectionString, IEnumerable<CustomerRepair> customerRepairs)
        {
            try
            {
              using (var context = new CustomerDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    foreach (var customerRepair in customerRepairs.Select(customerRepair => new CustomerRepair() { ID = customerRepair.ID }))
                    {
                        context.CustomerRepairs.Attach(customerRepair);
                        context.CustomerRepairs.Remove(customerRepair);
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

        #region UPDATE
        public static bool UpdateCustomerRepair(string connectionString, CustomerRepair customerRepair)
        {
            try
            {
                _connectionString = connectionString;
                
                var entry = DBContextInstance.Entry(customerRepair);
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

        public static bool UpdateCustomerRepairs(string connectionString, IEnumerable<CustomerRepair> customerRepairs)
        {
            try
            {
                _connectionString = connectionString;
                
                foreach (var customerRepair in customerRepairs)
                {
                    DBContextInstance.CustomerRepairs.Add(customerRepair);
                    var entry = DBContextInstance.Entry(customerRepair);
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

        #region GET
       

        public Dictionary<string, string> GetCustomerRepairSearchDictionary(string connectionString, string strSearchText)
        {
            try
            {
                using (var context = new CustomerDBContext(connectionString))
                {
                    return null;

                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public static CustomerRepairCollection GetCustomerRepairList(string connectionString)
        {
            try
            {
                _connectionString = connectionString;
                
                var items = DBContextInstance.CustomerRepairs.Select(x => x).ToList();
                return new CustomerRepairCollection(items);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }

        public static CustomerRepairCollection GetCustomerRepairList(string connectionString, Customer customer)
        {
            try
            {
                lock (thisRepairLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.CustomerRepairs.Where(x => x.Customer.ID == customer.ID).OrderByDescending(y => y.ReceivedDate).ToList();
                    return new CustomerRepairCollection(items);
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

        #endregion

        #region Customer Hearing Aid Order

        #region GET
        public static CustomerHearingAidOrderCollection GetCustomerHearingAidOrderList(string connectionString)
        {
            try
            {
                _connectionString = connectionString;

                var items = DBContextInstance.CustomerHearingAidOrders.Select(x => x).ToList();
                return new CustomerHearingAidOrderCollection(items);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }

        public static CustomerHearingAidOrderCollection GetCustomerHearingAidOrderList(string connectionString, Customer customer)
        {
            try
            {
                lock (thisLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.CustomerHearingAidOrders.Where(x => x.Customer.ID == customer.ID).OrderByDescending(y => y.OrderDate).ToList();
                    return new CustomerHearingAidOrderCollection(items);
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
        public static bool AddCustomerHearingAidOrder(string connectionString, CustomerHearingAidOrder customerHearingAidOrder)
        {
            try
            {
                using (var context = new CustomerDBContext(connectionString))
                {
                    //_connectionString = connectionString;
                    var cloneObject = customerHearingAidOrder.CreateAClone<CustomerHearingAidOrder>();
                    context.CustomerHearingAidOrders.Add(cloneObject);
                    var result = context.SaveChanges();

                    #region Adding Reminder
                    var reminder = new Reminder() { ReminderName = "Warrenty Reminder for 1st Year " + customerHearingAidOrder.Customer.FormattedName, ReminderDate = customerHearingAidOrder.OrderDate.AddMonths(11), ReminderType = ReminderType.Information, ReminderMessage = "Warrenty Reminder for 1st Year " + Environment.NewLine + customerHearingAidOrder.Customer.FormattedName + Environment.NewLine + "Order Date " + customerHearingAidOrder.OrderDate };
                    ReminderAction.AddReminder(connectionString, reminder);
                    var reminderConti = new Reminder() { ReminderName = "Warrenty Reminder for 2nd Year " + customerHearingAidOrder.Customer.FormattedName, ReminderDate = customerHearingAidOrder.OrderDate.AddMonths(23), ReminderType = ReminderType.Information, ReminderMessage = "Warrenty Reminder for 2nd Year " + Environment.NewLine + customerHearingAidOrder.Customer.FormattedName + Environment.NewLine + "Order Date " + customerHearingAidOrder.OrderDate };
                    ReminderAction.AddReminder(connectionString, reminderConti);
                    #endregion
                   
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
        public static bool DeleteCustomerHearingAidOrder(string connectionString, CustomerHearingAidOrder customerHearingAidOrder)
        {
            try
            {
                //using (var context = new CustomerDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    DBContextInstance.CustomerHearingAidOrders.Attach(new CustomerHearingAidOrder() { ID = customerHearingAidOrder.ID });
                    DBContextInstance.CustomerHearingAidOrders.Remove(customerHearingAidOrder);
                    var result = DBContextInstance.SaveChanges();
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

        public static bool DeleteCustomerHearingAidOrders(string connectionString, IEnumerable<CustomerHearingAidOrder> customerHearingAidOrders)
        {
            try
            {
               using (var context = new CustomerDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    foreach (var customerHearingAidOrder in customerHearingAidOrders.Select(customerHearingAidOrder => new CustomerHearingAidOrder() { ID = customerHearingAidOrder.ID }))
                    {
                        context.CustomerHearingAidOrders.Attach(customerHearingAidOrder);
                        context.CustomerHearingAidOrders.Remove(customerHearingAidOrder);
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

        #region UPDATE
        public static bool UpdateCustomerHearingAidOrder(string connectionString, CustomerHearingAidOrder customerHearingAidOrder)
        {
            try
            {
                _connectionString = connectionString;

                var entry = DBContextInstance.Entry(customerHearingAidOrder);
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

        public static bool UpdateCustomerHearingAidOrders(string connectionString, IEnumerable<CustomerHearingAidOrder> customerHearingAidOrders)
        {
            try
            {
                _connectionString = connectionString;

                foreach (var customerHearingAidOrder in customerHearingAidOrders)
                {
                    DBContextInstance.CustomerHearingAidOrders.Add(customerHearingAidOrder);
                    var entry = DBContextInstance.Entry(customerHearingAidOrder);
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
        #endregion

        #region Customer Ear Mold Order

        #region GET
        public static CustomerEarMoldOrderCollection GetCustomerEarMoldOrderList(string connectionString)
        {
            try
            {
                _connectionString = connectionString;

                var items = DBContextInstance.CustomerEarMoldOrders.Select(x => x).ToList();
                return new CustomerEarMoldOrderCollection(items);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }

        public static CustomerEarMoldOrderCollection GetCustomerEarMoldOrderList(string connectionString, Customer customer)
        {
            try
            {
                
                lock (thisEMLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.CustomerEarMoldOrders.Where(x => x.Customer.ID == customer.ID).OrderByDescending(y => y.OrderDate).ToList();
                    return new CustomerEarMoldOrderCollection(items);
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
        public static bool AddCustomerEarMoldOrder(string connectionString, CustomerEarMoldOrder customerEarMoldOrder)
        {
            try
            {
                using (var context = new CustomerDBContext(connectionString))
                {
                    var cloneObject = customerEarMoldOrder.CreateAClone<CustomerEarMoldOrder>();
                    context.CustomerEarMoldOrders.Add(cloneObject);
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
        public static bool DeleteCustomerEarMoldOrder(string connectionString, CustomerEarMoldOrder customerEarMoldOrder)
        {
            try
            {
                //using (var context = new CustomerDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    DBContextInstance.CustomerEarMoldOrders.Attach(new CustomerEarMoldOrder() { ID = customerEarMoldOrder.ID });
                    DBContextInstance.CustomerEarMoldOrders.Remove(customerEarMoldOrder);
                    var result = DBContextInstance.SaveChanges();
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

        public static bool DeleteCustomerEarMoldOrders(string connectionString, IEnumerable<CustomerEarMoldOrder> customerEarMoldOrders)
        {
            try
            {
                 using (var context = new CustomerDBContext(connectionString))
                {
                    foreach (var customerEarMoldOrder in customerEarMoldOrders.Select(customerEarMoldOrder => new CustomerEarMoldOrder() { ID = customerEarMoldOrder.ID }))
                    {
                        //CustomerEarMoldOrder temp = customerEarMoldOrder;
                        context.CustomerEarMoldOrders.Attach(customerEarMoldOrder);
                        context.CustomerEarMoldOrders.Remove(customerEarMoldOrder);
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

        #region UPDATE
        public static bool UpdateCustomerEarMoldOrder(string connectionString, CustomerEarMoldOrder customerEarMoldOrder)
        {
            try
            {
                _connectionString = connectionString;

                var entry = DBContextInstance.Entry(customerEarMoldOrder);
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

        public static bool UpdateCustomerEarMoldOrders(string connectionString, IEnumerable<CustomerEarMoldOrder> customerEarMoldOrders)
        {
            try
            {
                _connectionString = connectionString;

                foreach (var customerEarMoldOrder in customerEarMoldOrders)
                {
                    DBContextInstance.CustomerEarMoldOrders.Add(customerEarMoldOrder);
                    var entry = DBContextInstance.Entry(customerEarMoldOrder);
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
        #endregion

        #region Customer Warranty Informed

        #region ADD
        public static bool AddCustomerWarrantyInformed(string connectionString, CustomerWarrantyInformed customerWarrantyInformed)
        {
            try
            {
                using (var context = new CustomerDBContext(connectionString))
                {
                    var cloneObject = customerWarrantyInformed.CreateAClone();
                    cloneObject.ApplyCurrentDateTime();
                    context.CustomerWarrantyInformeds.Add(cloneObject);

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
            return false;
        }
        #endregion

        #region DELETE
        public static bool DeleteCustomerWarrantyInformed(string connectionString, CustomerWarrantyInformed customerWarrantyInformed)
        {
            try
            {
                //using (var context = new CustomerDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    DBContextInstance.CustomerWarrantyInformeds.Attach(new CustomerWarrantyInformed() { ID = customerWarrantyInformed.ID });
                    DBContextInstance.CustomerWarrantyInformeds.Remove(customerWarrantyInformed);
                    var result = DBContextInstance.SaveChanges();
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

        public static bool DeleteCustomerWarrantyInformeds(string connectionString, IEnumerable<CustomerWarrantyInformed> customerWarrantyInformeds)
        {
            try
            {
                using (var context = new CustomerDBContext(connectionString))
                {
                    _connectionString = connectionString;
                    foreach (var customerWarrantyInformed in customerWarrantyInformeds.Select(customerWarrantyInformed => new CustomerWarrantyInformed() { ID = customerWarrantyInformed.ID }))
                    {
                        context.CustomerWarrantyInformeds.Attach(customerWarrantyInformed);
                        context.CustomerWarrantyInformeds.Remove(customerWarrantyInformed);
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

        #region UPDATE
        public static bool UpdateCustomerWarrantyInformed(string connectionString, CustomerWarrantyInformed customerWarrantyInformed)
        {
            try
            {
                _connectionString = connectionString;

                var entry = DBContextInstance.Entry(customerWarrantyInformed);
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

        public static bool UpdateCustomerWarrantyInformeds(string connectionString, IEnumerable<CustomerWarrantyInformed> customerWarrantyInformeds)
        {
            try
            {
                _connectionString = connectionString;

                foreach (var customerWarrantyInformed in customerWarrantyInformeds)
                {
                    DBContextInstance.CustomerWarrantyInformeds.Add(customerWarrantyInformed);
                    var entry = DBContextInstance.Entry(customerWarrantyInformed);
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

        #region GET


        public static CustomerWarrantyInformedCollection GetCustomerWarrantyInformedList(string connectionString)
        {
            try
            {
                _connectionString = connectionString;

                var items = DBContextInstance.CustomerWarrantyInformeds.Select(x => x).ToList();
                return new CustomerWarrantyInformedCollection(items);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }

        }

        public static CustomerWarrantyInformedCollection GetCustomerWarrantyInformedList(string connectionString, Customer customer)
        {
            try
            {
                lock (thisRepairLock)
                {
                    _connectionString = connectionString;
                    var items = DBContextInstance.CustomerWarrantyInformeds.Where(x => x.Customer.ID == customer.ID).OrderByDescending(y => y.InformedDate).ToList();
                    return new CustomerWarrantyInformedCollection(items);
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

        #endregion
    }
}