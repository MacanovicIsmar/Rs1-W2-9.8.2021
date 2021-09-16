using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.Wm
{
	public class DodajtakmicenjeWM
	{
		public int SkolaId { get; set; }

		public List<SelectListItem> Listaskola { get; set; }

		public string Predmetnaziv { get; set; }

		public List<SelectListItem> ListaPredmeta { get; set; }

		public DateTime Datum { get; set; }





	}
}
