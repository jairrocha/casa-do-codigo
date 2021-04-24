
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
        });
    }

}

var carrinho = new Carrinho();


