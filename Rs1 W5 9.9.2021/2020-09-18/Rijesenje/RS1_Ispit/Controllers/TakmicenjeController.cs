using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.WM;
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

			var model = new PrikaztakmicenjaWm
			{
				spisakpredmeta = konekcija
				.Predmet
				.GroupBy(X => X.Naziv)
				.Select(Y => Y.First())
				.AsEnumerable()
				.Select(X =>
				{

					return new SelectListItem
					{
						Value = X.Naziv,
						Text = X.Naziv



					};




				}).ToList(),

				spisakskola = konekcija
				.Skola
				.AsEnumerable()
				.Select(X =>
				{

					return new SelectListItem
					{
						Value = X.Id.ToString(),
						Text = X.Naziv



					};




				}).ToList(),










			};



			model.spisakskola.Add(new SelectListItem { Text = "not selected", Value = "0",Selected=true });
			model.spisakpredmeta.Add(new SelectListItem { Text = "not selected", Value = "0",Selected=true });





			return View(model);
		}


		public IActionResult PrikazTakmicenja(PrikaztakmicenjaWm model)
		{

			model.Spisaktakmicenja =
				konekcija.Takmicenje
				.Include(X => X.Skola)
				.Include(X => X.Predmet)
				.Where(X => X.Skola.Id == model.SkolaId || model.SkolaId == 0)
				.Where(X => X.Predmet.Naziv == model.PredmetNaziv || model.PredmetNaziv == "0")
				.AsEnumerable()
				.Select(X =>
				{

					var najtakmicar = konekcija
					.TakmicenjeUcesnik
					.Include(Y => Y.OdjeljenjeStavka)
					.Include(Y => Y.OdjeljenjeStavka.Odjeljenje)
					.Include(Y => Y.OdjeljenjeStavka.Ucenik)
					.Include(Y => Y.OdjeljenjeStavka.Odjeljenje.Skola)
					.Where(Y => Y.Pristupio == true && Y.TakmicenjeId == Y.TakmicenjeId)
					.OrderByDescending(Y => Y.bodovi)
					.FirstOrDefault();




					return new PrikaztakmicenjaWm.row
					{
						Datum = X.Datum.ToString("format:dd.mm.yyyy"),
						NIme = najtakmicar.OdjeljenjeStavka.Ucenik.ImePrezime,
						Nodljenje = najtakmicar.OdjeljenjeStavka.Odjeljenje.Oznaka,
						Nskola = najtakmicar.OdjeljenjeStavka.Odjeljenje.Skola.Naziv,
						Predmet = X.Predmet.Naziv,
						Razred = X.Razred.ToString(),
						Skolanaziv = X.Skola.Naziv,
						TakmicenjeId = X.Id,




					};






				}).ToList();
			



















			return PartialView(model);
		}



	}
}
