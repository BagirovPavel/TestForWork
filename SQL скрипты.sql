----
-- Таблицы для Игры в Жизнь
----

-- Таблица для сохранений игры
CREATE TABLE ProjectLife.dbo.game_save (
  id int IDENTITY,
  guid uniqueidentifier NOT NULL,
  id_parent int NULL,
  sequence nvarchar(max) NOT NULL,
  [current_date] datetime NOT NULL CONSTRAINT DF_game_save_current_date DEFAULT (getdate()),
  generation int NOT NULL DEFAULT (1),
  CONSTRAINT PK_game_save PRIMARY KEY CLUSTERED (id)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE ProjectLife.dbo.game_save
  ADD CONSTRAINT FK_game_save_id_parent FOREIGN KEY (id_parent) REFERENCES dbo.game_save (id)
GO

-- Таблица для хранения логов
CREATE TABLE ProjectLife.dbo.sys_query_log (
  sequence nvarchar(max) NOT NULL,
  [current_date] datetime NOT NULL CONSTRAINT DF_sys_query_log_current_date DEFAULT (getdate()),
  type nvarchar(50) NOT NULL DEFAULT ('START')
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO


----
-- Процедуры для Игры в Жизнь
----

-- Работа с сохранениями - добавление/удаление
CREATE proc dbo.iud_game_save (@flag char(1), @sequence nvarchar(max) = null, @guid uniqueidentifier, @generation int = null)
as
begin

  /* 04.08.2020 Багиров - Запись сохранения */

  begin try
    begin tran

      if @flag = 'I'
      BEGIN
        declare @id_child int = (select gs.id FROM game_save gs WHERE gs.guid = @guid AND gs.id_parent IS NULL);
      
        insert game_save ([sequence], guid, [current_date], generation)
          VALUES (@sequence, @guid, GETDATE(), @generation);
      
        declare @id_parent int = SCOPE_IDENTITY();
      
        update game_save
           set id_parent = @id_parent
         where id = @id_child;
      end;
      
      if @flag = 'D'
      BEGIN
        delete game_save where guid = @guid;
      END;

    COMMIT;
  end TRY
  BEGIN CATCH
    ROLLBACK;

    select 'Ошибка';
  END CATCH;

END;
GO


-- Запись в логи
CREATE proc dbo.iud_query_log (@sequence nvarchar(max), @type nvarchar(50))
as
begin

  /* 04.08.2020 Багиров - Запись в логи */

  insert sys_query_log (sequence, [current_date], type)
    VALUES (@sequence, GETDATE(), @type);
END;
GO


----
-- Функции для Игры в Жизнь
----

-- Проверка существования сохранения с указанным состоянием игрового поля
create function dbo.sel_check_save(@sequence nvarchar(max))
returns @table table (code int, message nvarchar(100))
as
begin

  /* 05.08.2020 Багиров - проверка наличия сохранений */

  if exists (select * from game_save gs where gs.sequence = @sequence)
    insert @table (code, message)
      VALUES (0, 'Данное сохранение уже создано');
  ELSE
    insert @table (code, message)
      values (1, 'Успех');

  RETURN;

end;
GO


-- Получение списка сохранений
CREATE function dbo.sel_game_save()
returns @table table (id int, [sequence] nvarchar(max), guid uniqueidentifier, [current_date] datetime, generation int)
as
begin
  
  /* 04.08.2020 Багиров - получение списка сохранённых игр */

  insert @table (id, sequence, guid, [current_date], generation)
    select gs.id
          ,gs.sequence
          ,gs.guid
          ,gs.[current_date]
          ,gs.generation
      from game_save gs
     where gs.id_parent is NULL
     ORDER by gs.[current_date] DESC;

  RETURN;

end;
GO


-- Получение списка логов
CREATE function dbo.sel_query_log()
returns @table table (oper_type nvarchar(50), [current_date] datetime)
as
begin

  /* 05.08.2020 Багиров - получение логов */

  insert @table (oper_type, [current_date])
    select sql.type, sql.[current_date]
      from sys_query_log sql
     ORDER by sql.[current_date] DESC;

  RETURN;
end;
GO


----
-- Задание SQL
----

CREATE function dbo.test_task1(@ArtID int)
returns @table table (DayID date, CntrID int, ArtID int, EndQnty decimal(16, 3), daydiff int)
as
begin
  
  /* 05.08.2020 Багиров - тестовое задание 1 */

  with simpledate as (
    select * 
      from tTestTable1 t1 
     where t1.ArtID = @ArtID),
       firstdate as (
    select * 
      from tTestTable1 t2 
     where t2.ArtID = @ArtID
       and not EXISTS (select * 
                         from tTestTable1 te 
                        where te.ArtID = t2.ArtID
                          and CAST(te.DayID as DATETIME) + 1 = CAST(t2.DayID as DATETIME))),
       dates as (
    select s.ID as 'id'
          ,s.DayID as 'dayID'
          ,s.CntrID as 'cntrID'
          ,s.ArtID as 'artID'
          ,s.EndQnty as 'endQnty'
          ,MAX(f.DayID) as 'firstDayOfSequence' 
      from simpledate s, firstdate f 
     where s.DayID >= f.DayID 
     GROUP by s.DayID, s.ID, s.CntrID, s.ArtID, s.EndQnty)
    INSERT @table (DayID, CntrID, ArtID, EndQnty, daydiff)
      select dt.dayID
            ,dt.cntrID
            ,dt.artID
            ,dt.endQnty
            ,DATEDIFF(DAY, dt.firstDayOfSequence, dt.dayID) + 1
        from dates dt
       order by dt.id ASC;

  RETURN;
end;

GO


CREATE function dbo.test_task2(@ArtID int)
returns @table table (DayID date, CntrID int, ArtID int, EndQnty decimal(16, 3), daydiff int, qntyDiff decimal(16, 3))
as
begin
  
  /* 05.08.2020 Багиров - тестовое задание 2 */

  with simpledate as (
    select * 
      from tTestTable1 t1 
     where t1.ArtID = @ArtID),
       firstdate as (
    select * 
      from tTestTable1 t2 
     where t2.ArtID = @ArtID
       and not EXISTS (select * 
                         from tTestTable1 te 
                        where te.ArtID = t2.ArtID
                          and CAST(te.DayID as DATETIME) + 1 = CAST(t2.DayID as DATETIME))),
       dates as (
    select s.ID as 'id'
          ,s.DayID as 'dayID'
          ,s.CntrID as 'cntrID'
          ,s.ArtID as 'artID'
          ,s.EndQnty as 'endQnty'
          ,MAX(f.DayID) as 'firstDayOfSequence' 
      from simpledate s, firstdate f 
     where s.DayID >= f.DayID 
     GROUP by s.DayID, s.ID, s.CntrID, s.ArtID, s.EndQnty)
    INSERT @table (DayID, CntrID, ArtID, EndQnty, daydiff, qntyDiff)
      select dt.dayID
            ,dt.cntrID
            ,dt.artID
            ,dt.endQnty
            ,DATEDIFF(DAY, dt.firstDayOfSequence, dt.dayID) + 1
            ,(select t1.EndQnty - dt.endQnty
                from tTestTable1 t1
               where t1.DayID = dt.firstDayOfSequence
                 and t1.ArtID = dt.artID)
        from dates dt
       order by dt.id ASC;

  RETURN;
end;

GO


CREATE function dbo.test_task3()
returns @table table (volumePercent decimal(16, 3), weekNumber int)
as
begin
  
  /* 05.08.2020 Багиров - тестовое задание 3 */

  with firstvolume as (
    select t1.Volume
          ,DATEPART(WEEK, t1.DayID) as 'weekNumber'
      from tTestTable2 t1
     where t1.SignID = 1
       and t1.IncomingType = 1
    ),
       wholevolume as (
    select t1.Volume
          ,DATEPART(WEEK, t1.DayID) as 'weekNumber'
      from tTestTable2 t1
     where t1.IncomingType in (1, 2)
    )
  INSERT @table (volumePercent, weekNumber)
    select (select COALESCE(SUM(f.Volume), 0) from firstvolume f where f.weekNumber = w.weekNumber) / SUM(w.Volume), w.weekNumber
      from wholevolume w
     GROUP by w.weekNumber
     ORDER BY w.weekNumber ASC;

  RETURN;
end;

GO


CREATE function dbo.test_task4()
returns @table table (weekNumber int, artID int, volumePercent decimal(16, 3), priceDiff decimal(16, 3))
as
begin
  
  /* 05.08.2020 Багиров - тестовое задание 4 */

  with resultData as (
    select t1.Volume
          ,t1.ArtID
          ,DATEPART(WEEK, t1.DayID) as 'weekNumber'
          ,(t1.Price - t1.PriceCalc) * t1.Volume as 'priceDiff'
      from tTestTable2 t1
     where t1.PriceCalc < t1.Price
       and t1.SignID = 1
       and t1.IncomingType = 1
    ),
       wholeData as (
    select t1.Volume
          ,t1.ArtID
          ,DATEPART(WEEK, t1.DayID) as 'weekNumber'
          ,(t1.Price - t1.PriceCalc) * t1.Volume as 'priceDiff'
      from tTestTable2 t1
    )
  insert @table (weekNumber, artID, volumePercent, priceDiff)
    select rd.weekNumber
          ,rd.ArtID
          ,SUM(rd.Volume) / (select SUM(wd.Volume) from wholeData wd where wd.ArtID = rd.ArtID and wd.weekNumber = rd.weekNumber)
          ,rd.priceDiff
      from resultData rd
     GROUP BY rd.weekNumber, rd.ArtID, rd.priceDiff

  RETURN;
end;

GO