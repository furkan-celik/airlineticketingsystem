-- phpMyAdmin SQL Dump
-- version 4.9.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 07, 2020 at 07:43 PM
-- Server version: 10.4.8-MariaDB
-- PHP Version: 7.3.11

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ticketingappdb`
--

-- --------------------------------------------------------

--
-- Table structure for table `addresses`
--

CREATE TABLE `addresses` (
  `Id` int(11) NOT NULL,
  `Name` longtext NOT NULL,
  `AddressLine` longtext NOT NULL,
  `OwnerId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `airplanes`
--

CREATE TABLE `airplanes` (
  `Id` varchar(95) NOT NULL,
  `BusinessRowNo` int(11) NOT NULL,
  `BusinessColumnNo` int(11) NOT NULL,
  `EconomyRowNo` int(11) NOT NULL,
  `EconomyColumnNo` int(11) NOT NULL,
  `SuperCheapRowNo` int(11) NOT NULL,
  `SuperCheapColumnNo` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `airports`
--

CREATE TABLE `airports` (
  `Id` int(11) NOT NULL,
  `AirportName` longtext NOT NULL,
  `CityId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetroleclaims`
--

CREATE TABLE `aspnetroleclaims` (
  `Id` int(11) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetroles`
--

CREATE TABLE `aspnetroles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `aspnetroles`
--

INSERT INTO `aspnetroles` (`Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`) VALUES
('5a9707a0-de5b-4621-a440-4944b8fd7446', 'User', 'USER', '352022b3-73d0-41f3-b1aa-bbc023da3c07'),
('806108b1-cb17-4a27-8f97-803288876664', 'WebAdmin', 'WEBADMIN', 'e197f6b6-f733-4c4b-921c-b2a7f853a431'),
('949851f5-272d-4356-93e9-d43abad56dba', 'UserAdmin', 'USERADMIN', '80aad53c-c32c-47ea-8465-8040f1b61d6d'),
('f17a2f23-27c2-4a28-9fa9-bda53d6e21cb', 'CompAdmin', 'COMPADMIN', 'f100835a-8520-4f55-b62e-191239aef74b');

-- --------------------------------------------------------

--
-- Table structure for table `aspnetuserclaims`
--

CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetuserlogins`
--

CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(128) NOT NULL,
  `ProviderDisplayName` longtext DEFAULT NULL,
  `UserId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetuserroles`
--

CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetusers`
--

CREATE TABLE `aspnetusers` (
  `Id` varchar(255) NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext DEFAULT NULL,
  `SecurityStamp` longtext DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  `PhoneNumber` longtext DEFAULT NULL,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `Name` longtext NOT NULL,
  `Surname` longtext NOT NULL,
  `Birthday` datetime(6) DEFAULT NULL,
  `Gender` longtext NOT NULL,
  `TC` longtext DEFAULT NULL,
  `ManagingCompanyId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `aspnetusertokens`
--

CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(128) NOT NULL,
  `Name` varchar(128) NOT NULL,
  `Value` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `cities`
--

CREATE TABLE `cities` (
  `CityId` int(11) NOT NULL,
  `CityName` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `companies`
--

CREATE TABLE `companies` (
  `Id` int(11) NOT NULL,
  `Name` longtext NOT NULL,
  `LogoLocation` longtext DEFAULT NULL,
  `Description` longtext NOT NULL,
  `Type` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `creditcards`
--

CREATE TABLE `creditcards` (
  `CardNumber` bigint(20) NOT NULL,
  `OwnerId` varchar(95) NOT NULL,
  `Id` int(11) NOT NULL,
  `Month` int(11) NOT NULL DEFAULT 0,
  `Year` int(11) NOT NULL DEFAULT 0,
  `HashedCardNumber` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `flights`
--

CREATE TABLE `flights` (
  `Id` int(11) NOT NULL,
  `CompanyId` int(11) NOT NULL,
  `Name` longtext NOT NULL,
  `RefundTime` time(6) NOT NULL,
  `ResCancelTime` time(6) NOT NULL,
  `RefundPortion` float NOT NULL,
  `Date` datetime(6) NOT NULL,
  `RouteId` int(11) NOT NULL,
  `FlightNo` longtext DEFAULT NULL,
  `AirplaneId` varchar(95) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `offerflights`
--

CREATE TABLE `offerflights` (
  `OfferId` int(11) NOT NULL,
  `FlightId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `offers`
--

CREATE TABLE `offers` (
  `Id` int(11) NOT NULL,
  `Name` longtext NOT NULL,
  `Description` longtext NOT NULL,
  `Price` float NOT NULL,
  `Type` int(11) NOT NULL DEFAULT 0,
  `ChildPrice` float NOT NULL DEFAULT 0,
  `CompanyId` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `offertickets`
--

CREATE TABLE `offertickets` (
  `TicketId` int(11) NOT NULL,
  `OfferId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `offertypes`
--

CREATE TABLE `offertypes` (
  `Id` int(11) NOT NULL,
  `Name` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `offertypes`
--

INSERT INTO `offertypes` (`Id`, `Name`) VALUES
(1, 'Economy'),
(2, 'Business'),
(3, 'Super Cheap'),
(4, 'Others');

-- --------------------------------------------------------

--
-- Table structure for table `promotions`
--

CREATE TABLE `promotions` (
  `Id` varchar(95) NOT NULL,
  `Discount` float NOT NULL,
  `CompanyId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `purchases`
--

CREATE TABLE `purchases` (
  `Id` int(11) NOT NULL,
  `ProcessTime` datetime(6) NOT NULL,
  `OwnerId` varchar(95) NOT NULL,
  `IsProcessed` tinyint(1) NOT NULL,
  `Price` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `reservationoffers`
--

CREATE TABLE `reservationoffers` (
  `ReservationId` int(11) NOT NULL,
  `OfferId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `reservations`
--

CREATE TABLE `reservations` (
  `Id` int(11) NOT NULL,
  `OwnerId` varchar(255) NOT NULL,
  `FlightId` int(11) NOT NULL,
  `processTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `numOfAdult` int(11) NOT NULL DEFAULT 0,
  `numOfChild` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `routes`
--

CREATE TABLE `routes` (
  `RouteId` int(11) NOT NULL,
  `ETA` time(6) NOT NULL,
  `ArrivalId` int(11) NOT NULL DEFAULT 0,
  `DepartureId` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `seats`
--

CREATE TABLE `seats` (
  `Id` int(11) NOT NULL,
  `Row` longtext NOT NULL,
  `Col` int(11) NOT NULL,
  `Availability` tinyint(1) NOT NULL,
  `TypeId` int(11) NOT NULL,
  `ReservationId` int(11) DEFAULT NULL,
  `TicketId` int(11) DEFAULT NULL,
  `FlightId` int(11) NOT NULL,
  `IsConfirmed` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `tickets`
--

CREATE TABLE `tickets` (
  `Id` int(11) NOT NULL,
  `ProcessTime` datetime(6) NOT NULL,
  `OwnerId` varchar(255) NOT NULL,
  `EventId` int(11) NOT NULL DEFAULT 0,
  `isChild` tinyint(1) NOT NULL DEFAULT 0,
  `PurchaseId` int(11) DEFAULT NULL,
  `CheckIn` tinyint(1) NOT NULL DEFAULT 0,
  `Name` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(95) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20200327210830_InitialTables', '3.1.2'),
('20200328153055_SlightChange', '3.1.2'),
('20200328161353_GenderToEnum', '3.1.2'),
('20200409122249_CompUserRelation', '3.1.2'),
('20200409140948_SmallFix', '3.1.2'),
('20200409141044_SmallFix2', '3.1.2'),
('20200415181007_foreignKeyFix', '3.1.2'),
('20200415181406_foreignKeyFix2', '3.1.2'),
('20200415181839_foreignKeyFix2', '3.1.2'),
('20200416150543_CityAndTimeSpan', '3.1.2'),
('20200419155050_TicketSeatFK', '3.1.3'),
('20200419155452_TicketSeatFKFix', '3.1.3'),
('20200421115711_nullablefix', '3.1.3'),
('20200422202102_Departure', '3.1.3'),
('20200426192501_FlightToRoute', '3.1.3'),
('20200426202008_IntRouteId', '3.1.3'),
('20200426203709_EventToFlight', '3.1.3'),
('20200428154905_Airport', '3.1.3'),
('20200428162740_CitiesFix', '3.1.3'),
('20200428164017_AirportsFix', '3.1.3'),
('20200430063310_AddedOfferType', '3.1.3'),
('20200507100733_ChildPriceToOffer', '3.1.3'),
('20200507130155_IsChildInTicket', '3.1.3'),
('20200507143312_SmallFixDK', '3.1.3'),
('20200509154029_ChildToReservation', '3.1.3'),
('20200510191008_PurchaseTable', '3.1.3'),
('20200513133855_creditcardfix', '3.1.3'),
('20200513143448_creditcardfix2', '3.1.3'),
('20200513171747_hashedcardnumber', '3.1.3'),
('20200515144559_OfferType', '3.1.3'),
('20200515145758_FixOnTypes', '3.1.3'),
('20200516123116_OfferTypeToSeat', '3.1.3'),
('20200516132408_FKFix', '3.1.3'),
('20200523164619_airplane_v_promotion', '3.1.3'),
('20200525100508_OffersBelongToCompany', '3.1.3'),
('20200525114523_OffersFlightsFix', '3.1.3'),
('20200526113146_checkinticket', '3.1.3'),
('20200529083634_ticketname', '3.1.3'),
('20200601131628_AirplaneFlight', '3.1.3'),
('20200601141938_ETATime', '3.1.3'),
('20200602122129_ResTime', '3.1.3'),
('20200602131825_PurchaseIdToTicket', '3.1.3'),
('20200602210049_ReservationChildChange', '3.1.3');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `addresses`
--
ALTER TABLE `addresses`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Addresses_OwnerId` (`OwnerId`);

--
-- Indexes for table `airplanes`
--
ALTER TABLE `airplanes`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `airports`
--
ALTER TABLE `airports`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Airports_CityId` (`CityId`);

--
-- Indexes for table `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`);

--
-- Indexes for table `aspnetroles`
--
ALTER TABLE `aspnetroles`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `RoleNameIndex` (`NormalizedName`);

--
-- Indexes for table `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AspNetUserClaims_UserId` (`UserId`);

--
-- Indexes for table `aspnetuserlogins`
--
ALTER TABLE `aspnetuserlogins`
  ADD PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  ADD KEY `IX_AspNetUserLogins_UserId` (`UserId`);

--
-- Indexes for table `aspnetuserroles`
--
ALTER TABLE `aspnetuserroles`
  ADD PRIMARY KEY (`UserId`,`RoleId`),
  ADD KEY `IX_AspNetUserRoles_RoleId` (`RoleId`);

--
-- Indexes for table `aspnetusers`
--
ALTER TABLE `aspnetusers`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  ADD KEY `EmailIndex` (`NormalizedEmail`),
  ADD KEY `IX_AspNetUsers_ManagingCompanyId` (`ManagingCompanyId`);

--
-- Indexes for table `aspnetusertokens`
--
ALTER TABLE `aspnetusertokens`
  ADD PRIMARY KEY (`UserId`,`LoginProvider`,`Name`);

--
-- Indexes for table `cities`
--
ALTER TABLE `cities`
  ADD PRIMARY KEY (`CityId`);

--
-- Indexes for table `companies`
--
ALTER TABLE `companies`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `creditcards`
--
ALTER TABLE `creditcards`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_CreditCards_OwnerId` (`OwnerId`);

--
-- Indexes for table `flights`
--
ALTER TABLE `flights`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Flights_RouteId` (`RouteId`),
  ADD KEY `IX_Flights_CompanyId` (`CompanyId`),
  ADD KEY `IX_Flights_AirplaneId` (`AirplaneId`);

--
-- Indexes for table `offerflights`
--
ALTER TABLE `offerflights`
  ADD PRIMARY KEY (`OfferId`,`FlightId`),
  ADD KEY `IX_OfferFlights_FlightId` (`FlightId`);

--
-- Indexes for table `offers`
--
ALTER TABLE `offers`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Offers_Type` (`Type`),
  ADD KEY `IX_Offers_CompanyId` (`CompanyId`);

--
-- Indexes for table `offertickets`
--
ALTER TABLE `offertickets`
  ADD PRIMARY KEY (`OfferId`,`TicketId`),
  ADD KEY `IX_OfferTickets_TicketId` (`TicketId`);

--
-- Indexes for table `offertypes`
--
ALTER TABLE `offertypes`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `promotions`
--
ALTER TABLE `promotions`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Promotions_CompanyId` (`CompanyId`);

--
-- Indexes for table `purchases`
--
ALTER TABLE `purchases`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Purchases_OwnerId` (`OwnerId`);

--
-- Indexes for table `reservationoffers`
--
ALTER TABLE `reservationoffers`
  ADD PRIMARY KEY (`OfferId`,`ReservationId`),
  ADD KEY `IX_ReservationOffers_ReservationId` (`ReservationId`);

--
-- Indexes for table `reservations`
--
ALTER TABLE `reservations`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Reservations_OwnerId` (`OwnerId`),
  ADD KEY `IX_Reservations_FlightId` (`FlightId`);

--
-- Indexes for table `routes`
--
ALTER TABLE `routes`
  ADD PRIMARY KEY (`RouteId`),
  ADD KEY `IX_Routes_ArrivalId` (`ArrivalId`),
  ADD KEY `IX_Routes_DepartureId` (`DepartureId`);

--
-- Indexes for table `seats`
--
ALTER TABLE `seats`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Seats_TypeId` (`TypeId`),
  ADD KEY `IX_Seats_ReservationId` (`ReservationId`),
  ADD KEY `IX_Seats_TicketId` (`TicketId`),
  ADD KEY `IX_Seats_FlightId` (`FlightId`);

--
-- Indexes for table `tickets`
--
ALTER TABLE `tickets`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Tickets_OwnerId` (`OwnerId`),
  ADD KEY `IX_Tickets_EventId` (`EventId`),
  ADD KEY `IX_Tickets_PurchaseId` (`PurchaseId`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `addresses`
--
ALTER TABLE `addresses`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `airports`
--
ALTER TABLE `airports`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `cities`
--
ALTER TABLE `cities`
  MODIFY `CityId` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `companies`
--
ALTER TABLE `companies`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `creditcards`
--
ALTER TABLE `creditcards`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `flights`
--
ALTER TABLE `flights`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `offers`
--
ALTER TABLE `offers`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `offertypes`
--
ALTER TABLE `offertypes`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `purchases`
--
ALTER TABLE `purchases`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `reservations`
--
ALTER TABLE `reservations`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `routes`
--
ALTER TABLE `routes`
  MODIFY `RouteId` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `seats`
--
ALTER TABLE `seats`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tickets`
--
ALTER TABLE `tickets`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `addresses`
--
ALTER TABLE `addresses`
  ADD CONSTRAINT `FK_Addresses_AspNetUsers_OwnerId` FOREIGN KEY (`OwnerId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `airports`
--
ALTER TABLE `airports`
  ADD CONSTRAINT `FK_Airports_Cities_CityId` FOREIGN KEY (`CityId`) REFERENCES `cities` (`CityId`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  ADD CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  ADD CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `aspnetuserlogins`
--
ALTER TABLE `aspnetuserlogins`
  ADD CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `aspnetuserroles`
--
ALTER TABLE `aspnetuserroles`
  ADD CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `aspnetusers`
--
ALTER TABLE `aspnetusers`
  ADD CONSTRAINT `FK_AspNetUsers_Companies_ManagingCompanyId` FOREIGN KEY (`ManagingCompanyId`) REFERENCES `companies` (`Id`);

--
-- Constraints for table `aspnetusertokens`
--
ALTER TABLE `aspnetusertokens`
  ADD CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `creditcards`
--
ALTER TABLE `creditcards`
  ADD CONSTRAINT `FK_CreditCards_AspNetUsers_OwnerId` FOREIGN KEY (`OwnerId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `flights`
--
ALTER TABLE `flights`
  ADD CONSTRAINT `FK_Flights_Airplanes_AirplaneId` FOREIGN KEY (`AirplaneId`) REFERENCES `airplanes` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Flights_Companies_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `companies` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_Flights_Routes_RouteId` FOREIGN KEY (`RouteId`) REFERENCES `routes` (`RouteId`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `offerflights`
--
ALTER TABLE `offerflights`
  ADD CONSTRAINT `FK_OfferFlights_Flights_FlightId` FOREIGN KEY (`FlightId`) REFERENCES `flights` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_OfferFlights_Offers_OfferId` FOREIGN KEY (`OfferId`) REFERENCES `offers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `offers`
--
ALTER TABLE `offers`
  ADD CONSTRAINT `FK_Offers_Companies_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `companies` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Offers_OfferTypes_Type` FOREIGN KEY (`Type`) REFERENCES `offertypes` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `offertickets`
--
ALTER TABLE `offertickets`
  ADD CONSTRAINT `FK_OfferTickets_Offers_OfferId` FOREIGN KEY (`OfferId`) REFERENCES `offers` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_OfferTickets_Tickets_TicketId` FOREIGN KEY (`TicketId`) REFERENCES `tickets` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `promotions`
--
ALTER TABLE `promotions`
  ADD CONSTRAINT `FK_Promotions_Companies_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `companies` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `purchases`
--
ALTER TABLE `purchases`
  ADD CONSTRAINT `FK_Purchases_AspNetUsers_OwnerId` FOREIGN KEY (`OwnerId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `reservationoffers`
--
ALTER TABLE `reservationoffers`
  ADD CONSTRAINT `FK_ReservationOffers_Offers_OfferId` FOREIGN KEY (`OfferId`) REFERENCES `offers` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_ReservationOffers_Reservations_ReservationId` FOREIGN KEY (`ReservationId`) REFERENCES `reservations` (`Id`) ON DELETE CASCADE;

--
-- Constraints for table `reservations`
--
ALTER TABLE `reservations`
  ADD CONSTRAINT `FK_Reservations_AspNetUsers_OwnerId` FOREIGN KEY (`OwnerId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Reservations_Flights_FlightId` FOREIGN KEY (`FlightId`) REFERENCES `flights` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `routes`
--
ALTER TABLE `routes`
  ADD CONSTRAINT `FK_Routes_Airports_ArrivalId` FOREIGN KEY (`ArrivalId`) REFERENCES `airports` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_Routes_Airports_DepartureId` FOREIGN KEY (`DepartureId`) REFERENCES `airports` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `seats`
--
ALTER TABLE `seats`
  ADD CONSTRAINT `FK_Seats_Flights_FlightId` FOREIGN KEY (`FlightId`) REFERENCES `flights` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_Seats_OfferTypes_TypeId` FOREIGN KEY (`TypeId`) REFERENCES `offertypes` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Seats_Reservations_ReservationId` FOREIGN KEY (`ReservationId`) REFERENCES `reservations` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_Seats_Tickets_TicketId` FOREIGN KEY (`TicketId`) REFERENCES `tickets` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Constraints for table `tickets`
--
ALTER TABLE `tickets`
  ADD CONSTRAINT `FK_Tickets_AspNetUsers_OwnerId` FOREIGN KEY (`OwnerId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Tickets_Flights_EventId` FOREIGN KEY (`EventId`) REFERENCES `flights` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_Tickets_Purchases_PurchaseId` FOREIGN KEY (`PurchaseId`) REFERENCES `purchases` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
