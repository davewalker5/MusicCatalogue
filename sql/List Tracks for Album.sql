SELECT DISTINCT al.Title, ar.Name, t.Number, t.Title, strftime('%M:%S', (t.Duration / 1000), 'unixepoch') AS "Duration"
FROM ARTISTS ar
INNER JOIN ALBUMS al ON al.ArtistId = ar.Id
INNER JOIN TRACKS t ON t.AlbumId = al.Id
WHERE ar.Name = 'The Beatles'
AND al.Title = 'The Beatles'
ORDER BY t.Number ASC;
