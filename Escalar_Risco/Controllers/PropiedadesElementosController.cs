using Escalar_Risco.Logica;
using Escalar_Risco.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Escalar_Risco.Controllers
{
    public class PropiedadesElementosController : Controller
    {
        private readonly LogicaBd _logicaBd;

        public PropiedadesElementosController(LogicaBd logicaBd)
        {
            _logicaBd = logicaBd;
        }

        public IActionResult Index()
        {
            List<PropiedadesElementos> productos = _logicaBd.GetAllProducts();

            return View(productos);
        }

        [HttpPost]
        public IActionResult Valores(PropiedadesElementos valores)
        {
            List<PropiedadesElementos> elementos = _logicaBd.GetAllProducts();

            // Definir la matriz de programación dinámica para el problema de la mochila 
            int[,] dinamica = new int[elementos.Count + 1, valores.Peso + 1];

            // Llenar la matriz dp utilizando el algoritmo de la mochila
            for (int i = 0; i <= elementos.Count; i++)
            {
                for (int j = 0; j <= valores.Peso; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        dinamica[i, j] = 0;
                    }
                    else if (elementos[i - 1].Peso <= j)
                    {
                        // Calcular el valor máximo tomando o no el elemento actual en la mochila
                        dinamica[i, j] = Math.Max(dinamica[i - 1, j], dinamica[i - 1, j - elementos[i - 1].Peso] + elementos[i - 1].Calorias);
                    }
                    else
                    {
                        dinamica[i, j] = dinamica[i - 1, j];
                    }
                }
            }

            // Reconstruir el conjunto óptimo de elementos seleccionados
            List<PropiedadesElementos> conjuntoOptimo = new List<PropiedadesElementos>();
            int iOptimo = elementos.Count;
            int jOptimo = valores.Peso;

            while (iOptimo > 0 && jOptimo > 0)
            {
                if (dinamica[iOptimo, jOptimo] != dinamica[iOptimo - 1, jOptimo])
                {
                    conjuntoOptimo.Add(elementos[iOptimo - 1]);
                    jOptimo -= elementos[iOptimo - 1].Peso;
                }
                iOptimo--;
            }
            List<PropiedadesElementos> elementosOrdenados = conjuntoOptimo.OrderBy(e => e.Nombre).ToList();
            ViewBag.Resultados = elementosOrdenados;

            return View("Index", elementos);
        }

    }
}
