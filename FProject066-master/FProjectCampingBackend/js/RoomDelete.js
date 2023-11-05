
document.addEventListener("DOMContentLoaded", function () {
    $(document).ready(function () {
        $(".delete-link").click(handleDelete);
    });

    function handleDelete(e) {
        e.preventDefault();
        var roomId = $(this).data("id");
        Swal.fire({
            title: '確定要刪除嗎?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: '確定',
            cancelButtonText: '取消'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "DELETE",
                    url: "/api/Rooms/Delete/" + roomId,
                    success: function () {
                        Swal.fire({
                            icon: 'success',
                            title: '删除成功',
                            showConfirmButton: false, 
                            timer: 3000 
                        });
                        location.reload();
                    },
                    error: function (xhr) {
                        if (xhr.status === 400) {
                            var errorMessage = xhr.responseText;
                            Swal.fire({
                                icon: 'error',
                                title: errorMessage,
                            });
                        } else {
                            alert('刪除失敗：' + xhr.statusText);
                            console.log(xhr.statusText);
                        }
                    }
                });
            }
        });
    }
});