using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.Wm
{
	public class TakmicenjePrikazWM
	{
		public int SkolaId { get; set; }

		public List<SelectListItem> Listaskola { get; set; }

		public string Predmetnaziv { get; set; }

		public List<SelectListItem> ListaPredmeta { get; set; }

		public List<row> ListaTakmicenja { get; set; }


		public class row 
		{
			public string Skolanaziv { get; set; }

			public string razred { get; set; }

			public string Datum { get; set; }

			public string Predmet { get; set; }

			public string Nskola  { get; set; }

			public string Nodjeljenje { get; set; }

			public string Nime { get; set; }

			public int TakmicenjeId { get; set; }

		}


	}
}
