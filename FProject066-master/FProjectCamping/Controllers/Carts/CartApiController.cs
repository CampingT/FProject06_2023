
using FProjectCamping.Models.EFModels;
using FProjectCamping.Models.Respositories;
using FProjectCamping.Models.Services;
using FProjectCamping.Models.ViewModels.Carts;
using FProjectCamping.Models.ViewModels.Rooms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FProjectCamping.Controllers.Carts
{
	public class CartApiController : ApiController
	{
        //[HttpPost]
        //[Route("api/Cart/AddCartItem")]
        //public IHttpActionResult AddCartItem([FromBody] CartItemsVm requestData)
        //{
        //	var service = new CartService();
        //	string buyer = User.Identity.Name; // 買家帳號
        //	var result = service.AddToCart(buyer, requestData); //加入購物車


        //	return Ok(result);
        //}



        [HttpPost]
        [Route("api/Cart/AddCartItem")]
        public IHttpActionResult AddCartItem([FromBody] CartItemRequestModel requestData)
        {
			new CartItemsRepository().GetRoomPrice(requestData);

            var param = new CartItemsVm()
            {
				RoomId = requestData.RoomId,
                CheckInDate = requestData.CheckInDate,
                CheckOutDate = requestData.CheckOutDate,
                ExtraBed = requestData.ExtraBed,
                ExtraBedPrice = requestData.ExtraBedPrice,
                Days = requestData.Days,
                SubTotal = requestData.RoomPrice,
			};

            var service = new CartService();
            string buyer = User.Identity.Name; // 買家帳號
            var result = service.AddToCart(buyer, param); //加入購物車

            return Ok(result);
        }



	}
}