using System.Threading.Tasks;
using System.Web.Mvc;
using MediatR;

namespace PDiffy.Features.TextDifferences
{
	public partial class TextDifferencesController : Controller
    {
	    readonly IMediator _mediator;

		public TextDifferencesController(IMediator mediator)
	    {
		    _mediator = mediator;
	    }

		public virtual async Task<ActionResult> Index(Index.Query query)
	    {
		    var model = await _mediator.SendAsync(query);

            return View(MVC.TextDifferences.Views.Index, model);
        }

		public virtual async Task<ActionResult> Approve(Approve.Command model)
	    {
		    await _mediator.SendAsync(model);

		    return RedirectToAction(MVC.TextDifferences.Index());
	    }

		public virtual async Task<ActionResult> Delete(Delete.Command model)
	    {
		    await _mediator.SendAsync(model);

			return RedirectToAction(MVC.TextDifferences.Index());
	    }

		public virtual async Task<ActionResult> DeleteAll(DeleteAll.Command message)
		{
			if (message.DeleteAll)
				await _mediator.SendAsync(message);

			return RedirectToAction(MVC.TextDifferences.Index());
		}
    }
}