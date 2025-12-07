-- Create a GENRES table manually
CREATE TABLE GENRES (
    Id int NOT NULL PRIMARY KEY,
    Name Text NOT NULL );

-- Add the Genre ID column to the ALBUMS table
ALTER TABLE ALBUMS ADD GenreId int NULL;

-- Check the WITH construct does what's required in this instance
WITH CURRENT_GENRES ( Id, Name ) AS
(
    SELECT DISTINCT RANK() OVER ( ORDER BY Genre ), Genre
    FROM ALBUMS
    GROUP BY Genre
)
SELECT *
FROM CURRENT_GENRES;

-- Test the migration script to move genres to the new table
WITH CURRENT_GENRES ( Id, Name ) AS
(
    SELECT DISTINCT RANK() OVER ( ORDER BY Genre ), Genre
    FROM ALBUMS
)
INSERT INTO GENRES ( Id, Name )
SELECT Id, Name
FROM CURRENT_GENRES;

-- And to update the Genre ID against the album
UPDATE ALBUMS
SET GenreId = ( SELECT Id FROM GENRES WHERE Name = ALBUMS.Genre );

-- Cross-check - should produce 0 rows
SELECT al.Genre, g.Name
FROM ALBUMS al
INNER JOIN GENRES g ON g.Id = al.GenreId
WHERE al.Genre <> g.Name;

-- Clean up
ALTER TABLE ALBUMS DROP GenreId;
DROP TABLE GENRES;
