namespace VMS.Api.Exceptions
{
    public class ExecutionDateRangeMustBeAfterRegistrationDateRangeException : BaseException
    {
        public ExecutionDateRangeMustBeAfterRegistrationDateRangeException() : base("executionDateRange_must_be_after_registrationDateRange", "executionDateRange must be 3 days after registrationDateRange")
        {

        }
    }
}