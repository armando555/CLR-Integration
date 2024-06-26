# CLR-Integration
This repo is to make a CLR integration in SQL SERVER

This is the clr script remember that THIS IS ONLY FOR TESTS IN LOCAL ENVIRONMENT, in a PROD ENVIRONMENT we must registry the assemblies with cert or something like that

# NOTES:

Remember that you need to press rigth click in the project database and publish it in the database but you must generate the script because it doesn't work automatically. The project is configure in the properteis with the sql clr UNSAFE OPTION.

Also, in this case we enable the trustworthy that is the same accept whatever origin of assembly without any block generating a security issue
```sql
EXEC sp_configure 'clr enabled', 1;  
RECONFIGURE;  
GO
ALTER DATABASE pruebaclr SET TRUSTWORTHY ON;

CREATE ASSEMBLY [Opa.Databased.Integration.Td]
    AUTHORIZATION [dbo]
    FROM ... BYTESCHAR LONG
    WITH PERMISSION_SET = UNSAFE;


GO
ALTER ASSEMBLY [Opa.Databased.Integration.Td]
    DROP FILE ALL
    ADD FILE FROM ... BYTESCHAR LONG


GO
PRINT N'Creating Procedure [dbo].[CallApi]...';


GO
CREATE PROCEDURE [dbo].[CallApi]
AS EXTERNAL NAME [Opa.Databased.Integration.Td].[StoredProcedures].[CallApi]


GO
PRINT N'Creating Procedure [dbo].[InserDb]...';


GO
CREATE PROCEDURE [dbo].[InserDb]
AS EXTERNAL NAME [Opa.Databased.Integration.Td].[StoredProcedures].[InserDb]


GO


exec CallApi

```