using MarketBackEnd.Models;
using MarketBackEnd.Controllers;
using System.Data.SQLite;

namespace MarketBackEnd
{
    public class CommonFunctions
    {
        /*public List<Jugador> Calcularjugadorequipo(string[] jugadores)
        {
            List<Jugador> lista = new List<Jugador>();
            JugadorController jcont = new JugadorController();
            for (int i = 0; i < jugadores.Length; i++)
            {
                var jid = Int32.Parse(jugadores[i]);
                var aux = jcont.Get(jid);
                lista.Add(aux);
            }
            return lista;
        }

        public Equipo ObtenerEquipo(int id)
        {
            EquipoController equipos = new EquipoController();
            return equipos.Get(id);
        }

        public void ActualizarPartido(Game juego)
        {
            GameController controller = new GameController();
            controller.Put(juego.id,juego);
        }*/

        public bool ExisteUsuario(string correo,string avatar)
        {
            bool result = false;
            string connection = @"Data Source = .\SQLite\sqlite_partido.db; Version = 3";
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                using SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = "SELECT * FROM User WHERE correo =\'" + correo + "\'" + "OR avatar=\'" + avatar+"\'" ;
                cmd.Connection = conn;
                conn.Open();
                SQLiteDataReader rdr = cmd.ExecuteReader();
                if (rdr.StepCount > 0)
                {
                    result = true;
                }
                conn.Close();
            }

            return result;

        }

    }
}
