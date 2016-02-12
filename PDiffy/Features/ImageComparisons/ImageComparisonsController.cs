using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using MediatR;
using PDiffy.Features.Shared;

namespace PDiffy.Features.ImageComparisons
{
	public class ImageComparisonsController : ApiController
	{
		readonly IMediator _mediator;

		public ImageComparisonsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IHttpActionResult> PostImageComparison()
		{
			string requestBody;
			using (var requestStream = await Request.Content.ReadAsStreamAsync())
			using (var streamReader = new StreamReader(requestStream, Encoding.Unicode, true))
				requestBody = streamReader.ReadToEnd();

			var body = new JavaScriptSerializer().Deserialize<ApiRequestBody>(requestBody);

			if (string.IsNullOrEmpty(body.Name))
				return BadRequest();

			Bitmap image;
			using (var ms = new MemoryStream(body.Content))
				image = new Bitmap(ms);
			
			await _mediator.SendAsync(
				new Image.Command
				{
					Name = body.Name,
					Page = body.Page,
					Site = body.Site,
					Image = image
				});

			return Ok();
		}
	}
}
