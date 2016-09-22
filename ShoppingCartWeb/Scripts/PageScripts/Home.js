//HARDCODED VARIABLE FOR DEMO
var cartId = 2;

//Page Ready
$(document).ready(function () {
    GetItems();
    GetCartItems();
});

//Rebind Items after a change
function ReBindItems() {

    $('#divCartItems').attr("hidden", "hidden");
    $('#divCartItemsLoading').removeAttr("hidden");
    $('#divCartItems').empty();
    
    $('#divAllItems').attr("hidden", "hidden");
    $('#divAllItemsLoading').removeAttr("hidden");
    $('#divAllItems').empty();

    GetItems();
    GetCartItems();
}

/* --------------------------
    Add all Items
---------------------------*/
function GetItems() {
    $.getJSON(url + "/item",
    {
        token: token,
        format: "json"
    })
   .done(function (data) {
       $.each(data, function (i, item) {
           //set color of quantity in stock if less than 5
           var stockColor = '';
           if (item.Quantity < 5) stockColor = 'class="stockColor"';

           //Generate items on screen
           var itemView = '<div id="item_' + item.ID + '" class="col-md-3">' +
                               '<h2>' + item.Name + '</h2>' +
                               '<p>Description: ' + item.Description + '</p>' +
                               '<p>Price: $' + item.Price.toFixed(2) + '</p>' +
                               '<p>Quantity: <input type="number" id="txtQty' + item.ID + '" value="1" min="1" max="' + item.Quantity + '" maxlength="2" class="qtyInput" /></p>' +
                               '<p ' + stockColor + '>In Stock: ' + item.Quantity + '</p><input id="txtStock' + item.ID + '" type="hidden" value="' + item.Quantity + '"></input>' +
                               '<p><button id="btnAdd' + item.ID + '" type="button" class="btn btn-default" onclick="AddItemToCart(this)">Add to cart &raquo;</button></p>' +
                           '</div>';

           $('#divAllItems').append(itemView);
       });
       
       $('#divAllItemsLoading').attr("hidden", "hidden");
       $('#divAllItems').removeAttr("hidden");
   })
   .fail(function (XMLHttpRequest, textStatus, error) {
       alert(XMLHttpRequest.responseJSON.Message);
   });
}

/* --------------------------
    Get Cart Items
---------------------------*/
function GetCartItems() {
    $.getJSON(url + "/cartitem",
    {
        token: token,
        id: cartId
    })
    .done(function (data) {
        var numberOfItems = 0;
        var totalPrice = 0;

        $.each(data, function (i, item) {
            //Generate items on screen
            var itemView = '<div id="cartItem_' + item.ID + '" class="col-md-3">' +
                            '<h2>' + item.Name + '</h2>' +
                            '<p>Description: ' + item.Description + '</p>' +
                            '<p>Price: $' + item.Price.toFixed(2) + '</p>' +
                            '<p>Quantity: ' + item.Quantity + '</p>' +
                            '<p><b>Sub-total: $' + item.SubTotalPrice.toFixed(2) + '</b></p>' +
                            '<p><button id="btnRemove' + item.ID + '"  type="button" class="btn btn-default" onclick="RemoveItemFromCart(this)">Remove from cart &raquo;</button></p>' +
                        '</div>';

            $('#divCartItems').append(itemView);
            numberOfItems = numberOfItems + 1;
            totalPrice = totalPrice + item.SubTotalPrice;
        })

        $('#lblNumberOfItems').text(numberOfItems);
        $('#lblTotalPrice').text('$' + totalPrice.toFixed(2));

        $('#divCartItemsLoading').attr("hidden", "hidden");
        $('#divNoItems').attr("hidden", "hidden");
        $('#divCartItems').removeAttr("hidden");
    })
    .fail(function (XMLHttpRequest, textStatus, error) {
        if (error == 'Not Found') {
            $('#divNoItems').removeAttr("hidden");
            $('#divCartItemsLoading').attr("hidden", "hidden");
            $('#divCartItems').attr("hidden", "hidden");

            $('#lblNumberOfItems').text(0);
            $('#lblTotalPrice').text('$' + 0);
        }
        else {
            alert(XMLHttpRequest.responseJSON.Message);
        }
    });
}

/* --------------------------
    Add Item to Cart
---------------------------*/
function AddItemToCart(btnItem) {
    var id = btnItem.id.replace("btnAdd", "");

    //get quantity
    var txtQty = "#txtQty" + id;
    var quantity = $(txtQty).val();

    //validate quantity
    if (quantity < 0 || quantity > $("#txtStock" + id).val()) {
        alert("Invalid Quantity");
        $(txtQty).focus();
        return;
    }

    $.ajax({
        url: url + "/cartitem",
        type: "POST",
        dataType: 'json',
        data: JSON.stringify({
            token: token,
            cartId: cartId,
            itemId: id,
            quantity: quantity
        }),
        async: true,
        cache: false,
        traditional: true,
        contentType: 'application/json',
        success: function (data) {
            alert(data.Message);
            ReBindItems();
        },
        error: function (XMLHttpRequest, textStatus, error) {
            alert(XMLHttpRequest.responseJSON.Message);
        }
    });
}

/* --------------------------
    Remove Item from Cart
---------------------------*/
function RemoveItemFromCart(btnRemove){
    var id = btnRemove.id.replace("btnRemove", "");

    $.ajax({
        url: url + "/cartitem?token=" + token + "&id=" + id,
        type: "Delete",
        dataType: 'json',
        async: true,
        cache: false,
        traditional: true,
        contentType: 'application/json',
        success: function (data) {
            alert(data.Message);
            ReBindItems();
        },
        error: function (XMLHttpRequest, textStatus, error) {
            alert(XMLHttpRequest.responseJSON.Message);
        }
    });
}

/* --------------------------
    Remove All Items from Cart
---------------------------*/
function ClearAll() {
    $.ajax({
        url: url + "/cart?token=" + token + "&id=" + cartId,
        type: "Delete",
        dataType: 'json',
        async: true,
        cache: false,
        traditional: true,
        contentType: 'application/json',
        success: function (data) {
            alert(data.Message);
            ReBindItems();
        },
        error: function (XMLHttpRequest, textStatus, error) {
            alert(XMLHttpRequest.responseJSON.Message);
        }
    });
}

/* --------------------------
    Checkout
---------------------------*/
function Checkout() {
    alert("Sorry, feature not available yet!");
}