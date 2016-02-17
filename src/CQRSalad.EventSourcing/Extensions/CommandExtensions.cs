namespace CQRSalad.EventSourcing
{
    public static class CommandExtensions
    {
        //internal static void ValidateMetadata(this Command command)
        //{
        //    if (command.Meta == null)
        //    {
        //        throw new InvalidCommandException("CommandMetadata is null.", command);
        //    }

        //    if (IsNullOrWhiteSpace(command.Meta.CommandId))
        //    {
        //        throw new InvalidCommandException("CommandId is null or empty.", command);
        //    }

        //    if (IsNullOrWhiteSpace(command.Meta.SenderId))
        //    {
        //        throw new InvalidCommandException("SenderId is null or empty.", command);
        //    }

        //    if (command.Meta.Timestamp > DateTime.UtcNow)
        //    {
        //        throw new InvalidCommandException("Timestamp is greater than UtcNow.", command);
        //    }
        //}

        
    }
}