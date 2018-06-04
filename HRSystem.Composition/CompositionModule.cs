using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using HRSystem.ActiveDirectory;
using HRSystem.ActiveDirectory.Services.Connection;
using HRSystem.ActiveDirectory.Services.Connection.Interfaces;
using HRSystem.Common.Extensions;
using Module = Autofac.Module;

namespace HRSystem.Composition
{
    public sealed class CompositionModule : Module
    {
        private static readonly IEnumerable<string> AssemblyNames = new[]
        {
            $"{nameof(HRSystem)}.{nameof(ActiveDirectory)}",
            $"{nameof(HRSystem)}.{nameof(ActiveDirectory)}.{nameof(ActiveDirectory.Dal)}",
            $"{nameof(HRSystem)}.{nameof(Bll)}",
            $"{nameof(HRSystem)}.{nameof(Commands)}",
            $"{nameof(HRSystem)}.{nameof(Queries)}",
            $"{nameof(HRSystem)}.{nameof(Common)}",
            $"{nameof(HRSystem)}.{nameof(Core)}",
            $"{nameof(HRSystem)}.{nameof(Data)}",
            $"{nameof(HRSystem)}.{nameof(Domain)}",
            $"{nameof(HRSystem)}.{nameof(Infrastructure)}"
        };

        protected override void Load(ContainerBuilder builder)
        {
            var referencedAssemblies = GetAssemblies();
            RegisterServices(builder, referencedAssemblies);
            RegisterActiveDirectoryConnectionOpenStrategies(builder);
        }

        private static void RegisterServices(ContainerBuilder builder, Assembly[] assemblies)
        {
            var servicesPostfixes = new[] {"Service", "Provider", "Repository", "Job", "Command", "Query", "Handler", "Response", "Validator"};
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => servicesPostfixes.Any(postfix =>
                    t.Name.EndsWith(postfix, StringComparison.OrdinalIgnoreCase)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        private static void RegisterActiveDirectoryConnectionOpenStrategies(ContainerBuilder builder)
        {
            builder.RegisterType<WindowsIdentityUserActiveDirectoryConnectionOpenStrategy>()
                .Keyed<IActiveDirectoryConnectionOpenStrategy>(TechincalUserAuthenticationMode.WindowsIdentity);
            builder.RegisterType<SettingsUserActiveDirectoryConnectionOpenStrategy>()
                .Keyed<IActiveDirectoryConnectionOpenStrategy>(TechincalUserAuthenticationMode.Settings);

            builder.Register<Func<TechincalUserAuthenticationMode, IActiveDirectoryConnectionOpenStrategy>>(
                c =>
                {
                    var context = c.Resolve<IComponentContext>();

                    return userMode => context.ResolveKeyed<IActiveDirectoryConnectionOpenStrategy>(userMode);
                });
        }

        private static Assembly[] GetAssemblies()
        {
            return AssemblyNames.Select(Assembly.Load)
                .With(Assembly.GetExecutingAssembly())
                .ToArray();
        }
    }
}