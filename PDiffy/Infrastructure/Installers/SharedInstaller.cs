using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using PDiffy.Data.Stores;
using PDiffy.Features.Shared;

namespace PDiffy.Infrastructure.Installers
{
	public class SharedInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Component.For<IImageStore>().ImplementedBy<FileImageStore>().LifestyleTransient());
		}
	}
}