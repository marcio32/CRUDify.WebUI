
const connectionProducts = new signalR.HubConnectionBuilder().withUrl("/productHub").build();

connectionProducts.on("ReceiveProductUpdate", (product) => {
    console.log("Producto Actualizado: ", product.id);

    const row = document.querySelector(`tr[data-product-id='${product.id}']`);

    if (!product.active) {
        if (row) {
            row.remove();
        }
        return;
    }

    if (row) {
        row.innerHTML = `
                    <td>${product.id}</td>
                    <td>${product.name}</td>
                    <td>${product.price}</td>
                    <td>${product.description}</td>
                    <td>${product.image ? `<img src=data:image/jpeg;base64,${product.image} alt="Imagen del Producto" style="width:100px; height:100px"/>` : '<span>Sin Imagen</span>'}</td>
                    <td>${product.stock}</td>
                    <td>${product.active ? "Si" : "No"}</td>
                     <td>
                        <button type="button" class="btn btn-sm btn-primary editProductBtn" data-product-id="${product.id}">Editar</button>
                        <button type="button" class="btn btn-sm btn-danger deleteProductBtn" data-product-id="${product.id}">Eliminar</button>
                    </td>
                `;
    } else {
        const newRow = document.createElement('tr');
        newRow.setAttribute('data-product-id', product.id);
        newRow.innerHTML = `
                <td>${product.id}</td>
                    <td>${product.name}</td>
                    <td>${product.price}</td>
                    <td>${product.description}</td>
                    <td>${product.image ? `<img src=data:image/jpeg;base64,${product.image} alt="Imagen del Producto" style="width:100px; height:100px"/>` : '<span>Sin Imagen</span>'}</td>
                    <td>${product.stock}</td>
                    <td>${product.active ? "Si" : "No"}</td>
                     <td>
                        <button type="button" class="btn btn-sm btn-primary editProductBtn" data-product-id="${product.id}">Editar</button>
                        <button type="button" class="btn btn-sm btn-danger deleteProductBtn" data-product-id="${product.id}">Eliminar</button>
                </td>
                `
        document.getElementById('productsBody').appendChild(newRow);
    }
});

connectionProducts.start().catch(error => console.error(err.toString()));

$(document).ready(function () {
    $('#productsTable').DataTable();

    $('.addProductBtn').click(function () {
        var url = 'Products/ProductPartial';
        $.ajax({
            url: url,
            type: "GET",
            success: function (data) {
                $('#productContent').html(data);
                var modal = new bootstrap.Modal(document.getElementById('editProductModal'));
                modal.show();

                $('#productForm').submit(function (e) {
                    e.preventDefault();
                    var formData = new FormData(this);

                    $.ajax({
                        url: url,
                        type: "POST",
                        data: formData,
                        processData: false,
                        contentType: false,
                        success: function (result) {
                            if (result.success) {
                                modal.hide();
                                location.reload();
                            }
                        },
                        error: function () {
                            alert("error al guardar los cambios");
                        }
                    })
                });
            },
            error: function () {
                alert("error al guardar los cambios");
            }
        })
    });


    $('.editProductBtn').click(function () {
        var productId = $(this).data('product-id');
        var url = 'Products/ProductPartial?id=' + productId;

        $.ajax({
            url: url,
            type: "GET",
            success: function (data) {
                $('#productContent').html(data);
                var modal = new bootstrap.Modal(document.getElementById('editProductModal'));
                modal.show();

                $('#productForm').submit(function (e) {
                    e.preventDefault();
                    var formData = new FormData(this);
                    $.ajax({
                        url: url,
                        type: "PUT",
                        data: formData,
                        processData: false,
                        contentType: false,
                        success: function (result) {
                            if (result.success) {
                                modal.hide();
                                location.reload();
                            }
                        },
                        error: function () {
                            alert("error al guardar los cambios");
                        }
                    })

                    //NO USAR JQUERY PARA CONSULTAS
                    // $.put(url, $(this).serialize(),function(result){
                    //     if(result.success){
                    //         modal.hide();
                    //         location.reload();
                    //     }
                    // }).fail(function(){
                    //     alert("error al guardar los cambios");
                    // });
                });
            },
            error: function () {
                alert("error al guardar los cambios");
            }
        })
    });

    $('.deleteProductBtn').click(function () {
        Swal.fire({
            title: "Eliminar",
            text: "Estas por eliminar un producto!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Eliminar",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            if (result.isConfirmed) {
                var productId = $(this).data("product-id");
                var url = 'Products/ProductPartial';
                $.ajax({
                    url: '@Url.Page("/Products/ProductPartial", new { handler = "Product" })',
                    type: "DELETE",
                    headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                    contentType: "application/json",
                    data: JSON.stringify({ Id: productId }),
                    success: function (response) {
                        if (response.success) {
                            Swal.fire({
                                title: "Eliminado!",
                                text: "El producto fue eliminado.",
                                icon: "success"
                            });
                            location.reload();
                        }
                        else {
                            Swal.fire({
                                title: "Error!",
                                text: "Hubo un problema al eliminar el producto.",
                                icon: "error"
                            })
                        }
                    }

                });
            }
        });
    });
});