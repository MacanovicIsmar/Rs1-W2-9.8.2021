using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_2020_07_06.WM
{
	public class RezultatiTakmicenjaView
	{
		public int TakmicenjeId { get; set; }

		public string skolaNaziv { get; set; }

		public string PredmetNaziv { get; set; }

		public int Razred { get; set; }

		public string Datum { get; set; }

		public string Iszakljucano { get; set; }

		public List<row> listatakmicara { get; set;}

		public class row
		{
			public int TakmicenjeucesnikId { get; set; }

			public string Odjeljenje { get; set; }

			public int BrojuDnevniku { get; set; }

			public string pristupio { get; set; }

			public int bodovi { get; set; }


		}





	}
}
