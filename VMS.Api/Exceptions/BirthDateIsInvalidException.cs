using VMS.Core;
using System;

namespace VMS.Api.Exceptions
{
    public class BirthDateIsInvalidException : BaseException
    {
        public BirthDateIsInvalidException() : base("birthDate_is_invalid", $"birthdate must be from { Constants.MinBirthDate.Year } to { DateTime.Today.Year - Constants.MinAge }")
        {

        }
    }
}