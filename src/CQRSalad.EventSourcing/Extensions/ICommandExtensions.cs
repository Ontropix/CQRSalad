namespace CQRSalad.EventSourcing
{
    internal static class ICommandExtensions
    {
        internal static string GetAggregateId(this ICommand command)
        {
            var handler = CommandsPropertyCache.GetPropertyHandler(command.GetType());
            return handler(command);
        }
    }
}