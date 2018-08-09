using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simpleness.DataEntityFramework
{
    public static class EntityModelBuilderExtenions
    {
        private static IEnumerable<Type> LoadEntityConfigrations(this Assembly assembly, Type mappingInterface)
        {
            return assembly.GetTypes()
                .Where(b => !b.GetTypeInfo().IsAbstract //不是抽象类
                                                        //继承接口类型为泛型，并且与mappingInterface类型一致
                 && b.GetInterfaces().Any(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == mappingInterface));

        }

        public static void AddEntityConfigurationFromAssembly(this ModelBuilder modelBuilder, Assembly assembly)
        {
            var mappingTypes = assembly.LoadEntityConfigrations(typeof(IEntityTypeConfiguration<>));
            foreach (var item in mappingTypes)
            {
                //创建EntityConfiguration实例，并赋值             
                dynamic configurationInstance = Activator.CreateInstance(item);
                modelBuilder.ApplyConfiguration(configurationInstance);
            }
        }
    }
}
