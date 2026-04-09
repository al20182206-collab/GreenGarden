using UnityEngine;
using System;

public class EstadoPlanta : MonoBehaviour
{
    // Las nuevas listas para las 3 plantas
    public static float[] armonias = { 100f, 100f, 100f };
    public static float[] progresos = { 0f, 0f, 0f };
    public static bool[] vivas = { true, true, true };

    public static string ultimaVezVisto;

    // --- "PUENTES" PARA EVITAR ERRORES ---
    // Esto hace que si algo busca 'armoniaActual', use el espacio 0 del arreglo
    public static float armoniaActual 
    {
        get { return armonias[0]; }
        set { armonias[0] = value; }
    }

    public static float progresoCrecimiento 
    {
        get { return progresos[0]; }
        set { progresos[0] = value; }
    }

    public static bool estaViva 
    {
        get { return vivas[0]; }
        set { vivas[0] = value; }
    }
    // ---------------------------------------

    // Esto es para que puedas ver los valores en el Inspector (opcional)
    [Header("Visualización en tiempo real")]
    public float crecimientoDebug;
    public float armoniaDebug;
    
    
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