using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.Wmodels
{
	public class PrikazTakmicenjaWM
	{
		public int SkolaId { get; set; }

		public List<SelectListItem> ListaSkola { get; set; }

		public string razred { get; set; }

		public List<SelectListItem> ListaRazreda { get; set; }

		public List<row> Listatakmicenja { get; set; }

		public class row 
		{
			public int SkolaId { get; set; }

			public string Skolanaziv { get; set; }

			public int Razred { get; set; }

			public string Datum { get; set; }

			public int Predmetid { get; set; }

			public string NazivPredmeta { get; set; }

			public string Nskola { get; set; }

			public string Nodjeljenje { get; set; }

			public string NIme { get; set; }

		}















	}
}
