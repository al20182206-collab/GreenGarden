using UnityEngine;

public class ControladorFondo : MonoBehaviour
{
    private SpriteRenderer sRenderer;

    [Header("Sprites de Ambiente")]
    public Sprite imagenDia;
    public Sprite imagenAtardecer;
    public Sprite imagenNoche;

    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        CambiarSpriteSegunFase(); // Al empezar la escena, ver en qué hora estamos
    }

    public void CambiarSpriteSegunFase()
    {
        if (sRenderer == null) return;

        // Leemos la fase del Cerebro Global
        switch (CicloDiaNoche.faseActual)
        {
            case CicloDiaNoche.FaseDia.Dia:
                sRenderer.sprite = imagenDia;
                break;
            case CicloDiaNoche.FaseDia.Atardecer:
                sRenderer.sprite = imagenAtardecer;
                break;
            case CicloDiaNoche.FaseDia.Noche:
                sRenderer.sprite = imagenNoche;
                break;
        }
    }
}