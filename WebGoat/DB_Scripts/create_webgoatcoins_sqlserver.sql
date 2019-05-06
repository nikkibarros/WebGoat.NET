/******************************************************************************
 * Copyright (c) 2005 Actuate Corporation.
 * All rights reserved. This file and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.eclipse.org/legal/epl-v10.html
 *
 * Contributors:
 *  Actuate Corporation  - initial implementation
 *
 * Classic Models Inc. sample database developed as part of the
 * Eclipse BIRT Project. For more information, see http:\\www.eclipse.org\birt
 *
 *******************************************************************************/
/*******************************************************************************
* Changes made Jan 2012 - Copyright 2012
* Updated BIRT to be the webgoat coins database
* Images copyright US Mint and the Perth Mint
* Contributers:
*	Jerry Hoff - Infrared Security, LLC
*
*******************************************************************************/



/* Recommended DATABASE name is classicmodels. */


USE [master];


GO

IF (DB_ID(N'classicmodels') IS NOT NULL) 
BEGIN
    ALTER DATABASE [classicmodels]
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [classicmodels];
END

GO
PRINT N'Creating classicmodels...'
GO
CREATE DATABASE [classicmodels]
GO
USE [classicmodels];


GO

/* DROP the existing tables. Comment this out if it is not needed. */

/* webgoat.net note: use name webgoat_coins" */


DROP TABLE IF EXISTS Customers;
DROP TABLE IF EXISTS CustomerLogin;
DROP TABLE IF EXISTS SecurityQuestions;
DROP TABLE IF EXISTS Employees;
DROP TABLE IF EXISTS Offices;
DROP TABLE IF EXISTS OrderDetails;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS Payments;
DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS Categories;
DROP TABLE IF EXISTS Comments;


/* Create the full set of Classic Models Tables */

CREATE TABLE Customers (
  CustomerNumber INT NOT NULL IDENTITY,
  CustomerName VARCHAR(50) NOT NULL,
  LogoFileName VARCHAR(100) NULL,
  ContactLastName VARCHAR(50) NOT NULL,
  ContactFirstName VARCHAR(50) NOT NULL,
  Phone VARCHAR(50) NOT NULL,
  AddressLine1 VARCHAR(50) NOT NULL,
  AddressLine2 VARCHAR(50) NULL,
  City VARCHAR(50) NOT NULL,
  State VARCHAR(50) NULL,
  PostalCode VARCHAR(15) NULL,
  Country VARCHAR(50) NOT NULL,
  SalesRepEmployeeNumber INT NULL,
  CreditLimit DECIMAL(10,2) NULL,
  CONSTRAINT [PK_Customers] PRIMARY KEY (CustomerNumber)
);

CREATE TABLE CustomerLogin (
	Email VARCHAR(100) NOT NULL,
	CustomerNumber INT NOT NULL,
	Password VARCHAR(40) NOT NULL,
	QuestionId SMALLINT NULL,
	Answer VARCHAR(50) NULL,
	CONSTRAINT [PK_CustomerLogin] PRIMARY KEY (Email)
);

CREATE TABLE SecurityQuestions (
	QuestionId SMALLINT NOT NULL IDENTITY,
	QuestionText VARCHAR(400) NOT NULL,
	CONSTRAINT [PK_SecurityQuestions] PRIMARY KEY (QuestionId)
);


CREATE TABLE Employees (
  EmployeeNumber INT NOT NULL IDENTITY,
  LastName VARCHAR(50) NOT NULL,
  FirstName VARCHAR(50) NOT NULL,
  Extension VARCHAR(10) NOT NULL,
  Email VARCHAR(100) NOT NULL,
  OfficeCode VARCHAR(10) NOT NULL,
  ReportsTo INT NULL,
  JobTitle VARCHAR(50) NOT NULL,
  CONSTRAINT [PK_Employees] PRIMARY KEY (EmployeeNumber)
);

CREATE TABLE Offices (
  OfficeCode VARCHAR(10) NOT NULL,
  City VARCHAR(50) NOT NULL,
  Phone VARCHAR(50) NOT NULL,
  AddressLine1 VARCHAR(50) NOT NULL,
  AddressLine2 VARCHAR(50) NULL,
  State VARCHAR(50) NULL,
  Country VARCHAR(50) NOT NULL,
  PostalCode VARCHAR(15) NOT NULL,
  Territory VARCHAR(10) NOT NULL,
  CONSTRAINT [PK_Offices] PRIMARY KEY (OfficeCode)
);

CREATE TABLE OrderDetails (
  OrderNumber INT NOT NULL,
  ProductCode VARCHAR(15) NOT NULL,
  QuantityOrdered INT NOT NULL,
  PriceEach DECIMAL(10,2) NOT NULL,
  OrderLineNumber SMALLINT NOT NULL,
  CONSTRAINT [PK_OrderDetails] PRIMARY KEY (OrderNumber, ProductCode)
);

CREATE TABLE Orders (
  OrderNumber INT NOT NULL IDENTITY,
  OrderDate DATETIME NOT NULL,
  RequiredDate DATETIME NOT NULL,
  ShippedDate DATETIME NULL,
  Status VARCHAR(15) NOT NULL,
  Comments TEXT NULL,
  CustomerNumber INT NOT NULL,
  CONSTRAINT [PK_Orders] PRIMARY KEY (OrderNumber)
);


CREATE TABLE Payments (
  CustomerNumber INT NOT NULL,  
  CardType VARCHAR(50) NOT NULL,
  CreditCardNumber VARCHAR(50) NOT NULL,
  VerificationCode SMALLINT NOT NULL,
  CardExpirationMonth VARCHAR(3) NOT NULL,
  CardExpirationYear VARCHAR(5) NOT NULL,  
  ConfirmationCode VARCHAR(50) NOT NULL,
  PaymentDate DATETIME NOT NULL,
  Amount DECIMAL(10,2) NOT NULL,
  CONSTRAINT [PK_Payments] PRIMARY KEY (CustomerNumber, ConfirmationCode)
);

CREATE TABLE Categories(
  CatNumber INT NOT NULL IDENTITY,
  CatName VARCHAR(50) NOT NULL,
  CatDesc TEXT NOT NULL,
  CONSTRAINT [PK_Categories] PRIMARY KEY (CatNumber)
);

CREATE TABLE Products (
  ProductCode VARCHAR(15) NOT NULL,
  ProductName VARCHAR(200) NOT NULL,
  CatNumber INT NOT NULL,
  ProductImage VARCHAR(100) NOT NULL,
  ProductVendor VARCHAR(50) NOT NULL,
  ProductDescription TEXT NOT NULL,
  QuantityInStock SMALLINT NOT NULL,
  BuyPrice DECIMAL(10,2) NOT NULL,
  MSRP DECIMAL(10,2) NOT NULL,
  CONSTRAINT [PK_Products] PRIMARY KEY (ProductCode)
);

CREATE TABLE Comments(
	CommentNumber INT NOT NULL IDENTITY,
	ProductCode VARCHAR(15) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	Comment TEXT NOT NULL,
	CONSTRAINT [PK_Comments] PRIMARY KEY (CommentNumber)
);
