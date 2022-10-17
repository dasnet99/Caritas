public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
        //CODIGO PARA DESENCRIPTAR
        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        // PUT: api/[controller]/Sellar
        [HttpPut("[action]")]
        public async Task<IActionResult> SellarEnviar([FromBody] ActividadesPersonalSellarEnviarVM viewModel)
        {
            using (_context)
            {
                try
                {
                    // Verifica que los valores de las propiedades cumplen con las condiciones de los DataAnnotations
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    // Verifica que el Id del elemento sea válido
                    if (viewModel.ActividadesPersonalId <= 0)
                    {
                        return BadRequest();
                    }

                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        // Obtiene el elemento que se va a modificar de la base de datos
                        ActividadesPersonal registroPorEditar = await _context.ActividadesPersonal.FirstOrDefaultAsync(x => x.ActividadesPersonalId == viewModel.ActividadesPersonalId);

                        // Verifica que el elemento exista para poder actualizarlo
                        if (registroPorEditar == null)
                        {
                            return NotFound();
                        }

                        //ActividadesPersonalId = viewModel.ActividadesPersonalId;

                        //Crear string con todos los datos a encryptar
                        string textToEncrypt = viewModel.ActividadesPersonalId.ToString() + '+' +
                            viewModel.Anio.ToString() + '+' + viewModel.Mes + '+' + viewModel.FechaCreacion.ToString().Substring(0,10) + '+' +
                            viewModel.NombreUsuarioCreacion + '+' + viewModel.ApPusuarioCreacion + '+' +
                            viewModel.ApMusuarioCreacion + '+' + viewModel.Descripcion;

                        string idusuarioLimpio = _idUsuario.Replace("-", "");
                        string tamanioCadena = textToEncrypt.Length.ToString();

                        string key = idusuarioLimpio.Substring(0, 16)
                        + _UserName.Substring(0, 4)
                        + idusuarioLimpio.Substring(0, 1)
                        + idusuarioLimpio.Substring(idusuarioLimpio.Length - 1, 1)
                        + idusuarioLimpio.Substring(2, 1)
                        + tamanioCadena.Substring(tamanioCadena.Length - 1, 1);

                        var InformacionEncriptada = EncryptString(key, textToEncrypt);

                        //registroPorEditar.Descripcion = viewModel.Descripcion;
                        //registroPorEditar.Anio = viewModel.Anio;
                        //registroPorEditar.Mes = viewModel.Mes;
                        registroPorEditar.SelloPrestador = InformacionEncriptada;
                        //registroPorEditar.SelloAutorizacion = null;
                        // Se llenan los siguientes parámetros por default
                        //registroPorEditar.FechaCreacion = DateTime.Now;
                        //FechaModificacion = DateTime.Now;
                        //FechaAutorizacion = DateTime.Now;
                        //FechaEliminacion = DateTime.Now;
                        //IdUsuarioCreacion = _idUsuario;
                        //registroPorEditar.IdUsuarioModificacion = _idUsuario;
                        registroPorEditar.FechaSelloPrestador = DateTime.Now;
                        //IdUsuarioEvaluacion = _idUsuario;
                        //IdUsuarioAutorizacion = _idUsuario;                            
                        //Privado = false;
                        //Autorizado = false;
                        //Inactivo = false;

                        // Se agrega el elemento a la tabla y se actualiza la BD
                        //_context.ActividadesPersonal.Add(registroPorEditar);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }

                    //Envia correo de notificación en caso de que sea requerido

                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    return BadRequest();
                }
            }
        }

        // PUT: api/[controller]/Sellar
        [HttpPut("[action]")]
        public async Task<IActionResult> SelloAutorizacion([FromBody] ActividadesPersonalSellarEnviarVM viewModel)
        {
            using (_context)
            {
                try
                {
                    // Verifica que los valores de las propiedades cumplen con las condiciones de los DataAnnotations
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    // Verifica que el Id del elemento sea válido
                    if (viewModel.ActividadesPersonalId <= 0)
                    {
                        return BadRequest();
                    }

                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        // Obtiene el elemento que se va a modificar de la base de datos
                        ActividadesPersonal registroPorEditar = await _context.ActividadesPersonal.FirstOrDefaultAsync(x => x.ActividadesPersonalId == viewModel.ActividadesPersonalId);

                        // Verifica que el elemento exista para poder actualizarlo
                        if (registroPorEditar == null)
                        {
                            return NotFound();
                        }

                        //ActividadesPersonalId = viewModel.ActividadesPersonalId;

                        //Crear string con todos los datos a encryptar
                        string textToEncrypt = viewModel.ActividadesPersonalId.ToString() + '+' +
                            viewModel.Anio.ToString() + '+' + viewModel.Mes + '+' + viewModel.FechaCreacion.ToString().Substring(0,10) + '+' +
                            viewModel.NombreUsuarioCreacion + '+' + viewModel.ApPusuarioCreacion + '+' +
                            viewModel.ApMusuarioCreacion + '+' + viewModel.Descripcion;
                        string idusuarioLimpio = _idUsuario.Replace("-", "");
                        string tamanioCadena = textToEncrypt.Length.ToString();

                        string key = idusuarioLimpio.Substring(0, 16)
                        + _UserName.Substring(0, 4)
                        + idusuarioLimpio.Substring(0, 1)
                        + idusuarioLimpio.Substring(idusuarioLimpio.Length - 1, 1)
                        + idusuarioLimpio.Substring(2, 1)
                        + tamanioCadena.Substring(tamanioCadena.Length - 1, 1);

                         var InformacionEncriptada = EncryptString(key, textToEncrypt);

                        //registroPorEditar.Descripcion = viewModel.Descripcion;
                        //registroPorEditar.Anio = viewModel.Anio;
                        //registroPorEditar.Mes = viewModel.Mes;
                        //registroPorEditar.SelloPrestador = InformacionEncriptada;
                        registroPorEditar.SelloAutorizacion = InformacionEncriptada;
                        // Se llenan los siguientes parámetros por default
                        //registroPorEditar.FechaCreacion = DateTime.Now;
                        //FechaModificacion = DateTime.Now;
                        //FechaAutorizacion = DateTime.Now;
                        //FechaEliminacion = DateTime.Now;
                        //IdUsuarioCreacion = _idUsuario;
                        //registroPorEditar.IdUsuarioModificacion = _idUsuario;
                        //registroPorEditar.FechaSelloPrestador = DateTime.Now;
                        registroPorEditar.FechaSelloAutorizacion = DateTime.Now;
                        //IdUsuarioEvaluacion = _idUsuario;
                        //IdUsuarioAutorizacion = _idUsuario;                            
                        //Privado = false;
                        //Autorizado = false;
                        //Inactivo = false;

                        // Se agrega el elemento a la tabla y se actualiza la BD
                        //_context.ActividadesPersonal.Add(registroPorEditar);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }

                    //Envia correo de notificación en caso de que sea requerido

                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    return BadRequest();
                }
            }
        }

        private string GenerarQR(string Usuario, string Sello, int IdReporte)
        {
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode = new QrCode();
            string Liga = "http://si.nl.gob.mx:8081/RevisarCodigo/" + Sello + "/" + IdReporte.ToString();
            qrEncoder.TryEncode(Liga, out qrCode);

            string NombreImagen = IdReporte.ToString() + "_" + Usuario + ".png";
            string RutaArchivo = _configuration["RutaReportesQR"].Replace("\\", "/");
            string RutaFinal = RutaArchivo + NombreImagen;

            GraphicsRenderer renderer = new GraphicsRenderer(new FixedCodeSize(400, QuietZoneModules.Zero), Brushes.Black, Brushes.White);
            MemoryStream ms = new MemoryStream();
            renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
            var imageTemporal = new Bitmap(ms);
            var imagen = new Bitmap(imageTemporal, new Size(new Point(200, 200)));
            imagen.Save(RutaFinal, ImageFormat.Png);

            return RutaFinal;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GeneraPDF(long id)
        {
            try
            {

            VwReporteActividades viewModel = await _context.VwReporteActividades.FirstOrDefaultAsync(x => x.ActividadesPersonalId == id);
            //Genera pdf y descargar
            string RutaQRPrestador = GenerarQR("Prestador", viewModel.SelloPrestador.Replace("/","***").Replace("+","---"), Convert.ToInt32(viewModel.ActividadesPersonalId));
            string RutaQRAutorizador = GenerarQR("Autorizador", viewModel.SelloAutorizacion.Replace("/","***").Replace("+","---"), Convert.ToInt32(viewModel.ActividadesPersonalId));
            string Cantidad0s = new string('0', 6 - viewModel.ActividadesPersonalId.ToString().Length);
            string FolioReporte = Cantidad0s + viewModel.ActividadesPersonalId.ToString();

            
                string ReporteHtml = @"<html>
                     <head>
                     <style>
                     table { font-size: 0.8em; font-family: arial, sans-serif; border-collapse: collapse; width: 100%; border: white 1px solid; margin: 0 auto;}
                     td, th { text-align: left; padding: 8px;} 
                     th {background-color: #7FC7A0; color: black; border: white 1px solid;}
                     h2 {font-family: arial, sans-serif; text-align: center;}
                     div { width: 100%;}
                     .actividades{border: #FFCF9F 2px solid; width: 100%;  margin-top: 25px; }
                     .actividadesth{border: #FFCF9F 2px solid; }
                     .footer { line-height: 14px; color: #FFCF9F; font-family: arial, sans-serif; font-size: small; }
                     .imgizquierda { width: 50%; float: left;}
                     .imgderecha { width: 50%; float: right; padding-left: 20px; }
                     .tablaQR { font-size: 0.8em; font-family: arial, sans-serif; border-collapse: collapse; border: black 1px solid; width: 100%; margin: 0 auto;}
                     .thQR {background-color: white; color: black; border: black 1px solid; text-align: center; width: 50%;}
                     .trQR {background-color: white; color: black;}
                     .thText {background-color: white;}
                     .trText {background-color: white;}
                                         </style> 
                                         </head>
                                         <body style ='width: 100%; text-align: center;'>  
                      <div class='imgizquierda'>
                      < img height = '60px'  style = 'float: left;' src='http://si.nl.gob.mx/acceso/assets/Movilidad%20Y%20Planeacion.png'/>
                           </ div >
                           < div class='imgderecha'>
                     <img height = '80px' style='float: right;' src='http://si.nl.gob.mx/acceso/assets/Escudo%20Nuevo%20NL.png'/> 
                      </div>
                          
                                               < br></br>
                    <br></br>
                     <h2>REPORTE DE ACTIVIDADES <br></br> DE PRESTADORES DE SERVICIOS PROFESIONALES</h2>
                     <table>
                       <tr><th style='width: 40;'>MES</th><th>" + viewModel.Mes.ToUpper() + " DEL " + viewModel.Anio.ToString() + @"</th></tr>
                       <tr><th style='width: 40;'>NOMBRE</th><th>" + viewModel.NombreUsuarioCreacion.ToUpper() + " " + viewModel.ApPusuarioCreacion.ToUpper() + " " +
                       viewModel.ApMusuarioCreacion.ToUpper() + " " + @"</th></tr>
                       <tr><th style='width: 40;'>FUNCION/PUESTO</th><th>" + viewModel.Cargo + @"</th></tr>
                     </table>
                     <table class='actividades'>
                      <tr><th class='actividadesth' style='font-family: arial, sans-serif; color: gray;  background-color: white;' >DESCRIPCIÓN DE ACTIVIDADES</th></tr>
                      <tr><th class='actividadesth' style='font-family: arial, sans-serif; color: #7FC7A0;  background-color: #7FC7A0;' >hola<br></br></th></tr>
                      <tr><th class='actividadesth' style='font-family: arial, sans-serif; color: black;  background-color: white;' >" + viewModel.Descripcion.Replace("\n", "<br></br>") +
                      @"</th></tr>
                      <tr><th class='actividadesth' style='font-family: arial, sans-serif; color: #7FC7A0;  background-color: #7FC7A0;' >hola<br></br></th></tr>
                     </table>
              < br >  </br>

<table class='tablaQR'>
  <tr class='trQR'>
    <th class='thQR'>
                 <span style='line-height: 15px; font-family: arial, sans-serif; color: black; '><b>" + viewModel.NombreUsuarioCreacion.ToUpper() + " " + viewModel.ApPusuarioCreacion.ToUpper() + " " +
                       viewModel.ApMusuarioCreacion.ToUpper() + @"
                <br></br>PRESTADOR DE SERVICIOS</b></span>
    </th>
    <th class='thQR'> 
<span style = 'line-height: 10px; font-family: arial, sans-serif; color: black;' >< b >" + viewModel.UsuarioResponsable.ToUpper() + @"<br></br>DIRECTOR(A) DEL AREA</b></span>
     </th>
  </tr>
  <tr class='trQR'>
        <th class='thQR' style='width: 30%;'>
            <img height='100px' style='margin-left: 20px; vertical-align: text-top;' src='" + RutaQRPrestador + @"'/>
        </th>
    <th class='thQR'>
             <img height='100px' style='margin-left: 20px;' src='" + RutaQRAutorizador + @"'/> 
    </th>
  </tr>
</table>
<p style='font-family: Arial, Helvetica, sans-serif; font-size: small; text-align: center; '>
   Folio del reporte: " + FolioReporte + @"
</p>

              < br >  </br>
              < br >  </br>
               <div style='text-align: center;'> 
                 
                <img height='50px' style='margin-left: 20px;' src='http://si.nl.gob.mx/acceso/assets/Leon%20NL%20Dorado.png'/> 
                <p class='footer'>movilidadyplaneacion@nuevoleon.gob.mx | www.nl.gob/Movilidad.mx 
                  < br >< /br > Washington #2000, Obrera, 64000 Monterrey, Nuevo León. | Tel. 81 2020 6710 o 12</p>
               </ div >
                     </body>";

                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    StringReader sr = new StringReader(ReporteHtml);
                    Document pdfDoc = new Document(PageSize.A4, 40f, 40f, 40f, 40f);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();





                    var pdf = Pdf.From(ReporteHtml);
                    byte[] content = pdf.Content();

                    //Folder where the file will be created 
                    string directory = _configuration["RutaReporteActividades"];
                    // Name of the PDF
                    string filename = "ReporteActividad_" + _UserName.Trim() + DateTime.Now.ToString("dd-MM-yyyy") + ".pdf";

                    //Verifica que exista la direccion y si no existe la crea
                    if (!System.IO.Directory.Exists(directory))
                    {
                        System.IO.Directory.CreateDirectory(directory);
                    }


                    // Open file for reading
                    FileStream _FileStream = new FileStream(directory + filename, FileMode.Create, FileAccess.Write);
                    // Writes a block of bytes to this stream using data from  a byte array.
                    _FileStream.Write(stream.ToArray(), 0, stream.ToArray().Length);

                    // Close file stream
                    _FileStream.Close();

                    return File(stream.ToArray(), "application/pdf", filename);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(ex.ToString());
            }


        }
        //POST: api/[controller]/Crear
        [HttpPost("[action]")]
        public async Task<ActionResult> RevisarCodigo([FromBody] RevisarCodigoVM viewModel)
        {
                try
                {
                    VwReporteActividades ReporteActividades = await _context.VwReporteActividades.FirstOrDefaultAsync(x => x.ActividadesPersonalId == viewModel.ActividadesPersonalId);
                    string textToEncrypt = ReporteActividades.ActividadesPersonalId.ToString() + '+' +
                                        ReporteActividades.Anio.ToString() + '+' + ReporteActividades.Mes + '+' + ReporteActividades.FechaCreacion.ToString().Substring(0,10) + '+' +
                                        ReporteActividades.NombreUsuarioCreacion + '+' + ReporteActividades.ApPusuarioCreacion + '+' +
                                        ReporteActividades.ApMusuarioCreacion + '+' + ReporteActividades.Descripcion;

                    string idusuarioLimpio = ReporteActividades.IdUsuarioCreacion.Replace("-", "");
                    string idAutorizador = ReporteActividades.IdUsuarioResponsable.Replace("-", "");
                    string tamanioCadena = textToEncrypt.Length.ToString();

                    string key = idusuarioLimpio.Substring(0, 16)
                    + ReporteActividades.UserNameCreacion.Substring(0, 4)
                    + idusuarioLimpio.Substring(0, 1)
                    + idusuarioLimpio.Substring(idusuarioLimpio.Length - 1, 1)
                    + idusuarioLimpio.Substring(2, 1)
                    + tamanioCadena.Substring(tamanioCadena.Length - 1, 1);

                string keyAut = idAutorizador.Substring(0, 16)
                + ReporteActividades.UserNameAutorizador.Substring(0, 4)
                + idAutorizador.Substring(0, 1)
                + idAutorizador.Substring(idAutorizador.Length - 1, 1)
                + idAutorizador.Substring(2, 1)
                + tamanioCadena.Substring(tamanioCadena.Length - 1, 1);

                string CadenaInformacion;
                try
                {
                   CadenaInformacion= DecryptString(keyAut, viewModel.Llave);
                }
                catch(Exception ex)
                {
                   CadenaInformacion = DecryptString(key, viewModel.Llave);
                }


                if (CadenaInformacion == textToEncrypt)
                {

                    // Se busca un registro en la tabla de la BD mediante su Id
                    var elementoBD = await _context.VwReporteActividades.Where(x => x.ActividadesPersonalId == viewModel.ActividadesPersonalId).ToListAsync();
               

                    // Verifica que sí se haya encontrado un registro en la BD
                    if (elementoBD == null)
                    {
                        return BadRequest("La informacion no es correcta");
                    }

                    return Ok(elementoBD);
                }
                return BadRequest("Codigo incorrecto");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    return BadRequest();
                }
        }
