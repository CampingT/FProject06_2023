
    document.addEventListener("DOMContentLoaded", function () {

            const fileInput = document.getElementById("formFile");
    const filePreview = document.getElementById("filePreview");

    fileInput.addEventListener("change", function () {
                const selectedFile = fileInput.files[0];

    if (selectedFile) {
                    const reader = new FileReader();

    reader.onload = function (e) {
        filePreview.src = e.target.result;
                    };

    reader.readAsDataURL(selectedFile);
                }
            });
        });
