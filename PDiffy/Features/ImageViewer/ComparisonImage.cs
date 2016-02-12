using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Quarks.ImageExtensions;

namespace PDiffy.Features.ImageViewer
{
	public class ComparisonImage 
	{
		public class Query : IAsyncRequest<Result>
		{
			public string Name { get; set; }
			public string Page { get; set; }
			public string Site { get; set; }
		}

		public class Result
		{
			public byte[] ImageData { get; set; }
		}

		public class Handler : IAsyncRequestHandler<Query, Result>
		{
			public async Task<Result> Handle(Query message)
			{
				var comparison = Data.Biggy.ImageComparisons.Single(x => x.Name == message.Name && x.Page == message.Page && x.Site == message.Site);

				return new Result { ImageData = comparison.ComparisonImage.ToByteArray() };
			}
		}
	}
}