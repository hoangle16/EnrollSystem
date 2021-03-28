using EnrollSystem.Models.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Helpers
{
    public class CheckSchedule
    {
        public static bool IsConflict(ScheduleModel current, ScheduleModel db)
        {
            bool isConflict = false;
            //var listCurrentDay = ListDate(current);
            //var listDbDay = ListDate(db);
            string[] currentDayOfWeek = current.Schedule.Split(",");
            string[] DbDayOfWeek = db.Schedule.Split(",");
            //check overlap starttime, endtime
            if (current.StartTime <= db.EndTime && db.StartTime <= current.EndTime)
            {
                //check overlap schedule
                foreach (var day in currentDayOfWeek)
                {
                    if (Array.IndexOf(DbDayOfWeek, day) > -1)
                    {
                        //check overlap startday, endday
                        if (DateTime.Compare(current.StartDay.Date, db.EndDay.Date) <= 0 && DateTime.Compare(db.StartDay.Date, current.EndDay.Date) <= 0)
                        {
                            isConflict = true;
                            break;
                        }
                    }
                }
            }
            return isConflict;
        }
        public static List<DateTime> ListDate(ScheduleModel schedule)
        {
            string[] dayOfWeek = schedule.Schedule.Split(",");
            List<DateTime> listDay = new List<DateTime>();
            var startDay = schedule.StartDay;
            while(DateTime.Compare(startDay, schedule.EndDay) <= 0)
            {
                foreach(var DoW in dayOfWeek)
                {
                    if ((int)startDay.DayOfWeek == int.Parse(DoW))
                    {
                        listDay.Add(startDay);
                        break;
                    }
                }
                startDay = startDay.AddDays(1);
            }
            return listDay;
        }
        public static bool ContainsAny(string str, params string[] values)
        {
            if (!string.IsNullOrEmpty(str) || values.Length > 0)
            {
                foreach (string value in values)
                {
                    if (str.Contains(value))
                        return true;
                }
            }

            return false;
        }
    }
}
