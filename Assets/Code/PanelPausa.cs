using UnityEngine;

public class ControladorPausa : MonoBehaviour
{
    public GameObject objetoMenuPausa; // Arrastra tu Panel de Pausa aquí
    private bool estaPausado = false;

    // Este método lo llamará el botón de "Pausa" en tu pantalla principal
    public void AlternarPausa()
    {
        estaPausado = !estaPausado;

        if (estaPausado)
        {
            Pausar();
        }
        else
        {
            Continuar();
        }
    }

    void Pausar()
    {
        Time.timeScale = 0f; // Congela el tiempo del juego
        objetoMenuPausa.SetActive(true); // Muestra el menú
    }

    public void Continuar()
    {
        Time.timeScale = 1f; // Devuelve el tiempo a la normalidad
        objetoMenuPausa.SetActive(false); // Esconde el menú
        estaPausado = false;
    }
}
