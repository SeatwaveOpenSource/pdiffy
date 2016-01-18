using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Quarks;

namespace PDiffy.Features.ImageDifferences
{
	public class Approve
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
			protected override async Task HandleCore(Command message)
			{
				var comparison = Data.Biggy.ImageComparisons.Single(x => x.Name == message.Name && x.Page == message.Page && x.Site == message.Site);

				comparison.OriginalImageUrl = comparison.ComparisonImageUrl;
				comparison.OriginalImagePath = comparison.ComparisonImagePath;
				comparison.ComparisonImageUrl = null;
				comparison.ComparisonImagePath = null;
				comparison.DifferenceImagePath = null;
				comparison.HumanComparisonRequired = false;
				comparison.LastComparisonDate = SystemTime.Now;

				Data.Biggy.ImageComparisons.Update(comparison);
			}
		}
	}
}