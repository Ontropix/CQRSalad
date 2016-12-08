using System.Reflection;

namespace CQRSalad.EventSourcing.ValueInjection
{
    internal abstract class ConventionInjection
    {
        protected abstract bool Match(ConventionInfo c);

        protected virtual object SetValue(ConventionInfo convention)
        {
            return convention.SourceProp.Value;
        }
        
        public void Inject(object source, object target)
        {
            PropertyInfo[] sourceProps = PropertyInfoCache.GetProperties(source);
            PropertyInfo[] targetProps = PropertyInfoCache.GetProperties(target);

            var conventionInfo = new ConventionInfo
            {
                Source =
                {
                    Type = source.GetType(),
                    Value = source
                },
                Target =
                {
                    Type = target.GetType(),
                    Value = target
                }
            };

            foreach (PropertyInfo sourceProp in sourceProps)
            {
                conventionInfo.SourceProp.Name = sourceProp.Name;
                conventionInfo.SourceProp.Value = sourceProp.GetValue(source);
                conventionInfo.SourceProp.Type = sourceProp.PropertyType;

                foreach (PropertyInfo targetProp in targetProps)
                {
                    conventionInfo.TargetProp.Name = targetProp.Name;
                    conventionInfo.TargetProp.Value = targetProp.GetValue(target);
                    conventionInfo.TargetProp.Type = targetProp.PropertyType;
                    if (Match(conventionInfo))
                    {
                        targetProp.SetValue(target, SetValue(conventionInfo));
                    }
                }
            }
        }
    }
}