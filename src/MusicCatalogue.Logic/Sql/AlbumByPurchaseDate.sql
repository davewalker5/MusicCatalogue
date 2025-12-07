SELECT			RANK() OVER ( ORDER BY ar.Name, al.Title ) AS 'Id',
				ar.Name AS 'Artist',
				al.Title,
				g.Name AS 'Genre', 
				IFNULL( al.Released, 0 ) AS 'Released',
				IFNULL( DATE(al.Purchased), DATE('1900-01-01')) AS 'Purchased',
				IFNULL( al.Price, 0.0 ) AS 'Price',
				IFNULL( r.Name, '') AS 'Retailer'
FROM            ALBUMS al
INNER JOIN      GENRES g ON g.Id = al.GenreId
INNER JOIN      ARTISTS ar ON ar.Id = al.ArtistId
LEFT OUTER JOIN	RETAILERS r ON r.Id = al.RetailerId
WHERE           CAST( STRFTIME( '%Y', al.Purchased ) AS INTEGER ) = $year
AND             CAST( STRFTIME( '%m', al.Purchased ) AS INTEGER ) = $month
AND             IFNULL( al.IsWishListItem, 0 ) = 0
ORDER BY		ar.Name ASC,
				al.Title ASC
