using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SistemaDeVendas.Uteis
{
    public class DAL
    {
        private static string Server = "mysql5045.site4now.net"; //mysql5045.site4now.net
        private static string Database = "db_a7e0a6_saleswe";    //db_a7e0a6_saleswe
        private static string User = "a7e0a6_saleswe";           //a7e0a6_saleswe
        private static string Password = "rafael123";            //rafael123
        private static string connectionString = $"Server={Server}; Database={Database}; Uid={User}; Pwd={Password};";
        private static MySqlConnection Connection;

        public DAL()
        {
            Connection = new MySqlConnection(connectionString);
            Connection.Open();
        }

        public DataTable RetDataTable(string sql)
        {
            DataTable data = new DataTable();
            MySqlCommand command = new MySqlCommand(sql, Connection);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public DataTable RetDataTable(MySqlCommand command)
        {
            DataTable data = new DataTable();
            command.Connection = Connection;
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(data);
            return data;
        }

        public void ExecutarComandoSQL(string sql)
        {
            MySqlCommand command = new MySqlCommand(sql, Connection);
            command.ExecuteNonQuery();
        }

        public void FecharConexao()
        {
            Connection.Close();
            Connection.Dispose();
        }
    }
}
