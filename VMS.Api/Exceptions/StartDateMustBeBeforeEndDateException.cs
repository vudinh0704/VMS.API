namespace VMS.Api.Exceptions
{
    public class StartDateMustBeBeforeEndDateException : BaseException
    {
        public StartDateMustBeBeforeEndDateException() : base("startDate_must_be_before_endDate", "startDate must be before endDate")
        {

        }
    }
}