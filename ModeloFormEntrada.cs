using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos.Contratos;
using AccesoDatos.Entidades;
using AccesoDatos.Repositorios;
using Dominio.ObjetosValores;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Modelos
{
    public class ModeloFormEntrada: IDisposable
    {
        private int folio;
        private int reciboDonativo;
        private DateTime fechaEntrada;
        private int id_aliado;
        private string Programa;
        private decimal tarimas;
        private decimal mermas;

        private iFormatoEntrada formatoEntrada;
        public EstadoEntidadFormEntrada estado { private get; set; }

        public int FolioEntrada
        {
            get { return folio; }
            set { folio = value; }
        }
        public int ReciboDonativoEntrada
        {
            get { return reciboDonativo; }
            set { reciboDonativo = value; }
        }
        public DateTime fechaReciboEntrada
        {
            get { return fechaEntrada; }
            set { fechaEntrada = value; }
        }
        public int idAliadoEntrada
        {
            get { return id_aliado; }
            set { id_aliado = value; }
        }
        public string ProgramaEnt
        {
            get { return Programa; }
            set { Programa = value; }
        }
        public decimal tarimasEntrada
        {
            get { return tarimas; }
            set { tarimas = value; }
        }
        public decimal mermasEntrada
        {
            get { return mermas; }
            set { mermas = value; }
        }

        public ModeloFormEntrada()
        {
            formatoEntrada = new FormEntradaDAO();
        }
        public string GuardarCambios()
        {
            string mensaje = null;
            try
            {
                var ModeloDataFormatoEntrada = new FormatoEntrada();
                ModeloDataFormatoEntrada.folio = FolioEntrada;
                ModeloDataFormatoEntrada.reciboDonativo = ReciboDonativoEntrada;
                ModeloDataFormatoEntrada.fechaEntrada = fechaReciboEntrada;
                ModeloDataFormatoEntrada.id_aliado = idAliadoEntrada;
                ModeloDataFormatoEntrada.Programa = Programa;
                ModeloDataFormatoEntrada.tarimas = tarimasEntrada;
                ModeloDataFormatoEntrada.mermas = mermasEntrada;

                switch (estado)
                {
                    case EstadoEntidadFormEntrada.Agregado:
                    formatoEntrada.Agregar(ModeloDataFormatoEntrada);
                    mensaje = "Entrada Agregada";
                    break;

                    case EstadoEntidadFormEntrada.Editado:
                    formatoEntrada.Editar(ModeloDataFormatoEntrada);
                    mensaje = "Producto Editado";
                    break;
                }
            }
            catch(Exception ex)
            {
                System.Data.SqlClient.SqlException sqlex = ex as System.Data.SqlClient.SqlException;
                if (sqlex != null && sqlex.Number == 2627) mensaje = "La informacion introducida es incorrecta";
                else
                    mensaje = ex.ToString();
            }
            return mensaje;
        }
        public IEnumerable<ModeloFormEntrada> BuscarFormEntrada(string palabra)
        {
            return VerTablaFormEntradas().FindAll(e => (e.idAliadoEntrada.ToString()).IndexOf(palabra, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        public IEnumerable<ModeloFormEntrada> BuscarFiltroEntrada(string palabra)
        {
            return VerTablaFormEntradas().FindAll(e => (e.ReciboDonativoEntrada + e.ProgramaEnt).IndexOf(palabra, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        public List<ModeloFormEntrada> VerTablaFormEntradas()
        {
            var ModeloDatosFormEntrada = formatoEntrada.VerTabla();
            var ListaFormEntrada = new List<ModeloFormEntrada>();
            foreach (FormatoEntrada item in ModeloDatosFormEntrada)
            {
                ListaFormEntrada.Add(new ModeloFormEntrada
                {
                    FolioEntrada = item.folio,
                    ReciboDonativoEntrada = item.reciboDonativo,
                    fechaReciboEntrada = item.fechaEntrada,
                    idAliadoEntrada = item.id_aliado,
                    Programa = item.Programa,
                    tarimasEntrada = item.tarimas,
                    mermasEntrada = item.mermas
                });
            }
            return ListaFormEntrada;
        }
        public bool VerificarFolio(string folio)
        {
            FormEntradaDAO objaux = new FormEntradaDAO();
            return objaux.VerifacarFolio(Convert.ToInt32(folio));
        }
        public void EliminarEntrada(string folio)
        {
            FormEntradaDAO objaux = new FormEntradaDAO();
            objaux.EliminarEntrada(Convert.ToInt32(folio));
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
