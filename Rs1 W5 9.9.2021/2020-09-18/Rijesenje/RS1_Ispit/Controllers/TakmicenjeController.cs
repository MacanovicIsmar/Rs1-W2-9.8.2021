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
	public class TakmicenjeController : Controller
	{

		private MojContext konekcija;

		public TakmicenjeController(MojContext konekcija_)
		{


			konekcija = konekcija_;
		
		
		}









		public IActionResult Index(int skolaId=0)
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



			//model.spisakskola.Add(new SelectListItem { Text = "not selected", Value = "0",Selected=true });
			model.spisakpredmeta.Add(new SelectListItem { Text = "not selected", Value = "0",Selected=true });


			if (skolaId != 0)
			{

				model.spisakskola.Where(X => X.Value == skolaId.ToString()).FirstOrDefault().Selected = true;
			
			
			
			
			}







			return View(model);
		}


		public IActionResult PrikazTakmicenja(PrikaztakmicenjaWm model)
		{

			model.Spisaktakmicenja =
				konekcija.Takmicenje
				.Include(X => X.Skola)
				.Include(X => X.Predmet)
				.Where(X => X.Skola.Id == model.SkolaId)
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
						Datum = X.Datum.ToString("dd.mm.yyyy"),
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


		public IActionResult DodajTakmicenje(string skola, string predmet)
		{

			var model = new DodajTamicenjaWm
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

				Datum=DateTime.Today

			};

			model.spisakskola.Where(X => X.Value == skola)
				.FirstOrDefault().Selected = true;





















			return View(model);
		
		}

		public IActionResult Snimi(DodajTamicenjaWm model)
		{

			var takmicenje = new Takmicenje
			{
				Datum = model.Datum,
				PredmetId = konekcija.Predmet.Where(X => X.Naziv == model.PredmetNaziv).FirstOrDefault().Id,
				SkolaId = konekcija.Skola.Find(model.SkolaId).Id,
				Zakljucano = false,
				Razred = 1,


			};

			konekcija.Takmicenje.Add(takmicenje);
			konekcija.SaveChanges();


			var spisaktakmicara = konekcija
				.DodjeljenPredmet
				.Include(X => X.OdjeljenjeStavka)
				.Include(X => X.OdjeljenjeStavka.Ucenik)
				.Include(X => X.Predmet)
				.Where(X => X.Predmet.Naziv == model.PredmetNaziv)
				.Where(X => X.ZakljucnoKrajGodine == 5)
				.AsEnumerable()
				.Select(X =>
				{

					return new TakmicenjeUcesnik
					{
						bodovi=0,
						OdjeljenjeStavkaId=X.OdjeljenjeStavkaId,
						Pristupio=true,
						TakmicenjeId=takmicenje.Id



					};



				}).ToList();


			foreach (var a in spisaktakmicara)
			{
				bool flag =
					   konekcija.DodjeljenPredmet
					   .Where(X => X.OdjeljenjeStavkaId == a.OdjeljenjeStavkaId)
					   .Select(X => X.ZakljucnoKrajGodine)
					   .Average() > 4;


				if (flag == true)
				{



					konekcija.TakmicenjeUcesnik.Add(a);
					konekcija.SaveChanges();
				
				}



			
			
			
			
			
			}

			return RedirectToAction("Index", "Takmicenje", new { skolaId = model.SkolaId });
		}




	}
}
