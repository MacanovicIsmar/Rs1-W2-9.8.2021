using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RS1_2020_01_30.EF;
using RS1_2020_01_30.EntityModels;
using RS1_2020_01_30.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_2020_01_30.Controllers
{
	public class TakmicenjeController : Controller
	{
		private readonly MojContext CTX;

		//test for git
		//test for git 2
                //test for git 3
		public TakmicenjeController(MojContext context)
		{

			CTX = context;


		}
		public IActionResult Index()
		{
			var Razredi = new List<string> { "", "1", "2", "3", "4" };

			var model = new OdabirTakmicenja
			{

				//konvertovanje u select list item
				Skole = CTX.Skola
				.Select(X => new SelectListItem
				{
					Value = X.Id.ToString(),
					Text = X.Naziv
				})
				.OrderBy(i => i.Text)
				.ToList(),

				Razredi = Razredi
				.ConvertAll(i => {
					return new SelectListItem
					{
						Value = i,
						Text = i,
						Selected = false
					};
				})
			};

			return View(model);
		}

		public IActionResult Odaberi(OdabirTakmicenja model)
		{


			Skola Skola = CTX.Skola.Find(model.skolaId);






			var newmodel = new PrikazTakmicenjaModel
			{
				SkolaId = model.skolaId,
				Skola = Skola.Naziv,
				Razred = model.Razred
			};


			newmodel.Lista = CTX.Takmicenje
				.Include(X => X.Skola)
				.Include(X => X.Predmet)
				.Where(X => X.Skola.Naziv == Skola.Naziv)
				.AsEnumerable()
				.Select(X =>
				{
					int Brojucesnika_ = CTX.TakmicenjeUcesnik
					.Where(Y => Y.TakmicenjeId == X.TakmicenjeId)
					.Where(Y => Y.ispristupio == false)
					.Count();

					TakmicenjeUcesnik ucesnik = CTX.TakmicenjeUcesnik
					.Include(Y => Y.OdjeljenjeStavka)
					.Include(Y => Y.OdjeljenjeStavka.Ucenik)
					.Include(Y => Y.OdjeljenjeStavka.Odjeljenje)
					.Where(Y => Y.TakmicenjeId == X.TakmicenjeId)
					.Where(Y => Y.ispristupio == true)
					.OrderByDescending(Y => Y.Bodovi)
					.FirstOrDefault();




					return new PrikazTakmicenjaModel.row
					{
						Brojucesnika = Brojucesnika_,
						Datum = X.Datum == null ? "null" : X.Datum.ToString(),
						ime = ucesnik != null ? ucesnik.OdjeljenjeStavka.Ucenik.ImePrezime : "Null",
						odjeljenje = ucesnik != null ? ucesnik.OdjeljenjeStavka.Odjeljenje.Oznaka : "Null",
						PredmetNaziv = X.Predmet.Naziv,
						Razred = X.Razred != null ? X.Razred.ToString() : "Null",
						SkolaNaziv = ucesnik != null ? X.Skola.Naziv : "Null",
						//prezime = ucesnik.OdjeljenjeStavka.Ucenik.ImePrezime,
						PredmetId = X.PredmetId == null ? (int)X.PredmetId : 0,
						TakmicenjeId = X.TakmicenjeId
					};

				})
				.ToList();


			if (model.Razred != null)
			{
				newmodel.Lista = newmodel.Lista.Where(X => X.Razred.ToString() == model.Razred).ToList();
			}









			return View(newmodel);
		}

		public IActionResult DodajView(int Id)
		{


			var listarazreda = new List<SelectListItem>();

			listarazreda.Add(new SelectListItem { Text = "1", Value = "1" });
			listarazreda.Add(new SelectListItem { Text = "2", Value = "2" });
			listarazreda.Add(new SelectListItem { Text = "3", Value = "3" });
			listarazreda.Add(new SelectListItem { Text = "4", Value = "4" });







			var newmodel = new DodajTakmicenjeWM
			{

				ListaPredmeta = CTX.Predmet
			  .GroupBy(i => i.Naziv)
			  .Select(i => i.First())
			  .AsEnumerable()
			  .Select(X =>
			  {

				  return new SelectListItem
				  {
					  Value = X.Id.ToString(),
					  Text = X.Naziv

				  };

			  }).ToList(),
				ListaRazreda = listarazreda,
				SkolaId = Id,
				SkolaNaziv = CTX.Skola.Find(Id).Naziv,
				Datum = DateTime.Now

			};











			return View(newmodel);
		}

		public IActionResult Snimi(DodajTakmicenjeWM model)
		{
			var entry = new Takmicenje
			{
				Datum = model.Datum,
				PredmetId = model.PredmetId,
				Razred = int.Parse(model.Razred),
				SkolaId = model.SkolaId,
				iszakljucano=false,
				
			};

			CTX.Takmicenje.Add(entry);
			CTX.SaveChanges();


			//u bazi tabela koja je vezena za uslov

			var spisaktakmicara =
				CTX.DodjeljenPredmet
				.Where(X => X.PredmetId == model.PredmetId)
				.Where(X => X.ZakljucnoKrajGodine == 5)
				.AsEnumerable()
				.Select(X => {

					return new TakmicenjeUcesnik
					{
						OdjeljenjeStavkaId= X.OdjeljenjeStavkaId,
						Bodovi = 0,
						ispristupio = false,
						TakmicenjeId=entry.TakmicenjeId



					};
				})
				.ToList();

			//uslov 2 avarage
			//
			foreach (var a in spisaktakmicara)
			{
				bool flag = CTX.DodjeljenPredmet
					 .Where(X => X.OdjeljenjeStavkaId == a.OdjeljenjeStavkaId)
					 .AsEnumerable()
					 .Select(X =>
					 {

						 return X.ZakljucnoKrajGodine;




					 }).Average() > 4;


				if (flag)
				{


					//a.TakmicenjeId = entry.TakmicenjeId;

					CTX.Add(a);
					CTX.SaveChanges();
				
				
				}




			
			
			}

			



				

			
				





			return Redirect("/Takmicenje/Index");
		
		}

		public IActionResult Rezultati(int Id)
		{

			Takmicenje paket = CTX.Takmicenje.Where(i => i.TakmicenjeId== Id)
				.SingleOrDefault();

			//converting datetime
			DateTime datum = paket.Datum??new DateTime(2000,1,1);

			var model = new RezultatiWM
			{
				takmicenjeId=paket.TakmicenjeId,
				Datum = datum.ToString(format: "d.MM.yyyy"),
				PredmetId = (int)paket.PredmetId,
				PredmetNaziv = CTX.Predmet.Find(paket.PredmetId).Naziv,
				Razred = (int)paket.Razred,
				skolaId = paket.SkolaId,
				Skolanaziv = CTX.Skola.Find(paket.SkolaId).Naziv,
				//TakmicenjeId=paket.TakmicenjeId,

				SpisakUcesnika = CTX.TakmicenjeUcesnik
				 .Where(x => x.TakmicenjeId == paket.TakmicenjeId)
				 .Include(X=>X.OdjeljenjeStavka)
				 .Include(X=>X.OdjeljenjeStavka.Odjeljenje)
				 .AsEnumerable()
				 .Select(x =>
				 {
					 return new RezultatiWM.row
					 {
						 Brojudnevniku=x.OdjeljenjeStavka.BrojUDnevniku.ToString(),
						 odjeljeneId=x.OdjeljenjeStavka.Odjeljenje.Id,
						 odjeljeneNaziv=x.OdjeljenjeStavka.Odjeljenje.Oznaka,
						 odjeljeneStavkaId=(int)x.OdjeljenjeStavkaId,
						 Pristupio=x.ispristupio,
						 Rezultatibodovi=x.Bodovi
					 };
				 })
				 .ToList()
			};

			return View("Rezultati", model);
		}

		public IActionResult Zakljucaj(int Id)
		{
			var takmicenje =
				CTX.Takmicenje.Find(Id);

			
				takmicenje.iszakljucano = !takmicenje.iszakljucano;
				CTX.SaveChanges();
			
			
			

			//return "hell yea";

			//return Content("<html><p><i>Hello! You are trying to view <u>something!</u></i></p></html>", "text/html");

			return RedirectToAction("ajax", "AjaxStavke", new { Id = Id });
		}



	}
}
