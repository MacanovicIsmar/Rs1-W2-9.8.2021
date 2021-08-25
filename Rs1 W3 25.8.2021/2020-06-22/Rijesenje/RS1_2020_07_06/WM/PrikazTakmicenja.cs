using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_2020_07_06.WM
{
	public class PrikazTakmicenja
	{
		public int SkolaId { get; set; }

		public List<SelectListItem> ListaSkola { get; set;}

		public List<row> Listatakmicenja { get; set;}


		public class row
		{
			public int TakmicenjeId { get; set; }

			public string SkolaNaziv { get; set; }

			public int Razred { get; set; }

			public string Datum { get; set; }

			public string PredmetNaziv { get; set; }

			public string SkolaNazivN { get; set; }

			public string OdjeljenjeOznaka { get; set; }

			public string ImeUcesnika { get; set; }

			public int brojucesnika { get; set;}


		}












	}
}
