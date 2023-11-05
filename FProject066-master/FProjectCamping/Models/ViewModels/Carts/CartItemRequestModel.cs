using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FProjectCamping.Models.ViewModels.Carts
{
    public class CartItemRequestModel
	{
		public int RoomId { get; set; }
		public int CartId { get; set; }
		public DateTime CheckInDate { get; set; }
		public DateTime CheckOutDate { get; set; }
        public int RoomPrice { get; set; }

        public bool ExtraBed { get; set; }
		public int ExtraBedPrice { get; set; }
		public int Days { get; set; }

		public int Subtotal { get; set; }


	}

}