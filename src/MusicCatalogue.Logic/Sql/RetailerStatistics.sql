WITH RETAILER_SPEND ( Id, Name, IsWishListItem, Spend ) AS
(
    SELECT          r.Id,
                    r.Name,
                    IFNULL( al.IsWishListItem, 0),
                    SUM( IFNULL( al.Price, 0 ) )
    FROM            ALBUMS al
    LEFT OUTER JOIN RETAILERS r ON r.Id = al.RetailerId
    GROUP BY        r.Id, r.Name, IFNULL( al.IsWishListItem, 0)
)
SELECT      RANK() OVER ( ORDER BY rsp.Name ASC ) AS "Id",
            rsp.Name,
            COUNT( DISTINCT a.Id ) AS "Artists",
            COUNT( DISTINCT al.Id ) AS "Albums",
            COUNT( DISTINCT t.Id ) AS "Tracks",
            rsp.Spend
FROM        RETAILER_SPEND rsp
INNER JOIN  ALBUMS al ON al.RetailerId = rsp.Id
INNER JOIN  TRACKS t ON t.AlbumId = al.Id
INNER JOIN  ARTISTS a ON a.Id = al.ArtistId
WHERE       rsp.IsWishListItem = $wishlist
AND         IFNULL( al.IsWishListItem, 0 ) = $wishlist
GROUP BY    rsp.Name
