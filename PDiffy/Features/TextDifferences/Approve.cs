using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Quarks;

namespace PDiffy.Features.TextDifferences
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
				var textComparison = Data.Biggy.TextComparisons.Single(x => x.Name == message.Name && x.Page == message.Page && x.Site == message.Site);

				textComparison.OriginalText = textComparison.ComparisonText;
				textComparison.ComparisonText = string.Empty;
				textComparison.DifferenceText = string.Empty;
				textComparison.HumanComparisonRequired = false;
				textComparison.LastComparisonDate = SystemTime.Now;

				Data.Biggy.TextComparisons.Update(textComparison);
			}
		}
	}
}