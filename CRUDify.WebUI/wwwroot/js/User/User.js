
const connectionUsers = new signalR.HubConnectionBuilder().withUrl("/userHub").build();

connectionUsers.on("ReceiveUserUpdate", (user) => {
    console.log("Usuario Actualizado: ", user.id);
    const row = document.querySelector(`tr[data-user-id='${user.id}']`);


    if (row) {
        row.innerHTML = `
                    <td>${user.id}</td>
                    <td>${user.email}</td>
                    <td>${user.lockoutEnabled ? "Si" : "No"}</td>
                     <td>
                        <button type="button" class="btn btn-sm btn-primary editUserBtn" data-user-id="${user.id}">Editar</button>
                        <button type="button" class="btn btn-sm btn-danger deleteUserBtn" data-user-id="${user.id}">Eliminar</button>
                    </td>
                `;
    } else {
        const newRow = document.createElement('tr');
        newRow.setAttribute('data-user-id', user.id);
        newRow.innerHTML = `
                 <td>${user.id}</td>
                 <td>${user.email}</td>
                 <td>${user.lockoutEnabled ? "Si" : "No"}</td>
                 <td>
                        <button type="button" class="btn btn-sm btn-primary editUserBtn" data-user-id="${user.id}">Editar</button>
                        <button type="button" class="btn btn-sm btn-danger deleteUserBtn" data-user-id="${user.id}">Eliminar</button>
                </td>
                `
        document.getElementById('usersBody').appendChild(newRow);
    }
});

connectionUsers.start().catch(error => console.error(err.toString()));

$(document).ready(function () {
    $('#usersTable').DataTable();

    $('.addUserBtn').click(function () {
        var url = 'Users/UserPartial';
        $.ajax({
            url: url,
            type: "GET",
            success: function (data) {
                $('#userContent').html(data);
                var modal = new bootstrap.Modal(document.getElementById('editUserModal'));
                modal.show();

                $('#userForm').submit(function (e) {
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


    $('.editUserBtn').click(function () {
        var userId = $(this).data('user-id');
        var url = 'Users/UserPartial?id=' + userId;

        $.ajax({
            url: url,
            type: "GET",
            success: function (data) {
                $('#userContent').html(data);
                var modal = new bootstrap.Modal(document.getElementById('editUserModal'));
                modal.show();

                $('#userForm').submit(function (e) {
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

    $('.deleteUserBtn').click(function () {
        Swal.fire({
            title: "Eliminar",
            text: "Estas por eliminar un usero!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Eliminar",
            cancelButtonText: "Cancelar"
        }).then((result) => {
            if (result.isConfirmed) {

                var userId = $(this).data("user-id");
                var url = 'Users/UserPartial';

                $.ajax({
                    url: '@Url.Page("/Users/UserPartial", new { handler = "User" })',
                    type: "DELETE",
                    headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                    contentType: "application/json",
                    data: JSON.stringify({ Id: userId }),
                    success: function (response) {
                        if (response.success) {
                            Swal.fire({
                                title: "Eliminado!",
                                text: "El usuario fue eliminado.",
                                icon: "success"
                            });
                        }
                        else {
                            Swal.fire({
                                title: "Error!",
                                text: "Hubo un problema al eliminar el usero.",
                                icon: "error"
                            })
                        }
                    }

                });
            }
        });
    });
});