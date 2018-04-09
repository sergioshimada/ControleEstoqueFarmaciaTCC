using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ControleEstoque.Web.Models
{
    public class InventarioEstoqueModel
    {
        public int Id { get; set; }
        public int idLote { get; set; }
        public string nome { get; set; }
        public int qtdProduto { get; set; }
        public DateTime dataEntrada { get; set; }
        public DateTime dataVencimento { get; set; }

        public static List<InventarioEstoqueModel> RecuperarInventario()
        {
            var ret = new List<InventarioEstoqueModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select produto.id as idProduto , lote.id as idLote, produto.nome, lote.dt_entrada, lote.dt_vencimento,lote.qtd_produto from produto inner join lote_produto on produto.id = lote_produto.id_produto inner join lote on lote.id = lote_produto.id_lote";
                    var reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        {
                                ret.Add(new InventarioEstoqueModel
                                {
                                    Id = (int)reader["idProduto"],
                                    idLote = (int)reader["idLote"],
                                    nome = (string)reader["nome"],
                                    qtdProduto = (int)reader["qtd_produto"],
                                    dataEntrada = (DateTime)reader["dt_entrada"],
                                    dataVencimento = (DateTime)reader["dt_vencimento"]
                                });
                            
                        }
                    }
                }

                return ret;
            }
        }
    }
}