using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using VH.ErrorLog;
using VH.Model;
using VH.Resources;

namespace VH.DataAccess
{
    public class ReminderAction
    {
        #region Add Reminder

        public static bool AddReminder(string connectionString, Reminder reminder)
        {
            try
            {
                using (var context = new ReminderDBContext(connectionString))
                {
                    context.Reminders.Add(reminder);
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
        public static bool DeleteReminder(string connectionString, Reminder reminder)
        {
            try
            {
                using (var context = new ReminderDBContext(connectionString))
                {
                    context.Reminders.Attach(new Reminder() { ID = reminder.ID });
                    context.Reminders.Remove(reminder);
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

        public static bool DeleteReminders(string connectionString, IEnumerable<Reminder> reminders)
        {
            try
            {
                using (var context = new ReminderDBContext(connectionString))
                {
                    foreach (var customer in reminders.Select(callReg => new Reminder() { ID = callReg.ID }))
                    {
                        context.Reminders.Attach(customer);
                        context.Reminders.Remove(customer);
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
        public static ReminderCollection GetReminders(string connectionString, DateTime? reminderDate)
        {
            try
            {
                using (var context = new ReminderDBContext(connectionString))
                {
                    var items = context.Reminders.Where(x => EntityFunctions.TruncateTime(x.ReminderDate) == EntityFunctions.TruncateTime(reminderDate.Value));
                    return new ReminderCollection(items.ToList());
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
                return null;
            }
        }

        public static ReminderCollection GetReminders(string connectionString)
        {
            try
            {
                using (var context = new ReminderDBContext(connectionString))
                {
                    return new ReminderCollection(context.Reminders.ToList());
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

        public static bool UpdateReminder(string connectionString, Reminder reminder)
        {
            try
            {
                using (var context = new ReminderDBContext(connectionString))
                {
                    context.Reminders.Add(reminder);
                    var entry = context.Entry(reminder);
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

        public static bool UpdateReminders(string connectionString, IEnumerable<Reminder> reminders)
        {
            try
            {
                using (var context = new ReminderDBContext(connectionString))
                {
                    foreach (var reminder in reminders)
                    {
                        context.Reminders.Add(reminder);
                        var entry = context.Entry(reminder);
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