using FProjectCamping.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FProjectCamping.Models.ViewModels.Rooms
{
    public class RoomtypeVM
	{
		public int RoomtypeId { get; set; }
		public int Id { get; set; }
		public int RoomId { get; set; }
		public string RoomName { get; set; }
		public string Description { get; set; }

		[DisplayFormat(DataFormatString = "{0:#,#}")]
		public int WeekendPrice { get; set; }

		[DisplayFormat(DataFormatString = "{0:#,#}")]
		public int WeekdayPrice { get; set; }
		public string RoomTypeName { get; set; }
		public string Photo { get; set; }

	}
}