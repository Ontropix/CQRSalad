namespace CQRSalad.EventSourcing.ValueInjection
{
    internal static class ValueInjecter
    {
        public static object Inject<TConvention>(object target, object source)
            where TConvention : ConventionInjection, new()
        {
            var convention = new TConvention();
            convention.Inject(source, target);
            return target;
        }

        public static object Inject<TConvention>(object target, params object[] sources)
            where TConvention : ConventionInjection, new()
        {
            var convention = new TConvention();
            foreach (object source in sources)
            {
                convention.Inject(source, target);
            }

            return target;
        }

        public static object Inject(object target, ConventionInjection convention, params object[] sources)
        {
            foreach (object source in sources)
            {
                convention.Inject(source, target);
            }
            return target;
        }
    }
}