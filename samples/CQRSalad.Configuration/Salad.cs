using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSalad.Configuration
{
    //StructureMap.Container()
    //            .UseGuidIdGenerator()
    //            .UseInMemoryKutcha()
    //            .RegisterKutchaRoots()
    //            .UseAssemblyRuleScanning()
    //            .UseAsyncDispatcherSingleton()
    //            .UseInMemoryBuses()
    //            .UseInMemoryEventStore()
    //            .UseCommandProcessorSingleton()
    //            .UseFluentMessageValidator();

    public class Salad
    {

    }

    public static class SaladExtensions
    {
        public static Salad UseDispatcher(this Salad salad)
        {
            return salad;
        }
    }
}
