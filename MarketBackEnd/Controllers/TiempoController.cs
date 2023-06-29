using MarketBackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;

namespace MarketBackEnd.Controllers
{
    [Route("api/ShopTiempo")]
    [ApiController]
    public class TiempoController : ControllerBase
    {
        [HttpGet()]
        public Tiempo Get(User user)
        {
            Tiempo tiempo = new Tiempo();
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                using SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = "SELECT * FROM Tiempo WHERE Bloque = 'shop' AND email = \'" + user.correo + "\'";
                cmd.Connection = conn;
                conn.Open();
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    tiempo.hora = Convert.ToInt32(rdr["hora"]);
                    tiempo.minuto = Convert.ToInt32(rdr["minuto"]);
                }
            }
            return tiempo;

        }

        [HttpPost]
        public void Post(Tiempo tiempo)
        {
            vaciaTabla(tiempo.email);
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = @"INSERT INTO Tiempo (hora, minuto, Bloque, email) VALUES (@hora, @minuto, 'shop', @email)";
                cmd.Connection = conn;
                conn.Open();
                cmd.Parameters.Add(new SQLiteParameter("@hora", tiempo.hora));
                cmd.Parameters.Add(new SQLiteParameter("@minuto", tiempo.minuto));
                cmd.Parameters.Add(new SQLiteParameter("@email", tiempo.email));
                cmd.ExecuteNonQuery();
            }
        }

        private void vaciaTabla(string email)
        {
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = @"DELETE FROM Tiempo WHERE Bloque = 'shop' AND email = '" + email + "'";
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
