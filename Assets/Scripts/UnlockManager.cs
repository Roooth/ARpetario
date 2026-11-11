using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    [SerializeField] private List<ARSpeciesData> especies;

    private HashSet<string> especiesDesbloqueadas = new HashSet<string>();

    private const string PREFIJO_DESBLOQUEO = "Especie_Desbloqueada_";

    private void Awake()
    {
        CargarDesbloqueos();
    }

    public ARSpeciesData ObtenerEspecie(string id)
    {
        foreach (ARSpeciesData especie in especies)
        {
            if (especie.id == id)
            {
                return especie;
            }
        }

        return null;
    }

    public bool EstaDesbloqueada(string id)
    {
        return especiesDesbloqueadas.Contains(id);
    }

    public bool DesbloquearEspecie(string id)
    {
        if (EstaDesbloqueada(id))
        {
            return false;
        }

        especiesDesbloqueadas.Add(id);

        PlayerPrefs.SetInt(PREFIJO_DESBLOQUEO + id, 1);
        PlayerPrefs.Save();

        return true;
    }

    private void CargarDesbloqueos()
    {
        foreach (ARSpeciesData especie in especies)
        {
            string clave = PREFIJO_DESBLOQUEO + especie.id;

            if (PlayerPrefs.GetInt(clave, 0) == 1)
            {
                especiesDesbloqueadas.Add(especie.id);
            }
        }
    }

    public List<ARSpeciesData> ObtenerEspeciesDesbloqueadas()
    {
        List<ARSpeciesData> desbloqueadas = new List<ARSpeciesData>();

        foreach (ARSpeciesData especie in especies)
        {
            if (EstaDesbloqueada(especie.id))
            {
                desbloqueadas.Add(especie);
            }
        }

        return desbloqueadas;
    }

    public List<ARSpeciesData> ObtenerTodasLasEspecies()
    {
        return new List<ARSpeciesData>(especies);
    }
}