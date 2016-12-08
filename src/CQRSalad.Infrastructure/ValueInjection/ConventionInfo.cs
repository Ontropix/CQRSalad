using System;

namespace CQRSalad.EventSourcing.ValueInjection
{
    internal class ConventionInfo
    {
        public ConventionInfo()
        {
            Source = new TypeInfo();
            Target = new TypeInfo();
            SourceProp = new PropInfo();
            TargetProp = new PropInfo();
        }

        public TypeInfo Source { get; set; }
        public TypeInfo Target { get; set; }

        public PropInfo SourceProp { get; set; }
        public PropInfo TargetProp { get; set; }

        public class PropInfo
        {
            public string Name { get; set; }
            public Type Type { get; set; }
            public object Value { get; set; }
        }

        public class TypeInfo
        {
            public Type Type { get; set; }
            public object Value { get; set; }
        }
    }
}