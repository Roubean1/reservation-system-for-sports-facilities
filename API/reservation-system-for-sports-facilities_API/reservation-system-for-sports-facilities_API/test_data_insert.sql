PRAGMA foreign_keys = ON;


-- INSERT tabulka Users
INSERT INTO Users (Id, Email, PasswordHash, FullName, Membership, CreatedAt) VALUES
(1, 'jan.novak@email.cz', 'hash_jan_123', 'Jan Novák', 'BASIC', '2026-01-05 09:15:00'),
(2, 'petra.svobodova@email.cz', 'hash_petra_123', 'Petra Svobodová', 'PREMIUM', '2026-01-07 14:20:00'),
(3, 'martin.dvorak@email.cz', 'hash_martin_123', 'Martin Dvořák', 'STANDART', '2026-01-10 11:00:00'),
(4, 'eva.prochazkova@email.cz', 'hash_eva_123', 'Eva Procházková', 'BASIC', '2026-01-12 17:45:00'),
(5, 'lukas.cerny@email.cz', 'hash_lukas_123', 'Lukáš Černý', 'PREMIUM', '2026-01-15 08:30:00'),
(6, 'tereza.kralova@email.cz', 'hash_tereza_123', 'Tereza Králová', 'STANDART', '2026-01-18 19:10:00');

-- INSERT tabulka Sports
INSERT INTO Sports (Id, Name) VALUES
(1, 'Tenis'),
(2, 'Badminton'),
(3, 'Squash'),
(4, 'Basketbal'),
(5, 'Volejbal'),
(6, 'Futsal'),
(7, 'Stolní tenis');

-- INSERT tabulka Venues
INSERT INTO Venues (Id, Name, Address, City) VALUES
(1, 'Sport Arena Brno', 'Kounicova 12', 'Brno'),
(2, 'Multisport Centrum Praha', 'Radlická 45', 'Praha'),
(3, 'Hala Sever Ostrava', 'Nádražní 88', 'Ostrava');

-- INSERT tabulka Facilities
INSERT INTO Facilities (Id, VenueId, Name) VALUES
(1, 1, 'Tenisový kurt A'),
(2, 1, 'Multifunkční hala 1'),
(3, 1, 'Squashový kurt 1'),
(4, 2, 'Badmintonová hala A'),
(5, 2, 'Multifunkční hala B'),
(6, 2, 'Stůl na stolní tenis 1'),
(7, 3, 'Hlavní hala'),
(8, 3, 'Tenisový kurt B');

-- INSERT vazební tabulka facility_sports (M:N)
INSERT INTO facility_sports (facility_id, sport_id) VALUES
(1, 1), (2, 4), (2, 5), (2, 6), (3, 3), (4, 2), (5, 4), (5, 5), (5, 6), (6, 7), (7, 2), (7, 4), (7, 5), (7, 6), (8, 1);

-- INSERT tabulka PriceLists
INSERT INTO PriceLists (Id, FacilityId, SportId, Membership, PricePerHour, Currency) VALUES
(1, 1, 1, 'BASIC', 320, 'CZK'), (2, 1, 1, 'STANDART', 290, 'CZK'), (3, 1, 1, 'PREMIUM', 250, 'CZK'),
(4, 2, 4, 'BASIC', 500, 'CZK'), (5, 2, 4, 'STANDART', 460, 'CZK'), (6, 2, 4, 'PREMIUM', 420, 'CZK'),
(7, 2, 5, 'BASIC', 480, 'CZK'), (8, 2, 5, 'STANDART', 440, 'CZK'), (9, 2, 5, 'PREMIUM', 400, 'CZK'),
(10, 2, 6, 'BASIC', 520, 'CZK'), (11, 2, 6, 'STANDART', 480, 'CZK'), (12, 2, 6, 'PREMIUM', 430, 'CZK'),
(13, 3, 3, 'BASIC', 260, 'CZK'), (14, 3, 3, 'STANDART', 230, 'CZK'), (15, 3, 3, 'PREMIUM', 210, 'CZK'),
(16, 4, 2, 'BASIC', 340, 'CZK'), (17, 4, 2, 'STANDART', 300, 'CZK'), (18, 4, 2, 'PREMIUM', 270, 'CZK'),
(19, 5, 4, 'BASIC', 550, 'CZK'), (20, 5, 4, 'STANDART', 500, 'CZK'), (21, 5, 4, 'PREMIUM', 460, 'CZK'),
(22, 5, 5, 'BASIC', 530, 'CZK'), (23, 5, 5, 'STANDART', 490, 'CZK'), (24, 5, 5, 'PREMIUM', 450, 'CZK'),
(25, 5, 6, 'BASIC', 560, 'CZK'), (26, 5, 6, 'STANDART', 520, 'CZK'), (27, 5, 6, 'PREMIUM', 470, 'CZK'),
(28, 6, 7, 'BASIC', 180, 'CZK'), (29, 6, 7, 'STANDART', 160, 'CZK'), (30, 6, 7, 'PREMIUM', 140, 'CZK'),
(31, 7, 2, 'BASIC', 420, 'CZK'), (32, 7, 2, 'STANDART', 390, 'CZK'), (33, 7, 2, 'PREMIUM', 350, 'CZK'),
(34, 7, 4, 'BASIC', 620, 'CZK'), (35, 7, 4, 'STANDART', 580, 'CZK'), (36, 7, 4, 'PREMIUM', 530, 'CZK'),
(37, 7, 5, 'BASIC', 600, 'CZK'), (38, 7, 5, 'STANDART', 560, 'CZK'), (39, 7, 5, 'PREMIUM', 510, 'CZK'),
(40, 7, 6, 'BASIC', 640, 'CZK'), (41, 7, 6, 'STANDART', 600, 'CZK'), (42, 7, 6, 'PREMIUM', 550, 'CZK'),
(43, 8, 1, 'BASIC', 300, 'CZK'), (44, 8, 1, 'STANDART', 270, 'CZK'), (45, 8, 1, 'PREMIUM', 240, 'CZK');

-- INSERT tabulka Equipments
INSERT INTO Equipments (Id, VenueId, Name, Quantity, PricePerHour) VALUES
(1, 1, 'Tenisová raketa', 12, 70),
(2, 1, 'Sada tenisových míčků', 30, 20),
(3, 1, 'Squashová raketa', 8, 60),
(4, 2, 'Badmintonová raketa', 20, 50),
(5, 2, 'Košíčky na badminton', 40, 15),
(6, 2, 'Míč na volejbal', 10, 25),
(7, 2, 'Míč na basketbal', 10, 25),
(8, 3, 'Rozlišovací dresy', 25, 30),
(9, 3, 'Míč na futsal', 8, 30),
(10, 3, 'Pálka na stolní tenis', 16, 35);

-- INSERT tabulka Reservations
INSERT INTO Reservations (Id, UserId, FacilityId, SportId, StartAt, EndAt, Status, TotalPrice) VALUES
(1, 1, 1, 1, '2026-02-01 10:00:00', '2026-02-01 11:00:00', 'ACTIVE', 320),
(2, 2, 1, 1, '2026-02-01 11:00:00', '2026-02-01 13:00:00', 'ACTIVE', 500),
(3, 3, 2, 4, '2026-02-02 18:00:00', '2026-02-02 19:30:00', 'ACTIVE', 750),
(4, 4, 2, 5, '2026-02-03 17:00:00', '2026-02-03 18:30:00', 'CANCELLED', 720),
(5, 5, 3, 3, '2026-02-03 19:00:00', '2026-02-03 20:00:00', 'ACTIVE', 210),
(6, 1, 4, 2, '2026-02-04 16:00:00', '2026-02-04 17:00:00', 'ACTIVE', 340),
(7, 2, 4, 2, '2026-02-04 17:00:00', '2026-02-04 19:00:00', 'ACTIVE', 540),
(8, 6, 5, 6, '2026-02-05 20:00:00', '2026-02-05 21:30:00', 'ACTIVE', 780),
(9, 3, 6, 7, '2026-02-06 15:00:00', '2026-02-06 16:00:00', 'ACTIVE', 160),
(10, 5, 7, 4, '2026-02-06 18:00:00', '2026-02-06 20:00:00', 'ACTIVE', 1060),
(11, 4, 7, 5, '2026-02-07 10:00:00', '2026-02-07 12:00:00', 'ACTIVE', 1200),
(12, 2, 8, 1, '2026-02-07 13:00:00', '2026-02-07 14:00:00', 'ACTIVE', 240),
(13, 1, 2, 6, '2026-02-08 19:00:00', '2026-02-08 20:30:00', 'ACTIVE', 780),
(14, 6, 7, 2, '2026-02-08 17:00:00', '2026-02-08 18:30:00', 'ACTIVE', 630),
(15, 3, 3, 3, '2026-02-09 18:00:00', '2026-02-09 19:00:00', 'ACTIVE', 230),
(16, 2, 5, 4, '2026-02-09 20:00:00', '2026-02-09 21:30:00', 'ACTIVE', 690),
(17, 5, 5, 5, '2026-02-10 18:30:00', '2026-02-10 20:00:00', 'ACTIVE', 675),
(18, 4, 4, 2, '2026-02-10 16:00:00', '2026-02-10 17:00:00', 'ACTIVE', 340),
(19, 6, 6, 7, '2026-02-11 14:00:00', '2026-02-11 15:00:00', 'ACTIVE', 160),
(20, 1, 8, 1, '2026-02-11 09:00:00', '2026-02-11 10:00:00', 'CANCELLED', 300);

-- INSERT tabulka EquipmentRentals
INSERT INTO EquipmentRentals (Id, UserId, EquipmentId, ReservationId, Qty, StartAt, EndAt) VALUES
(1, 1, 1, 1, 2, '2026-02-01 10:00:00', '2026-02-01 11:00:00'),
(2, 2, 2, 2, 1, '2026-02-01 11:00:00', '2026-02-01 13:00:00'),
(3, 5, 3, 5, 2, '2026-02-03 19:00:00', '2026-02-03 20:00:00'),
(4, 1, 4, 6, 2, '2026-02-04 16:00:00', '2026-02-04 17:00:00'),
(5, 2, 5, 7, 1, '2026-02-04 17:00:00', '2026-02-04 19:00:00'),
(6, 6, 7, 8, 1, '2026-02-05 20:00:00', '2026-02-05 21:30:00'),
(7, 3, 10, 9, 2, '2026-02-06 15:00:00', '2026-02-06 16:00:00'),
(8, 5, 8, 10, 10, '2026-02-06 18:00:00', '2026-02-06 20:00:00'),
(9, 4, 6, 11, 1, '2026-02-07 10:00:00', '2026-02-07 12:00:00'),
(10, 2, 1, 12, 1, '2026-02-07 13:00:00', '2026-02-07 14:00:00'),
(11, 1, 9, 13, 1, '2026-02-08 19:00:00', '2026-02-08 20:30:00'),
(12, 6, 4, 14, 2, '2026-02-08 17:00:00', '2026-02-08 18:30:00');

-- INSERT tabulka SupportTickets
INSERT INTO SupportTickets (Id, UserId, Email, Subject, Message, Status, CreatedAt) VALUES
(1, 1, 'jan.novak@email.cz', 'Problém s rezervací', 'Po vytvoření rezervace mi nepřišel potvrzovací email.', 'OPEN', '2026-02-02 12:10:00'),
(2, 3, 'martin.dvorak@email.cz', 'Chybná cena pronájmu', 'U squashové rezervace se mi zdá špatně spočítaná cena.', 'RESOLVED', '2026-02-05 09:40:00'),
(3, NULL, 'neprihlaseny.navstevnik@email.cz', 'Dotaz na členství', 'Jaký je rozdíl mezi BASIC a PREMIUM členstvím?', 'OPEN', '2026-02-06 16:25:00'),
(4, 5, 'lukas.cerny@email.cz', 'Zrušení rezervace', 'Nelze mi zrušit rezervaci přes aplikaci.', 'RESOLVED', '2026-02-08 11:00:00');