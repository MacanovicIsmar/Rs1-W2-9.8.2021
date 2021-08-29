using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RS1_2020_07_06.EntityModels;
using RS1_2020_07_06.WM;
using RS1_Ispit_asp.net_core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_2020_07_06.Controllers
{
	public class TakmicenjeController : Controller
	{
		private readonly MojContext konekcija;



        public TakmicenjeController(MojContext konekcija_)
		{
			konekcija = konekcija_;

        }



		
		
		public IActionResult Index()
		{

			var Model = new PrikazTakmicenja
			{
				SkolaId = 0,
				ListaSkola = konekcija.Skola
			  .AsEnumerable()
			  .Select(X =>
			  {
				  return new SelectListItem
				  {
					  Value = X.Id.ToString(),
					  Text = X.Naziv.ToString()

				  };

			  }).ToList(),

				Listatakmicenja = konekcija.Takmicenja
				.AsEnumerable()
				.Select(X => {

					int brojucesnika_ = konekcija.TakmicenjeUcesnici
					.Where(Y => Y.pristupio == false).Count();

					DateTime Datum = X.Datum == null ? new DateTime(2000, 1, 1) : X.Datum;

					TakmicenjeUcesnik najbolji = konekcija.TakmicenjeUcesnici
					.Include(Y=>Y.OdjeljenjeStavka)
					.Include(Y=>Y.OdjeljenjeStavka.Ucenik)
					.Include(Y =>Y.OdjeljenjeStavka.Odjeljenje)
					.Include(Y => Y.OdjeljenjeStavka.Odjeljenje.Skola)
					.Where(Y=>Y.TakmicenjeId==X.Id)
					.OrderByDescending(Y => Y.Bodovi)
					.First();
					


					return new PrikazTakmicenja.row
					{
						brojucesnika=brojucesnika_,
						Datum= Datum.ToString(format:"dd.mm.yyyy"),
						ImeUcesnika=najbolji.OdjeljenjeStavka.Ucenik.ImePrezime,
						OdjeljenjeOznaka=najbolji.OdjeljenjeStavka.Odjeljenje.Oznaka,
						PredmetNaziv=konekcija.Predmet.Find(X.PredmetId).Naziv,
						Razred=X.Razred,
						SkolaNaziv=konekcija.Skola.Find(X.SkolaId).Naziv,
						SkolaNazivN=najbolji.OdjeljenjeStavka.Odjeljenje.Skola.Naziv,
						TakmicenjeId=X.Id

					};


				}).ToList()
				

			};
			Model.ListaSkola.Add(new SelectListItem { Text = "not selected", Value = "0", Selected = true });











			return View("PrikazTakmicenja",Model);
		}

		public IActionResult DodajTakmicenjeView()
		{
			var model = new DodajTakmicenjeWM
			{
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
					   Value = X.Id.ToString()

				   };
			   }).ToList(),

				ListaSkola = konekcija
				.Skola
				.AsEnumerable()
				.Select(
					X => {
						return new SelectListItem
						{
							Text=X.Naziv,
							Value=X.Id.ToString()

						};


					}).ToList(),

				PredmetId=0,
				SkolaId=0,






			};






			return View(model);
		}

		public IActionResult DodajTakmicenje(DodajTakmicenjeWM Model)
		{



			var takmicenje = new Takmicenje
			{
				Datum = Model.Datum,
				PredmetId = Model.PredmetId,
				Razred = 1,
				SkolaId = Model.SkolaId,
				zakkljucano = false,
			};

			konekcija.Takmicenja.Add(takmicenje);
			konekcija.SaveChanges();


			var UcesniciLista = konekcija
				.DodjeljenPredmet
				.Where(X => X.PredmetId == Model.PredmetId)
				.Where(X => X.ZakljucnoKrajGodine == 5)
				.AsEnumerable()
				.Select(X =>
				{

					return new TakmicenjeUcesnik
					{
						Bodovi = 0,
						OdjeljenjeStavkaId = X.OdjeljenjeStavkaId,
						pristupio = false,
						TakmicenjeId = takmicenje.Id

					};



				}).ToList();


			foreach (var a in UcesniciLista)
			{

				bool flag = konekcija
					.DodjeljenPredmet
					.Where(X => X.OdjeljenjeStavkaId == a.OdjeljenjeStavkaId)
					.Select(X => X.ZakljucnoKrajGodine)
					.Average() > 4.0;

				if (flag)
				{


					konekcija.TakmicenjeUcesnici.Add(a);
				
				
				}


			
			
			
			}

			konekcija.SaveChanges();















			return RedirectToAction("Index");
		}

	}
}
