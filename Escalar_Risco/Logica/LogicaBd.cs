using Escalar_Risco.Models;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Escalar_Risco.Logica
{
    public class LogicaBd
    {
        private readonly IConfiguration _configuration;

        public LogicaBd(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<PropiedadesElementos> GetAllProducts()
        {
            string cadena = _configuration.GetConnectionString("DefaultConnection");

            List<PropiedadesElementos> ListaElementos = new List<PropiedadesElementos>();
            using (SqlConnection oconexion = new SqlConnection(cadena))
            {

                oconexion.Open();
                SqlCommand cmd = new SqlCommand("select * from tblPropiedadesElementos", oconexion);
                cmd.CommandType = CommandType.Text;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        PropiedadesElementos Elementos = new PropiedadesElementos();
                        Elementos.Nombre = dr["Nombre"].ToString();
                        Elementos.Peso = Convert.ToInt32(dr["Peso"]);
                        Elementos.Calorias = Convert.ToInt32(dr["Calorias"]);
                        ListaElementos.Add(Elementos);
                    }
                }
            }
            return ListaElementos;
        }
    }
}
