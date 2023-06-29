using MarketBackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;

namespace MarketBackEnd.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpPost]
        [Route("Login")]
        public string Login(User user)
        {
            string connection = @"Data Source = .\SQLite\sqlite_partido.db; Version = 3";
            string respuesta = "";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                string passdb="";
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = "SELECT password FROM User WHERE correo =\'" + user.correo + "\'";
                cmd.Connection = conn;
                conn.Open();
                SQLiteDataReader rdr = cmd.ExecuteReader();
                if (rdr.StepCount==0)
                {
                    respuesta = "No existe el usuario";
                }
                else
                {
                    while (rdr.Read())
                    {
                        passdb = Convert.ToString(rdr["password"]);
                    }
                    if (passdb == user.password)
                    {
                        respuesta = "Login Success";
                    }
                    else
                    {
                        respuesta = "Las credenciales son incorrectas";
                    }
                }

                conn.Close();
            }

            return respuesta;
        }


        [HttpPost]
        [Route("Register")]
        public string Register(User user)
        {
            string connection = @"Data Source = .\SQLite\sqlite_partido.db; Version = 3";
            string respuesta = "";
            CommonFunctions common = new CommonFunctions();
            if (common.ExisteUsuario(user.correo,user.avatar))
            {
                respuesta = "Este usuario ya existe";
            }
            else
            {
                using (SQLiteConnection conn = new SQLiteConnection(connection))
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.CommandText = @"INSERT INTO User (correo, password, nombre, avatar) VALUES (@correo, @password, @nombre, @avatar)";
                    cmd.Connection = conn;
                    conn.Open();

                    cmd.Parameters.Add(new SQLiteParameter("@correo", user.correo));
                    cmd.Parameters.Add(new SQLiteParameter("@password", user.password));
                    cmd.Parameters.Add(new SQLiteParameter("@nombre", user.nombre));
                    cmd.Parameters.Add(new SQLiteParameter("@avatar", user.avatar));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                respuesta = "Registro Success";
            }
           

            return respuesta;
        }



        [HttpGet]
        [Route("GetUser")]
        public User GetUser(User user)
        {
            string connection = @"Data Source = .\SQLite\sqlite_partido.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                using SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = "SELECT * FROM User WHERE correo =\'" + user.correo+"\'";
                cmd.Connection = conn;
                conn.Open();
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    user.id = Convert.ToInt32(rdr["id"]);
                    user.nombre = Convert.ToString(rdr["nombre"]);
                    user.avatar = Convert.ToString(rdr["avatar"]);
                }
                conn.Close();
            }
            return user;

        }

    }

}
