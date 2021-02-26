using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VMS.Api.Models.Accounts
{
    [Serializable]
    public class AccountAuthenticateResponse
    {
        public string AccountId { get; set; }

        public string Token { get; set; }

        public List<string> FunctionCodes { get; set; }
    }
}