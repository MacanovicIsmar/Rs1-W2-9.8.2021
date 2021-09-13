using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.WM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.Controllers
{
	public class UcesniciController : Controller
	{
		private MojContext konekcija;

		public UcesniciController(MojContext konekcija_)
		{


			konekcija = konekcija_;


		}


		public IActionResult Index(int Id)
		{

			var takmicenje = konekcija.Takmicenje.Find(Id);


			var model = new PrikazUcesnikaWm
			{
				TakmicenjeId = Id,
				Datum = takmicenje.Datum.ToString("dd.mm.yyyy"),
				PredmetNaziv=konekcija.Predmet.Find(takmicenje.PredmetId).Naziv,
				Razred=takmicenje.Razred.ToString(),
				SkolaNaziv=konekcija.Skola.Find(takmicenje.SkolaId).Naziv,
				Zakljucano=takmicenje.Zakljucano

			};


			return View(model);
		}

		public IActionResult PrikazUcesnika(PrikazUcesnikaWm model)
		{
			var modelnew = new PrikazUcesnikaWm
			{
				Spisakucesnika = konekcija
				.TakmicenjeUcesnik
				.Include(X => X.OdjeljenjeStavka.Odjeljenje)
				.Include(X => X.OdjeljenjeStavka.Odjeljenje.Skola)
				.Include(X => X.OdjeljenjeStavka.Ucenik)
				.Where(X => X.TakmicenjeId == model.TakmicenjeId)
				.AsEnumerable()
				.Select(X => {



					return new PrikazUcesnikaWm.row
					{
						BrojUdnevniku = X.OdjeljenjeStavka.BrojUDnevniku.ToString(),
						odjeljenje = X.OdjeljenjeStavka.Odjeljenje.Oznaka,
						Pristupio=X.Pristupio==true?"Da":"Ne",
						Rezultatbodovi=X.bodovi.ToString(),
						UcesnikId=X.Id


					};



				}).ToList()











			};

			modelnew.TakmicenjeId = model.TakmicenjeId;
			modelnew.Zakljucano = konekcija.Takmicenje.Find(model.TakmicenjeId).Zakljucano;




			return PartialView(modelnew);

		}

		public IActionResult UcesnikNijePristupio(int Id,int TakId)
		{
			var takmicenje = konekcija.TakmicenjeUcesnik.Find(Id);

			takmicenje.Pristupio = false;

			konekcija.SaveChanges();


			return RedirectToAction("PrikazUcesnika", "Ucesnici", new { TakmicenjeId = TakId });
		}

		public IActionResult UcesnikjePristupio(int Id, int TakId)
		{
			var takmicenje = konekcija.TakmicenjeUcesnik.Find(Id);

			takmicenje.Pristupio = true;

			konekcija.SaveChanges();


			return RedirectToAction("PrikazUcesnika", "Ucesnici", new { TakmicenjeId = TakId });
		}







	}
}
