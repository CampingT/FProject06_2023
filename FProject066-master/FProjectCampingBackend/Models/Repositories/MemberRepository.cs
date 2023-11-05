using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using FProjectCampingBackend.Models.ViewModels;
using FProjectCampingBackend.Models.EFModels;
using Dapper;
using FProjectCampingBackend.Models.Services;
using FProjectCampingBackend.Models.ViewModels.Members;
using FProjectCampingBackend.Models.ViewModels.Orders;
using FProjectCampingBackend.Models.ViewModels.Home;
using PagedList;
using System.Net.NetworkInformation;

namespace FProjectCampingBackend.Models.Repositories
{
    public class MemberRepository
    {
        private AppDbContext db;


        public MemberRepository(AppDbContext dbContext)
        {
            db = dbContext;
        }
        private readonly IDbConnection _dbConnection;

        public MemberRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
		public List<MemberVm> GetMembers(MemberSearchCriteria vm)
		{
			IQueryable<Member> query = db.Members;

			if (!string.IsNullOrEmpty(vm.Name))
			{
				query = query.Where(m => m.Name.Contains(vm.Name));
			}

			if (!string.IsNullOrEmpty(vm.Account))
			{
				query = query.Where(m => m.Account.Contains(vm.Account));
			}

			if (vm.FirstTime != null)
			{
				query = query.Where(m => m.CreatedTime >= vm.FirstTime);
			}

			if (vm.EndTime != null)
			{
				DateTime endDatePlusOneDay = vm.EndTime.Value.AddDays(1);
				query = query.Where(m => m.CreatedTime <= endDatePlusOneDay);
			}

			if (vm.Enabled != null)
			{
				query = query.Where(m => m.Enabled == vm.Enabled);
			}

			if (vm.IsConfirmed != null)
			{
				query = query.Where(m => m.IsConfirmed == vm.IsConfirmed);
			}

			var membersVm = query
				.OrderByDescending(m => m.Id)
				.ToList()
				.Select(m => m.ToMemberVm())
				.ToList();

			return membersVm;
		}



		public List<Member> GetLatestMembers()
        {
            string sql = "SELECT TOP 5 * FROM Members ORDER BY Id DESC";
            return _dbConnection.Query<Member>(sql).ToList();
        }


public List<OrderMemberNewVm> GetNewOrders()
{
    string connectionString = "data source=.\\sql2022;initial catalog=FDB06;user id=sa5;password=sa5;MultipleActiveResultSets=True;App=EntityFramework\" providerName=\"System.Data.SqlClient";

    using (IDbConnection dbConnection = new SqlConnection(connectionString))
    {
        dbConnection.Open();

        string sql = @"
            SELECT TOP 5 o.OrderNumber, oi.RoomName, oi.CheckInDate, oi.Days, m.Account, o.TotalPrice, m.Photo, o.Status
            FROM Orders o 
            INNER JOIN Members m ON o.MemberId = m.Id
            INNER JOIN OrderItems oi ON oi.OrderId = o.Id
            WHERE oi.CheckInDate > GETDATE()
            AND o.Status = 2
            ORDER BY o.OrderTime DESC";

        var orderMemberNewVms = dbConnection.Query<OrderMemberNewVm>(sql).ToList();

        return orderMemberNewVms;
    }
}

        public List<TodayCheckVm> GetTodayCheckOrder()
        {
            string connectionString = @"data source=.\sql2022;initial catalog=FDB06;user id=sa5;password=sa5;MultipleActiveResultSets=True;App=EntityFramework";
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                string sql = @"
            SELECT o.OrderNumber, o.Status, oi.CheckInDate, oi.CheckOutDate, m.Account, m.Photo, r.RoomName, r.Photo as RoomPhoto, 
            oi.SubTotal, rt.Name as RoomTypeName, oi.Days
            FROM Orders o
            INNER JOIN Members m ON o.MemberId = m.Id
            INNER JOIN OrderItems oi ON oi.OrderId = o.Id
            INNER JOIN Rooms r ON r.Id = oi.RoomId
            INNER JOIN RoomTypes rt ON rt.Id = r.RoomTypeId
            WHERE oi.CheckInDate >= @StartDate
            AND oi.CheckInDate < @EndDate
            AND o.Status = @Status
            ORDER BY r.RoomName ASC";

                var parameters = new
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1),
                    Status = 2
                };

                var todayCheckVms = dbConnection.Query<TodayCheckVm>(sql, parameters).ToList();
                return todayCheckVms;
            }
        }

        public List<LoginVm> GetUser()
        {
            string sql = @"SELECT Account FROM Users";

            var loginvm = _dbConnection.Query<LoginVm>(sql).ToList();

            return loginvm;
        }


    }
}