function add_anti_forgery_token(data) {
    data.__RequestVerificationToken = $('[name=__RequestVerificationToken]').val();
    return data;
}

function get_dados_form() {
    return {
        Id: $('#data-id').val(),
        qtdProduto: $('#txt_qtdsaida').val()
    };
}


$(document).on('click', '.btn-saida', function () {
    var btn = $(this),
        tr = btn.closest('tr'),
        id = tr.attr('data-id'),
        url = url_saida,
        param = { 'id': id };

    bootbox.confirm({
        message: "Realmente deseja remover o " + tituloPagina + "?",
        buttons: {
            confirm: {
                label: 'Sim',
                className: 'btn-success'
            },
            cancel: {
                label: 'Não',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                $.post(url, add_anti_forgery_token(param), function (response) {
                    if (response) {
                        alert("Alterado com sucesso !");
                    }
                });
            }
        }
    });
})


//    .on('click', '.btn-saida', function () {
//    var btn = $(this);
//    var url = url_alterar;
//    var param = {
//        Id: btn.attr('data-id'),
//        qtdProduto: $('#txt_qtdsaida').val()
//    };

//    $.post(url, add_anti_forgery_token(param), function (response) {
//        if (response.Resultado == "OK") {
//            if (param.Id == 0) {
//                param.Id = response.IdSalvo;
//                var table = $('#grid_inventario').find('tbody');
//            }
//            else if (response.Resultado == "ERRO") {
//                $('#msg_aviso').hide();
//                $('#msg_mensagem_aviso').hide();
//                $('#msg_erro').show();
//            }
//            else if (response.Resultado == "AVISO") {
//                $('#msg_mensagem_aviso').html(formatar_mensagem_aviso(response.Mensagens));
//                $('#msg_aviso').show();
//                $('#msg_mensagem_aviso').show();
//                $('#msg_erro').hide();
//            }
//        }
//    }).error(function (e) {

//        console.log(e);

//    });
//})