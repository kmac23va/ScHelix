using Sitecore.SecurityModel;
using Sitecore.Shell.Framework.Commands;

namespace ScHelix.Foundation.HelixCore.Commands {
    public class UnlockItem : Command {
        public override void Execute(CommandContext context) {
            if (context.Items.Length != 1) {
                return;
            }

            Sitecore.Data.Items.Item obj = context.Items[0];

            if (!obj.Locking.IsLocked() || !obj.Access.CanWrite()) {
                return;
            }

            using (new SecurityDisabler()) {
                obj.Locking.Unlock();
            }
        }

        public override CommandState QueryState(CommandContext context) {
            if (context.Items.Length != 1) {
                return (CommandState) 3;
            }

            Sitecore.Data.Items.Item obj = context.Items[0];

            return !obj.Locking.IsLocked() || !obj.Access.CanWrite() ? (CommandState) 3 : base.QueryState(context);
        }
    }
}
