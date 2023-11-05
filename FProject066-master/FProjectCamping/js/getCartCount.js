
function updateCartItemCount(count) {
    var cartItemCountElement = document.getElementById('cartIcon');
    if (cartItemCountElement) {
        console.log(count);
        cartItemCountElement.textContent = count;

    }
}

//    let cart = cart.items.Length = 0
//        ? 0
//        : cart.items
//            .map(function (item) {
//                return item.qty;
//            })
//} 
//let panel = doucment.getElementById('cartIcon');
//panel.innerHtml = `${count}`;