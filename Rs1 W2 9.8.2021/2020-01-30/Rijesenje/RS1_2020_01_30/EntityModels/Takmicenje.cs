using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_2020_01_30.EntityModels
{
	public class Takmicenje
	{

		[Key]
		public int TakmicenjeId { get; set; }

		public virtual Predmet Predmet { get; set; }

		public int? PredmetId { get; set; }

		public int? Razred { get; set;}

		public DateTime? Datum { get; set; }

		//public int OdjeljenjeId { get; set;}

		//public Odjeljenje Odjeljenje { get; set; }

		public int SkolaId { get; set;}

		public Skola Skola { get; set;}

		public bool iszakljucano { get; set; }



	}
}
