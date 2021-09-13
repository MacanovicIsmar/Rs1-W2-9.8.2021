using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
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
				PredmetNaziv = konekcija.Predmet.Find(takmicenje.PredmetId).Naziv,
				Razred = takmicenje.Razred.ToString(),
				SkolaNaziv = konekcija.Skola.Find(takmicenje.SkolaId).Naziv,
				Zakljucano = takmicenje.Zakljucano

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
						Pristupio = X.Pristupio == true ? "Da" : "Ne",
						Rezultatbodovi = X.bodovi.ToString(),
						UcesnikId = X.Id


					};



				}).ToList()











			};

			modelnew.TakmicenjeId = model.TakmicenjeId;
			modelnew.Zakljucano = konekcija.Takmicenje.Find(model.TakmicenjeId).Zakljucano;




			return PartialView(modelnew);

		}

		public IActionResult UcesnikNijePristupio(int Id, int TakId)
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

		public IActionResult UrediucesnikaWm(int Id,int TakId)
		{

			var ucesnik = konekcija.TakmicenjeUcesnik.Find(Id);


			var model = new DodajUcesnikaWm
			{
				Bodovi = ucesnik.bodovi,
				TakmicenjeId = ucesnik.TakmicenjeId,
				ucesnikid = ucesnik.Id,
				flag="U",
				spisakucesnika = konekcija.TakmicenjeUcesnik
				.Include(X => X.OdjeljenjeStavka.Odjeljenje)
				.Include(X => X.OdjeljenjeStavka.Odjeljenje.Skola)
				.Include(X => X.OdjeljenjeStavka.Ucenik)
			   .Where(X => X.TakmicenjeId == TakId)
			   .AsEnumerable()
			   .Select(X =>
			   {





				   return new SelectListItem
				   {
					   Text = X.OdjeljenjeStavka.Odjeljenje.Oznaka + "-" +
					   X.OdjeljenjeStavka.Ucenik.ImePrezime + "-" +
					   X.OdjeljenjeStavka.BrojUDnevniku,

					   Value = X.Id.ToString()




				   };

			   }).ToList()





			};
			

			return PartialView(model);

        }

		public IActionResult Snimi(DodajUcesnikaWm modelold)
		{

			if (modelold.flag == "U")
			{

				var ucesnik = konekcija.TakmicenjeUcesnik.Find(modelold.ucesnikid);

				ucesnik.bodovi = modelold.Bodovi;

				konekcija.SaveChanges();


			}
			else
			{
				var ucesnik = new TakmicenjeUcesnik
				{
					bodovi=modelold.Bodovi,
					OdjeljenjeStavkaId=modelold.ucesnikid,
					Pristupio=true,
					TakmicenjeId=modelold.TakmicenjeId,
					





				};

				konekcija.TakmicenjeUcesnik.Add(ucesnik);

				konekcija.SaveChanges();

			
			
			
			
			}

			return RedirectToAction("PrikazUcesnika", "Ucesnici", new { TakmicenjeId = modelold.TakmicenjeId });
		
		}

		public IActionResult Dodajucesnika (int Id)
		{

			var spisakidova = konekcija
				.TakmicenjeUcesnik
				.Where(X => X.TakmicenjeId == Id)
				.Select(X => X.OdjeljenjeStavkaId)
				.ToList();
				








			var model = new DodajUcesnikaWm
			{
				Bodovi = 0,
				TakmicenjeId = Id,
				ucesnikid = 0,
				flag = "D",
				spisakucesnika = konekcija.OdjeljenjeStavka
				.Include(X => X.Odjeljenje)
				.Include(X => X.Odjeljenje.Skola)
				.Include(X => X.Ucenik)
			   .Where(X => spisakidova.Contains(X.Id)==false)
			   .AsEnumerable()
			   .Select(X =>
			   {





				   return new SelectListItem
				   {
					   Text = X.Odjeljenje.Oznaka + "-" +
					   X.Ucenik.ImePrezime + "-" +
					   X.BrojUDnevniku,
					   Value = X.Id.ToString()
				   };

			   }).ToList()

			};








			return PartialView("UrediucesnikaWm",model);
		
		}

		public void EditBodovi(int bodovi,int ucesnikId)
		{
			var ucesnik = konekcija.TakmicenjeUcesnik.Find(ucesnikId);
			
			
			ucesnik.bodovi = bodovi;
			
			
			konekcija.SaveChanges();









		}





	}
}
