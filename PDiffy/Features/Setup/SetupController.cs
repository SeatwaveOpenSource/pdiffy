using System.Web.Mvc;
using Environment = PDiffy.Infrastructure.Environment;

namespace PDiffy.Features.Setup
{
	public partial class SetupController : Controller
	{
		public virtual ActionResult Index()
		{
			if (!string.IsNullOrWhiteSpace(Environment.ImageStorePath) && !string.IsNullOrWhiteSpace(Environment.DataStorePath))
				return RedirectToAction(MVC.ImageDifferences.Index());
			return View(MVC.Setup.Views.Index);
		}

		public virtual ActionResult Setup(string imageStorePath, string dataStorePath)
		{
			Environment.ImageStorePath = imageStorePath;
			Environment.DataStorePath = dataStorePath;

			return RedirectToAction(MVC.ImageDifferences.Index());
		}
	}
}