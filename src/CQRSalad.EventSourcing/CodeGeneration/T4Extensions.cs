using System;
using System.Collections.Generic;
using CQRSalad.EventSourcing.CodeGeneration;

namespace CQRSalad.EventSourcing
{
    public static class T4Extensions
    {
        public static string Compile(this ApplicationServiceTemplate template, Type aggregateType)
        {
            template.Session = new Dictionary<string, object>() { { "AggregateType", aggregateType } };
            template.Initialize();

            return template.TransformText().Replace("\n", "").Replace("\r", "").Replace("\t", "");
        }
    }
}