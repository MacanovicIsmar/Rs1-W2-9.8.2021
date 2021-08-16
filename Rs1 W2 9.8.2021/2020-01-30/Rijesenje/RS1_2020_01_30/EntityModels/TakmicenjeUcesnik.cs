using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_2020_01_30.EntityModels
{
	public class TakmicenjeUcesnik
	{
		[Key]
		public int TakmicenjeUcesnikId { get; set; }

		public int? TakmicenjeId { get; set; }

		public virtual Takmicenje Takmicenje { get; set; }

		public bool ispristupio { get; set;}

		public int? OdjeljenjeStavkaId { get; set; }

		public virtual OdjeljenjeStavka OdjeljenjeStavka { get; set; }

		public int Bodovi { get; set; }

	}
}
