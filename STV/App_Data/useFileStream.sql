ALTER DATABASE stvdb
ADD FILEGROUP FileStreamFileGroup CONTAINS FILESTREAM
GO

ALTER DATABASE stvdb
ADD FILE (NAME = FileStreamData, FILENAME = 'C:\SqlServerFS\BlobSTV')
TO FILEGROUP FileStreamFileGroup
GO

EXEC sp_configure filestream_access_level, 2  
RECONFIGURE  


ALTER TABLE Arquivo ADD  CONSTRAINT [DF_Arquivo_Idarquivo]  DEFAULT (newid()) FOR [Idarquivo]

ALTER TABLE Arquivo
ADD UNIQUE (Idarquivo)
ADD Blob [varbinary](max) FILESTREAM  NULL


ADD Idmaterial int  NOT NULL
ADD Blob [varbinary](max) FILESTREAM  NULL

CREATE TABLE Arquivo(
    id int IDENTITY(1,1) NOT NULL,
    Idarquivo [uniqueidentifier] unique ROWGUIDCOL  NOT NULL,
    Nome [nvarchar](max) NULL,
    Blob [varbinary](max) FILESTREAM  NULL,
 CONSTRAINT [PK_Arquivo] PRIMARY KEY CLUSTERED 
(
    id ASC
))
 
GO
 
ALTER TABLE Arquivo ADD  CONSTRAINT [DF_Arquivo_Idarquivo]  DEFAULT (newid()) FOR [Idarquivo]
GO

select * from __Migrationhistory

delete from __Migrationhistory where rowid = MigrationId = '201606232011535_vs12'