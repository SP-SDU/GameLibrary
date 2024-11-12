-- Delete migration history
DELETE FROM __EFMigrationsHistory;

-- Drop existing tables if they exist
DROP TABLE IF EXISTS UserFavorites;
DROP TABLE IF EXISTS Reviews;
DROP TABLE IF EXISTS Games;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS Users;
