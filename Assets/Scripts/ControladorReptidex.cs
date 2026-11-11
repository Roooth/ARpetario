using UnityEngine;

public class ControladorReptidex : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject canvasLista;
    [SerializeField] private GameObject canvasInformacion;

    [Header("Datos")]
    [SerializeField] private UnlockManager unlockManager;
    [SerializeField] private ReptidexInformacion informacion;

    private void Start()
    {
        MostrarLista();
    }

    public void MostrarLista()
    {
        canvasLista.SetActive(true);
        canvasInformacion.SetActive(false);
    }

    public void MostrarInformacion(ARSpeciesData especie)
    {
        if (!unlockManager.EstaDesbloqueada(especie.id))
        {
            return;
        }

        informacion.MostrarEspecie(especie);

        canvasLista.SetActive(false);
        canvasInformacion.SetActive(true);
    }
}