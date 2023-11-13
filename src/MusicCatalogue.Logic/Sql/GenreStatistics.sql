WITH ALBUM_SUMMARY ( Id, ArtistId, Genre, Tracks, Price, IsWishListItem ) AS
(
    SELECT      a.Id,
                a.ArtistId,
                a.Genre,
                COUNT( t.Id ),
                IFNULL( a.Price, 0 ),
                IFNULL( a.IsWishListItem, 0) 
    FROM        ALBUMS a
    INNER JOIN  TRACKS t ON t.AlbumId = a.Id
    GROUP BY    a.Id, a.ArtistId, a.Genre, a.Price
)
SELECT      Genre,
            COUNT( DISTINCT ArtistId) AS "Artists",
            COUNT( DISTINCT Id ) AS "Albums",
            SUM( Tracks ) AS "Tracks",
            SUM( Price ) AS "Spend"
FROM        ALBUM_SUMMARY
WHERE       IsWishListItem = $wishlist
GROUP BY    Genre
ORDER BY    Genre ASC;
