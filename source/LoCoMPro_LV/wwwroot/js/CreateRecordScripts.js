

function handleImageDelete(index) {
    var container = document.getElementById("imagePreviewContainer");
    var images = container.getElementsByClassName("img-preview-container");

    container.removeChild(images[index]);
    var input = document.getElementById("fileInput");
    var files = input.files;
    var newFiles = Array.from(files);
    newFiles.splice(index, 1);
    input.files = newFiles;
}

function previewImages(event) {
    var input = event.target;
    var container = document.getElementById("imagePreviewContainer");
    console.log(container);
    var files = input.files;
    for (var i = 0; i < files.length; i++) {
        var file = files[i];
        if (file.type.startsWith("image/")) {
            var reader = new FileReader();
            reader.onload = (function (file, index) {
                return function (e) {
                    var containerItem = document.createElement("div");
                    containerItem.className = "img-preview-container";

                    var img = document.createElement("img");
                    img.src = e.target.result;
                    img.className = "img-preview";
                    containerItem.appendChild(img);

                    var deleteButton = document.createElement("button");
                    deleteButton.innerHTML = "x";
                    deleteButton.className = "delete-button";
                    deleteButton.onclick = function () {
                        handleImageDelete(index);
                    };
                    containerItem.appendChild(deleteButton);
                    container.appendChild(containerItem);
                };
            })(file, i);
            reader.readAsDataURL(file);
        }
    }
}