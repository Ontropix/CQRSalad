using System;

namespace CQRSalad.EventSourcing.ValueInjection.Convensions
{
    internal class CommandToEventConvention : CaseInsensitiveConvention
    {
        protected override bool Match(ConventionInfo convention)
        {
            //Note: We don't map metadata, because ApplicationService does it.
            Type targetPropType = convention.TargetProp.Type;
            Type sourcePropType = convention.SourceProp.Type;

            if (sourcePropType != targetPropType)
            {
                return false;
            }

            return base.Match(convention);
        }
    }
}