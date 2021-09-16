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
	public class TakmicenjeController : Controller
	{
		private MojContext konekcija;


		public TakmicenjeController(MojContext konekcija_)
		{

			konekcija = konekcija_;

		}




		public class najtakmicari
		{
			public int id { get; set; }

			public double prosjek { get; set; }



		}



		public IActionResult Index()
		{


			var model = new TakmicenjePrikazWM
			{
				SkolaId = 0,
				Predmetnaziv = "0",



				ListaPredmeta = konekcija.Predmet
				.GroupBy(X => X.Naziv)
				.Select(X => X.First())
				.AsEnumerable()
				.Select(X => {



					return new SelectListItem
					{
						Text = X.Naziv,
						Value = X.Naziv





					};



				}).ToList(),


				Listaskola = konekcija.Skola
				.AsEnumerable()
				.Select(X => {

					return new SelectListItem
					{
						Text = X.Naziv,
						Value = X.Id.ToString()
					};

				}).ToList(),








			};


			model.ListaPredmeta.Add(new SelectListItem { Value = "0", Text = "not-Selected", Selected = true });













			return View(model);
		}

		public IActionResult Filtriraj(TakmicenjePrikazWM Modelold)
		{
			var model = new TakmicenjePrikazWM
			{
				ListaTakmicenja = konekcija.Takmicenje
			   .Include(X => X.Skola)
			   .Include(X => X.Predmet)
			   .Where(X => X.SkolaId == Modelold.SkolaId || X.SkolaId == 0)
			   .Where(X => X.Predmet.Naziv == Modelold.Predmetnaziv || Modelold.Predmetnaziv == "0")
			   .AsEnumerable()
			   .Select(X =>
			   {

				   var najbolji = konekcija.TakimicenjeUcesnik
				  .Include(Y => Y.OdjeljenjeStavka)
				  .Include(Y => Y.OdjeljenjeStavka.Odjeljenje)
				  .Include(Y => Y.OdjeljenjeStavka.Ucenik)
				  .Include(Y => Y.OdjeljenjeStavka.Odjeljenje.Skola)
				  .Where(Y => Y.TakmicenjeId == X.Id && Y.Pristupio == true)
				  .OrderByDescending(Y => Y.Bodovi)
				  .FirstOrDefault();

				   return new TakmicenjePrikazWM.row
				   {
					   Datum = X.Datum.ToString("dd.mm.yyyy"),
					   Nime = najbolji!=null?najbolji.OdjeljenjeStavka.Ucenik.ImePrezime:"Nema",
					   Nodjeljenje =najbolji!=null?najbolji.OdjeljenjeStavka.Odjeljenje.Oznaka:"NEMA",
					   Nskola = najbolji!=null?najbolji.OdjeljenjeStavka.Odjeljenje.Skola.Naziv: "Nema",
					   Predmet = X.Predmet.Naziv,
					   razred = X.Razred.ToString(),
					   Skolanaziv = X.Skola.Naziv,
					   TakmicenjeId = X.Id,
				   };




			   }).ToList()












			};






















			return PartialView("PrikazTakmicenja", model);

		}

		public IActionResult Zakljucaj (UcesniciPrikazWM modeld)
		{

			var takmicenje = konekcija.Takmicenje.Find(modeld.TakmicenjaId);

			takmicenje.Zakljucano = false;
			konekcija.SaveChanges();


	
			return RedirectToAction("Prikaz","Ucesnici",new {TakmicenjaId=modeld.TakmicenjaId});
         }

		public IActionResult DodajTakmicenje(int Id)
		{

			var model = new DodajtakmicenjeWM
			{

				SkolaId = Id,
				Datum = DateTime.Today,
				ListaPredmeta = konekcija
				.Predmet
				.GroupBy(X => X.Naziv)
				.Select(X => X.First())
				.AsEnumerable()
				.Select(X =>
				{

					return new SelectListItem
					{
						Text = X.Naziv,
						Value = X.Naziv

					};


				}).ToList(),


				//Predmetnaziv = "0",

				Listaskola = konekcija
				.Skola
				.AsEnumerable()
				.Select(X =>
				{

					return new SelectListItem
					{
						Text = X.Naziv,
						Value = X.Id.ToString(),

					};


				}).ToList()




			};














			return View(model);
		
		}

		public IActionResult Snimi(DodajtakmicenjeWM model)
		{
			//takmicenje 1 


			var takmicenje = new Takmicenje
			{
				Datum = model.Datum,
				PredmetId = konekcija.Predmet.Where(X => X.Naziv == model.Predmetnaziv).FirstOrDefault().Id,
				SkolaId = model.SkolaId,
				Zakljucano = true,
				Razred = 1,

			};
			konekcija.Takmicenje.Add(takmicenje);
			konekcija.SaveChanges();

			//korak 2 filter 


			var spisakucesnika = konekcija.DodjeljenPredmet
				.Include(X => X.Predmet)
				.Where(X => X.ZakljucnoKrajGodine == 5 && X.Predmet.Naziv == model.Predmetnaziv)
				.AsEnumerable()
				.Select(X=> {

					return new TakmicenjeUcesnik
					{
						Bodovi=0,
						OdjeljenjeStavkaId=X.OdjeljenjeStavkaId,
						Pristupio=true,
						TakmicenjeId=takmicenje.Id,
						
						




					};

				
				
				})
				.ToList();

			//korak 3 

			var najtak = new List<najtakmicari>();

			foreach (var a in spisakucesnika)
			{
				double prosjek = konekcija.DodjeljenPredmet
					.Where(X => X.OdjeljenjeStavkaId == a.OdjeljenjeStavkaId)
					.Select(X => X.ZakljucnoKrajGodine).Average();

				najtak.Add(new najtakmicari { prosjek = prosjek, id = a.OdjeljenjeStavkaId });
			
			
			
			}

			//korak 4 

			var najboljih = najtak.OrderByDescending(X => X.prosjek).Select(X => X.id).Take(5);

			//korak 5 dodavanje 

			foreach (var a in spisakucesnika)
			{
				if (najboljih.Contains(a.OdjeljenjeStavkaId))
				{

					konekcija.TakimicenjeUcesnik.Add(a);
					konekcija.SaveChanges();
				
				}





			}


			return RedirectToAction("Index", "Takmicenje");
		}
	}
}
