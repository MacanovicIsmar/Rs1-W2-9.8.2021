using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.Wm
{
	public class UcesniciPrikazWM
	{
		//od proslih 

		public int TakmicenjaId { get; set; }


		public string Skolanaziv { get; set; }


		public string Predmetnaziv { get; set; }

		public string Razred { get; set; }

		public string Datum { get; set; }

		public bool Zakljucano { get; set; }


		public List<row> ListaUcesnika { get; set; }


		public class row 
		{
			public string Odjeljenje { get; set; }

			public string BrojUdnevniku { get; set; }

			public string Pristupio { get; set; }

			public string Rezultatbodovi { get; set; }

			public int UcesnikId { get; set; }


		}


	}
}
