using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using PDiffy.Data.Stores;
using Environment = PDiffy.Infrastructure.Environment;

namespace PDiffy.Features.History
{
	public class Index
	{
		public class Validator : AbstractValidator<Query>
		{
			public Validator()
			{
				RuleFor(x => x.Name).NotEmpty();
			}
		};

		public class Query : IAsyncRequest<Result>
		{
			public string Name { get; set; }
			public string Page { get; set; }
			public string Site { get; set; }
		}

		public class Result
		{
			public IEnumerable<HistoricalImage> DifferenceImages { get; set; }
			public string Name { get; set; }
			public string Page { get; set; }
			public string Site { get; set; }
			public DateTime? LastComparisonDate { get; set; }
			public bool ComparisonStillValid { get; set; }
			public bool HumanComparisonRequired { get; set; }
		}

		public class Handler : IAsyncRequestHandler<Query, Result>
		{
			readonly IImageStore _imageStore;

			public Handler(IImageStore imageStore)
			{
				_imageStore = imageStore;
			}

			public async Task<Result> Handle(Query message)
			{
				var comparison = Data.Biggy.ImageComparisons.Single(x => x.Name == message.Name && x.Page == message.Page && x.Site == message.Site);
				
				var result = new Result
				{
					Name = comparison.Name,
					Page = comparison.Page,
					Site = comparison.Site,
					LastComparisonDate = comparison.LastComparisonDate,
					ComparisonStillValid = comparison.ComparisonStillValid,
					HumanComparisonRequired = comparison.HumanComparisonRequired,
					DifferenceImages = _imageStore.Get(message.Name, Environment.DifferenceId).Select(x => new HistoricalImage(x))
				};

				return result;
			}
		}
	}
}