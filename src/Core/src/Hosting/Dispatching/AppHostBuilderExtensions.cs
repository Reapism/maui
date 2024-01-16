using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Dispatching;

namespace Microsoft.Maui.Hosting
{
	public static partial class AppHostBuilderExtensions
	{
		public static MauiAppBuilder ConfigureDispatching(this MauiAppBuilder builder)
		{
			// register the DispatcherProvider as a singleton for the entire app
			builder.Services.TryAddSingleton<IDispatcherProvider>(svc =>
				// the DispatcherProvider might have already been initialized, so ensure that we are grabbing the
				// Current and putting it in the DI container.
				DispatcherProvider.Current);

			// register the Dispatcher as a singleton for the entire app as it may be different to secondary windows
			builder.Services.TryAddSingleton(GetDispatcherFromDispatcherProvider);
			// register the initializer so we can get the dispatcher in the application thread
			builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IMauiInitializeService, DispatcherInitializer>());

			// register the Dispatcher as a scoped service as each window may have different dispatchers
			builder.Services.TryAddScoped(GetDispatcherFromDispatcherProvider);
			// register the initializer so we can get the dispatcher in the window thread
			builder.Services.TryAddEnumerable(ServiceDescriptor.Scoped<IMauiInitializeScopedService, DispatcherInitializer>());

			return builder;

			static IDispatcher GetDispatcherFromDispatcherProvider(IServiceProvider svc)
			{
				var provider = svc.GetRequiredService<IDispatcherProvider>();
				if (DispatcherProvider.SetCurrent(provider))
					svc.CreateLogger<Dispatcher>()?.LogWarning("Replaced an existing DispatcherProvider with one from the service provider.");

				return Dispatcher.GetForCurrentThread()!;
			}
		}

		class DispatcherInitializer : IMauiInitializeService, IMauiInitializeScopedService
		{
			public void Initialize(IServiceProvider services)
			{
				_ = services.GetRequiredService<IDispatcher>();
			}
		}
	}
}