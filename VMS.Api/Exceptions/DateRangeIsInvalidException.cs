namespace VMS.Api.Exceptions
{
    public class DateRangeIsInvalidException : BaseException
    {
        public DateRangeIsInvalidException() : base("dateRange_is_invalid", $"endDate must be { AppSettings.MaxDateRange } days after startDate")
        {

        }
    }
}