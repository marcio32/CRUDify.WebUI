

const connectionService = new signalR.HubConnectionBuilder().withUrl("/serviceHub").build();

connectionService.on("ReceiveServiceUpdate", (service) => {
    console.log("Servicio Actualizado: ", service.id);
    debugger
    const row = document.querySelector(`tr[data-service-id='${service.id}']`);

    if (!service.activo) {
        if (row) {
            row.remove();
        }
        return;
    }

    if (row) {
        row.innerHTML = `
                    <td>${service.id}</td>
                    <td>${service.nombre}</td>
                    <td>${service.activo ? "Si" : "No"}</td>
                     <td>
                        <button type="button" class="btn btn-sm btn-primary editServiceBtn" data-service-id="${service.id}">Editar</button>
                        <button type="button" class="btn btn-sm btn-danger deleteServiceBtn" data-service-id="${service.id}">Eliminar</button>
                    </td>
                `;
    } else {
        const newRow = document.createElement('tr');
        newRow.setAttribute('data-service-id', service.id);
        newRow.innerHTML = `
                <td>${service.id}</td>
                <td>${service.nombre}</td>
                <td>${service.activo ? "Si" : "No"}</td>
                <td>
                    <button type="button" class="btn btn-sm btn-primary editServiceBtn" data-service-id="${service.id}">Editar</button>
                    <button type="button" class="btn btn-sm btn-danger deleteServiceBtn" data-service-id="${service.id}">Eliminar</button>
                </td>
                `
        document.getElementById('servicesBody').appendChild(newRow);
    }
});

connectionService.start().catch(error => console.error(err.toString()));


$(document).ready(function () {
    $('#servicesTable').DataTable();
});

$('.addServiceBtn').click(function () {
    var url = 'Services/ServicePartial';
    $.ajax({
        url: url,
        type: "GET",
        success: function (data) {
            $("#serviceContent").html(data);
            var modal = new bootstrap.Modal(document.getElementById('editServiceModal'));
            modal.show();


            $('#serviceForm').submit(function (e) {
                debugger
                e.preventDefault();
                var formData = new FormData(this);
                $.ajax({
                    url: url,
                    type: "POST",
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        if (response.success) {
                            modal.hide();
                            location.reload();
                        }
                    },
                    error: function () {
                        alert("error al guardar los cambios");
                    }

                });
            });

        }
    });

});


$('.editServiceBtn').click(function () {

    var serviceId = $(this).data('service-id');
    var url = 'Services/ServicePartial?id=' + serviceId;
    $.ajax({
        url: url,
        type: "GET",
        success: function (data) {
            $("#serviceContent").html(data);
            var modal = new bootstrap.Modal(document.getElementById('editServiceModal'));
            modal.show();


            $('#serviceForm').submit(function (e) {
                debugger
                e.preventDefault();
                var formData = new FormData(this);
                $.ajax({
                    url: url,
                    type: "PUT",
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        if (response.success) {
                            modal.hide();
                            location.reload();
                        }
                    },
                    error: function () {
                        alert("error al guardar los cambios");
                    }

                });
            });

        }
    });

});


$('.deleteServiceBtn').click(function () {
    Swal.fire({
        title: "Eliminar",
        text: "Estas por eliminar un servicio!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Eliminar",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            debugger
            var serviceId = $(this).data("service-id");
            var url = 'Services/ServicePartial';
            $.ajax({
                url: '@Url.Page("/Services/ServicePartial", new { handler = "Service" })',
                type: "DELETE",
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                contentType: "application/json",
                data: JSON.stringify({ Id: serviceId }),
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            title: "Eliminado!",
                            text: "El servicio fue eliminado.",
                            icon: "success"
                        });
                        location.reload();
                    }
                    else {
                        Swal.fire({
                            title: "Error!",
                            text: "Hubo un problema al eliminar el servicio.",
                            icon: "error"
                        })
                    }
                }

            });
        }
    });
});