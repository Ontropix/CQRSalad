using CQRSalad.EventSourcing.ValueInjection;

namespace CQRSalad.Infrastructure.ValueInjection
{
    internal static class ValueInjecter
    {
        public static void Inject<TConvention>(object target, object source)
            where TConvention : ConventionInjection, new()
        {
            var convention = new TConvention();
            convention.Inject(source, target);
        }
    }
}