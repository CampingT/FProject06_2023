using FProjectCampingBackend.Models.EFModels;
using FProjectCampingBackend.Models.Rooms;
using FProjectCampingBackend.Models.ViewModels;
using FProjectCampingBackend.Models.ViewModels.Members;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FProjectCampingBackend.Models.Repositories
{
	public class RoomsRepository
	{

		private readonly AppDbContext _dbContext;

		public RoomsRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IPagedList<RoomsVm> GetRooms(SearchRoomsVm vm, int pageNumber = 1, int pageSize =10)
		{
			IQueryable<Room> rooms = _dbContext.Rooms;

			if (vm.RoomTypeId > 0)
			{
				rooms = rooms.Where(r => r.RoomTypeId == vm.RoomTypeId);
			}

			if (!string.IsNullOrEmpty(vm.RoomtypeName))
			{
				rooms = rooms.Where(r => r.RoomName == vm.RoomtypeName);
			}

			var roomVm = rooms
				 .Select(r => new RoomsVm
				 {
					 Id = r.Id,
					 RoomName = r.RoomName,
					 WeekendPrice = r.WeekendPrice,
					 WeekdayPrice = r.WeekdayPrice,
					 Description = r.Description,
					 RoomTypeName = r.RoomType.Name,
					 Photo = r.Photo
				 })
				 .ToList();
			var pagedRoomsVm = roomVm.OrderBy(r => r.RoomName).ToPagedList(pageNumber, pageSize);

			return pagedRoomsVm;
		}


	}

}