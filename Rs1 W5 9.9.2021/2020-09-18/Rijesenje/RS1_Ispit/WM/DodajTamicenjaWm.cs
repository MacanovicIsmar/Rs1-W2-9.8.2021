using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.WM
{
	public class DodajTamicenjaWm
	{

		public int SkolaId { get; set; }

		public string PredmetNaziv { get; set; }

		public List<SelectListItem> spisakskola { get; set; }

		public List<SelectListItem> spisakpredmeta { get; set; }

		public DateTime Datum { get; set; }


	}
}
