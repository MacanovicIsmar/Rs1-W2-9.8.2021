using RS1_Ispit_asp.net_core.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_2020_07_06.EntityModels
{
	public class Takmicenje
	{
		[Key]
		public int Id { get; set; }

		public int? SkolaId { get; set; }

		public virtual Skola Skola {get; set;}

		public int? PredmetId { get; set; }

		public virtual Predmet Predmet { get; set; }

		public int Razred { get; set; }

		public DateTime Datum { get; set;}

		public bool zakkljucano { get; set;}


	}
}
