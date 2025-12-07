-- Main catalogue/wish list album counts
SELECT  COUNT( 1 )
FROM    ALBUMS
WHERE   IsWishListItem IS NULL OR IsWishListItem = 0;

SELECT  COUNT( 1 )
FROM    ALBUMS
WHERE   IsWishListItem = 1;

-- Main catalogue/wish list artist counts
SELECT  COUNT( DISTINCT ArtistId)
FROM    ALBUMS
WHERE   IsWishListItem IS NULL OR IsWishListItem = 0;

SELECT  COUNT( DISTINCT ArtistId)
FROM    ALBUMS
WHERE   IsWishListItem = 1;
