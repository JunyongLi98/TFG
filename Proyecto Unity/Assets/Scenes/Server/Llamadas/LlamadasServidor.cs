using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;

public class LlamadasServidor : MonoBehaviour
{
    #region llamadas Equipo
    /*public void RecuperarEquiposJugador(int id)
    {
        StartCoroutine(ObtenerEquiposUser(id));
    }

    public void GenerarEquipo(Equipo equipo)
    {
        StartCoroutine(EnviarEquipo(equipo));
    }

    public void ActualizarEquipo(Equipo equipo)
    {
        StartCoroutine(ActualizarEquipoJugador(equipo));
    }

    private IEnumerator ActualizarEquipoJugador(Equipo equipo)
    {
        string json = generarJsonPut(equipo);
        var www = new UnityWebRequest("https://localhost:7095/api/equipo/Update", "Put");
        DownloadHandlerBuffer dh = new DownloadHandlerBuffer();
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = dh;
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.result);
        }
    }

    public string generarJsonPut(Equipo equipo)
    {
        string json = "";
        json = JsonConvert.SerializeObject(equipo);
        return json;
    }

    private IEnumerator ObtenerEquiposUser(int id)
    {
        var www = new UnityWebRequest("https://localhost:7095/api/equipo/equipos/"+id, "Get");
        DownloadHandlerBuffer dh = new DownloadHandlerBuffer();
        www.downloadHandler = dh;
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {

            Equipo[] equipos = JsonConvert.DeserializeObject<Equipo[]>(www.downloadHandler.text);
            Main.instance.eqhelper.deserializar(equipos);
        }

    }

    private IEnumerator EnviarEquipo(Equipo equipo)
    {
        var json = GenerarJsonPostEquipo(equipo);
        var www = UnityWebRequest.Post("https://localhost:7095/api/equipo/CrearEquipo",json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Equipo enviado con exito");
        }
    }

    private string GenerarJsonPostEquipo(Equipo equipo)
    {
        string json = "";
        json = JsonConvert.SerializeObject(equipo);
        return json;
    }
    #endregion

    #region llamadas Partido
    public void RecuperarPartido(Game game)
    {
        StartCoroutine(ObtenerPartido(game));
    }

    public void BuscarPartidos(List<Game> game)
    {
        StartCoroutine(ObtenerPartidosPendientes(game));
    }

    public void CrearPartido(Game game)
    {
        StartCoroutine(EnviarPartido(game));
    }

    private IEnumerator EnviarPartido(Game game)
    {
        var json = generarJsonPostPartido(game);
        var www = UnityWebRequest.Post("https://localhost:7095/api/game/partidos", json);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Equipo enviado con exito");
        }
    }


    private string generarJsonPostPartido(Game game)
    {
        var json = JsonUtility.ToJson(game);
        return json;
    }

    private IEnumerator ObtenerPartidosPendientes(List<Game> games)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://localhost:7095/api/game/partidos");
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string jsonResponse = www.downloadHandler.text;
            Game[] aux = JsonConvert.DeserializeObject<Game[]>(jsonResponse);
            if (aux.Length > 0)
            {
                for(int i = 0; i < aux.Length; i++)
                {
                    games.Add(aux[i]);
                }
            }
        }
    }

    private IEnumerator ObtenerPartido(Game game)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://localhost:7095/api/game/partido/" + game.id);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string jsonResponse = www.downloadHandler.text;
            Game aux = JsonConvert.DeserializeObject<Game>(jsonResponse);
            if (aux != null)
            {
                game.equipovisitanteid = aux.equipovisitanteid;
                game.golesvisitante = aux.golesvisitante;
                game.goleslocal = aux.goleslocal;
                game.jugadorvisitanteid = aux.jugadorvisitanteid;
                game.acciones = aux.acciones;
                game.estado = aux.estado;
            }
        }
    }


    #endregion

    #region llamadas Jugador

    public void RecuperarJugadores()
    {
        StartCoroutine(ObtenerJugadores());
    }

    private IEnumerator ObtenerJugadores()
    {
        int id = LoginManagerScript.Instance.user.id;
        var www = new UnityWebRequest("https://localhost:7095/api/jugador/jugadores/" + id, "Get");
        DownloadHandlerBuffer dh = new DownloadHandlerBuffer();
        www.downloadHandler = dh;
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {

            Jugador[] jugadores = JsonConvert.DeserializeObject<Jugador[]>(www.downloadHandler.text);
            Main.instance.jhelper.deserializar(jugadores);
            GameObject integrantes = GameObject.FindGameObjectWithTag("integrantes");
            GameObject gameObject = GameObject.FindGameObjectWithTag("editorequipopanel");
            integrantes.GetComponent<ListaIntegrantes>().borrarNoguardados();
            gameObject.GetComponent<CreadorEquipopanel>().MostrarTotalJugadores();

            if (gameObject.GetComponent<CreadorEquipopanel>().creacion == false)
            {
                gameObject.GetComponent<CreadorEquipopanel>().MostrarIntEquipo();
            }
            


        }
    }*/
    #endregion
    
    #region llamadas PubSub

    public void Subscribe()
    {

    }

    public void Unsubscribe()
    {

    }

    public void Publish()
    {

    }

    public void CreateChannel()
    {

    }

    public void DeleteChannel()
    {

    }


    #endregion

    #region llamada login
    public void Registrar(string correo, string password, string nombre, string nick,GameObject gameObject)
    {
        StartCoroutine(register(correo,password,nombre,nick,gameObject));
    }

    public void Loguear(string correo, string password, GameObject gameObject)
    {
        StartCoroutine(Login(correo,password,gameObject));
    }

    public void getUser(string correo, string password, GameObject gameObject)
    {
        StartCoroutine(GetUser(correo, password,gameObject));
    }

    private IEnumerator register(string correo, string password, string nombre, string nick, GameObject gameObject)
    {
        string json = generarJsonCrearUser(correo,password,nombre,nick);
        var www = new UnityWebRequest("https://localhost:7095/api/users/Register", "Post");
        DownloadHandlerBuffer dh = new DownloadHandlerBuffer();
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = dh;
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            LoginManagerScript.Instance.Respuesta = www.downloadHandler.text;
            gameObject.GetComponent<RegistrarHelper>().darrespuesta();

        }
    }

    public string generarJsonCrearUser(string correo, string password, string nombre, string nick)
    {
        string json = "";
        User user = new User();
        user.correo = correo;
        user.password = password;
        user.nombre = nombre;
        user.avatar = nick;
        json = JsonConvert.SerializeObject(user);
        return json;
    }

    public IEnumerator Login(string correo, string password,GameObject gameObject)
    {
        string json = generarJsonLogin(correo, password);
        var www = new UnityWebRequest("https://localhost:7095/api/users/Login", "Post");
        DownloadHandlerBuffer dh = new DownloadHandlerBuffer();
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = dh;
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            LoginManagerScript.Instance.Respuesta= www.downloadHandler.text;
            LoginManagerScript.Instance.user.correo= correo;
            gameObject.GetComponent<LoginHelper>().darrespuesta();
            
        }

    }

    public string generarJsonLogin(string correo, string password)
    {
        string json = "";
        User user = new User();
        user.correo = correo;
        user.password = password;
        user.nombre = "";
        user.avatar = "";
        json = JsonConvert.SerializeObject(user);

        return json;
    }

    private IEnumerator GetUser(string correo, string password, GameObject gameObject)
    {
        string json = generarJsonLogin(correo, password);
        var www = new UnityWebRequest("https://localhost:7095/api/users/GetUser", "Get");
        DownloadHandlerBuffer dh = new DownloadHandlerBuffer();
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = dh;
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {

            User user = JsonConvert.DeserializeObject<User>(www.downloadHandler.text);
            LoginManagerScript.Instance.user = user;
            gameObject.GetComponent<LoginHelper>().Entrar();

            

        }
    }

    #endregion





}
