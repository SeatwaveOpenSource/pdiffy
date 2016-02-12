using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FluentValidation;
using MediatR;
using PDiffy.Features.Shared.Libraries;
using Quarks;

namespace PDiffy.Features.TextComparisons
{
	public class Text
	{
		public class Validator : AbstractValidator<Command>
		{
			public Validator()
			{
				RuleFor(x => x.Name).NotEmpty();
				RuleFor(x => x.Page).NotEmpty();
				RuleFor(x => x.Site).NotEmpty();
				RuleFor(x => x.Text).NotNull();
			}
		}

		public class Command : IAsyncRequest
		{
			public string Name { get; set; }
			public string Text { get; set; }
			public string Page { get; set; }
			public string Site { get; set; }
		}

		public class CommandHandler : AsyncRequestHandler<Command>
		{
			protected override async Task HandleCore(Command message)
			{
				var textComparison = Data.Biggy.TextComparisons.SingleOrDefault(x => x.Name == message.Name && x.Page == message.Page && x.Site == message.Site);

				if (textComparison == null)
				{
					Data.Biggy.TextComparisons.Add(
						new Data.TextComparison
						{
							Name = message.Name,
							Page = message.Page,
							Site = message.Site,

							OriginalText = message.Text,
						});
				}
				else
				{
					textComparison.ComparisonText = message.Text;

					if (textComparison.HumanComparisonRequired == false)
					{
						await Task.Run(() =>
						{
							var differences = new diff_match_patch().diff_main(textComparison.OriginalText, textComparison.ComparisonText);

							if (differences.Any(d => d.operation != Operation.Equal))
							{
								textComparison.HumanComparisonRequired = true;
								textComparison.DifferenceText = HttpUtility.HtmlEncode(new diff_match_patch().diff_prettyHtml(differences));
							}

							textComparison.LastComparisonDate = SystemTime.Now;
						});

						Data.Biggy.TextComparisons.Update(textComparison);
					}
				}
			}
		}
	}
}