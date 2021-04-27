class Carrinho {

    ClickIncremento(button) {

        let data = this.getData(button);
        data.Quantidade++;
        this.postQuantidade(data);

        //debugger;
    }

    ClickDecremento(button) {

        let data = this.getData(button);
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
        var novaQuantidade = $(linhaDoItem).find('input').val();

        var data = {
            id: itemId,
            Quantidade: novaQuantidade
        };

        return data;
    }

    postQuantidade(data) {

        //CodMud1> Obtendo token
        let token = $('[name=__RequestVerificationToken]').val();
        let headers = {};
        headers['RequestVerificationToken'] = token;
        //CodMud1/>

        $.ajax({
            url: '/Pedido/UpdateQuantidade',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: headers //CodMud1: Enviando token
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
