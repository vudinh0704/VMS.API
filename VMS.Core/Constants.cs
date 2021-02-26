using System;
using System.Data.SqlTypes;

namespace VMS.Core
{
    public static class Constants
    {
        public static readonly DateTime MinDate = SqlDateTime.MinValue.Value;

        /// <summary>
        /// CurrentYear - MaxAge
        /// </summary>
        public static DateTime MinBirthDate => new DateTime(DateTime.Now.Year - MaxAge, 1, 1);

        /// <summary>
        /// 18
        /// </summary>
        public const int MinAge = 18;

        /// <summary>
        /// 99
        /// </summary>
        public const int MaxAge = 99;
    }
}