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
				SkolaId = model.SkolaId
			};

			return View();
		
		}


	}
}
