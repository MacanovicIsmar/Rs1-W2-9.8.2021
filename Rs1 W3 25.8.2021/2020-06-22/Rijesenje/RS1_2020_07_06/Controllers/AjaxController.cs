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
	public class AjaxController : Controller
	{
		private readonly MojContext konekcija;



		public AjaxController(MojContext konekcija_)
		{
			konekcija = konekcija_;

		}



		public IActionResult Index()
		{
			return View();
		}




		public IActionResult Filtriraj(int Id)
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
					.Include(Y => Y.OdjeljenjeStavka)
					.Include(Y => Y.OdjeljenjeStavka.Ucenik)
					.Include(Y => Y.OdjeljenjeStavka.Odjeljenje)
					.Include(Y => Y.OdjeljenjeStavka.Odjeljenje.Skola)
					.Where(Y => Y.TakmicenjeId == X.Id)
					.OrderByDescending(Y => Y.Bodovi)
					.First();



					return new PrikazTakmicenja.row
					{

						brojucesnika = brojucesnika_,
						Datum = Datum.ToString(format: "dd.mm.yyyy"),
						ImeUcesnika = najbolji.OdjeljenjeStavka.Ucenik.ImePrezime,
						OdjeljenjeOznaka = najbolji.OdjeljenjeStavka.Odjeljenje.Oznaka,
						PredmetNaziv = konekcija.Predmet.Find(X.PredmetId).Naziv,
						Razred = X.Razred,
						SkolaNaziv = konekcija.Skola.Find(X.SkolaId).Naziv,
						SkolaNazivN = najbolji.OdjeljenjeStavka.Odjeljenje.Skola.Naziv,
						TakmicenjeId = X.Id,
						SkolaId = X.SkolaId == null ? 0 : X.SkolaId.Value


					};


				}).ToList()


			};
			Model.ListaSkola.Add(new SelectListItem { Text = "not selected", Value = "0", Selected = true });


			if (Id != 0)
			{

				Model.Listatakmicenja = Model.Listatakmicenja.Where(X => X.SkolaId == Id).ToList();


			}










			return PartialView("Filtriraj", Model);
		}


	}
}
