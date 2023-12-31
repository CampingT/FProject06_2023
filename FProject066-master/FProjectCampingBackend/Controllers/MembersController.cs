﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using FProjectCampingBackend.Models.EFModels;
using FProjectCampingBackend.Models.Repositories;
using FProjectCampingBackend.Models.Services;
using FProjectCampingBackend.Models.ViewModels;
using FProjectCampingBackend.Models.ViewModels.Members;
using PagedList;
using PagedList.Mvc;

namespace FProjectCampingBackend.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private AppDbContext db = new AppDbContext();
        private readonly DropdownListService _dropdownListService =  new DropdownListService();


		// GET: Members
		public ActionResult Index(MemberSearchCriteria vm, int? page, int pageSize = 10)
		{
			ViewData["Enabled"] = _dropdownListService.GetEnabledDropdownList();
			ViewData["IsConfirmed"] = _dropdownListService.GetIsConfirmedDropdownList();

			var repo = new MemberRepository(db);

			// 計算pageNumber
			int pageNumber = page ?? 1;

			// 呼叫GetMembers方法以獲取查詢結果
			var query = repo.GetMembers(vm);
            var memberpage = query.ToPagedList(pageNumber, pageSize);



            return View(memberpage); // 返回PagedList對象
		}






		// POST: Members/Create
		// 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
		// 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Account,Password,Name,Email,PhoneNum,Birthday,Enabled,Photo,CreatedTime,IsConfirmed,ConfirmCode")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);

            if (member == null)
            {
                return HttpNotFound();
            }

            var memberEditVm = member.ToMemberEditVm();

            return View(memberEditVm);
        }

        // POST: Members/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, bool enabled)
        {
     
            var member = db.Members.Find(id);

			if (member == null)
            {
                return HttpNotFound(); 
            }

            // 更新 Enabled 属性
            member.Enabled = enabled;

            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
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
