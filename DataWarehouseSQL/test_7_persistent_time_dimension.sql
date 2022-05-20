--Script from https://www.sqlservercentral.com/articles/mastering-dimensions-of-time
USE [sep4_dwh_test]

GO

CREATE TABLE [edw].[DimTime](
       [TimeKey] [int] NOT NULL,
       [TimeAltKey] [time](0) NULL,
       [HourOfDay] [tinyint] NULL,
       [MinuteOfHour] [tinyint] NULL,
       [SecondOfMinute] [tinyint] NULL,
       [TimeString] [varchar](8) NULL,
 CONSTRAINT [PK_DimTime] PRIMARY KEY CLUSTERED
(
       [TimeKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

-- Declare and set variables for loop
Declare
@StartTime time(0),
@EndTime time(0),
@Time time(0),
@Interval int
Set @StartTime = '00:00:00'
Set @EndTime = '23:59:59'
Set @Time = @StartTime
Set @Interval = 1
-- Loop through Times
WHILE @Time <= @EndTime
BEGIN
    -- Insert record in dimension table if Time does not exist
    IF NOT EXISTS ( SELECT 'X' from edw.DimTime (NOLOCK) where TimeAltKey = @Time )
    BEGIN
  
    INSERT Into edw.DimTime with(rowlock)
    (
    TimeKey,
    TimeAltKey,
    [HourOfDay],
    [MinuteOfHour],
    SecondOfMinute,
    TimeString
    )
    Values
    (
    @Interval,
    @Time,
    datename(hh,@Time),
    datename(minute,@time),
    datename(second,@time),
    CONVERT(varchar(8), @time)
    )
    END
    -- Goto next second
    Select @Time = Dateadd(ss,1, @Time)
    Select @Interval = @Interval + 1
END