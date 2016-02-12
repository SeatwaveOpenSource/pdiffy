using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;
using MediatR;

namespace PDiffy.Features.ImageDifferences
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
		}

		public class QueryHandler : IAsyncRequestHandler<Query, IList<Model>>
		{
			public async Task<IList<Model>> Handle(Query message)
			{
				var pages = Data.Biggy.ImageComparisons.Select(imageComparison => new Model
				{
					Name = imageComparison.Name,
					Page = imageComparison.Page,
					Site = imageComparison.Site,

					LastComparisonDate = imageComparison.LastComparisonDate,
					ComparisonStillValid = imageComparison.ComparisonStillValid,
					HumanComparisonRequired = imageComparison.HumanComparisonRequired,
					ComparisonExists = !(string.IsNullOrWhiteSpace(imageComparison.ComparisonImageUrl) && string.IsNullOrWhiteSpace(imageComparison.ComparisonImagePath))
				}).ToList();

				return pages;
			}
		}
	}
}