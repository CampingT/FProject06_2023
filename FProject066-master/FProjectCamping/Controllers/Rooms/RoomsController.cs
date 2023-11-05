
using FProjectCamping.Models.EFModels;
using FProjectCamping.Models.ViewModels;
using FProjectCamping.Models.ViewModels.Rooms;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FProjectCamping.Controllers.Rooms
{
	public class RoomsController : Controller
	{
		// GET: Rooms
		[Authorize]
		public ActionResult Roomtype(int roomtypeid = 0, int roomTypeId = 0)
		{
			int roomTypeIdFromDatabase = 0; // 初始化为0，以防获取失败或不需要时的默认值
			if (roomTypeId != 0)
			{
				// 使用 selectedRoomTypeId 来执行特定操作
				var branches = GetRoomTypeVm(roomTypeId);
				ViewBag.hotrooms = branches;
			}
			else
			{
				// 使用 roomtypeid 参数来执行默认操作
				var branches = GetRoomTypeVm(roomtypeid);
				ViewBag.hotrooms = branches;


			}

			// 将 roomTypeIdFromDatabase 和 selectedRoomTypeId 传递给视图
			ViewBag.RoomTypeIdFromDatabase = roomTypeIdFromDatabase;
			ViewBag.SelectedRoomTypeId = roomTypeId;

			return View();
		}
		public List<RoomtypeVM> GetRoomTypeVm(int id = 0)
		{
			var db = new AppDbContext();
			IQueryable<RoomtypeVM> query = db.Rooms.OrderBy(r=>r.RoomName).Include("RoomType")
				.Select(c => new RoomtypeVM
				{
					RoomId = c.Id,
					RoomName = c.RoomName,
					WeekendPrice = c.WeekendPrice,
					WeekdayPrice = c.WeekdayPrice,
					RoomTypeName = c.RoomType.Name,
					RoomtypeId = c.RoomTypeId,
					Description = c.Description,
					Photo =c.Photo
				});

			if (id != 0)
			{
				query = query.Where(c => c.RoomtypeId == id);
			};

			var result = query.OrderBy(c => c.RoomName).ToList();

			//foreach (var r in result)
			//{
			//	r.FileName = db.Photos.FirstOrDefault(c => c.RoomTypeId == r.Id);
			//}

			return result;
		}

		public ActionResult Forestarea()
		{
			string RoomtypeName = "森林";

			var db = new AppDbContext();
			var roomtype = db.RoomTypes.Where(r => r.Name.Contains(RoomtypeName)).ToList();
			List<Room> rooms = new List<Room>();
			foreach (var r in roomtype)
			{
				var roomsWithRoomTypes = db.Rooms
		   .Include(room => room.RoomType)
		   .FirstOrDefault(room => room.RoomTypeId == r.Id);

				if (roomsWithRoomTypes != null)
				{
					rooms.Add(roomsWithRoomTypes);
				}

			}

			ViewBag.Area = roomtype;
			ViewBag.Rooms = rooms;


			return View();
		}
		public ActionResult RiversideDistrict()
		{

			string RoomtypeName = "河畔";
			var db = new AppDbContext();
			var roomtype = db.RoomTypes.Where(r => r.Name.Contains(RoomtypeName)).ToList();

			List<Room> rooms = new List<Room>();
			foreach (var r in roomtype)
			{
				var roomsWithRoomTypes = db.Rooms
				   .Include(room => room.RoomType)
				   .FirstOrDefault(room => room.RoomTypeId == r.Id);

				if (roomsWithRoomTypes != null)
				{
					rooms.Add(roomsWithRoomTypes);
				}

			}

			ViewBag.Area = roomtype;
			ViewBag.Rooms = rooms;


			return View();
		}

		public ActionResult RoomsPartial()
		{
			ViewBag.HotProducts = BranchroomPartial();
			return View();
		}

		private List<RoomtypeVM> BranchroomPartial()
		{
			var db = new AppDbContext();
			var branches = db.Rooms
				.Select(c => new RoomtypeVM
				{
					Id = c.Id,
					RoomName = c.RoomName,
					WeekendPrice = c.WeekendPrice,
					WeekdayPrice = c.WeekdayPrice,

				}).ToList();

			//foreach (var branch in branches)
			//{
			//	branch.FileName = db.Photos.FirstOrDefault(c => c.RoomTypeId == branch.Id);
			//}


			return branches;
		}
	}
}