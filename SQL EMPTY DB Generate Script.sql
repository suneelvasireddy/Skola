USE [master]
GO
/****** Object:  Database [Skola]    Script Date: 7/16/2024 4:54:00 PM ******/
CREATE DATABASE [Skola]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Skola', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Skola.mdf' , SIZE = 79936KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Skola_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Skola_log.ldf' , SIZE = 82824KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Skola] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Skola].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Skola] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Skola] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Skola] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Skola] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Skola] SET ARITHABORT OFF 
GO
ALTER DATABASE [Skola] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Skola] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Skola] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Skola] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Skola] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Skola] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Skola] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Skola] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Skola] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Skola] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Skola] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Skola] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Skola] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Skola] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Skola] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Skola] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Skola] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Skola] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Skola] SET  MULTI_USER 
GO
ALTER DATABASE [Skola] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Skola] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Skola] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Skola] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Skola] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Skola] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Skola] SET QUERY_STORE = ON
GO
ALTER DATABASE [Skola] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Skola]
GO
/****** Object:  Table [dbo].[Schools]    Script Date: 7/16/2024 4:54:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schools](
	[SchoolID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Address] [varchar](255) NOT NULL,
	[ContactPerson] [varchar](255) NULL,
	[PhoneNumber] [varchar](255) NULL,
 CONSTRAINT [PK__SchoolID] PRIMARY KEY CLUSTERED 
(
	[SchoolID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Attendance]    Script Date: 7/16/2024 4:54:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance](
	[AttendanceID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[IsPresent] [bit] NOT NULL,
	[WorkingDayID] [int] NOT NULL,
	[SchoolID] [int] NULL,
 CONSTRAINT [PK__Attendance] PRIMARY KEY CLUSTERED 
(
	[AttendanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Student_WorkingDay] UNIQUE NONCLUSTERED 
(
	[StudentID] ASC,
	[WorkingDayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 7/16/2024 4:54:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[StudentID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](255) NOT NULL,
	[LastName] [varchar](255) NOT NULL,
	[PersonNumber] [varchar](255) NOT NULL,
	[SchoolAttendingID] [int] NULL,
	[Age] [int] NULL,
	[ContactNumber] [varchar](255) NULL,
	[ClassAttending] [varchar](255) NULL,
	[GradeID] [int] NOT NULL,
 CONSTRAINT [PK__StudentsID] PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UC_Student_School] UNIQUE NONCLUSTERED 
(
	[StudentID] ASC,
	[SchoolAttendingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_TotalAbsencesPerStudent]    Script Date: 7/16/2024 4:54:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_TotalAbsencesPerStudent] AS
SELECT
    s.StudentID,
    s.FirstName,
    s.LastName,
    SUM(CASE WHEN ab.IsPresent = 0 THEN 1 ELSE 0 END) AS TotalAbsences
FROM
    Students s
JOIN
    Attendance ab ON s.StudentID = ab.StudentID
JOIN
    Schools sch ON s.SchoolAttendingID = sch.SchoolID
GROUP BY
    s.StudentID, s.FirstName, s.LastName;
GO
/****** Object:  Table [dbo].[Grades]    Script Date: 7/16/2024 4:54:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Grades](
	[GradeID] [int] NOT NULL,
	[GradeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__Grades] PRIMARY KEY CLUSTERED 
(
	[GradeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkingDays]    Script Date: 7/16/2024 4:54:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkingDays](
	[WorkingDayID] [int] IDENTITY(1,1) NOT NULL,
	[SchoolID] [int] NOT NULL,
	[DayDate] [date] NOT NULL,
	[IsWorkingDay] [bit] NOT NULL,
 CONSTRAINT [PK_WorkingDays] PRIMARY KEY CLUSTERED 
(
	[WorkingDayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_WorkingDays_WorkingDayID_SchoolID] UNIQUE NONCLUSTERED 
(
	[WorkingDayID] ASC,
	[SchoolID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Attendance_WorkingDayID]    Script Date: 7/16/2024 4:54:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Attendance_WorkingDayID] ON [dbo].[Attendance]
(
	[WorkingDayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Schools_Name]    Script Date: 7/16/2024 4:54:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Schools_Name] ON [dbo].[Schools]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkingDays_SchoolID]    Script Date: 7/16/2024 4:54:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkingDays_SchoolID] ON [dbo].[WorkingDays]
(
	[SchoolID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_Attendance_Schools] FOREIGN KEY([SchoolID])
REFERENCES [dbo].[Schools] ([SchoolID])
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_Schools]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_Attendance_WorkingDays] FOREIGN KEY([WorkingDayID])
REFERENCES [dbo].[WorkingDays] ([WorkingDayID])
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_WorkingDays]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_Attendance_WorkingDays_Schools] FOREIGN KEY([WorkingDayID], [SchoolID])
REFERENCES [dbo].[WorkingDays] ([WorkingDayID], [SchoolID])
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_WorkingDays_Schools]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_StudentID_Attendance] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Students] ([StudentID])
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_StudentID_Attendance]
GO
ALTER TABLE [dbo].[Students]  WITH NOCHECK ADD  CONSTRAINT [FK_Students_Grades] FOREIGN KEY([GradeID])
REFERENCES [dbo].[Grades] ([GradeID])
GO
ALTER TABLE [dbo].[Students] NOCHECK CONSTRAINT [FK_Students_Grades]
GO
ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_SchoolAttendingID] FOREIGN KEY([SchoolAttendingID])
REFERENCES [dbo].[Schools] ([SchoolID])
GO
ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_SchoolAttendingID]
GO
ALTER TABLE [dbo].[WorkingDays]  WITH NOCHECK ADD  CONSTRAINT [FK_WorkingDays_Schools] FOREIGN KEY([SchoolID])
REFERENCES [dbo].[Schools] ([SchoolID])
GO
ALTER TABLE [dbo].[WorkingDays] CHECK CONSTRAINT [FK_WorkingDays_Schools]
GO
/****** Object:  StoredProcedure [dbo].[GenerateUniquePersonNumber]    Script Date: 7/16/2024 4:54:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [dbo].[GetAbsentStudentsBySchoolName]    Script Date: 7/16/2024 4:54:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAbsentStudentsBySchoolName]
    @SchoolName NVARCHAR(100),
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SchoolID INT;

    -- Get SchoolID based on SchoolName
    SELECT @SchoolID = SchoolID
    FROM Schools
    WHERE Name = @SchoolName;

    -- Temporary table to store paginated results
    CREATE TABLE #TempResults (
        RowNum INT IDENTITY(1,1),
        StudentID INT,
        Name NVARCHAR(255),
        TotalAbsentDays INT
    )

    -- Insert paginated results into temporary table
    INSERT INTO #TempResults (StudentID, Name, TotalAbsentDays)
    SELECT 
        s.StudentID,
        s.FirstName + ' ' + s.LastName AS [Name],
        SUM(CAST(a.IsPresent AS INT)) AS TotalAbsentDays
    FROM 
        Students s
    INNER JOIN 
        Attendance a ON s.StudentID = a.StudentID
    WHERE 
        s.SchoolAttendingID = @SchoolID
    GROUP BY 
        s.StudentID, s.FirstName, s.LastName
    ORDER BY 
        TotalAbsentDays DESC;

    -- Get total count of records
    SELECT @TotalCount = COUNT(*)
    FROM #TempResults;

    -- Return paginated results based on @PageNumber and @PageSize
    SELECT 
        StudentID,
        Name,
        TotalAbsentDays
    FROM #TempResults
    WHERE RowNum BETWEEN (@PageNumber - 1) * @PageSize + 1 AND @PageNumber * @PageSize;

    -- Drop temporary table
    DROP TABLE #TempResults;

    SET NOCOUNT OFF;
END
GO
USE [master]
GO
ALTER DATABASE [Skola] SET  READ_WRITE 
GO
