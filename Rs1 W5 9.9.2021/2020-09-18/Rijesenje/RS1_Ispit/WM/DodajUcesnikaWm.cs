using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.WM
{
	public class DodajUcesnikaWm
	{
		public int TakmicenjeId { get; set; }

		public int ucesnikid { get; set; }

		public List<SelectListItem> spisakucesnika { get; set; }

		public int Bodovi { get; set; }

		public string flag { get; set; }

	}
}
