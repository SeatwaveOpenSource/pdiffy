using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using PDiffy.Features.Shared;
using PDiffy.Features.Shared.Libraries;
using Quarks;

namespace PDiffy.Features.ImageComparisons
{
	public class WebImage
	{
		public class Validator : AbstractValidator<Image.Command>
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
			public string Url { get; set; }
		}

		public class CommandHandler : AsyncRequestHandler<Command>
		{
			protected override async Task HandleCore(Command message)
			{
				var page = Data.Biggy.ImageComparisons.SingleOrDefault(x => x.Name == message.Name);

				if (page == null)
					Data.Biggy.ImageComparisons.Add(new Data.ImageComparison { Name = message.Name, OriginalImageUrl = message.Url });
				else if (!page.HumanComparisonRequired)
				{
					page.ComparisonImageUrl = message.Url;
					await Task.Run(() =>
					{
						var equal = false;
						try
						{
							equal = new ImageDiffTool().Compare(page.OriginalImage, page.ComparisonImage);
						}
						catch { /*assume inequality on comparison exceptions*/ }

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