using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace PDiffy.Features.TextDifferences
{
	public class Index
	{
		public class Query : IAsyncRequest<IList<Model>> { }

		public class Model
		{
			public string Name { get; set; }
			public string Page { get; set; }
			public string Site { get; set; }

			public DateTime? LastComparisonDate { get; set; }
			public bool HumanComparisonRequired { get; set; }

			public bool ComparisonExists { get; set; }
			public bool ComparisonStillValid { get; set; }

			public string OriginalText { get; set; }
			public string ComparisonText { get; set; }
			public string DifferenceText { get; set; }
		}

		public class QueryHandler : IAsyncRequestHandler<Query, IList<Model>>
		{
			public async Task<IList<Model>> Handle(Query message)
			{
				var comparisons = Data.Biggy.TextComparisons.Select(comparison => new Model
				{
					Name = comparison.Name,
					Page = comparison.Page,
					Site = comparison.Site,

					LastComparisonDate = comparison.LastComparisonDate,
					ComparisonStillValid = comparison.ComparisonStillValid,
					HumanComparisonRequired = comparison.HumanComparisonRequired,
					ComparisonExists = string.IsNullOrWhiteSpace(comparison.DifferenceText) == false,

					OriginalText = comparison.OriginalText,
					ComparisonText = comparison.ComparisonText,
					DifferenceText = comparison.DifferenceText,
				}).ToList();

				return comparisons;
			}
		}
	}
}