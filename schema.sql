-- vytvoreni tabulek pro game tracker
-- tady jsou cizi klice a takovy veci

CREATE TABLE IF NOT EXISTS Platform (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS Game (
    Id SERIAL PRIMARY KEY,
    Title VARCHAR(200) NOT NULL,
    Developer VARCHAR(200),
    ReleaseYear INT,
    PlatformId INT,
    FOREIGN KEY (PlatformId) REFERENCES Platform(Id) ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS GameSession (
    Id SERIAL PRIMARY KEY,
    GameId INT NOT NULL,
    DatePlayed DATE NOT NULL,
    HoursPlayed DECIMAL(4,1) NOT NULL,
    FOREIGN KEY (GameId) REFERENCES Game(Id) ON DELETE CASCADE
);
