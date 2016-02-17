using System;

namespace CQRSalad.EventSourcing.ValueInjection.Convensions
{
    internal class CaseInsensitiveConvention : ConventionInjection
    {
        protected override bool Match(ConventionInfo convention)
        {
            return String.Equals(convention.SourceProp.Name, convention.TargetProp.Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
