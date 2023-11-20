WITH ARTIST_SPEND ( Id, IsWishListItem, Spend ) AS
(
    SELECT      al.ArtistId, IFNULL( al.IsWishListItem, 0 ), SUM( al.Price )
    FROM        ALBUMS al
    GROUP BY    al.ArtistId, IFNULL( al.IsWishListItem, 0 )
)
SELECT      RANK() OVER ( ORDER BY a.Name ASC ) AS "Id",
            a.Name,
            COUNT( DISTINCT al.Id ) AS "Albums",
            COUNT( DISTINCT t.Id ) AS "Tracks",
            asp.Spend AS "Spend"
FROM        ARTIST_SPEND asp
INNER JOIN  ARTISTS a ON a.Id = asp.Id
INNER JOIN  ALBUMS al ON al.ArtistId = a.Id
INNER JOIN  TRACKS t ON t.ALbumId = al.Id
WHERE       IFNULL( al.IsWishListItem, 0 ) = 0
AND         asp.IsWishListItem = 0
GROUP BY    a.Id, a.Name
ORDER BY    a.Name ASC
