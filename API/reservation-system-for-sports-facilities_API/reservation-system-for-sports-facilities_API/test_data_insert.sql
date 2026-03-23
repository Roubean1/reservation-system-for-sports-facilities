PRAGMA foreign_keys = ON;

-- 1. INSERT tabulka Roles (Nové)
INSERT INTO Roles (Id, Name) VALUES
(1, 'Admin'),
(2, 'Employee'),
(3, 'Customer');

-- 2. INSERT tabulka Users (Přidáno RoleId)
-- Hesla jsou stále 'hash_...', v reálné aplikaci by zde byly BCrypt hashe
INSERT INTO Users (Id, RoleId, Email, PasswordHash, FullName, Membership, CreatedAt) VALUES
(1, 1, 'jan.novak@sportarena.cz', 'hash_jan_123', 'Jan Novák', 'PREMIUM', '2026-01-05 09:15:00'),
(2, 3, 'petra.svobodova@email.cz', 'hash_petra_123', 'Petra Svobodová', 'PREMIUM', '2026-01-07 14:20:00'),
(3, 3, 'martin.dvorak@email.cz', 'hash_martin_123', 'Martin Dvořák', 'STANDART', '2026-01-10 11:00:00'),
(4, 3, 'eva.prochazkova@email.cz', 'hash_eva_123', 'Eva Procházková', 'BASIC', '2026-01-12 17:45:00'),
(5, 3, 'lukas.cerny@email.cz', 'hash_lukas_123', 'Lukáš Černý', 'PREMIUM', '2026-01-15 08:30:00'),
(6, 3, 'tereza.kralova@email.cz', 'hash_tereza_123', 'Tereza Králová', 'STANDART', '2026-01-18 19:10:00');

-- 3. INSERT tabulka Sports
INSERT INTO Sports (Id, Name) VALUES
(1, 'Tenis'), (2, 'Badminton'), (3, 'Squash'), (4, 'Basketbal'), (5, 'Volejbal'), (6, 'Futsal'), (7, 'Stolní tenis');

-- 4. INSERT tabulka Venues
INSERT INTO Venues (Id, Name, Address, City) VALUES
(1, 'Sport Arena Brno', 'Kounicova 12', 'Brno'),
(2, 'Multisport Centrum Praha', 'Radlická 45', 'Praha'),
(3, 'Hala Sever Ostrava', 'Nádražní 88', 'Ostrava');

-- 5. INSERT tabulka Facilities
INSERT INTO Facilities (Id, VenueId, Name) VALUES
(1, 1, 'Tenisový kurt A'), (2, 1, 'Multifunkční hala 1'), (3, 1, 'Squashový kurt 1'),
(4, 2, 'Badmintonová hala A'), (5, 2, 'Multifunkční hala B'), (6, 2, 'Stůl na stolní tenis 1'),
(7, 3, 'Hlavní hala'), (8, 3, 'Tenisový kurt B');

-- 6. INSERT vazební tabulka facility_sports (M:N)
INSERT INTO facility_sports (facility_id, sport_id) VALUES
(1, 1), (2, 4), (2, 5), (2, 6), (3, 3), (4, 2), (5, 4), (5, 5), (5, 6), (6, 7), (7, 2), (7, 4), (7, 5), (7, 6), (8, 1);

-- 7. INSERT tabulka PriceLists
INSERT INTO PriceLists (Id, FacilityId, SportId, Membership, PricePerHour, Currency) VALUES
(1, 1, 1, 'BASIC', 320, 'CZK'), (2, 1, 1, 'STANDART', 290, 'CZK'), (3, 1, 1, 'PREMIUM', 250, 'CZK'),
(4, 2, 4, 'BASIC', 500, 'CZK'), (5, 2, 4, 'STANDART', 460, 'CZK'), (6, 2, 4, 'PREMIUM', 420, 'CZK'),
(13, 3, 3, 'BASIC', 260, 'CZK'), (16, 4, 2, 'BASIC', 340, 'CZK'), (28, 6, 7, 'BASIC', 180, 'CZK');

-- 8. INSERT tabulka Equipments
INSERT INTO Equipments (Id, VenueId, Name, Quantity, PricePerHour) VALUES
(1, 1, 'Tenisová raketa', 12, 70), (2, 1, 'Sada tenisových míčků', 30, 20), (4, 2, 'Badmintonová raketa', 20, 50);

-- 9. INSERT tabulka Reservations
INSERT INTO Reservations (Id, UserId, FacilityId, SportId, StartAt, EndAt, Status, TotalPrice) VALUES
(1, 1, 1, 1, '2026-02-01 10:00:00', '2026-02-01 11:00:00', 'ACTIVE', 320),
(2, 2, 1, 1, '2026-02-01 11:00:00', '2026-02-01 13:00:00', 'ACTIVE', 500);

-- 10. INSERT tabulka EquipmentRentals
INSERT INTO EquipmentRentals (Id, UserId, EquipmentId, ReservationId, Qty, StartAt, EndAt) VALUES
(1, 1, 1, 1, 2, '2026-02-01 10:00:00', '2026-02-01 11:00:00');

-- 11. INSERT tabulka SupportTickets (Přidáno Answer)
INSERT INTO SupportTickets (Id, UserId, Email, Subject, Message, Answer, Status, CreatedAt) VALUES
(1, 1, 'jan.novak@sportarena.cz', 'Problém s rezervací', 'Nepřišel mi email.', NULL, 'OPEN', '2026-02-02 12:10:00'),
(2, 3, 'martin.dvorak@email.cz', 'Chybná cena', 'Cena se mi nezdá.', 'Prověřili jsme to a cena odpovídá vašemu členství.', 'RESOLVED', '2026-02-05 09:40:00');