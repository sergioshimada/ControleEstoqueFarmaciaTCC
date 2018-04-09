using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ControleEstoque.Web.Models
{
    public class SaidaEstoqueModel
    {
        public int Id { get; set; }
        public int idLote { get; set; }
        public string nome { get; set; }
        public int qtdProduto { get; set; }
        public DateTime dataEntrada { get; set; }
        public DateTime dataVencimento { get; set; }

        public static List<SaidaEstoqueModel> RecuperarInventarioSaida()
        {
            var ret = new List<SaidaEstoqueModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select produto.id as idProduto , lote.id as idLote, produto.nome, lote.dt_entrada, lote.dt_vencimento,lote.qtd_produto from produto inner join lote_produto on produto.id = lote_produto.id_produto inner join lote on lote.id = lote_produto.id_lote where lote.qtd_produto > 0";
                    var reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        {
                            ret.Add(new SaidaEstoqueModel
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

        public int RemoverPeloId()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.Id);

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;


                    comando.CommandText = "update lote set qtd_produto=@qtdProduto where id = @id";
                    
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;
                        comando.Parameters.Add("@qtdProduto", SqlDbType.Int).Value = this.qtdProduto;

                        if (comando.ExecuteNonQuery() > 0)
                        {
                            ret = this.Id;
                        }
                    }
                }
            

            return ret;
        
    }

        public static SaidaEstoqueModel RecuperarPeloId(int id)
        {
            SaidaEstoqueModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select produto.id as idProduto , lote.id as idLote, produto.nome, lote.dt_entrada, lote.dt_vencimento,lote.qtd_produto from produto inner join lote_produto on produto.id = lote_produto.id_produto inner join lote on lote.id = lote_produto.id_lote where lote.id = @id";

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new SaidaEstoqueModel
                        {
                            Id = (int)reader["idProduto"],
                            idLote = (int)reader["idLote"],
                            nome = (string)reader["nome"],
                            qtdProduto = (int)reader["qtd_produto"],
                            dataEntrada = (DateTime)reader["dt_entrada"],
                            dataVencimento = (DateTime)reader["dt_vencimento"]
                        };
                    }
                }
            }

            return ret;
        }
    }

}
