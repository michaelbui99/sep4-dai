USE [sep4_dwh]
GO

/****** Create Date Table if it does not exist ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edw].[DimDate]') AND type in (N'U'))
CREATE TABLE [edw].[DimDate](
	[D_ID] [int] PRIMARY KEY NOT NULL ,
	[Date] [datetime] NOT NULL,
	[Day] [int] NOT NULL,
	[Month] [int] NOT NULL,
	[MonthName] [nvarchar](9) NOT NULL,
	[Week] [int] NOT NULL,
	[Quarter] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[DayOfWeek] [int] NOT NULL,
	[WeekdayName] [nvarchar](9) NOT NULL,)

/****** Adding data from start of times until end of times... well not really... just the next 100 years, but that should be enough ******/
DECLARE @StartDate DATETIME;
DECLARE @EndDate DATETIME;

SET @StartDate = 1996-01-01
SET @EndDate = DATEADD(YEAR, 100, getdate())



WHILE @StartDate <= @EndDate
 BEGIN
  INSERT INTO edw.[DimDate]
    ([D_ID]
      ,[Date]
      ,[Day]
      ,[Month]
      ,[MonthName]
      ,[Week]
      ,[Quarter]
      ,[Year]
      ,[DayOfWeek]
      ,[WeekdayName]
     )
  SELECT
     CONVERT(CHAR(8),   @StartDate, 112) as D_ID
		,@StartDate as [Date] 
		,DATEPART(day, @StartDate) as Day
		,DATEPART(month, @StartDate) as Month
		,DATENAME(month, @StartDate) as MonthName
		,DATEPART(week, @StartDate) as Week
		,DATEPART(QUARTER,  @StartDate) as Quarter
		,DATEPART(YEAR,     @StartDate) as Year
		,DATEPART(WEEKDAY, @StartDate) as DayOfWeek
		,DATENAME(weekday,@startDate) as WeekdayName

  SET @StartDate = DATEADD(dd, 1, @StartDate)
 END
go
