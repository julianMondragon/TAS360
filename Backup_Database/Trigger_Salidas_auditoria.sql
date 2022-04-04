CREATE TRIGGER Tr_Salidas_insert
on Salidas for insert as
begin
	declare @Id int
	declare @trans varchar(100)
	declare @Date varchar(6)
	declare @Time varchar(4)
	
	set @Id		= (select Max(Id) from Salidas)
	set @trans	= (select Transaccion from Salidas where Id = @Id)
	set @Date	= (select format(GETDATE(),'ddMMyy'))
	set @Time	= (select format(CURRENT_TIMESTAMP,'HHmm'))

	insert into Auditoria values(@Id,@Date,@Time,1,1,1,@trans)
end