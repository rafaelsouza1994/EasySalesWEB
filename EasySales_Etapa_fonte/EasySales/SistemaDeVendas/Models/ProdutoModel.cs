﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaDeVendas.Uteis;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SistemaDeVendas.Models
{
    public class ProdutoModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Informe o Nome do produto")]
        public string Nome { get; set; }


        [Required(ErrorMessage = "Informe a Descrição do produto")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Informe o Preço Unitário do produto")]
        [DataType(DataType.Currency)]
        public decimal? Preco_Unitario { get; set; }

        [Required(ErrorMessage = "Informe a Quantidade do produto")]
        public decimal? Quantidade_Estoque { get; set; }

        [Required(ErrorMessage = "Informe a Unidade de Medida do produto")]
        public string Unidade_Medida { get; set; }

        
        public string Link_Foto { get; set; }


        public List<ProdutoModel> ListarTodosProdutos()
        {
            List<ProdutoModel> lista = new List<ProdutoModel>();
            ProdutoModel item;
            DAL dal = new DAL();
            string sql = "SELECT id, nome, descricao, preco_unitario, quantidade_estoque, unidade_medida, link_foto" +
                         " FROM Produto ORDER BY nome ASC";
            DataTable dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new ProdutoModel
                {
                    Id = dt.Rows[i]["Id"].ToString(),
                    Nome = dt.Rows[i]["Nome"].ToString(),
                    Descricao = dt.Rows[i]["Descricao"].ToString(),
                    Preco_Unitario = decimal.Parse(dt.Rows[i]["Preco_Unitario"].ToString()),
                    Quantidade_Estoque = decimal.Parse(dt.Rows[i]["Quantidade_Estoque"].ToString()),
                    Unidade_Medida = dt.Rows[i]["Unidade_Medida"].ToString(),
                    Link_Foto = dt.Rows[i]["Link_Foto"].ToString()

                };

                lista.Add(item);
            }
            dal.FecharConexao();
            return lista;
        }

        public ProdutoModel RetornarProdutoId(int? id)
        {
            List<ProdutoModel> lista = new List<ProdutoModel>();
            ProdutoModel item;
            DAL dal = new DAL();
            string sql = $"SELECT id, nome, descricao, preco_unitario, quantidade_estoque, unidade_medida, link_foto" +
                         $" FROM Produto WHERE id = '{id}' ORDER BY nome ASC";
            DataTable dt = dal.RetDataTable(sql);

            item = new ProdutoModel
            {
                Id = dt.Rows[0]["Id"].ToString(),
                Nome = dt.Rows[0]["Nome"].ToString(),
                Descricao = dt.Rows[0]["Descricao"].ToString(),
                Preco_Unitario = decimal.Parse(dt.Rows[0]["Preco_Unitario"].ToString()),
                Quantidade_Estoque = decimal.Parse(dt.Rows[0]["Quantidade_Estoque"].ToString()),
                Unidade_Medida = dt.Rows[0]["Unidade_Medida"].ToString(),
                Link_Foto = dt.Rows[0]["Link_Foto"].ToString()
            };

            dal.FecharConexao();
            return item;
        }

        public void Gravar()
        {
            DAL dal = new DAL();
            string sql = string.Empty;
            if (Id != null)
            {
                 sql = $"UPDATE Produto SET" +
                       $" nome='{Nome}', " +
                       $" descricao='{Descricao}', " +
                       $" preco_unitario={Preco_Unitario.ToString().Replace(",",".")}, " +
                       $" quantidade_estoque='{Quantidade_Estoque}', " +
                       $" unidade_medida='{Unidade_Medida}', " +
                       $" link_foto='{Link_Foto}'  " +
                       $" WHERE id = '{Id}'";
            }
            else
            { 
                 sql = $"INSERT INTO Produto(nome, descricao, preco_unitario, quantidade_estoque, unidade_medida, link_foto)" +
                       $" VALUES ('{Nome}','{Descricao}','{Preco_Unitario.ToString().Replace(",", ".")}','{Quantidade_Estoque}','{Unidade_Medida}','{Link_Foto}')";
            }

            dal.ExecutarComandoSQL(sql);
            dal.FecharConexao();
        }

        public void Excluir(int id)
        {
            DAL dal = new DAL();
            string sql = $"DELETE FROM Produto WHERE ID ='{id}'";
            dal.ExecutarComandoSQL(sql);
            dal.FecharConexao();
        }

    }
}
