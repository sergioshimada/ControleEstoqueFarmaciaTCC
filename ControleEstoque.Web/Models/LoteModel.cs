using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ControleEstoque.Web.Models
{
    public class LoteModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha a quantidade.")]
        public int QtdProduto { get; set; }

        [Required(ErrorMessage = "Preencha o vencimento.")]
        public DateTime DtVencimento { get; set; }

        [Required(ErrorMessage = "Preencha a entrada.")]
        public DateTime DtEntrada = DateTime.Now;


        public List<GrupoProdutoModel> Produtos { get; set; }

        public LoteModel()
        {
            this.Produtos = new List<GrupoProdutoModel>();
        }

        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select count(*) from lote";
                    ret = (int)comando.ExecuteScalar();
                }
            }

            return ret;
        }

        public static List<LoteModel> RecuperarLista(int pagina, int tamPagina)
        {
            var ret = new List<LoteModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    var pos = (pagina - 1) * tamPagina;

                    comando.Connection = conexao;

                    comando.CommandText = string.Format(
                                                "select * from lote order by qtd_produto offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);

                    var reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        ret.Add(new LoteModel
                        {
                            Id = (int)reader["id"],
                            QtdProduto = (int)reader["qtd_produto"],
                            DtVencimento = (DateTime)reader["dt_vencimento"],
                            DtEntrada = (DateTime)reader["dt_entrada"]
                        });
                    }
                }
            }

            return ret;
        }

        public void CarregarProdutos()
        {
            this.Produtos.Clear();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText =
                        "select u.* " +
                        "from lote_produto pu, produtos u " +
                        "where (pu.id_lote = @id_lote) and (pu.id_produto = u.id)";

                    comando.Parameters.Add("@id_lote", SqlDbType.Int).Value = this.Id;

                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        this.Produtos.Add(new GrupoProdutoModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Descricao = (string)reader["descricao"]
                        });
                    }
                }
            }
        }

        public static List<LoteModel> RecuperarListaAtivos()
        {
            var ret = new List<LoteModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from lote where ativo=1 order by nome");
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new LoteModel
                        {
                            Id = (int)reader["id"],
                            QtdProduto = (int)reader["qtd_produto"],
                            DtVencimento = (DateTime)reader["dt_vencimento"],
                            DtEntrada = (DateTime)reader["dt_entrada"]
                        });
                    }
                }
            }

            return ret;
        }

        public static LoteModel RecuperarPeloId(int id)
        {
            LoteModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from lote where (id = @id)";

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = (new LoteModel
                        {
                            Id = (int)reader["id"],
                            QtdProduto = (int)reader["qtd_produto"],
                            DtVencimento = (DateTime)reader["dt_vencimento"],
                            DtEntrada = (DateTime)reader["dt_entrada"]
                        });
                    }
                }
            }

            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.CommandText = "delete from lote where (id = @id)";

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }

            return ret;
        }

        public int Salvar()
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

                    if (model == null)
                    {
                        comando.CommandText = "insert into lote ( qtd_produto, dt_vencimento, dt_entrada) values (@qtd_produto, @dt_vencimento, @dt_entrada); select convert(int, scope_identity())";

                        comando.Parameters.Add("@qtd_produto", SqlDbType.VarChar).Value = this.QtdProduto;
                        comando.Parameters.Add("@dt_vencimento", SqlDbType.VarChar).Value = this.DtVencimento;
                        comando.Parameters.Add("@dt_entrada", SqlDbType.VarChar).Value = this.DtEntrada;

                        ret = (int)comando.ExecuteScalar();
                    }
                    else
                    {
                        comando.CommandText = "update lote  set qtd_produto=@qtd_produto, dt_vencimento=@dt_vencimento,   dt_entrada = dt_entrada where id = @id";

                        comando.Parameters.Add("@qtd_produto", SqlDbType.VarChar).Value = this.QtdProduto;
                        comando.Parameters.Add("@dt_vencimento", SqlDbType.VarChar).Value = this.DtVencimento;
                        comando.Parameters.Add("@dt_entrada", SqlDbType.VarChar).Value = this.DtEntrada;
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;

                        if (comando.ExecuteNonQuery() > 0)
                        {
                            ret = this.Id;
                        }
                    }
                }
            }

            return ret;
        }

        public List<LoteModel> RecuperarListaInventario()
        {
            var ret = new List<LoteModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {

                    comando.Connection = conexao;

                    comando.CommandText = string.Format(
                                                "select * from lote order by qtd_produto offset {0} rows fetch next {1} rows only");

                    var reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        ret.Add(new LoteModel
                        {
                            Id = (int)reader["id"],
                            QtdProduto = (int)reader["qtd_produto"],
                            DtVencimento = (DateTime)reader["dt_vencimento"],
                            DtEntrada = (DateTime)reader["dt_entrada"]
                        });
                    }
                }
            }

            return ret;
        }
    }
}