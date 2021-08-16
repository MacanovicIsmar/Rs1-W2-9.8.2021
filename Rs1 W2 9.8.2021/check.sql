
use RS1_2020_01_30

select * from Odjeljenje

select * from Takmicenje 

select * from Skola 

select * from OdjeljenjeStavka

select * from Predmet

select * from TakmicenjeUcesnik

delete from TakmicenjeUcesnik
where OdjeljenjeStavkaId is null

delete from Takmicenje
where TakmicenjeId =8
