SELECT al.Title, ar.Name, al.Genre, al.Released, al.CoverUrl
FROM ARTISTS ar
INNER JOIN ALBUMS al ON al.ArtistId = ar.Id
WHERE ar.Name = 'The Beatles'
ORDER BY al.Title ASC;
