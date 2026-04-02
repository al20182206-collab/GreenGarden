using UnityEngine;
using UnityEngine.InputSystem; // Requerido para el New Input System

public class ControlRegadera : MonoBehaviour 
{
    [Header("Configuración Visual")]
    public GameObject aguaVisual; // Arrastra aquí el objeto hijo del agua

    private bool moviendo = false;

    void Update() 
    {
        // Obtenemos el control del puntero (Mouse o Touch)
        var pointer = Pointer.current;
        if (pointer == null) return;

        // 1. AL PRESIONAR (Clic o Toque inicial)
        if (pointer.press.wasPressedThisFrame) 
        {
            // Convertimos la posición de la pantalla al mundo del juego
            Vector2 posPantalla = pointer.position.ReadValue();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(posPantalla);

            // Buscamos si hay un collider donde tocamos
            Collider2D objetoTocado = Physics2D.OverlapPoint(worldPos);

            // Si tocamos esta regadera específicamente
            if (objetoTocado != null && objetoTocado.gameObject == gameObject) 
            {
                moviendo = true;

                // CAMBIO: Activación segura del agua
                if (aguaVisual != null) 
                {
                    aguaVisual.SetActive(true);
                    Debug.Log("Agua Activada");
                }
            }
        }

        // 2. MIENTRAS SE MUEVE (Arrastre)
        if (moviendo) 
        {
            Vector2 posPantalla = pointer.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(posPantalla);

            // Mantenemos la regadera en Z=0 para que no se pierda
            transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
        }
        // 3. AL SOLTAR (Dejar de presionar)
                if (pointer.press.wasReleasedThisFrame) 
                {
                    if (moviendo) 
                    {
                        moviendo = false;
        
                        // CAMBIO: Desactivación segura del agua
                        if (aguaVisual != null) 
                        {
                            aguaVisual.SetActive(false);
                            Debug.Log("Agua Desactivada");
                        }
                    }
                }
            }
        }