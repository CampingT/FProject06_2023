        let checkInDate;
        let checkOutDate;
        let daysDifference;

        const arDateInput = document.getElementById('ar_date');
        const deDateInput = document.getElementById('de_date');

        document.addEventListener("DOMContentLoaded", function () {
            var 今天 = new Date();
            今天.setDate(今天.getDate() + 1);
            arDateInput.min = 今天.toISOString().split("T")[0];

            arDateInput.addEventListener("change", function () {
                var selectedDate = new Date(arDateInput.value);
                checkInDate = selectedDate;

                // 设置退房日期的最小值为入住日期的隔天
                var nextDay = new Date(selectedDate);
                nextDay.setDate(selectedDate.getDate() + 1);
                deDateInput.min = nextDay.toISOString().split("T")[0];

                // 检查退房日期是否早于入住日期
                if (new Date(deDateInput.value) < selectedDate) {
                    deDateInput.value = arDateInput.value; // 将退房日期设置为入住日期
                }

                var maxDate = new Date(selectedDate);
                maxDate.setDate(selectedDate.getDate() + 14);
                deDateInput.max = maxDate.toISOString().split("T")[0];
            });
        });
        $(document).ready(function () {
            // 獲取按鈕元素
            const btnOrder = $(".btnOrder");

            // 初始化為全局變量
            let selectedRoomType;
            let checkInDateValue;
            let checkOutDateValue;

            // 初始時禁用按鈕
            btnOrder.prop('disabled', true);

            // 監聽 RoomtypeDropdown、ar_date 和 de_date 字段的變化
            $("#RoomtypeDropdown, #ar_date, #de_date, #booking").on("change", function () {
                selectedRoomType = $("#RoomtypeDropdown").val();
                checkInDateValue = arDateInput.value;
                checkOutDateValue = deDateInput.value;

                // 如果所有字段都已選擇，啟用按鈕，否則禁用按钮
                if (selectedRoomType && checkInDateValue && checkOutDateValue && $("#booking").is(":checked")) {
                    btnOrder.prop('disabled', true);

                } else {
                    btnOrder.prop('disabled', false);
                }
            });

            // 添加按鈕點擊事件監聽器
            btnOrder.click(function () {
                // 檢查是否所有字段都已選擇
                if (selectedRoomType && checkInDateValue && checkOutDateValue) {
                    const extraBed = false;
                    const roomId = $(this).data("roomid");

                    // 計算天數
                    const timeDifference = new Date(checkOutDateValue) - new Date(checkInDateValue);
                    daysDifference = Math.floor(timeDifference / (1000 * 60 * 60 * 24));

                    let SubTotal = 0;
                    let currentDate = new Date(checkInDateValue);
                    const hasExtraBed = true; /* 根据你的逻辑判断是否有加床 */

                    //while (currentDate < new Date(checkOutDateValue)) {
                    //    const dayOfWeek = currentDate.getDay(); 
                    //    let dailyPrice = 0;

                    //    if (dayOfWeek === 6 || dayOfWeek === 5) {
                    //        // 如果是星期五或星期六，使用價格weekendPrice
                    //        dailyPrice = $(this).data("weekendPrice");
                    //        console.log(dailyPrice);
                    //    } else {
                           
                    //        dailyPrice = $(this).data("weekdayPrice");
                    //        console.log(dailyPrice);
                    //    }

                    //    SubTotal += dailyPrice;

                    //    // 增加一天
                    //    currentDate.setDate(currentDate.getDate() + 1);
                    //}

                    const requestData = {
                        RoomId: roomId,
                        CheckInDate: checkInDateValue,
                        CheckOutDate: checkOutDateValue,
                        ExtraBed: extraBed,
                        Days: daysDifference,
                        //SubTotal:SubTotal
                      
                    };



                    $.ajax({
                        url: '/api/Cart/AddCartItem',
                        type: 'POST',
                        contentType: 'application/json', // 設置 Content-Type 為 JSON
                        data: JSON.stringify(requestData), // 將數據轉json
                        success: function (data) {
                            console.log('成功響應:', data);
                            

                            if (data === '加入購物車~') {
                                //updateCartItemCount(data.count);
                                //console.log(data.count);
                                Swal.fire({
                                 
                                    icon: 'success',
                                    title: '已加入購物車',
                                    showConfirmButton: false,
                                    timer: 1500
                                });
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: '不可新增重複的項目',
                                });
                            }
                        },

                        error: function (xhr, textStatus, errorThrown) {
                            console.error('發生錯誤:', errorThrown);
                        }
                    });
                } else {
                    // 如果有任何一个字段未选择,禁用按钮或显示错误消息
                    Swal.fire({
                        icon: 'error',
                        title: '請填選日期',
                    });
               
                }
            });

            // 在执行搜索后启用"加入訂單"
            $("#booking").click(function () {
                //搜尋邏輯

                // 啟用加入訂單
                btnOrder.prop('disabled', false);
            });
        });

