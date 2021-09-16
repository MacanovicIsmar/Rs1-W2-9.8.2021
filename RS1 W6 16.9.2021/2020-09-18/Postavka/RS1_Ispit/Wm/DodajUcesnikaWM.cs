using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.Wm
{
	public class DodajUcesnikaWM
	{
		public int UcesnikId { get; set; }

		public List<SelectListItem> Listucesnika { get; set; }

		public int Bodovi { get; set; }

		public string status { get; set; }

		public int TakId { get; set; }

	}
}
