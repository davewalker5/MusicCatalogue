WITH ARTIST_SPEND ( Id, Spend ) AS
(
    SELECT      al.ArtistId, SUM( al.Price )
    FROM        ALBUMS al
    GROUP BY    al.ArtistId 
)
SELECT      a.Name,
            COUNT( DISTINCT al.Id ) AS "Albums",
            COUNT( DISTINCT asp.Id ) AS "Tracks",
            asp.Spend AS "Spend"
FROM        ARTIST_SPEND asp
INNER JOIN  ARTISTS a ON a.Id = asp.Id
INNER JOIN  ALBUMS al ON al.ArtistId = a.Id
WHERE       IFNULL( al.IsWishListItem, 0 ) = $wishlist
GROUP BY    a.Id, a.Name
ORDER BY    a.Name ASC
