using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_2020_01_30.ViewModels
{
	public class PrikazTakmicenjaModel
	{
		public int SkolaId { get; set; }

		public string Skola { get; set; }

		public string Razred { get; set; }

		public List<row> Lista { get; set; }
		
		//public int TakmicenjeId { get; set; }

		public class row
		{

			public int TakmicenjeId { get; set; }

			public int PredmetId { get; set; }

			public string PredmetNaziv { get; set; }

			public string Razred { get; set; }

			public string Datum { get; set; }

			public int Brojucesnika { get; set; }

			//public int SkolaId { get; set; }

			public string SkolaNaziv { get; set; }

			public string odjeljenje { get; set; }

			public string ime { get; set; }

			public string prezime  { get; set; }


		}




	}
}
