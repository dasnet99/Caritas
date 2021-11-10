create database Caritas
go
use Caritas
go
create table UsuarioSistemas (
id_usuario int identity(1,1) primary key not null,
NombreUsuario nvarchar(100) unique not null,
contraseña nvarchar(100) not null,
nombres nvarchar(100) not null,
apellidos nvarchar(100) not null,
cargo nvarchar(100) not null,
email nvarchar(150) unique not null)
go
insert into UsuarioSistemas values ('nes9907','nestormanda','Nestor Daniel','Castillo Sifuentes','Administrador','shonleo75@gmail.com')
go

create table Aliados (
id_aliado int identity (1,1)primary key not null,
nombreAliado varchar(100) not null,
ciudad varchar(100) not null,
direccion varchar (100) not null
)
go
create table Categorias (
id_categoria int identity (1,1) primary key not null,
nombre_categoria varchar(100)
)
go
insert into Categorias values('Frutas y verduras')
insert into Categorias values('Abarrotes')
insert into Categorias values('Varios')
insert into Categorias values('Perecederos')
insert into Categorias values('Pan')
go

create table FormatoEntrada (
folio int primary key not null,
reciboDonativo int,
fechaEntrada date,
id_aliado int foreign key references Aliados(id_aliado),
tarimas float,
merma float,
Programa varchar(300)
)
go

create table PesoPiezasEntrada(
folio int foreign key references FormatoEntrada (folio),
id_categoria int foreign key references Categorias(id_categoria),
kilosEntrada float, piezas int, KilosTotal float, merma float, CB money
)
go

create table FormatoSalida (
IdSalida int identity (1,1) primary key not null,
FechaSalida date,
NumeroPaquetes int,
Destino varchar (150),
RevisadoPor varchar (150),
AceptadoRechazado varchar (20),
Observaciones nvarchar (500)
)
go

create table ProductosSalida (
IdSalida int foreign key references FormatoSalida(IdSalida),
Descripcion varchar(200) not null,
Kilogramos float,
Piezas int,
id_categoria int foreign key references Categorias(id_categoria),
FechaCaducidad date,
IntegridadPaquete varchar(5)
)
go

create procedure VerDetallesEntrada
@folio int
as begin
select ppe.folio,cat.nombre_categoria, ppe.kilosEntrada, ppe.piezas,
ppe.KilosTotal, ppe.merma, ppe.CB from Categorias cat,
PesoPiezasEntrada ppe where (ppe.id_categoria = cat.id_categoria)
and (ppe.folio = @folio)
end
go

create procedure MostrarInventario
as begin
select cat2.id_categoria,cat2.nombre_categoria ,
(select sum(ppe.kilosEntrada) from PesoPiezasEntrada as ppe
where ppe.id_categoria = cat2.id_categoria) - (
select sum(ps1.Kilogramos * fs1.NumeroPaquetes) from ProductosSalida as ps1, FormatoSalida as fs1
where ps1.IdSalida = fs1.IdSalida and cat2.id_categoria = ps1.id_categoria) as KgDisponibles,

(select sum(ppe1.piezas) from PesoPiezasEntrada as ppe1
where ppe1.id_categoria = cat2.id_categoria ) - (
select sum(ps2.Piezas * fs2.NumeroPaquetes) from ProductosSalida as ps2, FormatoSalida as fs2
where ps2.IdSalida = fs2.IdSalida and ps2.id_categoria = cat2.id_categoria) as PzsDisponibles

from Categorias as cat2
group by cat2.id_categoria, nombre_categoria
end
go

create procedure DetallesSalida
@IdSalida int 
as begin
select ps.IdSalida, ps.Descripcion, ps.Kilogramos, ps.Piezas, cat.nombre_categoria as Categoria, ps.FechaCaducidad,
ps.IntegridadPaquete from ProductosSalida as ps, Categorias as cat where ps.id_categoria = cat.id_categoria
and ps.IdSalida = @IdSalida
end
go

create procedure DetallesGeneralesEntrada
@opcion int,
@id_aliado int,
@fecha_inicio date,
@fecha_fin date
as begin 
if @opcion = 1 /*General*/
select cat.nombre_categoria, (select sum(ppe.kilosEntrada) from PesoPiezasEntrada as ppe where ppe.id_categoria = cat.id_categoria) as KgTotales,
(select sum(ppe.piezas) from PesoPiezasEntrada as ppe where ppe.id_categoria = cat.id_categoria) as PzsTotales,
(select sum(ppe.merma) from PesoPiezasEntrada as ppe where ppe.id_categoria = cat.id_categoria) as mermaTotal,
(Select sum(ppe.CB) from PesoPiezasEntrada as ppe where ppe.id_categoria = cat.id_categoria) as CBtotal
from Categorias as cat group by cat.id_categoria, cat.nombre_categoria

if @opcion = 2 /*Aliado*/
select cat.nombre_categoria, (select sum(ppe.kilosEntrada) from PesoPiezasEntrada as ppe, FormatoEntrada as fe
where ppe.id_categoria = cat.id_categoria and fe.folio = ppe.folio and fe.id_aliado = @id_aliado) as KgTotales,
(select sum(ppe.piezas) from PesoPiezasEntrada as ppe , FormatoEntrada as fe		 
where ppe.id_categoria = cat.id_categoria and fe.folio = ppe.folio and fe.id_aliado = @id_aliado) as PzsTotales,
(select sum(ppe.merma) from PesoPiezasEntrada as ppe, FormatoEntrada as fe			  
where ppe.id_categoria = cat.id_categoria and fe.folio = ppe.folio and fe.id_aliado = @id_aliado) as mermaTotal,
(Select sum(ppe.CB) from PesoPiezasEntrada as ppe, FormatoEntrada as fe				  
where ppe.id_categoria = cat.id_categoria and fe.folio = ppe.folio and fe.id_aliado = @id_aliado) as CBtotal
from Categorias as cat group by cat.id_categoria, cat.nombre_categoria

if @opcion = 3 /*Aliado y fecha */
select cat.nombre_categoria, (select sum(ppe.kilosEntrada) from PesoPiezasEntrada as ppe, FormatoEntrada as fe
where ppe.id_categoria = cat.id_categoria and fe.folio = ppe.folio and fe.id_aliado = @id_aliado and (fe.fechaEntrada between @fecha_inicio and @fecha_fin)) as KgTotales,
(select sum(ppe.piezas) from PesoPiezasEntrada as ppe , FormatoEntrada as fe
where ppe.id_categoria = cat.id_categoria and fe.folio = ppe.folio and fe.id_aliado = @id_aliado and (fe.fechaEntrada between @fecha_inicio and @fecha_fin)) as PzsTotales,
(select sum(ppe.merma) from PesoPiezasEntrada as ppe, FormatoEntrada as fe
where ppe.id_categoria = cat.id_categoria and fe.folio = ppe.folio and fe.id_aliado = @id_aliado and (fe.fechaEntrada between @fecha_inicio and @fecha_fin)) as mermaTotal,
(Select sum(ppe.CB) from PesoPiezasEntrada as ppe, FormatoEntrada as fe
where ppe.id_categoria = cat.id_categoria and fe.folio = ppe.folio and fe.id_aliado = @id_aliado and (fe.fechaEntrada between @fecha_inicio and @fecha_fin)) as CBtotal
from Categorias as cat group by cat.id_categoria, cat.nombre_categoria

if @opcion = 4 /* fechas*/
select cat.nombre_categoria, (select sum(ppe.kilosEntrada) from PesoPiezasEntrada as ppe, FormatoEntrada as fe
where ppe.id_categoria = cat.id_categoria and ppe.folio = fe.folio and (fe.fechaEntrada between @fecha_inicio and @fecha_fin)) as KgTotales,
(select sum(ppe.piezas) from PesoPiezasEntrada as ppe, FormatoEntrada as fe
where ppe.id_categoria = cat.id_categoria and ppe.folio = fe.folio and (fe.fechaEntrada between @fecha_inicio and @fecha_fin)) as PzsTotales,
(select sum(ppe.merma) from PesoPiezasEntrada as ppe, FormatoEntrada as fe
where ppe.id_categoria = cat.id_categoria and ppe.folio = fe.folio and (fe.fechaEntrada between @fecha_inicio and @fecha_fin)) as mermaTotal,
(Select sum(ppe.CB) from PesoPiezasEntrada as ppe, FormatoEntrada as fe
where ppe.id_categoria = cat.id_categoria and ppe.folio = fe.folio and (fe.fechaEntrada between @fecha_inicio and @fecha_fin)) as CBtotal
from Categorias as cat group by cat.id_categoria, cat.nombre_categoria
end
go

create procedure FiltrarEntradasFecha
@opcion int,
@idAliado int,
@fechaInicio date,
@fechaFin date
as begin
if @opcion = 1
select folio as FolioEntrada, reciboDonativo as ReciboDonativoEntrada,
fechaEntrada as fechaReciboEntrada,id_aliado as idAliadoEntrada, Programa as ProgramaEnt, tarimas as tarimasEntrada,
merma as mermasEntrada  from FormatoEntrada where fechaEntrada between @fechaInicio and @fechaFin
if @opcion = 2
select folio as FolioEntrada, reciboDonativo as ReciboDonativoEntrada,
fechaEntrada as fechaReciboEntrada,id_aliado as idAliadoEntrada, Programa as ProgramaEnt, tarimas as tarimasEntrada,
merma as mermasEntrada  from FormatoEntrada where fechaEntrada between @fechaInicio and @fechaFin and id_aliado = @idAliado
end
go

create procedure DetallesSalidasCategoria
@opcion int,
@fecha_inicio date,
@fecha_fin date
as begin
if @opcion = 1
select cat.nombre_categoria,(select Sum(ps.Kilogramos * fs.NumeroPaquetes) from ProductosSalida as ps, 
FormatoSalida as fs where ps.IdSalida = fs.IdSalida and ps.id_categoria = cat.id_categoria) as KgTotales,
(select Sum(ps.Piezas * fs.NumeroPaquetes) from ProductosSalida as ps, 
FormatoSalida as fs where ps.IdSalida = fs.IdSalida and ps.id_categoria = cat.id_categoria) as PzsTotales
from Categorias as cat
group by cat.nombre_categoria, cat.id_categoria

if @opcion = 2
select cat.nombre_categoria,(select Sum(ps.Kilogramos * fs.NumeroPaquetes) from ProductosSalida as ps, 
FormatoSalida as fs where ps.IdSalida = fs.IdSalida and ps.id_categoria = cat.id_categoria and fs.FechaSalida between @fecha_inicio and @fecha_fin) as KgTotales,
(select Sum(ps.Piezas * fs.NumeroPaquetes) from ProductosSalida as ps, 
FormatoSalida as fs where ps.IdSalida = fs.IdSalida and ps.id_categoria = cat.id_categoria and fs.FechaSalida between @fecha_inicio and @fecha_fin) as PzsTotales
from Categorias as cat
group by cat.nombre_categoria, cat.id_categoria
end
go

create procedure FiltrarSalidasFecha
@fecha_inicio date, @fecha_fin date
as begin
select IdSalida as IDSalida, FechaSalida as FechaSalidaFS, NumeroPaquetes as NumeroPaquetesFS,
Destino as DestinoFS, RevisadoPor as RevisadoPorFS, AceptadoRechazado AceptadoRechazadoFS,
Observaciones as ObservacionesFS from FormatoSalida where FechaSalida between @fecha_inicio and @fecha_fin
end
go

create procedure EliminarEntrada
@folio int
as begin
delete from PesoPiezasEntrada where folio = @folio
delete from FormatoEntrada where folio = @folio
end
go

create procedure EliminarSalida
@IdSalida int
as begin
delete from ProductosSalida where IdSalida = @IdSalida
delete from FormatoSalida where IdSalida = @IdSalida
end 
go