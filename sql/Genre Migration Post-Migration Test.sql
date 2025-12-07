-- "Before" Query
SELECT Title, Genre
FROM ALBUMS
ORDER BY Title ASC;

-- "After" Query
SELECT al.Title, g.Name AS "Genre"
FROM ALBUMS al
INNER JOIN GENRES g ON g.Id = al.GenreId
ORDER BY al.Title ASC;
