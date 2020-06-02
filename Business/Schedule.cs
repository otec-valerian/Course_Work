using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public enum WorkChange { Day, Night }
    public enum WorkDays { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }
    public enum Status { Engaged, Free }

    public struct WorkWeek
    {
        public WorkChange Change;
        public List<WorkDays> schedule;

        public WorkWeek(WorkChange type, List<WorkDays> week)
        {
            if (week.Count == 0 || week.Count > 7) { throw new ArgumentException("Invalid number of work-days"); }
            Change = type;
            schedule = new List<WorkDays>();
            for (int i = 0; i < week.Count; i++)
            {
                schedule.Add(week[i]);
            }
        }
    }
}
