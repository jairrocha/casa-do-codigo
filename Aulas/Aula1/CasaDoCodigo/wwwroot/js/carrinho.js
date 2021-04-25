//import { report } from "process";

class Carrinho {

    ClickIncremento(btn) {

        let data = this.getData(btn);
        data.Quantidade++;
        this.postQuantidade(data);

        //debugger;
    }

    ClickDecremento(btn) {

        let data = this.getData(btn);
        data.Quantidade--;
        this.postQuantidade(data);

    }

    updateQuantidade(input) {

        let data = this.getData(input);
        this.postQuantidade(data);

    }

    getData(elemento) {

        var linhaDoItem = $(elemento).parents('[item-id]')
        var itemId = $(linhaDoItem).attr('item-id');
        var novaQtde = $(linhaDoItem).find('input').val();

        var data = {
            id: itemId,
            Quantidade: novaQtde
        };

        return data;
    }

    postQuantidade(data) {

        $.ajax({
            url: '/Pedido/UpdateQuantidade',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data)
        }).done(function (response) {

            //Não indicado, pois atualiza a página toda
            //location.reload();

            let itemPedido = response.itemPedido;
            let linhaDoItem = $('[item-id=' + itemPedido.id + ']');
            linhaDoItem.find('input').val(itemPedido.quantidade);
            linhaDoItem.find('[subtotal]').html((itemPedido.subtotal).duasCasas());

            let carrinhoViewModel = response.carrinhoViewModel;

            $('[numero-itens]').html('Total: ' + carrinhoViewModel.itens.length + ' itens');

            $('[total]').html((carrinhoViewModel.total).duasCasas());

            if (itemPedido.quantidade == 0) {
                linhaDoItem.remove();
            }
         
            //debugger;

        });
    }

}

var carrinho = new Carrinho();


//formata número
Number.prototype.duasCasas = function () {
    return this.toFixed(2).replace('.', ',');
}
