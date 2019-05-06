/******************************************************************************
 * Copyright (c) 2005 Actuate Corporation.
 * All rights reserved. This file and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http:/www.eclipse.org/legal/epl-v10.html
 *
 * Contributors:
 *  Actuate Corporation  - initial implementation
 *
 * Classic Models Inc. sample database developed as part of the
 * Eclipse BIRT Project. For more information, see http:/www.eclipse.org/birt
 *
 *******************************************************************************/

/* Loads the Classic Models tables using the MySQL LOAD command */

/* Preparing the load files for importing. Input file requirements:
     - Column order in the file must be the same as the columns in the table
     - Columns are Comma delimited
     - Text is quoted (")
     - NULL columns must be ,NULL,  ( ,, is not acceptable)
     - Dates must be in YYYY-MM-DDD format

   Input files expected in the datafiles direcory, parallel to this script.
*/


/* First make sure all the tables are empty */

USE [classicmodels];


GO

DELETE FROM Customers;
DELETE FROM CustomerLogin;
DELETE FROM SecurityQuestions;
DELETE FROM Employees;
DELETE FROM Offices;
DELETE FROM OrderDetails;
DELETE FROM Orders;
DELETE FROM Payments;
DELETE FROM Products;
DELETE FROM Categories;
DELETE FROM Comments;


/* Load records into the tables. There should be no warnings.*/

BULK INSERT Customers FROM 'D:\Dev\Security\Forks\WebGoat.NET\WebGoat\DB_Scripts\datafiles\customers_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')
GO

BULK INSERT CustomerLogin FROM 'D:\Dev\Security\Forks\WebGoat.NET\WebGoat\DB_Scripts\datafiles\customerlogin_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')
GO

BULK INSERT SecurityQuestions FROM 'D:\Dev\Security\Forks\WebGoat.NET\WebGoat\DB_Scripts\datafiles\securityquestions_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')
GO

BULK INSERT Employees FROM 'D:\Dev\Security\Forks\WebGoat.NET\WebGoat\DB_Scripts\datafiles\employees_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')
GO

BULK INSERT Offices FROM 'D:\Dev\Security\Forks\WebGoat.NET\WebGoat\DB_Scripts\datafiles\offices_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')
GO

BULK INSERT OrderDetails FROM 'D:\Dev\Security\Forks\WebGoat.NET\WebGoat\DB_Scripts\datafiles\orderdetails_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')
GO

BULK INSERT Orders FROM 'D:\Dev\Security\Forks\WebGoat.NET\WebGoat\DB_Scripts\datafiles\orders_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')
GO

BULK INSERT Payments FROM 'D:\Dev\Security\Forks\WebGoat.NET\WebGoat\DB_Scripts\datafiles\payments_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')
GO

BULK INSERT Categories FROM 'D:\Dev\Security\Forks\WebGoat.NET\WebGoat\DB_Scripts\datafiles\categories_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')
GO

BULK INSERT Products FROM 'D:\Dev\Security\Forks\WebGoat.NET\WebGoat\DB_Scripts\datafiles\products_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')
GO

