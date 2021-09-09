using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.Wmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.Controllers
{
	public class AjaxController : Controller
	{
		private MojContext konekcija;

		public AjaxController(MojContext konekcija_)
		{
			konekcija = konekcija_;

		}


		public IActionResult Index()
		{
			return View();
		}



		public IActionResult TakmicenjaView(int skolaId ,int razred)
		{
			var model = new PrikazTakmicenjaWM
			{
				

				
				Listatakmicenja = konekcija
				.Takmicenja
				.Include(X => X.Skola)
				.Include(X => X.Predmet)
				.Where(X=>X.SkolaId==skolaId||skolaId==0)
				.AsEnumerable()
				.Select(X => {


					var najbolji = konekcija.Takmicenjeucesnik
					.Include(Y => Y.OdjeljenjeStavka)
					.Include(Y => Y.OdjeljenjeStavka.Odjeljenje)
					.Include(Y => Y.OdjeljenjeStavka.Ucenik)
					.Include(Y => Y.OdjeljenjeStavka.Odjeljenje.Skola)
					.OrderByDescending(Y => Y.bodovi).FirstOrDefault();




					return new PrikazTakmicenjaWM.row
					{
						Skolanaziv = X.Skola.Naziv,
						Datum = X.Datum.ToString(format: "dd.mm.yyyy"),
						NazivPredmeta = X.Predmet.Naziv,
						NIme = najbolji.OdjeljenjeStavka.Ucenik.ImePrezime,
						Nodjeljenje = najbolji.OdjeljenjeStavka.Odjeljenje.Oznaka,
						Nskola = najbolji.OdjeljenjeStavka.Odjeljenje.Skola.Naziv,
						Razred = X.Razred,



					};



				}).ToList()













			};





			return PartialView("TakmicenjaAjax",model);
		}

		public IActionResult TakmicenjaView2(PrikazTakmicenjaWM Model2)
		{

			var model = new PrikazTakmicenjaWM
			{



				Listatakmicenja = konekcija
				.Takmicenja
				.Include(X => X.Skola)
				.Include(X => X.Predmet)
				.Where(X => X.SkolaId == Model2.SkolaId || Model2.SkolaId == 0)
				.Where(X => X.Razred == int.Parse(Model2.razred) || Model2.razred == " ")
				.AsEnumerable()
				.Select(X => {


					var najbolji = konekcija.Takmicenjeucesnik
					.Include(Y => Y.OdjeljenjeStavka)
					.Include(Y => Y.OdjeljenjeStavka.Odjeljenje)
					.Include(Y => Y.OdjeljenjeStavka.Ucenik)
					.Include(Y => Y.OdjeljenjeStavka.Odjeljenje.Skola)
					.OrderByDescending(Y => Y.bodovi).FirstOrDefault();




					return new PrikazTakmicenjaWM.row
					{
						Skolanaziv = X.Skola.Naziv,
						Datum = X.Datum.ToString(format: "dd.mm.yyyy"),
						NazivPredmeta = X.Predmet.Naziv,
						NIme = najbolji.OdjeljenjeStavka.Ucenik.ImePrezime,
						Nodjeljenje = najbolji.OdjeljenjeStavka.Odjeljenje.Oznaka,
						Nskola = najbolji.OdjeljenjeStavka.Odjeljenje.Skola.Naziv,
						Razred = X.Razred,



					};



				}).ToList()













			};





			return PartialView("TakmicenjaAjax", model);
		}



	}
}
