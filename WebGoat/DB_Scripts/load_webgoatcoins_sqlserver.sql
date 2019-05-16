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

/* Loads the Classic Models tables using the MSSQL BULK INSERT command */

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

BULK INSERT Customers FROM '{dataFilesPath}\customers_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')


BULK INSERT CustomerLogin FROM '{dataFilesPath}\customerlogin_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')


BULK INSERT SecurityQuestions FROM '{dataFilesPath}\securityquestions_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')


BULK INSERT Employees FROM '{dataFilesPath}\employees_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')


BULK INSERT Offices FROM '{dataFilesPath}\offices_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')


BULK INSERT OrderDetails FROM '{dataFilesPath}\orderdetails_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')


BULK INSERT Orders FROM '{dataFilesPath}\orders_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')


BULK INSERT Payments FROM '{dataFilesPath}\payments_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')


BULK INSERT Categories FROM '{dataFilesPath}\categories_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')


BULK INSERT Products FROM '{dataFilesPath}\products_sqlserver.txt'
WITH (FIELDTERMINATOR = '|', ROWTERMINATOR = '\n')


