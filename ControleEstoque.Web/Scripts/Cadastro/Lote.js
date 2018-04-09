function set_dados_form(dados) {
    $('#id_lote').val(dados.Id);
    $('#txt_QtdProduto').val(dados.QtdProduto);
    $('#txt_DtVencimento').val(dados.DtVencimento);
    $('#txt_DtEntrada').val(dados.DtEntrada);

    var lista_produto = $('#lista_produto');
    lista_produto.find('input[type=checkbox]').prop('checked', false);

    if (dados.Produtos) {
        for (var i = 0; i < dados.Produtos.length; i++) {
            var produto = dados.Produtos[i];
            var cbx = lista_produto.find('input[data-id-produto=' + produto.Id + ']');
            if (cbx) {
                cbx.prop('checked', true);
            }
        }
    }
}


function set_focus_form() {
    $('#txt_QtdProduto').focus();
}

function set_dados_grid(dados) {
    return '<td>' + dados.Id + '</td>'
        + '<td>' + dados.QtdProduto + '</td>'
        + '<td>' + dados.DtVencimento + '</td>'
        + '<td>' + dados.DtEntrada + '</td>';      
}

function get_dados_inclusao() {
    return {
        Id: 0,
        QtdProduto: 0,
        DtVencimento: '',
        DtEntrada: ''
    };
}

function get_dados_form() {
    return {
        Id: $('#id_lote').val(),
        QtdProduto: $('#txt_QtdProduto').val(),
        DtVencimento: $('#txt_DtVencimento').val()
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.QtdProduto).end()
        .eq(1).html(param.DtVencimento);
}