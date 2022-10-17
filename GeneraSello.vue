<template>
    <v-container>
        <v-col>
            <v-row class="d-flex flex-row-reverse pb-md-6">
                <v-btn @click="dialogoCrearReporte = true" color="primary" dark>Nuevo Reporte</v-btn>
            </v-row>
        </v-col>
        <v-data-table :headers="headers" :items="listadoActividadesPersonal" :items-per-page="5" class="elevation-1">
            

            <template v-slot:item.acciones="{ item }">
                <v-tooltip bottom>
                    <template v-slot:activator="{ on }">

                <v-icon @click="View(item)" style="cursor:pointer;" v-on="on">
                    mdi-eye
                </v-icon>
                    </template>
                    <span>Ver detalles</span>
                </v-tooltip>
                
                <v-tooltip bottom>
                    <template v-slot:activator="{ on }">
                <v-icon @click="Editar(item)" style="cursor:pointer;" v-on="on" v-show="!item.selloPrestador" >
                    mdi-pencil
                </v-icon>
                    </template>
                    <span>Editar reporte</span>
                </v-tooltip>
                <v-tooltip bottom>
                    <template v-slot:activator="{ on }">
                <v-icon @click="ViewSellar(item)" style="cursor:pointer;" v-on="on" v-show="!item.selloPrestador">
                    mdi-stamper
                </v-icon>
                    </template>
                    <span>Sellar y enviar</span>
                </v-tooltip>
            </template>
        </v-data-table>


        <!--Modal para crear nuevo reporte-->
        <v-dialog v-model="dialogoCrearReporte" transition="dialog-bottom-transition" max-width="800">
            <v-card>
                <v-toolbar color="green darken-1" dark>Crear nuevo Reporte</v-toolbar>
                <v-card-text>
                    <v-row class="pt-md-5">                        
                        <v-col cols="12" md="6">
                            <v-autocomplete clearable :items="listMeses"
                                v-model="defaultItem.Mes"
                                label="Mes"></v-autocomplete>
                        </v-col>
                        <v-col cols="12" md="6">
                            <v-autocomplete clearable :items="listAnios"
                                v-model="defaultItem.Anio"
                                label="Año"></v-autocomplete>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12">
                            <v-textarea auto-grow rows="1" v-model="defaultItem.Descripcion" :rules="CampRules"
                                label="Descripción">
                            </v-textarea>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" sm="12" md="12" v-show="valida">
                            <div class="red--text" v-for="v in validaMensaje" :key="v" v-text="v">
                            </div>
                        </v-col>
                    </v-row>
                </v-card-text>
                <v-card-actions class="d-flex flex-row-reverse">
                    <v-btn color="primary" dark @click="Crear" class="mr-3">Crear</v-btn>
                    <v-btn color="primary" outlined @click="Cerrar" class="mr-3">Cancelar</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <!--Modal para editar reporte-->
        <v-dialog v-model="dialogoEditarReporte" transition="dialog-bottom-transition" max-width="800">
            <v-card>
                <v-toolbar color="green darken-1" dark>Editar Reporte</v-toolbar>
                <v-card-text>
                    <v-row>                        
                        <v-col cols="12" md="6">
                            <v-autocomplete clearable :items="listMeses"
                                v-model="editedItem.Mes"
                                label="Mes"></v-autocomplete>
                        </v-col>
                        <v-col cols="12" md="6">
                            <v-autocomplete clearable :items="listAnios"
                                v-model="editedItem.Anio"
                                label="Año"></v-autocomplete>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12">
                            <v-textarea auto-grow rows="1" v-model="editedItem.Descripcion" :rules="CampRules"
                                label="Descripción">
                            </v-textarea>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" sm="12" md="12" v-show="valida">
                            <div class="red--text" v-for="v in validaMensaje" :key="v" v-text="v">
                            </div>
                        </v-col>
                    </v-row>
                </v-card-text>
                <v-card-actions class="d-flex flex-row-reverse">
                    <v-btn color="primary" dark @click="Actualizar" class="mr-3">Actualizar</v-btn>
                    <v-btn color="primary" outlined @click="Cerrar" class="mr-3">Cancelar</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <!--Modal para visualizar reporte-->
        <v-dialog v-model="dialogoVerReporte" transition="dialog-bottom-transition" max-width="60%">
            <v-card>
                <v-toolbar color="green darken-1" dark>Reporte de  {{ VistaItem.NombreUsuarioCreacion }} {{ VistaItem.apPUsuarioCreacion}} {{ VistaItem.ApMUsuarioCreacion }} </v-toolbar>
                <v-card-text>
                    <v-row class="pt-md-8">                        
                        <v-col cols="12" md="4">
                            <v-text-field readonly
                                v-model="VistaItem.Mes"
                                label="Mes"></v-text-field>
                        </v-col>
                        <v-col cols="12" md="4">
                            <v-text-field readonly
                                v-model="VistaItem.Anio"
                                label="Año"></v-text-field>
                        </v-col>
                        <v-col cols="12" md="4">
                            <v-text-field readonly
                                v-model="VistaItem.FechaCreacion"
                                label="Fecha Creación"></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12">
                            <v-textarea auto-grow rows="1" readonly v-model="VistaItem.Descripcion"
                                label="Descripción">
                            </v-textarea>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" md="6">
                            <v-textarea rows="3" readonly v-model="VistaItem.SelloPrestador"
                                label="Sello Prestador">
                            </v-textarea>
                        </v-col>
                        <v-col cols="12" md="6">
                            <v-textarea  rows="1" readonly v-model="VistaItem.FechaSelloPrestador"
                                label="Fecha Sello Prestador">
                            </v-textarea>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" md="6">
                            <v-textarea rows="3" readonly v-model="VistaItem.SelloAutorizacion"
                                label="Sello Autorización">
                            </v-textarea>
                        </v-col>
                        <v-col cols="12" md="6">
                            <v-textarea auto-grow rows="1" readonly v-model="VistaItem.FechaSelloAutorizacion"
                                label="Fecha Sello Autorización">
                            </v-textarea>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" md="4">
                            <v-textarea auto-grow rows="1" readonly v-model="VistaItem.UsuarioResponsable"
                                label="Usuario Autorizador">
                            </v-textarea>
                        </v-col>
                    </v-row>
                </v-card-text>
                <v-card-actions class="d-flex flex-row-reverse">
                    <v-btn color="primary" dark @click="generarPdf" class="mr-3" v-show="VistaItem.SelloPrestador && VistaItem.SelloAutorizacion">Generar reporte</v-btn>
                    <v-btn color="primary" outlined @click="Cerrar" class="mr-3">Cerrar</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <!--Modal para Sellar reporte-->
        <v-dialog v-model="dialogoSellarReporte" transition="dialog-bottom-transition" max-width="60%">
            <v-card>
                <v-toolbar color="green darken-1" dark>Reporte de  {{ VistaItem.NombreUsuarioCreacion }} {{ VistaItem.apPUsuarioCreacion}} {{ VistaItem.ApMUsuarioCreacion }} </v-toolbar>
                <v-card-text>
                    <v-row class="pt-md-8">                        
                        <v-col cols="12" md="4">
                            <v-text-field readonly
                                v-model="VistaItem.Mes"
                                label="Mes"></v-text-field>
                        </v-col>
                        <v-col cols="12" md="4">
                            <v-text-field readonly
                                v-model="VistaItem.Anio"
                                label="Año"></v-text-field>
                        </v-col>
                        <v-col cols="12" md="4">
                            <v-text-field readonly
                                v-model="VistaItem.FechaCreacion"
                                label="Fecha Creación"></v-text-field>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12">
                            <v-textarea rows="1" readonly v-model="VistaItem.Descripcion"
                                label="Descripción">
                            </v-textarea>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" md="6">
                            <v-textarea rows="1" readonly v-model="VistaItem.SelloPrestador"
                                label="Sello Prestador">
                            </v-textarea>
                        </v-col>
                        <v-col cols="12" md="6">
                            <v-textarea auto-grow rows="1" readonly v-model="VistaItem.FechaSelloPrestador"
                                label="Fecha Sello Prestador">
                            </v-textarea>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col cols="12" md="4">
                            <v-textarea auto-grow rows="1" readonly v-model="VistaItem.FechaSelloAutorizacion"
                                label="Fecha Sello Autorización">
                            </v-textarea>
                        </v-col>
                        <v-col cols="12" md="4">
                            <v-textarea auto-grow rows="1" readonly v-model="VistaItem.SelloAutorizacion"
                                label="Sello Autorización">
                            </v-textarea>
                        </v-col>
                        <v-col cols="12" md="4">
                            <v-textarea auto-grow rows="1" readonly v-model="VistaItem.UsuarioResponsable"
                                label="Usuario Autorizador">
                            </v-textarea>
                        </v-col>

                    </v-row>
                </v-card-text>
                <v-card-actions class="d-flex flex-row-reverse">
                    <v-btn color="primary" dark @click="Sellar" class="mr-3">Sellar</v-btn>
                    <v-btn color="primary" outlined @click="Cerrar" class="mr-3">Cerrar</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <!-- MODAL DESCARGANDO -->

        <v-dialog v-model="Descarga" persistent width="150" >
          <v-card>
            <v-card-title class="body-1">Descargando...</v-card-title>
              <v-card-text>
                  <v-progress-circular :size="100" class="justify-center" color="primary" indeterminate >
                  </v-progress-circular>
              </v-card-text>
          </v-card>
        </v-dialog>
    </v-container>
</template>
<script>
    import axios from 'axios';
    export default {
        data: () => ({

            dialogoCrearReporte: false,
            dialogoVerReporte:false,
            dialogoSellarReporte: false,
            loadListadoActividadesPersonal: false,
            dialogoEditarReporte: false,
            Descarga: false,

            listAnios: [2021,2022,2023,2024,2025,2026,2027],
            listMeses: ['ENERO','FEBRERO','MARZO','ABRIL','MAYO','JUNIO','JULIO','AGOSTO','SEPTIEMBRE','OCTUBRE','NOVIEMBRE','DICIEMBRE'],
            listadoActividadesPersonal : [],

            defaultItem: {
                Descripcion: '',
                Anio: '',
                Mes: ''
            },

            editedItem: {
                ActividadesPersonalId:0,
                Descripcion:'',
                Anio:0,
                Mes:''
            },
            sellarItem:{
              ActividadesPersonalId:0,
                Descripcion:'',
                Anio:0,
                Mes:'',
                FechaCreacion:null,
                NombreUsuarioCreacion:null,
                ApPUsuarioCreacion:null,
                ApMUsuarioCreacion:null
            },
            VistaItem: {
              ActividadesPersonalId:0,
                Descripcion:'',
                Anio:0,
                Mes:'',
                SelloPrestador:'',
                SelloAutorizacion:'',
                Privado: null,
                FechaCreacion:null,
                FechaModificacion:null,
                FechaAutorizacion:null,
                FechaEliminacion:null,
                IdUsuarioCreacion:null,
                NombreUsuarioCreacion:null,
                ApPUsuarioCreacion:null,
                ApMUsuarioCreacion:null,
                IdUsuarioModificacion:null,
               IdUsuarioEvaluacion:null,
                Autorizado:null,
               Inactivo:null,
               FechaSelloPrestador:null,
               FechaSelloAutorizacion:null,
               AreaDeTrabajo:null,
               IdUsuarioResponsable:null, 
               UsuarioResponsable:null
           },

            headers: [
                { text: 'Acciones', value: 'acciones', sortable: false, class: "green white--text subtitle-1 ", },
                {
                    text: 'Año',
                    value: 'anio',
                    class: "green white--text subtitle-1 ",
                },
                {
                    text: 'Mes',
                    value: 'mes',
                    class: "green white--text subtitle-1 ",
                },
                {
                    text: 'Fecha de Envío',
                    value: 'fechaCreacion',
                    class: "green white--text subtitle-1 ",
                },
                {
                    text: 'Revision',
                    value: 'autorizado',
                    class: "green white--text subtitle-1 ",
                },

            ],
            CampRules: [
                v => !!v || 'El campo es requerido',
            ],

            valida: 0,
            validaMensaje: [],
        }),
        created() {
            this.Listar();
        },
        methods: {
            Listar() {
                console.log("Listar");
                let me = this;
                me.loadListadoActividadesPersonal = true;

                axios.get('api/ActividadesPersonal/ListarActividadesPersonales').then(function (response) {
                    debugger;
                    me.listadoActividadesPersonal = response.data;
                    me.loadListadoActividadesPersonal = false;

                    return true;
                }).catch(function (error) {
                    console.log(error);
                    me.loadListadoActividadesPersonal = false;
                    alert('La información no se cargó correctamente')
                    return false;
                });
            },
            CambiarTab(item) {

                console.log()
                this.$root.$refs.MacroProyectosRef.tab = "Detalle";
                this.$root.$refs.MacroProyectosRef.Nombre = item.nombreMacroProyecto;
                this.$root.$refs.MacroProyectosRef.MacroProyectoId = item.macroProyectoId;
                this.$root.$refs.MacroProyectosRef.Descripcion = item.descripcion;
                this.$root.$refs.MacroProyectosRef.Categoria = item.categoria;
                this.$root.$refs.MacroProyectosRef.FechaInicio = item.fechaInicio2;
                this.$root.$refs.MacroProyectosRef.FechaFin = item.fechaFin2;
                this.$root.$refs.MacroProyectosRef.Monto = item.montoTotal;

            },

            Crear() {
                if (this.validarCreacionRegistro()) {
                    return;
                }
                let me = this;
                me.loading = true;
                me.defaultItem.Anio = parseInt(me.defaultItem.Anio);

                console.log(me.defaultItem);
                axios.post('api/ActividadesPersonal/Crear',me.defaultItem).then(function(response){
                    me.loading = false;                    
                    
                    alert('Reporte registrado.');            
                    me.Cerrar();            
                    me.Listar();
                }).catch(function(error){
                    me.loading = false;
                    //alert('Ha ocurrido un error, verifica la información e intenta de nuevo.');
                    me.validaMensaje.push(error.response);
                    me.valida = true;
                    console.log(error.response);
                });
            },
            Sellar(){
                let me = this;
                me.loading = true;
                me.sellarItem.Anio = parseInt(me.VistaItem.Anio);
                me.sellarItem.ActividadesPersonalId = me.VistaItem.ActividadesPersonalId;
                me.sellarItem.Descripcion = me.VistaItem.Descripcion;
                me.sellarItem.Mes = me.VistaItem.Mes;
                me.sellarItem.FechaCreacion = me.VistaItem.FechaCreacion;
                me.sellarItem.NombreUsuarioCreacion = me.VistaItem.NombreUsuarioCreacion;
                me.sellarItem.ApPUsuarioCreacion = me.VistaItem.ApPUsuarioCreacion;
                me.sellarItem.ApMUsuarioCreacion = me.VistaItem.ApMUsuarioCreacion;

                console.log(me.sellarItem, 'datos');
                axios.put('api/ActividadesPersonal/SellarEnviar',me.sellarItem).then(function(response){
                    me.loading = false;
                    alert('Reporte sellado');
                    me.Cerrar();
                    me.Listar();
                }).catch(function(error){
                    me.loading = false;
                    console.log(error.response);
                })
            },
    generarPdf(){
                let me = this;
                me.Descarga = true;
                me.sellarItem.ActividadesPersonalId = me.VistaItem.ActividadesPersonalId;
    axios({
          url: 'api/ActividadesPersonal/GeneraPDF/' + me.sellarItem.ActividadesPersonalId.toString(), //your url
          method: 'GET',
          responseType: 'blob', // important/
        }).then((response) => {
          const url = window.URL.createObjectURL(new Blob([response.data]));
          const link = document.createElement('a');
          link.href = url;
          link.setAttribute('download', 'ReporteActividades.pdf'); //or any other extension
          document.body.appendChild(link);
          link.click();
          me.Descarga = false;
          return true;
        }).catch(function(error){
          console.log(error);
          return false;
        });
    },
            View(item){
                var selectedItemReportes = Object.assign({}, item)
                this.VistaItem.ActividadesPersonalId = selectedItemReportes.actividadesPersonalId;
                this.VistaItem.Descripcion = selectedItemReportes.descripcion;
                this.VistaItem.Anio = selectedItemReportes.anio;
                this.VistaItem.Mes = selectedItemReportes.mes;
                this.VistaItem.SelloPrestador = selectedItemReportes.selloPrestador;
                this.VistaItem.SelloAutorizacion = selectedItemReportes.selloAutorizacion;
                this.VistaItem.Privado = selectedItemReportes.privado;
                this.VistaItem.FechaCreacion = selectedItemReportes.fechaCreacion.substring(0,10);
                this.VistaItem.FechaModificacion = selectedItemReportes.fechaModificacion;
                this.VistaItem.FechaAutorizacion = selectedItemReportes.fechaAutorizacion;
                this.VistaItem.FechaEliminacion = selectedItemReportes.fechaEliminacion;
                this.VistaItem.IdUsuarioCreacion = selectedItemReportes.idUsuarioCreacion;
                this.VistaItem.NombreUsuarioCreacion = selectedItemReportes.nombreUsuarioCreacion;
                this.VistaItem.ApPUsuarioCreacion = selectedItemReportes.apPUsuarioCreacion;
                this.VistaItem.ApMUsuarioCreacion = selectedItemReportes.apMUsuarioCreacion;
                this.VistaItem.IdUsuarioModificacion = selectedItemReportes.idUsuarioModificacion;
                this.VistaItem.IdUsuarioEvaluacion = selectedItemReportes.idUsuarioEvaluacion;
                this.VistaItem.Autorizado = selectedItemReportes.autorizado;
                this.VistaItem.Inactivo = selectedItemReportes.inactivo;
                this.VistaItem.FechaSelloPrestador = selectedItemReportes.fechaSelloPrestador;
                this.VistaItem.FechaSelloAutorizacion = selectedItemReportes.fechaSelloAutorizacion;
                this.VistaItem.AreaDeTrabajo = selectedItemReportes.areaDeTrabajo;
                this.VistaItem.IdUsuarioResponsable = selectedItemReportes.idUsuarioResponsable;
                this.VistaItem.UsuarioResponsable = selectedItemReportes.usuarioResponsable;
                this.dialogoVerReporte = true;
            },
            ViewSellar(item){
                var selectedItemReportes = Object.assign({}, item)
                this.VistaItem.ActividadesPersonalId = selectedItemReportes.actividadesPersonalId;
                this.VistaItem.Descripcion = selectedItemReportes.descripcion;
                this.VistaItem.Anio = selectedItemReportes.anio;
                this.VistaItem.Mes = selectedItemReportes.mes;
                this.VistaItem.SelloPrestador = selectedItemReportes.selloPrestador;
                this.VistaItem.SelloAutorizacion = selectedItemReportes.selloAutorizacion;
                this.VistaItem.Privado = selectedItemReportes.privado;
                this.VistaItem.FechaCreacion = selectedItemReportes.fechaCreacion.substring(0,10);
                this.VistaItem.FechaModificacion = selectedItemReportes.fechaModificacion;
                this.VistaItem.FechaAutorizacion = selectedItemReportes.fechaAutorizacion;
                this.VistaItem.FechaEliminacion = selectedItemReportes.fechaEliminacion;
                this.VistaItem.IdUsuarioCreacion = selectedItemReportes.idUsuarioCreacion;
                this.VistaItem.NombreUsuarioCreacion = selectedItemReportes.nombreUsuarioCreacion;
                this.VistaItem.ApPUsuarioCreacion = selectedItemReportes.apPUsuarioCreacion;
                this.VistaItem.ApMUsuarioCreacion = selectedItemReportes.apMUsuarioCreacion;
                this.VistaItem.IdUsuarioModificacion = selectedItemReportes.idUsuarioModificacion;
                this.VistaItem.IdUsuarioEvaluacion = selectedItemReportes.idUsuarioEvaluacion;
                this.VistaItem.Autorizado = selectedItemReportes.autorizado;
                this.VistaItem.Inactivo = selectedItemReportes.inactivo;
                this.VistaItem.FechaSelloPrestador = selectedItemReportes.fechaSelloPrestador;
                this.VistaItem.FechaSelloAutorizacion = selectedItemReportes.fechaSelloAutorizacion;
                this.VistaItem.AreaDeTrabajo = selectedItemReportes.areaDeTrabajo;
                this.VistaItem.IdUsuarioResponsable = selectedItemReportes.idUsuarioResponsable;
                this.VistaItem.UsuarioResponsable = selectedItemReportes.usuarioResponsable;
                this.dialogoSellarReporte = true;
            },
            Editar(item){
                var selectedItemReportes = Object.assign({}, item)
                this.editedItem.ActividadesPersonalId = selectedItemReportes.actividadesPersonalId;
                this.editedItem.Descripcion = selectedItemReportes.descripcion;
                this.editedItem.Anio = selectedItemReportes.anio;
                this.editedItem.Mes = selectedItemReportes.mes;
                this.dialogoEditarReporte = true;
            },
            Actualizar() {
                if (this.validarEditRegistro()) {
                    return;
                }
                let me = this;
                me.loading = true;
                me.editedItem.Anio = parseInt(me.editedItem.Anio);

                console.log(me.editedItem, 'editar');
                axios.put('api/ActividadesPersonal/Actualizar',me.editedItem).then(function(response){
                    me.loading = false;                    
                    
                    alert('Reporte actualizado.');            
                    me.Cerrar();            
                    me.Listar();
                }).catch(function(error){
                    me.loading = false;
                    //alert('Ha ocurrido un error, verifica la información e intenta de nuevo.');
                    me.validaMensaje.push(error.response);
                    me.valida = true;
                    console.log(error.response);
                });
            },
            Cerrar() {
                let me = this;

                me.macroCrear = false;
                me.dialogoCrearReporte = false;
                me.dialogoVerReporte = false;
                me.dialogoSellarReporte = false;
                me.dialogoEditarReporte = false;
                me.defaultItem.Anio = 0;
                me.defaultItem.Mes = '';
                me.defaultItem.Descripcion = '';
                me.editedItem.ActividadesPersonalId = null;
                me.editedItem.Anio = 0;
                me.editedItem.Mes = '';
                me.editedItem.Descripcion = '';
                me.valida = 0;
                me.validaMensaje = [];
            },

            validarCreacionRegistro() {
                this.valida = 0;
                this.validaMensaje = [];

                if (!this.defaultItem.Descripcion) {
                    this.validaMensaje.push("El campo Descripcion no puede estar vacío.");
                }
                if (!this.defaultItem.Anio) {
                    this.validaMensaje.push("El campo año no puede estar vacío.");
                }
                if (!this.defaultItem.Mes) {
                    this.validaMensaje.push("El campo mes no puede estar vacío.");
                }

                if (this.validaMensaje.length) {
                    this.valida = 1;
                }

                return this.valida;
            },

            validarEditRegistro() {
                this.valida = 0;
                this.validaMensaje = [];                

                if (!this.editedItem.Descripcion) {
                    this.validaMensaje.push("El campo Descripcion no puede estar vacío.");
                }
                if (!this.editedItem.Anio) {
                    this.validaMensaje.push("El campo Fecha Inicio no puede estar vacío.");
                }
                if (!this.editedItem.Mes) {
                    this.validaMensaje.push("El campo Mes no puede estar vacío.");
                }

                if (this.validaMensaje.length) {
                    this.valida = 1;
                }

                return this.valida;
            },
        },

    }
</script>
