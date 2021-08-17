using Microsoft.AspNetCore.Mvc;
using RS1_2020_01_30.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_2020_01_30.Controllers
{
	public class AjaxStavkeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}


		public IActionResult ajax(RezultatiWM Lista)
		{
			//var model = new RezultatiWM
			//{
			//	SpisakUcesnika = Lista.


			//};



			return PartialView(Lista);
		}




	}
}
