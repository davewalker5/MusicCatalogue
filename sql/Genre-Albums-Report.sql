SELECT ar.Name, al.Title, g.Name AS 'Genre', al.Purchased, al.Price, r.Name 
FROM ALBUMS al
INNER JOIN GENRES g ON g.Id = al.GenreId
INNER JOIN ARTISTS ar ON ar.Id = al.ArtistId
INNER JOIN RETAILERS r ON r.Id = al.RetailerId
WHERE g.Name = 'Jazz'
AND IFNULL( al.IsWishListItem, 0 ) = 0
ORDER BY ar.Name, al.Title;
