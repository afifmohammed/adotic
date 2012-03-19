using System.Collections.Generic;
using System.Reflection;
using Ninject.Modules;

namespace Adotic
{
    public class Adoticness : NinjectModule
    {
        public override void Load()
        {
            Bind<IDictionary<string, IDictionary<string, PropertyInfo>>>()
                .ToMethod(c => new Dictionary<string, IDictionary<string, PropertyInfo>>())
                .InSingletonScope();
                
            Bind(typeof (IDataReaderMapper<>)).To(typeof (DataReaderMapper<>)).InSingletonScope();

            Bind<GetPropertyInfo>().ToSelf().InSingletonScope();

            Bind(typeof (IAdoSession<>)).To(typeof (AdoSession<>)).InRequestScope();
            Bind<IAdoSession>().To<AdoSession>().InRequestScope();
        }
    }
}