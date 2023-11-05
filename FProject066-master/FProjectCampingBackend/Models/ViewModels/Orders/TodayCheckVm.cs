using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace FProjectCampingBackend.Models.ViewModels.Orders
{
    public class TodayCheckVm
    {

        /// <summary>
        /// 訂單編號
        /// </summary>
        [Display(Name = "訂單編號")]
        public string OrderNumber { get; set; }


        /// <summary>
        /// 金額
        /// </summary>
        [Display(Name = "金額")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public int SubTotal { get; set; }

        /// <summary>
        /// 會員帳號
        /// </summary>
        [Display(Name = "會員帳號")]
        public string Account { get; set; }

        public string Photo { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public int Days { get; set; }

        public string RoomName { get; set; }
        public string RoomTypeName { get; set; }

        [StringLength(1000)]
        public string RoomPhoto { get; set; }

    }
}