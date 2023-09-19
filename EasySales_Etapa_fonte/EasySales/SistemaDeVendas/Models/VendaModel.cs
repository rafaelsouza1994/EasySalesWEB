﻿using Newtonsoft.Json;
using SistemaDeVendas.Uteis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using MySql.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaDeVendas.Models
{
    public class VendaModel
    {
        public string Id { get; set; }

        public string Data { get; set; }

        public string Cliente_Id { get; set; }

        public string Vendedor_Id { get; set; }

        public double Total { get; set; }

        public string ListaProdutos { get; set; }


        public List<VendaModel> ListagemVendas(string DataDe, string DataAte)
        {
            return RetornarListagemVendas(DataDe, DataAte);
        }

        // Lista Geral
        public List<VendaModel> ListagemVendas()
        {
            return RetornarListagemVendas(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
        }

        private List<VendaModel> RetornarListagemVendas(string DataDe, string DataAte)
        {
            List<VendaModel> lista = new List<VendaModel>();
            VendaModel item;
            DAL dal = new DAL();
            string sql = " SELECT v1.id, v1.data, v1.total, v2.nome as vendedor, c.nome as cliente FROM" +
                         " venda v1 INNER JOIN Vendedor v2 on v1.vendedor_id = v2.id INNER JOIN cliente c " +
                         " on v1.cliente_id = c.id" +
                        $" WHERE v1.data >='{DataDe}' and v1.data <='{DataAte}' " +
                         " ORDER BY data, total";
            DataTable dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new VendaModel
                {
                    Id = dt.Rows[i]["Id"].ToString(),
                    Data = DateTime.Parse(dt.Rows[i]["data"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                    Total = double.Parse(dt.Rows[i]["total"].ToString()),
                    Cliente_Id = dt.Rows[i]["cliente"].ToString(),
                    Vendedor_Id = dt.Rows[i]["vendedor"].ToString()
                };

                lista.Add(item);
            }
            dal.FecharConexao();
            return lista;
        }


        public List<ClienteModel> RetornarListaClientes()
        {
            return new ClienteModel().ListarTodosClientes();
        }

        public List<VendedorModel> RetornarListaVendedores()
        {
            return new VendedorModel().ListarTodosVendedores();
        }

        public List<ProdutoModel> RetornarListaProdutos()
        {
            return new ProdutoModel().ListarTodosProdutos();
        }

        public void Inserir()
        {
            DAL dal = new DAL();

            string dataVendas = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            string sql = "INSERT INTO Venda(data, total, vendedor_id, cliente_id) " +
                         $"VALUES('{dataVendas.ToString()}',{Total.ToString().Replace(",", ".")},{Vendedor_Id},{Cliente_Id})";
            dal.ExecutarComandoSQL(sql);


            sql = $"SELECT id FROM Venda WHERE data='{dataVendas.ToString()}' AND vendedor_id={Vendedor_Id} AND cliente_id={Cliente_Id} ORDER BY id DESC LIMIT 1 ";
            DataTable dt = dal.RetDataTable(sql);
            string id_venda = dt.Rows[0]["id"].ToString();

            List<ItemVendaModel> lista_produtos = JsonConvert.DeserializeObject<List<ItemVendaModel>>(ListaProdutos);

            for (int i = 0; i < lista_produtos.Count; i++)
            {
                sql = "INSERT INTO Itens_venda(venda_id, produto_id, qtd_produto, preco_produto)" +
                      $"VALUES({id_venda}, {lista_produtos[i].CodigoProduto.ToString()}," +
                      $" {lista_produtos[i].QtdProduto.ToString()}, " +
                      $"{lista_produtos[i].PrecoUnitario.ToString().Replace(",", ".")})";
                dal.ExecutarComandoSQL(sql);

                sql = "UPDATE Produto " +
                      $" SET quantidade_estoque = (quantidade_estoque - " + int.Parse(lista_produtos[i].QtdProduto.ToString()) + ") " +
                      $" WHERE id = {lista_produtos[i].CodigoProduto.ToString()}";
                dal.ExecutarComandoSQL(sql);
            }
            dal.FecharConexao();
        }
    }
}
