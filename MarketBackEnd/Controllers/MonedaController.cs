using MarketBackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;

namespace MarketBackEnd.Controllers
{ 
    [Route("api/Monedas")]
    [ApiController]
    public class MonedaController : ControllerBase
    {
        [HttpGet()]
        public int Get(User user)
        {
            int cantidad = 0;
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                using SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = "SELECT cantidad FROM Monedas WHERE email = \'" + user.correo + "\'";
                cmd.Connection = conn;
                conn.Open();
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    cantidad = Convert.ToInt32(rdr["cantidad"]);
                }
            }
            return cantidad;

        }

        [HttpPost]
        public void Post(Monedas moneda)
        {
            vaciaTabla(moneda.email);
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = @"INSERT INTO Monedas (cantidad, cantidadUsada, email) VALUES (@cantidad, @cantidadUsada, @email)";
                cmd.Connection = conn;
                conn.Open();
                cmd.Parameters.Add(new SQLiteParameter("@cantidad", moneda.cantidad));
                cmd.Parameters.Add(new SQLiteParameter("@cantidadUsada", moneda.cantidadUsada));
                cmd.Parameters.Add(new SQLiteParameter("@email", moneda.email));
                cmd.ExecuteNonQuery();
            }
        }

        private void vaciaTabla(string email)
        {
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = @"DELETE FROM Monedas WHERE email = '" + email + "'";
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    [Route("api/MonedasUsadas")]
    [ApiController]
    public class MonedaUsadaController : ControllerBase
    {
        [HttpGet()]
        public int Get(User user)
        {
            int cantidad = 0;
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                using SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = "SELECT cantidadUsada FROM Monedas WHERE email = \'" + user.correo + "\'";
                cmd.Connection = conn;
                conn.Open();
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    cantidad = Convert.ToInt32(rdr["cantidadUsada"]);
                }
            }
            return cantidad;

        }
    }
}
