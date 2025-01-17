﻿using Microsoft.AspNetCore.Mvc;
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

		public IActionResult Uredi(int Id ,int TakId)
		{

			TakmicenjeUcesnik ucesnik = CTX.TakmicenjeUcesnik.Find(Id);


			var model = new UrediUcesnikaWM
			{
				bodovi = ucesnik.Bodovi,
				TakmicenjeId = TakId,
				ucesnikId = ucesnik.TakmicenjeUcesnikId,

				Lista = CTX.TakmicenjeUcesnik
				.Where(X => X.TakmicenjeId == TakId)
				.Include(X=>X.OdjeljenjeStavka)
				.Include(X => X.OdjeljenjeStavka.Odjeljenje)
				.Include(X => X.OdjeljenjeStavka.Ucenik)
				.AsEnumerable()
				.Select(X => {

					if (X.TakmicenjeUcesnikId == Id)
					{
						return new SelectListItem
						{
							Selected = true,
							Value = X.TakmicenjeUcesnikId.ToString(),
							Text = X.OdjeljenjeStavka.Odjeljenje.Oznaka + "-" + X.OdjeljenjeStavka.Ucenik.ImePrezime
						};
					}
					else
					{
						return new SelectListItem
						{

							Value = X.TakmicenjeUcesnikId.ToString(),
							Text = X.OdjeljenjeStavka.Odjeljenje.Oznaka + "-" + X.OdjeljenjeStavka.Ucenik.ImePrezime

						};

					}
			
				}).ToList()		
			};


			


















			return PartialView("UrediDodajAjax",model);
		}

		public IActionResult Snimi(UrediUcesnikaWM model)
		{


			TakmicenjeUcesnik postoji = CTX.TakmicenjeUcesnik
				.Where(X => X.TakmicenjeId == model.TakmicenjeId)
				.Where(X => X.TakmicenjeUcesnikId == model.ucesnikId)
				.FirstOrDefault();

			if (postoji == null)
			{
				var takmicar = CTX.TakmicenjeUcesnik.Find(model.ucesnikId);


				TakmicenjeUcesnik ucesnik = new TakmicenjeUcesnik
				{
					Bodovi = model.bodovi,
					ispristupio = takmicar.ispristupio,
					OdjeljenjeStavkaId = takmicar.OdjeljenjeStavkaId,
					TakmicenjeId = model.TakmicenjeId,
				};

				CTX.TakmicenjeUcesnik.Add(ucesnik);
				CTX.SaveChanges();



			}
			else
			{

				var takmicar = CTX.TakmicenjeUcesnik.Find(model.ucesnikId);
				takmicar.Bodovi = model.bodovi;
				CTX.SaveChanges();		
			
			}





			return RedirectToAction("ajax",new {id=model.TakmicenjeId});
		}

		public IActionResult DodajForm(int Id)
		{
			var listadodanitakmicara = CTX.TakmicenjeUcesnik
				.Where(X => X.TakmicenjeId == Id)
				.Select(X=>X.OdjeljenjeStavkaId)
				.ToList();






			var model = new UrediUcesnikaWM
			{
				bodovi = 0,
				TakmicenjeId = Id,
				ucesnikId = 0,
				Lista = CTX.TakmicenjeUcesnik
			   .Where(X => listadodanitakmicara.Contains(X.OdjeljenjeStavkaId) == false)
			   .Include(X => X.OdjeljenjeStavka)
			   .Include(X => X.OdjeljenjeStavka.Ucenik)
			   .AsEnumerable()
			   .Select(X =>
			   {
				   return new SelectListItem
				   {
					   Value = X.TakmicenjeUcesnikId.ToString(),
					   Text = X.OdjeljenjeStavka.Ucenik.ImePrezime,


				   };
			   }).ToList(),
			};

			return PartialView("UrediDodajAjax",model);

			//i => i.Odjeljenje.Razred == Takmicenje.Razred &&
			//					Takmicari.Contains(i.Id) == false
		}

		public IActionResult UpdateBodovi(int Id, int bodovi)
		{
			var takmicar = CTX.TakmicenjeUcesnik.Find(Id);
			takmicar.Bodovi = bodovi;
			CTX.SaveChanges();




			return RedirectToAction("ajax", new { id = takmicar.TakmicenjeId });


		}

		//UpdateBodovi? Id = " + Id + " & bodovi = " + bodovi);

	}
}
