using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using HRSystem.Common.Extensions;
using OneInc.ADEditor.ActiveDirectory;
using OneInc.ADEditor.ActiveDirectory.Services.Connection;
using OneInc.ADEditor.ActiveDirectory.Services.Connection.Interfaces;
using Module = Autofac.Module;

namespace HRSystem.Composition
{
    public sealed class CompositionModule : Module
    {
        private static readonly IEnumerable<string> AssemblyNames = new[]
        {
            "OneInc.ADEditor.ActiveDirectory",
            "OneInc.ADEditor.Dal",
            $"{nameof(HRSystem)}.Bll",
            $"{nameof(HRSystem)}.Commands",
            $"{nameof(HRSystem)}.Queries",
            $"{nameof(HRSystem)}.Common",
            $"{nameof(HRSystem)}.Core",
            $"{nameof(HRSystem)}.Data",
            $"{nameof(HRSystem)}.Domain",
            $"{nameof(HRSystem)}.Infrastructure"
        };

        protected override void Load(ContainerBuilder builder)
        {
            var referencedAssemblies = GetAssemblies();
            RegisterServices(builder, referencedAssemblies);
            RegisterActiveDirectoryConnectionOpenStrategies(builder);
        }

        public static void RegisterServices(ContainerBuilder builder, Assembly[] assemblies)
        {
            var servicesPostfixes = new[] {"Service", "Provider", "Repository", "Job", "Command", "Query", "Handler", "Response", "Validator"};
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => servicesPostfixes.Any(postfix =>
                    t.Name.EndsWith(postfix, StringComparison.OrdinalIgnoreCase)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        public static void RegisterActiveDirectoryConnectionOpenStrategies(ContainerBuilder builder)
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