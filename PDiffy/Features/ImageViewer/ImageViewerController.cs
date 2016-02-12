using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using MediatR;

namespace PDiffy.Features.ImageViewer
{
	public partial class ImageViewerController : Controller
	{
		readonly IMediator _mediator;

		public ImageViewerController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[OutputCache(Duration = 1800, VaryByParam = "name; lastComparisonDate")]
		public async Task<FileContentResult> DifferenceImage([FromUri] DifferenceImage.Query query)
		{
			var model = await _mediator.SendAsync(query);
			return File(model.ImageData, "image/png");
		}

		[OutputCache(Duration = 1800, VaryByParam = "name; lastComparisonDate")]
		public async Task<FileContentResult> ComparisonImage([FromUri] ComparisonImage.Query query)
		{
			var model = await _mediator.SendAsync(query);
			return File(model.ImageData, "image/png");
		}

		[OutputCache(Duration = 1800, VaryByParam = "name; lastComparisonDate")]
		public async Task<FileContentResult> OriginalImage([FromUri] OriginalImage.Query query)
		{
			var model = await _mediator.SendAsync(query);
			return File(model.ImageData, "image/png");
		}
	}
}