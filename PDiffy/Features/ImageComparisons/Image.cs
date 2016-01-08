using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using PDiffy.Data.Stores;
using PDiffy.Features.Shared;
using PDiffy.Features.Shared.Libraries;
using Quarks;
using Environment = PDiffy.Infrastructure.Environment;

namespace PDiffy.Features.ImageComparisons
{
	public class Image
	{
		public class Validator : AbstractValidator<Command>
		{
			public Validator()
			{
				RuleFor(x => x.Name).NotEmpty();
				RuleFor(x => x.Image).NotNull();
			}
		}

		public class Command : IAsyncRequest
		{
			public string Name { get; set; }
			public Bitmap Image { get; set; }
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
				var page = Data.Biggy.ImageComparisons.SingleOrDefault(x => x.Name == message.Name);

				if (page == null)
				{
					Data.Biggy.ImageComparisons.Add(
						new Data.ImageComparison
						{
							Name = message.Name,
							OriginalImagePath = _imageStore.Save(message.Image, message.Name, Environment.OriginalId)
						});
				}
				else if (page.HumanComparisonRequired == false)
				{
					page.ComparisonImagePath = _imageStore.Save(message.Image, message.Name, Environment.ComparisonId);
					await Task.Run(() =>
					{
						var equal = new ImageDiffTool().Compare(page.OriginalImage, page.ComparisonImage);

						if (!equal)
							page.HumanComparisonRequired = true;
						else
						{
							page.ComparisonImageUrl = null;
							page.ComparisonImagePath = null;
						}

						page.LastComparisonDate = SystemTime.Now;
					});

					Data.Biggy.ImageComparisons.Update(page);
				}
			}
		}
	}
}