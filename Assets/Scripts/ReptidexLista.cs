
using System.Collections.Generic;
using UnityEngine;

public class ReptidexLista : MonoBehaviour
{
    [SerializeField] private UnlockManager unlockManager;
    [SerializeField] private Transform listaGrid;
    [SerializeField] private IconoEspecie prefabIcono;
    [SerializeField] private ControladorReptidex controlador;

    private void Start()
    {
        GenerarLista();
    }

    private void GenerarLista()
    {
        List<ARSpeciesData> especies = unlockManager.ObtenerTodasLasEspecies();

        foreach (ARSpeciesData especie in especies)
        {
            IconoEspecie nuevoIcono = Instantiate(prefabIcono, listaGrid);

            bool desbloqueada = unlockManager.EstaDesbloqueada(especie.id);

            nuevoIcono.Configurar(especie, desbloqueada, controlador);
        }
    }
}
