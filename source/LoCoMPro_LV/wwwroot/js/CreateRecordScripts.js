// Crea una previsualización de las imágenes.
function handleFileSelection(event) {
    var input = event.target;
    var container = document.getElementById("imagePreviewContainer");
    container.innerHTML = "";

    var maxFiles = 3;

    if (input.files.length > maxFiles) {
        document.getElementById("fileCountError").textContent = "Solo puedes seleccionar hasta 3 archivos";
        input.value = "";
        return;
    } else {
        document.getElementById("fileCountError").textContent = "";
    }

    // Mostrar previsualización para cada archivo
    for (var i = 0; i < input.files.length; i++) {
        var file = input.files[i];

        if (file.type.startsWith("image/")) {
            (function (file) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    // Crear elemento de imagen para la previsualización
                    var img = document.createElement("img");
                    img.src = e.target.result;
                    img.className = "img-preview";

                    var containerItem = document.createElement("div");
                    containerItem.className = "img-preview-container";
                    containerItem.appendChild(img);

                    container.appendChild(containerItem);
                };

                reader.readAsDataURL(file);
            })(file);
        }
    }
}


