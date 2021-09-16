using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.Wm;
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








		public IActionResult Index(int TakId)
		{
			var takmicenje = konekcija.Takmicenje.Find(TakId);


			var model = new UcesniciPrikazWM
			{
				Datum= takmicenje.Datum.ToString("dd.mm.yyyy"),
				Predmetnaziv=konekcija.Predmet.Find(takmicenje.PredmetId).Naziv,
				Razred=takmicenje.Razred.ToString(),
				Skolanaziv=konekcija.Skola.Find(takmicenje.SkolaId).Naziv,
				TakmicenjaId=TakId,
				Zakljucano=takmicenje.Zakljucano,
			};


			return View(model);
		}


		public IActionResult Prikaz(UcesniciPrikazWM oldmodel)
		{

			var takmicenje = konekcija.Takmicenje.Find(oldmodel.TakmicenjaId);


			var model = new UcesniciPrikazWM
			{
				TakmicenjaId=oldmodel.TakmicenjaId,
				Zakljucano=takmicenje.Zakljucano,


				ListaUcesnika = konekcija.TakimicenjeUcesnik
			   .Include(X => X.OdjeljenjeStavka.Odjeljenje)
			   .Include(X => X.OdjeljenjeStavka.Odjeljenje.Skola)
			   .Include(X => X.OdjeljenjeStavka.Ucenik)
			   .Where(X => X.TakmicenjeId == oldmodel.TakmicenjaId)
			   .AsEnumerable()
			   .Select(X =>
			   {

				
				   return new UcesniciPrikazWM.row
				   {
					  BrojUdnevniku=X.OdjeljenjeStavka.BrojUDnevniku.ToString(),
					  Odjeljenje=X.OdjeljenjeStavka.Odjeljenje.Oznaka,
					  Pristupio=X.Pristupio==true?"Da":"Ne",
					  Rezultatbodovi=X.Bodovi.ToString(),
					  UcesnikId=X.Id,

					  

					  







				   };




			   }).ToList()












			};


















			return PartialView(model);
		}

		public IActionResult Ucesnikjepristupio(int Id,int TakId,bool pristupio)
		{

			var ucesnik = konekcija.TakimicenjeUcesnik.Find(Id);
			ucesnik.Pristupio = false;
			konekcija.SaveChanges();
		

			return RedirectToAction("Prikaz", "Ucesnici", new { TakmicenjaId = TakId, Zakljucano = pristupio });
		
		}
		public IActionResult Ucesniknijepristupio(int Id, int TakId, bool pristupio)
		{

			var ucesnik = konekcija.TakimicenjeUcesnik.Find(Id);
			ucesnik.Pristupio = true;
			konekcija.SaveChanges();


			return RedirectToAction("Prikaz", "Ucesnici", new { TakmicenjaId = TakId, Zakljucano = pristupio });

		}

		public IActionResult DodajUrediucesnika(int Takid,int Id= 0)
		{

			if (Id != 0)
			{

				var takmicenjeucesnik = konekcija.TakimicenjeUcesnik.Find(Id);



				var model = new DodajUcesnikaWM
				{
					Bodovi = takmicenjeucesnik.Bodovi,
					status="E",
					TakId=Takid,
					Listucesnika=konekcija.TakimicenjeUcesnik
					.Include(X=>X.OdjeljenjeStavka)
					.Include(X => X.OdjeljenjeStavka.Odjeljenje)
					.Include(X => X.OdjeljenjeStavka.Odjeljenje.Skola)
					.Include(X => X.OdjeljenjeStavka.Ucenik)
					.Where(X=>X.TakmicenjeId==Takid)
					.AsEnumerable()
					.Select(X=> {

						return new SelectListItem
						{
							Text=X.OdjeljenjeStavka.Odjeljenje.Oznaka.ToString() + "-"+
							X.OdjeljenjeStavka.Ucenik.ImePrezime + "-" +
							X.OdjeljenjeStavka.BrojUDnevniku.ToString(),

							Value=X.Id.ToString()

							





						};
					
					
					}).ToList()









				};


				return PartialView(model);








			}
			else
			{

				var listaucesnikaid = konekcija.TakimicenjeUcesnik
					.Where(X => X.TakmicenjeId == Takid)
					.Select(X => X.OdjeljenjeStavkaId)
					.ToList();




				var model = new DodajUcesnikaWM
				{
					Bodovi = 0,
					status = "D",
					TakId = Takid,
					Listucesnika = konekcija.OdjeljenjeStavka
					.Include(X => X.Odjeljenje)
					.Include(X => X.Odjeljenje.Skola)
					.Include(X => X.Ucenik)
					.Where(X => listaucesnikaid.Contains(X.Id) == false)
					.AsEnumerable()
					.Select(X =>
					{

						return new SelectListItem
						{
							Text = X.Odjeljenje.Oznaka.ToString() + "-" +
							X.Ucenik.ImePrezime + "-" +
							X.BrojUDnevniku.ToString(),
							Value = X.Id.ToString()



						};


					}).ToList()







				};










				return PartialView(model);

			}

		




















			
		
		}

		public IActionResult Snimi(DodajUcesnikaWM model)
		{
			if (model.status == "E")
			{
				var takucesnik = konekcija.TakimicenjeUcesnik.Find(model.UcesnikId);

				takucesnik.Bodovi = model.Bodovi;

				konekcija.SaveChanges();







			}
			else
			{


				var ucesnik = new TakmicenjeUcesnik
				{
					Bodovi = model.Bodovi,
					OdjeljenjeStavkaId = model.UcesnikId,
					Pristupio = true,
					TakmicenjeId = model.TakId,



				};


				konekcija.TakimicenjeUcesnik.Add(ucesnik);

				konekcija.SaveChanges();
			
			
			
			
			
			
			
			
			}











			





			return RedirectToAction("Prikaz", "Ucesnici", new { TakmicenjaId=model.TakId });
		}

		public void Editbodovi(int Id,int bodovi)
		{

			var ucesnik = konekcija.TakimicenjeUcesnik.Find(Id);
			ucesnik.Bodovi = bodovi;
			konekcija.SaveChanges();
		
		
		
		


		
		}




	}
}
