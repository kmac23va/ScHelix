using System;
using System.Web.Security;

public static class Program {
    public static void Main(string[] args) {
        string userName;
        string password;
        bool unlockUser;

        if (args.Length < 1) {
            Console.WriteLine("Please enter a username (IE sitecore\\admin:");
            userName = Console.ReadLine();
        } else {
            userName = args[0];
        }

        if (args.Length < 2) {
            Console.WriteLine("Please enter a password:");
            password = Console.ReadLine();
        } else {
            password = args[1];
        }

        if (args.Length < 3) {
            Console.WriteLine("Should the user be unlocked? [y,n] (default y)");
            string unlockUserResponse = Console.ReadLine();
            unlockUser = string.IsNullOrEmpty(unlockUserResponse) || unlockUserResponse.Equals("y", StringComparison.InvariantCultureIgnoreCase) || unlockUserResponse.Equals("yes", StringComparison.InvariantCultureIgnoreCase);
        } else {
            unlockUser = args[2].Equals("true", StringComparison.InvariantCulture);
        }

        MembershipUser user = Membership.GetUser(userName, false);

        if (user == null) {
            Console.WriteLine("User not found");
            return;
        }

        if (unlockUser) {
            bool isUnlocked = user.UnlockUser();
            Console.WriteLine(isUnlocked ? "User has been unlocked" : "User has not been unlocked");
        }

        if (string.IsNullOrEmpty(password)) {
            Console.WriteLine("Password cannot be empty, please try again");
            return;
        }

        string oldPassword = user.ResetPassword();
        bool passwordHasBeenChanged = user.ChangePassword(oldPassword, password);

        if (passwordHasBeenChanged) {
            Console.WriteLine("Password has been changed");
        }
    }
}
