using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.WM
{
	public class PrikaztakmicenjaWm
	{

		public int SkolaId { get; set; }

		public string PredmetNaziv { get; set; }

		public List<SelectListItem> spisakskola { get; set; }

		public List<SelectListItem> spisakpredmeta { get; set; }

		public List<row> Spisaktakmicenja { get; set; }



		public class row {

			public string Skolanaziv { get; set; }

			public string Razred { get; set; }

			public string Datum { get; set; }

			public string Predmet { get; set; }

			public string Nskola { get; set; }

			public string Nodljenje { get; set; }

			public string NIme { get; set; }

			public int TakmicenjeId { get; set; }








		}
















	}
}
