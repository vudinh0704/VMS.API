using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace VMS.Api
{
    public class AppSettings
    {
        public static string JwtKey { get; private set; }

        public static string JwtIssuer { get; private set; }

        public static int JwtExpirationDays { get; private set; }

        /// <summary>
        /// Max Date Range per query
        /// </summary>
        public static int MaxDateRange { get; private set; }

        /// <summary>
        /// Run on startup
        /// </summary>
        public static void LoadSettings(IConfiguration configuration)
        {
            JwtKey = configuration.GetValue<string>("Jwt:Key");
            JwtIssuer = configuration.GetValue<string>("Jwt:Issuer");
            JwtExpirationDays = configuration.GetValue<int>("Jwt:ExpirationDays");

            MaxDateRange = configuration.GetValue<int>("VMS:MaxDateRange");
        }

        public static class VmsClaimTypes
        {
            public const string Name = ClaimTypes.Name;
            public const string AccountId = "AccountId";
            public const string FunctionIds = "FcIds";
        }

        public static class FunctionCodes
        {
            public const string Account_Full = "Account_Full"; // Full authority on account model
            public const string Account_Read_All = "Account_Read_All"; // Authority to view information of all accounts
            public const string Account_Read = "Account_Read"; // Authority to view information of your own account
            public const string Account_Modify_All = "Account_Modify_All"; // Authority to ban/unban a specific account
            public const string Account_Modify = "Account_Modify"; // Authority to change information of your own account
            public const string Account_Delete_All = "Account_Delete_All"; // Authority to delete all accounts
            public const string Account_Delete = "Account_Delete"; // Authority to delete your own account

            public const string Campaign_Full = "Campaign_Full"; // Full authority on campaign model
            public const string Campaign_Create = "Campaign_Create"; // Authority to create a new campaign
            public const string Campaign_Modify_All = "Campaign_Modify_All"; // Authority to approved/unapproved a specific campaign
            public const string Campaign_Modify = "Campaign_Modify"; // Authority to change information of your own campaign
            public const string Campaign_Delete_All = "Campaign_Delete_All"; // Authority to delete all campaigns
            public const string Campaign_Delete = "Campaign_Delete"; // Authority to delete your own campaign

            public const string Comment_Full = "Comment_Full"; // Full authority on comment model
            public const string Comment_Create = "Comment_Create"; // Authority to create a new comment
            public const string Comment_Modify = "Comment_Modify"; // Authority to change information of your own comment
            public const string Comment_Delete_All = "Comment_Delete_All"; // Authority to delete all comments
            public const string Comment_Delete = "Comment_Delete"; // Authority to delete your own comment

            public const string Function_Full = "Function_Full"; // Full authority on function model
            public const string Function_Create = "Function_Create"; // Authority to create a new function
            public const string Function_Read = "Function_Read"; // Authority to view information of all functions
            public const string Function_Modify = "Function_Modify"; // Authority to change information of all functions
            public const string Function_Delete = "Function_Delete"; // Authority to delete all functions

            public const string Group_Full = "Group_Full"; // Full authority on group model
            public const string Group_Create = "Group_Create"; // Authority to create a new group
            public const string Group_Read_All = "Group_Read_All"; // Authority to view information of all groups
            public const string Group_Read = "Group_Read"; // Authority to view information of your own group
            public const string Group_Modify_All = "Group_Modify_All"; // Authority to add/remove a function to a specific group
            public const string Group_Modify = "Group_Modify"; // Authority to change information of your own group
            public const string Group_Delete = "Group_Delete"; // Authority to delete all groups within permissible authority

            public const string Post_Full = "Post_Full"; // Full authority on post model
            public const string Post_Create = "Post_Create"; // Authority to create a new post
            public const string Post_Modify_All = "Post_Modify_All"; // Authority to approved/unapproved a specific post
            public const string Post_Modify = "Post_Modify"; // Authority to change information of your own post
            public const string Post_Delete_All = "Post_Delete_All"; // Authority to delete all posts
            public const string Post_Delete = "Post_Delete"; // Authority to delete your own post
        }
    }
}