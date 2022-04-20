CREATE TRIGGER Tr_Entradas_insert
on Entradas for insert as
begin
	declare @Id int
	declare @trans varchar(100)
	declare @Date varchar(6)
	declare @Time varchar(4)
	
	set @Id		= (select Max(Id) from Entradas)
	set @trans	= (select Transaccion from Entradas where Id = @Id)
	set @Date	= (select format(GETDATE(),'ddMMyy'))
	set @Time	= (select format(CURRENT_TIMESTAMP,'HHmm'))

	insert into Auditoria(Fecha,Hora,Estado,Flujo_datos,secuencial,Transaccion) values(@Date,@Time,1,1,1,@trans)
end


--select * from Entradas
--select * from Auditoria
--insert into Entradas (Id, Transaccion)values('2','0298586100021000000125260000001237629.540000150223160115022316100501361.038232012')