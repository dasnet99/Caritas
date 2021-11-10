using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaComun.Cache;
using AccesoDatos.Entidades;
using AccesoDatos.Contratos;

namespace AccesoDatos.Repositorios
{
    public class FormEntradaDAO:RepositorioMaestro, iFormatoEntrada
    {
        private string Añadir;
        private string Edicion;
        private string Borrar;
        private string MostrarFormEntrada;

        public FormEntradaDAO()
        {
            Añadir = "insert into FormatoEntrada values (@folio, @reciboDonativo, @fechaEntrada, @id_aliado, @tarimas, @merma, @Programa)";
            Edicion = "update FormatoEntrada set reciboDonativo = @reciboDonativo, fechaEntrada = @fechaEntrada, id_aliado = @id_aliado, Programa = @Programa, tarimas = @tarimas, merma = @merma where folio = @folio";
            Borrar = "delete from FormatoEntrada where folio = @folio";
            MostrarFormEntrada = "select * from FormatoEntrada";
        }
        public int Agregar(FormatoEntrada entidad)
        {
            parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@folio", entidad.folio));
            parametros.Add(new SqlParameter("@reciboDonativo", entidad.reciboDonativo));
            parametros.Add(new SqlParameter("@fechaEntrada", entidad.fechaEntrada));
            parametros.Add(new SqlParameter("@id_aliado", entidad.id_aliado));
            parametros.Add(new SqlParameter("@Programa", entidad.Programa));
            parametros.Add(new SqlParameter("@tarimas", entidad.tarimas));
            parametros.Add(new SqlParameter("@merma", entidad.mermas));
            return ExecuteNonQuery(Añadir);
        }
        public int Editar(FormatoEntrada entidad)
        {
            parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@folio", entidad.folio));
            parametros.Add(new SqlParameter("@reciboDonativo", entidad.reciboDonativo));
            parametros.Add(new SqlParameter("@fechaEntrada", entidad.fechaEntrada));
            parametros.Add(new SqlParameter("@id_aliado", entidad.id_aliado));
            parametros.Add(new SqlParameter("@Programa", entidad.Programa));
            parametros.Add(new SqlParameter("@tarimas", entidad.tarimas));
            parametros.Add(new SqlParameter("@merma", entidad.mermas));
            return ExecuteNonQuery(Edicion);
        }
        public int Eliminar(int folio)
        {
            throw new NotImplementedException();
        }


        public int EditarCargo(FormatoEntrada entidad)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FormatoEntrada> GetAll(int id_usuario)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FormatoEntrada> VerTabla()
        {
            var tabla = ExecuteReader(MostrarFormEntrada, 1);
            var listaFormEntrada = new List<FormatoEntrada>();
            foreach (DataRow item in tabla.Rows)
            {
                listaFormEntrada.Add(new FormatoEntrada
                {
                    folio = Convert.ToInt32(item[0]),
                    reciboDonativo = Convert.ToInt32(item[1]),
                    fechaEntrada = Convert.ToDateTime(item[2]),
                    id_aliado = Convert.ToInt32(item[3]),
                    tarimas = Convert.ToDecimal(item[4]),
                    mermas = Convert.ToDecimal(item[5]),
                    Programa = item[6].ToString()
                });
            }
            return listaFormEntrada;
        }
        public bool VerifacarFolio(int folio)
        {
            using (var conexion = ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "select * from FormatoEntrada where folio = @folio";
                    comando.CommandType = CommandType.Text;
                    comando.Parameters.AddWithValue("@folio", folio);
                    SqlDataReader lector = comando.ExecuteReader();
                    if (lector.HasRows)return false;
                    else return true;
                }
            }
        }
        public void EliminarEntrada(int folio)
        {
            using (var conexion = ObtenerConexion())
            {
                conexion.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexion;
                    comando.CommandText = "EliminarEntrada";
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@folio", folio);
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}
