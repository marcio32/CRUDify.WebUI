
const connectionRoles = new signalR.HubConnectionBuilder().withUrl("/roleHub").build();

connectionRoles.on("ReceiveRoleUpdate", (role) => {
    console.log("Rol Actualizado: ", role.id);
    debugger
    const row = document.querySelector(`tr[data-role-id='${role.id}']`);

    if (role.deleted != undefined) {
        if (row) {
            row.remove();
        }
        return;
    }

    if (row) {
        row.innerHTML = `
                    <td>${role.id}</td>
                    <td>${role.name}</td>
                     <td>
                        <button type="button" class="btn btn-sm btn-primary editRoleBtn" data-role-id="${role.id}">Editar</button>
                        <button type="button" class="btn btn-sm btn-danger deleteRoleBtn" data-role-id="${role.id}">Eliminar</button>
                    </td>
                `;
    } else {
        const newRow = document.createElement('tr');
        newRow.setAttribute('data-role-id', role.id);
        newRow.innerHTML = `
                <td>${role.id}</td>
                    <td>${role.name}</td>
                     <td>
                        <button type="button" class="btn btn-sm btn-primary editRoleBtn" data-role-id="${role.id}">Editar</button>
                        <button type="button" class="btn btn-sm btn-danger deleteRoleBtn" data-role-id="${role.id}">Eliminar</button>
                </td>
                `
        document.getElementById('rolesBody').appendChild(newRow);
    }
});

connectionRoles.start().catch(error => console.error(err.toString()));


$(document).ready(function () {
    $('#rolesTable').DataTable();

    $('.addRoleBtn').click(function () {
        var url = 'Roles/RolePartial';
        $.ajax({
            url: url,
            type: "GET",
            success: function (data) {
                $('#roleContent').html(data);
                var modal = new bootstrap.Modal(document.getElementById('editRoleModal'));
                modal.show();

                $('#roleForm').submit(function (e) {
                    e.preventDefault();
                    var formData = new FormData(this);
                    debugger
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


    $('.editRoleBtn').click(function () {
        var roleId = $(this).data('role-id');
        var url = 'Roles/RolePartial?id=' + roleId;

        $.ajax({
            url: url,
            type: "GET",
            success: function (data) {
                $('#roleContent').html(data);
                var modal = new bootstrap.Modal(document.getElementById('editRoleModal'));
                modal.show();

                $('#roleForm').submit(function (e) {
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
                });
            },
            error: function () {
                alert("error al guardar los cambios");
            }
        })
    });

    $('.deleteRoleBtn').click(function () {
        Swal.fire({
            title: "Eliminar",
            text: "Estas por eliminar un roleo!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Eliminar",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            if (result.isConfirmed) {
                debugger
                var roleId = $(this).data("role-id");
                var url = 'Roles/RolePartial';
                debugger
                $.ajax({
                    url: '@Url.Page("/Roles/RolePartial", new { handler = "Role" })',
                    type: "DELETE",
                    headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                    contentType: "application/json",
                    data: JSON.stringify({ Id: roleId.toString() }),
                    success: function (response) {
                        if (response.success) {
                            Swal.fire({
                                title: "Eliminado!",
                                text: "El roleo fue eliminado.",
                                icon: "success"
                            });
                            location.reload();
                        }
                        else {
                            Swal.fire({
                                title: "Error!",
                                text: "Hubo un problema al eliminar el roleo.",
                                icon: "error"
                            })
                        }
                    }

                });
            }
        });
    });
});