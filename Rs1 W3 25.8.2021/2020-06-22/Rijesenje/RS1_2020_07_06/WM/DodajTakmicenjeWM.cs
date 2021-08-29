using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_2020_07_06.WM
{
	public class DodajTakmicenjeWM
	{
		public int PredmetId { get; set; }

		public List<SelectListItem> ListaPredmeta { get; set; }

		public int SkolaId { get; set; }

		public List<SelectListItem> ListaSkola { get; set; }

		public DateTime Datum { get; set; }

	}
}
