using MarketBackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;

namespace MarketBackEnd.Controllers
{
    [Route("api/shopObjetos")]
    [ApiController]
    public class ObjetoController : ControllerBase
    {
        [HttpGet()]
        public Objeto[] Get(User user)
        {
            List<Objeto> objetos = new List<Objeto>();
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                using SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = "SELECT * FROM ShopObjeto WHERE email = \'" + user.correo + "\'";
                cmd.Connection = conn;
                conn.Open();
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    Objeto objeto = new Objeto();
                    objeto.id = Convert.ToInt32(rdr["objetoId"]);
                    objeto.cantidad = Convert.ToInt32(rdr["cantidad"]);
                    objeto.fuerza = Convert.ToInt32(rdr["fuerza"]);
                    objeto.agilidad = Convert.ToInt32(rdr["agilidad"]);
                    objeto.aguante = Convert.ToInt32(rdr["aguante"]);
                    objeto.movimiento = Convert.ToInt32(rdr["movimiento"]);
                    objeto.suerte = Convert.ToInt32(rdr["suerte"]);
                    objeto.durabilidadRestante = Convert.ToInt32(rdr["durabilidadRestante"]);
                    objeto.descuento = Convert.ToInt32(rdr["descuento"]);
                    objetos.Add(objeto);
                }
            }
            return objetos.ToArray();
            
        }

        [HttpPost]
        public void Post(List<Objeto> objetos)
        {
            vaciaTabla(objetos[0].email);
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = @"INSERT INTO ShopObjeto (objetoId, cantidad, fuerza, agilidad, aguante, movimiento, suerte, durabilidadRestante, descuento, email) VALUES (@objetoId, @cantidad ,@fuerza, @agilidad, @aguante, @movimiento, @suerte, @durabilidadRestante, @descuento, @email)";
                cmd.Connection = conn;
                conn.Open();
                int i = 0;
                foreach (Objeto objetoActual in objetos)
                {
                    cmd.Parameters.Add(new SQLiteParameter("@objetoId", objetoActual.id));
                    cmd.Parameters.Add(new SQLiteParameter("@cantidad", objetoActual.cantidad));
                    cmd.Parameters.Add(new SQLiteParameter("@fuerza", objetoActual.fuerza));
                    cmd.Parameters.Add(new SQLiteParameter("@agilidad", objetoActual.agilidad));
                    cmd.Parameters.Add(new SQLiteParameter("@aguante", objetoActual.aguante));
                    cmd.Parameters.Add(new SQLiteParameter("@movimiento", objetoActual.movimiento));
                    cmd.Parameters.Add(new SQLiteParameter("@suerte", objetoActual.suerte));
                    cmd.Parameters.Add(new SQLiteParameter("@durabilidadRestante", objetoActual.durabilidadRestante));
                    cmd.Parameters.Add(new SQLiteParameter("@descuento", objetoActual.descuento));
                    cmd.Parameters.Add(new SQLiteParameter("@email", objetoActual.email));
                    cmd.ExecuteNonQuery();
                    i++;
                }
            }
        }

        private void vaciaTabla(string email)
        {
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = @"DELETE FROM ShopObjeto WHERE email = '" + email + "'";
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    [Route("api/shopObjetos/Reclamar")]
    [ApiController]
    public class ObjetoReclamarController : ControllerBase
    {
        [HttpGet()]
        public Objeto[] Get(User user)
        {
            List<Objeto> objetos = new List<Objeto>();
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                using SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = "SELECT * FROM ReclamarObjeto WHERE email = \'" + user.correo + "\'";
                cmd.Connection = conn;
                conn.Open();
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Objeto objeto = new Objeto();
                    objeto.id = Convert.ToInt32(rdr["objetoId"]);
                    objeto.cantidad = Convert.ToInt32(rdr["cantidad"]);
                    objeto.fuerza = Convert.ToInt32(rdr["fuerza"]);
                    objeto.agilidad = Convert.ToInt32(rdr["agilidad"]);
                    objeto.aguante = Convert.ToInt32(rdr["aguante"]);
                    objeto.movimiento = Convert.ToInt32(rdr["movimiento"]);
                    objeto.suerte = Convert.ToInt32(rdr["suerte"]);
                    objeto.durabilidadRestante = Convert.ToInt32(rdr["durabilidadRestante"]);
                    objetos.Add(objeto);
                }
            }
            return objetos.ToArray();

        }

        [HttpPost]
        public void Post(List<Objeto> objetos)
        {
            vaciaTabla(objetos[0].email);
            if (objetos[0].id != -1)
            {
                string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
                using (SQLiteConnection conn = new SQLiteConnection(connection))
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.CommandText = @"INSERT INTO ReclamarObjeto (objetoId, cantidad, fuerza, agilidad, aguante, movimiento, suerte, durabilidadRestante, email) VALUES (@objetoId, @cantidad ,@fuerza, @agilidad, @aguante, @movimiento, @suerte, @durabilidadRestante, @email)";
                    cmd.Connection = conn;
                    conn.Open();
                    int i = 0;
                    foreach (Objeto objetoActual in objetos)
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@objetoId", objetoActual.id));
                        cmd.Parameters.Add(new SQLiteParameter("@cantidad", objetoActual.cantidad));
                        cmd.Parameters.Add(new SQLiteParameter("@fuerza", objetoActual.fuerza));
                        cmd.Parameters.Add(new SQLiteParameter("@agilidad", objetoActual.agilidad));
                        cmd.Parameters.Add(new SQLiteParameter("@aguante", objetoActual.aguante));
                        cmd.Parameters.Add(new SQLiteParameter("@movimiento", objetoActual.movimiento));
                        cmd.Parameters.Add(new SQLiteParameter("@suerte", objetoActual.suerte));
                        cmd.Parameters.Add(new SQLiteParameter("@durabilidadRestante", objetoActual.durabilidadRestante));
                        cmd.Parameters.Add(new SQLiteParameter("@email", objetoActual.email));
                        cmd.ExecuteNonQuery();
                        i++;
                    }
                }
            }
        }

        private void vaciaTabla(string email)
        {
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = @"DELETE FROM ReclamarObjeto WHERE email = '" + email + "'";
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    [Route("api/inventoryObjetos")]
    [ApiController]
    public class InventoryObjetoController : ControllerBase
    {
        [HttpGet()]
        public Objeto[] Get(User user)
        {
            List<Objeto> objetos = new List<Objeto>();
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                using SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = "SELECT * FROM InventoryObjeto WHERE email = \'" + user.correo + "\'";
                cmd.Connection = conn;
                conn.Open();
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Objeto objeto = new Objeto();
                    objeto.id = Convert.ToInt32(rdr["objetoId"]);
                    objeto.cantidad = Convert.ToInt32(rdr["cantidad"]);
                    objeto.fuerza = Convert.ToInt32(rdr["fuerza"]);
                    objeto.agilidad = Convert.ToInt32(rdr["agilidad"]);
                    objeto.aguante = Convert.ToInt32(rdr["aguante"]);
                    objeto.movimiento = Convert.ToInt32(rdr["movimiento"]);
                    objeto.suerte = Convert.ToInt32(rdr["suerte"]);
                    objeto.durabilidadRestante = Convert.ToInt32(rdr["durabilidadRestante"]);
                    objetos.Add(objeto);
                }
            }
            return objetos.ToArray();

        }

        [HttpPost]
        public void Post(List<Objeto> objetos)
        {
            vaciaTabla(objetos[0].email);
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = @"INSERT INTO InventoryObjeto (objetoId, cantidad, fuerza, agilidad, aguante, movimiento, suerte, durabilidadRestante, email) VALUES (@objetoId, @cantidad ,@fuerza, @agilidad, @aguante, @movimiento, @suerte, @durabilidadRestante, @email)";
                cmd.Connection = conn;
                conn.Open();
                int i = 0;
                foreach (Objeto objetoActual in objetos)
                {
                    cmd.Parameters.Add(new SQLiteParameter("@objetoId", objetoActual.id));
                    cmd.Parameters.Add(new SQLiteParameter("@cantidad", objetoActual.cantidad));
                    cmd.Parameters.Add(new SQLiteParameter("@fuerza", objetoActual.fuerza));
                    cmd.Parameters.Add(new SQLiteParameter("@agilidad", objetoActual.agilidad));
                    cmd.Parameters.Add(new SQLiteParameter("@aguante", objetoActual.aguante));
                    cmd.Parameters.Add(new SQLiteParameter("@movimiento", objetoActual.movimiento));
                    cmd.Parameters.Add(new SQLiteParameter("@suerte", objetoActual.suerte));
                    cmd.Parameters.Add(new SQLiteParameter("@durabilidadRestante", objetoActual.durabilidadRestante));
                    cmd.Parameters.Add(new SQLiteParameter("@email", objetoActual.email));
                    cmd.ExecuteNonQuery();
                    i++;
                }
            }
        }

        private void vaciaTabla(string email)
        {
            string connection = @"Data Source = .\SQLite\sqlite_objeto.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = @"DELETE FROM InventoryObjeto WHERE email = '" + email + "'";
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
