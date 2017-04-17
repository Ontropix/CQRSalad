namespace CQRSalad.EventSourcing
{
    internal static class ICommandExtensions
    {
        internal static string GetAggregateId(this ICommand command)
        {
            var handler = CommandsPropertyCache.GetAggregateIdProp(command.GetType());
            return handler(command);
        }
    }
}