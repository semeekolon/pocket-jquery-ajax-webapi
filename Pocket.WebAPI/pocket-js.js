console.log("test");


$(document).ready(function () {

    var list = [];
    var OrderMaster;
    var flagEdit = false;

    popDropDown();

    $('#ddlProducts').change(function () {
        var nId = $('#ddlProducts').val();
        if (nId > 0) {
            $.ajax({
                type: "GET",
                url: "getproduct/" + nId,
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    console.table([r]);
                    $("#txtPrice").val(r.Price);
                   
                }
            });
        }
        else {
            $("#txtPrice").val("");
        }
    });

    $('#txtQty').focusout(function () {
        showTotal();
    });

    $('#btnAdd').click(function () {

        var Id = $("#ddlProducts").val();
        var Name = $("#ddlProducts option:selected").text();
        var Price = $("#txtPrice").val();
        var Qty = $("#txtQty").val();
        var Total = $("#txtTotal").val();

        if (Id > 0 && Price !== "" && Qty !== "" && Total !== "") {

            var tr =
                '<tr>'
                + '<td class="productid" style="display:none;">' + Id + '</td>'
                + '<td class="name">' + Name + '</td>'
                + '<td class="price">' + Price + '</td>'
                + '<td class="qty">' + Qty + '</td>'
                + '<td class="total">' + Total + '</td>'
                + '<td>'
                + '<input type="button" id="btnRemove"  value="Remove" style="width:80px" class="btn btn-danger btnRemove" />'
                + '</td>'
                + '</tr>';

            $("#tbProductDets").append(tr);
        }
        else {
            alert('Please fill details!');
        }

        $('#ddlProducts').val(0);
        $('#txtPrice, #txtQty, #txtTotal').val('');

    });

    $('#btnSubmit').click(function () {

        if (flagEdit === false) {

            //Names must match with DB table column names
            $('#tbProductDets tr').each(function (index, ele) {

                var orderItem = {
                    Id: $('.id', this).text(),
                    ProductId: parseInt($('.productid', this).text()), // this here is tr
                    Name: $('.name', this).text(),
                    Price: parseInt($('.price', this).text()),
                    Quantity: parseInt($('.qty', this).text()),
                    Total: parseInt($('.total', this).text())
                };

                list.push(orderItem);
            });

            console.table(list);


            OrderMaster = {
                OrderDetails: list
            };

            var grandTotal = 0;
            $.each(list, function (idx, elem) {
                $("#tbMProductDets").append("'\<tr>  '<td>" + elem.Name + "</td>  <td>" + elem.Price + "</td>'    <td>" + elem.Quantity + "</td> <td>" + elem.Total + "</td>  </tr>");

                grandTotal += elem.Total;
            });
            $(".grandtotal").html(grandTotal);

        }
        else {

            list = [];
            //Names must match with DB table column names
            $('#tbProductDets tr').each(function (index, ele) {

                var orderItem = {
                    Id: $('.id', this).text(),
                    ProductId: parseInt($('.productid', this).text()), // this here is tr
                    Name: $('.name', this).text(),
                    Price: parseInt($('.price', this).text()),
                    Quantity: parseInt($('.qty', this).text()),
                    Total: parseInt($('.total', this).text())
                };

                list.push(orderItem);
            });

            console.table([list]);


            OrderMaster = {
                OrderDetails: list
            };

            var grandTotal = 0;
            $.each(list, function (idx, elem) {
                $("#tbMProductDets").append("'\<tr>  '<td>" + elem.Name + "</td>  <td>" + elem.Price + "</td>'    <td>" + elem.Quantity + "</td> <td>" + elem.Total + "</td>  </tr>");

                grandTotal += elem.Total;
            });
            $(".grandtotal").html(grandTotal);

            flagEdit = true;
        }
    });

    $('#btnSave').click(function () {


        $.ajax({
            type: 'POST',
            url: 'saveordermaster',
            data: JSON.stringify(OrderMaster), // data : form fills and sends to the server
            contentType: 'application/json',
            success: function (result) { // contents in ok is returned by the server method
                console.log('saveproducts method');
                console.table([result]);
                if (result) {
                    alert('Successfully saved');
                    //here we will clear the form
                    list = [];
                    //$('#orderNo,#orderDate,#description').val('');
                    //$('#orderdetailsItems').empty();
                    $("#btnSave").attr("disabled", true);
                }
                else {
                    alert('Error');
                }

            }
        });

    });

    $('#btnClose').click(function () {
        $('#tbMProductDets').empty();
        flagEdit = true;
    });
});

function popDropDown() {
    $.ajax({
        type: "GET",
        url: "GetAllProducts",
        data: '{}', // To Send something
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var ddlProducts = $('#ddlProducts');
            ddlProducts.empty().append('<option selected="selected" value="0">Please select</option>');
            $.each(result, function () {
                ddlProducts.append($("<option></option>").val(this['Id']).html(this['Name']));
            });

        },
        error: function (result) {
            var ra = r;
            alert(ra);
        }
    });
}

function showTotal() {
    $('#txtTotal').val($('#txtPrice').val() * $('#txtQty').val());
}

$(document).on('click', '.btnRemove', function () {
    this.closest('tr').remove();
});







