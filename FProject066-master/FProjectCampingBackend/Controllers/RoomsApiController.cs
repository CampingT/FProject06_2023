using FProjectCampingBackend.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace FProjectCampingBackend.Controllers
{
  
    public class RoomsApiController : ApiController
    {
        private AppDbContext db = new  AppDbContext();

        [HttpDelete]
        [Route("api/Rooms/Delete/{id}")]
        public IHttpActionResult Index(int id)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }
            bool hasRelatedOrderItems = db.OrderItems.Any(o => o.RoomId == id);

            if (hasRelatedOrderItems)
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responseMessage.Content = new StringContent("無法刪除因為有存在該房關聯的訂單", Encoding.UTF8, "text/plain");

                return ResponseMessage(responseMessage);
            }


            db.Rooms.Remove(room);
            db.SaveChanges();

            return Ok();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
