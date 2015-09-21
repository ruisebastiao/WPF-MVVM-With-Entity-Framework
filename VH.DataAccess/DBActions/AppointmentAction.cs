using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VH.ErrorLog;
using VH.Model;
using VH.Resources;

namespace VH.DataAccess
{
    public class AppointmentAction
    {
        #region Add Appointment
        public static bool AddAppointment(string connectionString, Appointment appointment)
        {
            try
            {
                using (var context = new AppointmentDBContext(connectionString))
                {
                    context.Appointments.Add(appointment);
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

        #region Delete Appointment
        public static bool DeleteAppointment(string connectionString, Appointment appointment)
        {
            try
            {
                using (var context = new AppointmentDBContext(connectionString))
                {
                    context.Appointments.Attach(new Appointment() {ID = appointment.ID});
                    context.Appointments.Remove(appointment);
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

        public static bool DeleteAppointments(string connectionString, IEnumerable<Appointment> appointments)
        {
            try
            {
                using (var context = new AppointmentDBContext(connectionString))
                {
                    foreach (var customer in appointments.Select(customer => new Appointment() {ID = customer.ID}))
                    {
                        context.Appointments.Attach(customer);
                        context.Appointments.Remove(customer);
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

        #region Get Appointments
         public static AppointmentCollection GetAppointments(string connectionString, DateTime appointmentDate)
        {
             try
             {
                 using (var context = new AppointmentDBContext(connectionString))
                 {
                     var items = context.Appointments.Where(x => x.AppointmentDate == appointmentDate);
                     return new AppointmentCollection(items.ToList());
                 }
             }
             catch (Exception exception)
             {
                 NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                     ExceptionResources.ExceptionOccuredLogDetail);
                 return null;
             }
        }

        public static AppointmentCollection GetAppointments(string connectionString, Customer customer)
        {
            try
            {
                using (var context = new AppointmentDBContext(connectionString))
                {
                    var items = context.Appointments.Where(x => x.Customer.ID == customer.ID);
                    return new AppointmentCollection(items.ToList());
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public static AppointmentCollection GetAppointments(string connectionString, Customer customer, DateTime appointmentDate)
        {
            try
            {
                using (var context = new AppointmentDBContext(connectionString))
                {
                    var items = context.Appointments.Where(x => x.Customer.ID == customer.ID && x.AppointmentDate == appointmentDate);
                    return new AppointmentCollection(items.ToList());
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

        #region Update Appintments
        public static bool UpdateAppointment(string connectionString, Appointment appointment)
        {
            try
            {
                using (var context = new AppointmentDBContext(connectionString))
                {
                    context.Appointments.Add(appointment);
                    var entry = context.Entry(appointment);
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

        public static bool UpdateAppointments(string connectionString, IEnumerable<Appointment> appointments)
        {
            try
            {
                using (var context = new AppointmentDBContext(connectionString))
                {
                    foreach (var appointment in appointments)
                    {
                        context.Appointments.Add(appointment);
                        var entry = context.Entry(appointment);
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