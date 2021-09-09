using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.Wmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.Controllers
{
	public class TakmicenjeController : Controller
	{

		private MojContext konekcija;

		public TakmicenjeController(MojContext konekcija_)
		{
			konekcija = konekcija_;
		
		}

		public IActionResult Index()
		{

			string[] razredi = new string[]{"1", "2", "3", "4" };



			var model = new PrikazTakmicenjaWM
			{
				ListaRazreda = razredi.
				AsEnumerable()
			   .Select(X =>
			   {



				   return new SelectListItem
				   {
					   Text = X,
					   Value = X,


				   };

			   }).ToList(),

				ListaSkola = konekcija
			   .Skola
			   .AsEnumerable()
			   .Select(X =>
			   {

				   return new SelectListItem
				   {
					   Text = X.Naziv,
					   Value = X.Id.ToString(),


				   };



			   }).ToList(),


				Listatakmicenja=konekcija
				.Takmicenja
				.Include(X=>X.Skola)
				.Include(X=>X.Predmet)
				.AsEnumerable()
				.Select(X=> {


					var najbolji = konekcija.Takmicenjeucesnik
					.Include(Y=>Y.OdjeljenjeStavka)
					.Include(Y=>Y.OdjeljenjeStavka.Odjeljenje)
					.Include(Y=>Y.OdjeljenjeStavka.Ucenik)
					.Include(Y => Y.OdjeljenjeStavka.Odjeljenje.Skola)
					.OrderByDescending(Y => Y.bodovi).FirstOrDefault();




					return new PrikazTakmicenjaWM.row
					{
						Skolanaziv=X.Skola.Naziv,
						Datum=X.Datum.ToString(format:"dd.mm.yyyy"),
						NazivPredmeta=X.Predmet.Naziv,
						NIme=najbolji.OdjeljenjeStavka.Ucenik.ImePrezime,
						Nodjeljenje=najbolji.OdjeljenjeStavka.Odjeljenje.Oznaka,
						Nskola=najbolji.OdjeljenjeStavka.Odjeljenje.Skola.Naziv,
						Razred=X.Razred,



					};



				}).ToList()













			};



			model.ListaSkola.Add(new SelectListItem { Text = "Not selected", Value = "0", Selected = true });
			model.ListaRazreda.Add(new SelectListItem { Text = " ", Value = "0", Selected = true });








			return View(model);
		}

		public IActionResult DodajtakmicenjeWM(string test)
		{

			//int broj = (int)TempData["Greeting"];




			return Content(test.ToString());
		
		}

	}
}
