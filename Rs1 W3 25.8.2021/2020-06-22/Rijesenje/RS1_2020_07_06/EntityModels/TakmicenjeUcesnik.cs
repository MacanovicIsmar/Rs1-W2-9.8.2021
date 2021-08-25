using RS1_Ispit_asp.net_core.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_2020_07_06.EntityModels
{
	public class TakmicenjeUcesnik
	{
		[Key]
		public int Id { get; set; }

		public int? OdjeljenjeStavkaId { get; set;}

		public virtual OdjeljenjeStavka OdjeljenjeStavka { get; set;}
		
		public bool pristupio { get; set; }

		public int Bodovi { get; set;}

		public int? TakmicenjeId { get; set; }

		public virtual Takmicenje Takmicenje { get; set; }





	}
}
