using UnityEngine;
using System;

public class EstadoPlanta : MonoBehaviour
{
    // Al ser "static", estas variables son globales. 
    // Si cambias de escena, el valor se queda guardado en la memoria.
    public static float progresoCrecimiento = 0f;
    public static float armoniaActual = 100f;
    public static bool estaViva = true;

    // Esto es para que puedas ver los valores en el Inspector (opcional)
    [Header("Visualización en tiempo real")]
    public float crecimientoDebug;
    public float armoniaDebug;
    
    // ... tus variables de antes ...
        public static string ultimaVezVisto; 
    
        // Agrega esto para calcular cuánto tiempo pasó
        public static float ObtenerTiempoTranscurrido()
        {
            if (string.IsNullOrEmpty(ultimaVezVisto)) return 0;
    
            DateTime fechaAnterior = DateTime.Parse(ultimaVezVisto);
            DateTime fechaActual = DateTime.Now;
    
            return (float)(fechaActual - fechaAnterior).TotalSeconds;
        }
        

    void Update()
    {
        // Solo para que veas en el Inspector cómo van los números
        crecimientoDebug = progresoCrecimiento;
        armoniaDebug = armoniaActual;
    }

    // Funciones para guardar en el disco duro
    public static void GuardarProgreso()
    {
        PlayerPrefs.SetFloat("CrecimientoGuardado", progresoCrecimiento);
        PlayerPrefs.SetFloat("ArmoniaGuardada", armoniaActual);
        PlayerPrefs.Save();
        Debug.Log("Progreso guardado en el disco.");
    }

    public static void CargarProgreso()
    {
        progresoCrecimiento = PlayerPrefs.GetFloat("CrecimientoGuardado", 0f);
        armoniaActual = PlayerPrefs.GetFloat("ArmoniaGuardada", 100f);
        Debug.Log("Progreso cargado del disco.");
    }
    
    
}