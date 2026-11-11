using UnityEngine;

public class DatosAR : MonoBehaviour
{
    public static DatosAR Instancia;

    public ARSpeciesData especieSeleccionada;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SeleccionarEspecie(ARSpeciesData especie)
    {
        especieSeleccionada = especie;
    }
}