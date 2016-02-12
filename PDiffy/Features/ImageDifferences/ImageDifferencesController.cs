using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;

namespace PDiffy.Features.ImageDifferences
{
	public partial class ImageDifferencesController : Controller
	{
		readonly IMediator _mediator;

		public ImageDifferencesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		public virtual async Task<ActionResult> Index(Index.Query query)
		{
			var model = await _mediator.SendAsync(query);

			return View(MVC.ImageDifferences.Views.Index, model);
		}

		public virtual async Task<ActionResult> Approve(Approve.Command model)
		{
			await _mediator.SendAsync(model);

			return RedirectToAction(MVC.ImageDifferences.Index());
		}

		public virtual async Task<ActionResult> Delete(Delete.Command model)
		{
			await _mediator.SendAsync(model);

			return RedirectToAction(MVC.ImageDifferences.Index());
		}
	}
}