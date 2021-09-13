using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.WM
{
	public class PrikazUcesnikaWm
	{

		public int TakmicenjeId { get; set; }

		public string SkolaNaziv { get; set; }

		public string PredmetNaziv { get; set; }

		public string Razred { get; set; }

		public string Datum { get; set; }

		public bool Zakljucano { get; set; }

		public List<row> Spisakucesnika { get; set; }



		public class row {

			public int UcesnikId { get; set; }

			public string odjeljenje { get; set; }

			public string BrojUdnevniku { get; set; }

			public string Pristupio { get; set; }

			public string Rezultatbodovi { get; set; }

			


		}
















	}
}
