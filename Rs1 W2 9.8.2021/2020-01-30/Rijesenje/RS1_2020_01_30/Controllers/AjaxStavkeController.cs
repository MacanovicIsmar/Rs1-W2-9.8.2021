using Microsoft.AspNetCore.Mvc;
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
	public class AjaxStavkeController : Controller
	{
		private readonly MojContext CTX;

		//test for git
		//test for git 2
		//test for git 3
		public AjaxStavkeController(MojContext context)
		{

			CTX = context;


		}



		public IActionResult Index()
		{
			return View();
		}


		public IActionResult ajax(int Id)
		{

			var paket = CTX.Takmicenje.Find(Id);


			var model = new RezultatiWM
			{
			    takmicenjeId=Id,	
				iszakljucano=paket.iszakljucano,
				SpisakUcesnika = CTX.TakmicenjeUcesnik
				 .Where(x => x.TakmicenjeId == Id)
				 .Include(X => X.OdjeljenjeStavka)
				 .Include(X => X.OdjeljenjeStavka.Odjeljenje)
				 .AsEnumerable()
				 .Select(x =>
				 {
					 return new RezultatiWM.row
					 {
						 Id = x.TakmicenjeUcesnikId,
						 Brojudnevniku = x.OdjeljenjeStavka.BrojUDnevniku.ToString(),
						 odjeljeneId = x.OdjeljenjeStavka.Odjeljenje.Id,
						 odjeljeneNaziv = x.OdjeljenjeStavka.Odjeljenje.Oznaka,
						 odjeljeneStavkaId = (int)x.OdjeljenjeStavkaId,
						 Pristupio = x.ispristupio,
						 Rezultatibodovi = x.Bodovi
					 };
				 })
				 .ToList()
			};


			return PartialView(model);
		}

		public IActionResult Uredi(int Id)
		{
			var ucesnik = new TakmicenjeUcesnik();

			if (Id != 0)
			{
				ucesnik = CTX.TakmicenjeUcesnik.Find(Id);
			
			
			
			
			
			}















			return View();
		}




	}
}
