using System;
using VMS.Api.Exceptions;

namespace VMS.Api.Models
{
    public abstract class BaseGetModelWithDate
    {
        public DateTime? StartDate { get; set; } = null;

        public DateTime? EndDate { get; set; } = null;

        public void Validate()
        {
            if (!StartDate.HasValue)
            {
                throw new IsRequiredException("startDate");
            }

            if (!EndDate.HasValue)
            {
                throw new IsRequiredException("endDate");
            }

            if (StartDate.Value > EndDate.Value)
            {
                throw new StartDateMustBeBeforeEndDateException();
            }

            TimeSpan dateRange = EndDate.Value - StartDate.Value;

            if (dateRange.TotalDays > AppSettings.MaxDateRange)
            {
                throw new DateRangeIsInvalidException();
            }
        }
    }
}