using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_2020_01_30.ViewModels
{
	public class DodajTakmicenjeWM
	{
		public int SkolaId { get; set; }

		public string SkolaNaziv { get; set; }

		public List<SelectListItem> ListaSkola { get; set;}

		public int PredmetId { get; set; }
       
		public List<SelectListItem> ListaPredmeta { get; set; }
		
		public List<SelectListItem> ListaRazreda { get; set; }

		public string Razred { get; set; }

		public DateTime Datum { get; set; }




	}

}
