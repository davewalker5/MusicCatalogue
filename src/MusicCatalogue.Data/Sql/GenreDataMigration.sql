-- Populate the GENRES table
WITH CURRENT_GENRES ( Id, Name ) AS
(
    SELECT DISTINCT RANK() OVER ( ORDER BY Genre ), Genre
    FROM ALBUMS
    GROUP BY Genre
)
INSERT INTO GENRES ( Id, Name )
SELECT Id, Name
FROM CURRENT_GENRES;

-- Set the Genre ID for each album
UPDATE ALBUMS
SET GenreId = ( SELECT Id FROM GENRES WHERE Name = ALBUMS.Genre );
