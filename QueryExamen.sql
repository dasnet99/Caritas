create database ProductosPrueba
go
use ProductosPrueba
go
Create Table Productos (
id int primary key identity,
categoria varchar(50),
nombre varchar(50),
cantidad int)
go
insert into Productos values ('Computacion', 'laptop',15)
insert into Productos values ('Computacion', 'teclado',25)
insert into Productos values ('Computacion', 'Monitor',40)
insert into Productos values ('Audio', 'Bocina',30)
insert into Productos values ('Audio', 'Audifonos',15)
insert into Productos values ('Ropa', 'Pantalon',6)
insert into Productos values ('Ropa', 'Camisa',5)

Select Categoria, Sum(cantidad) as Cantidad from Productos
group by categoria
having Sum(cantidad) > 30

Create table Ventas(
	idVenta int primary key identity,
	idProducto int foreign key references Productos(id),
	cantidad int,
	total money
)
insert into Ventas values (1,3,15000)
insert into Ventas values (1,4,20000)
insert into Ventas values (1,2,10000)
insert into Ventas values (2,3,300)
insert into Ventas values (3,2,2000)
insert into Ventas values (4,3,1500)
insert into Ventas values (5,8,800)

Select * from Ventas
go

create Function VentasPorProducto(@idProduct int) returns Table
as
return ( select idProducto, Sum(cantidad) as CantidadTotal, Sum(total) as SumaTotal
from Ventas 
where idProducto = @idProduct group by idProducto)
go

select p.id, p.nombre, p.categoria, v.CantidadTotal, v.SumaTotal
from Productos as p cross apply VentasPorProducto(p.id) v
