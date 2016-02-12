using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using PDiffy.Data.Stores;
using PDiffy.Infrastructure;

namespace PDiffy.Features.ImageDifferences
{
	public class Delete
	{
		public class Validator : AbstractValidator<Command>
		{
			public Validator()
			{
				RuleFor(x => x.Name).NotEmpty();
				RuleFor(x => x.Page).NotEmpty();
				RuleFor(x => x.Site).NotEmpty();
			}
		}

		public class Command : IAsyncRequest
		{
			public string Name { get; set; }
			public string Page { get; set; }
			public string Site { get; set; }
		}

		public class CommandHandler : AsyncRequestHandler<Command>
		{
			readonly IImageStore _imageStore;

			public CommandHandler(IImageStore imageStore)
			{
				_imageStore = imageStore;
			}

			protected override async Task HandleCore(Command message)
			{
				Data.Biggy.ImageComparisons.Remove(Data.Biggy.ImageComparisons.Single(x => x.Name == message.Name && x.Page == message.Page && x.Site == message.Site));

				_imageStore.Delete(message.Name, Environment.AllImageTypes);
			}
		}
	}
}