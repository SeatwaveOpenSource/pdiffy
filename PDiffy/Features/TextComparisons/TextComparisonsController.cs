using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using MediatR;
using PDiffy.Features.Shared;

namespace PDiffy.Features.TextComparisons
{
	public class TextComparisonsController : ApiController
	{
		readonly IMediator _mediator;

		public TextComparisonsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IHttpActionResult> PostTextComparison()
		{
			string requestBody;
			using (var requestStream = await Request.Content.ReadAsStreamAsync())
			using (var streamReader = new StreamReader(requestStream, Encoding.Unicode, true))
				requestBody = streamReader.ReadToEnd();

			var body = new JavaScriptSerializer().Deserialize<ApiRequestBody>(requestBody);

			if (string.IsNullOrEmpty(body.Name))
				return BadRequest();

			await _mediator.SendAsync(
				new Text.Command
				{
					Name = body.Name,
					Page = body.Page,
					Site = body.Site,
					Text = Encoding.Unicode.GetString(body.Content)
				});

			return Ok();
		}
	}
}
