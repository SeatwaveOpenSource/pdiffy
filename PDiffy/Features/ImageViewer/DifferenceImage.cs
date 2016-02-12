using System.Linq;
using System.Threading.Tasks;
using MediatR;
using PDiffy.Data.Stores;
using PDiffy.Infrastructure;
using Quarks.ImageExtensions;

namespace PDiffy.Features.ImageViewer
{
	public class DifferenceImage
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
			readonly IImageStore _imageStore;

			public Handler(IImageStore imageStore)
			{
				_imageStore = imageStore;
			}

			public async Task<Result> Handle(Query message)
			{
				var imageComparison = Data.Biggy.ImageComparisons.Single(x => x.Name == message.Name && x.Page == message.Page && x.Site == message.Site);

				if (string.IsNullOrWhiteSpace(imageComparison.DifferenceImagePath))
				{
					imageComparison.DifferenceImagePath = _imageStore.Save(imageComparison.DifferenceImage, imageComparison.Name, Environment.DifferenceId);
					Data.Biggy.ImageComparisons.Update(imageComparison);
				}

				return new Result { ImageData = imageComparison.DifferenceImage.ToByteArray()};
			}
		}
	}
}