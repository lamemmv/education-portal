namespace EP.Data.Constants
{
    public static class SystemKeyword
    {
        public const string UpdateActivityLogType = "UpdateActivityLogType";

        public const string CreateEmailAccount = "CreateEmailAccount";
        public const string UpdateEmailAccount = "UpdateEmailAccount";
        public const string DeleteEmailAccount = "DeleteEmailAccount";

        public const string CreateUser = "CreateUser";
        public const string UpdateUser = "UpdateUser";
        public const string DeleteUser = "DeleteUser";
        public const string ResetUserPassword = "ResetUserPassword";

        public static string GetSystemKeyword(string controller, string action)
        {
            string systemKeyword;

            switch (action)
            {
                case "Post":
                    systemKeyword = "Create";
                    break;

                case "Put":
                    systemKeyword = "Update";
                    break;

                default:
                    systemKeyword = action;
                    break;
            }

            return systemKeyword + controller;
        }
    }
}
