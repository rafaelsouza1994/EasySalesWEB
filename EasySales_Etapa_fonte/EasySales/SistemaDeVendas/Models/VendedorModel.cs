﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaDeVendas.Uteis;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace SistemaDeVendas.Models
{
    public class VendedorModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Informe o Nome do vendedor")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe o Email do vendedor")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "O Email informado é inválido!")]
        public string Email { get; set; }

        public string Senha { get; set; }

        public List<VendedorModel> ListarTodosVendedores()
        {
            List<VendedorModel> lista = new List<VendedorModel>();
            VendedorModel item;
            DAL dal = new DAL();
            string sql = "SELECT id, nome, email, senha FROM Vendedor ORDER BY nome ASC";
            DataTable dt = dal.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new VendedorModel
                {
                    Id = dt.Rows[i]["Id"].ToString(),
                    Nome = dt.Rows[i]["Nome"].ToString(),
                    Email = dt.Rows[i]["Email"].ToString(),
                    Senha = dt.Rows[i]["Senha"].ToString()
                };

                lista.Add(item);
            }
            dal.FecharConexao();
            return lista;
        }

        public VendedorModel RetornarVendedorId(int? id)
        {
            List<VendedorModel> lista = new List<VendedorModel>();
            VendedorModel item;
            DAL dal = new DAL();
            string sql = $"SELECT id, nome, email, senha FROM Vendedor WHERE id = '{id}' ORDER BY nome ASC";
            DataTable dt = dal.RetDataTable(sql);

                item = new VendedorModel
                {
                    Id = dt.Rows[0]["Id"].ToString(),
                    Nome = dt.Rows[0]["Nome"].ToString(),
                    Email = dt.Rows[0]["Email"].ToString(),
                    Senha = dt.Rows[0]["Senha"].ToString()
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
                 sql = $"UPDATE Vendedor SET NOME='{Nome}', EMAIL='{Email}', SENHA='{Senha}' WHERE ID = '{Id}'";
            }
            else
            { 
                 sql = $"INSERT INTO Vendedor(NOME, EMAIL, SENHA) VALUES ('{Nome}','{Email}', '{Senha}')";
            }

            dal.ExecutarComandoSQL(sql);
            dal.FecharConexao();
        }

        public void Excluir(int id)
        {
            DAL dal = new DAL();
            string sql = $"DELETE FROM Vendedor WHERE ID ='{id}'";
            dal.ExecutarComandoSQL(sql);
            dal.FecharConexao();
        }

    }
}
