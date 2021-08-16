using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_2020_01_30.ViewModels
{
	public class RezultatiWM
	{
		public int skolaId { get; set; }

		public string Skolanaziv { get; set; }

		public int PredmetId { get; set; }

		public string PredmetNaziv { get; set; }

		public int Razred { get; set; }

		public string Datum { get; set; }

		public List<row> SpisakUcesnika { get; set; }



		public class row 
		{
			public int odjeljeneId { get; set; }

			public string odjeljeneNaziv { get; set; }

			public int odjeljeneStavkaId  { get; set; }

			public string Brojudnevniku { get; set; }

			public bool Pristupio { get; set; }

			public int Rezultatibodovi { get; set; }





		}




	}
}
