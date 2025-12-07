-- Record counts
SELECT COUNT( Id ) FROM ARTISTS;
SELECT COUNT( Id ) FROM ALBUMS;
SELECT COUNT( Id ) FROM GENRES;

-- Record count cross checks : Genre count may be 1 less than that from genres table due to
-- the fallback "Other" genre
SELECT COUNT( DISTINCT ArtistId ) FROM ALBUMS;
SELECT COUNT( DISTINCT GenreId ) FROM ALBUMS;

-- Artists not used by albums : 0, unless an album's been deleted and the artist hasn't
SELECT * FROM ARTISTS WHERE Id NOT IN ( SELECT ArtistId FROM ALBUMS );

-- Genres not used by albums : 0 or 1, depending on whether the fallback genre is used
SELECT Id FROM GENRES WHERE Id NOT IN ( SELECT GenreId FROM ALBUMS );

-- Album counts by wish list flag
SELECT IsWishListItem, COUNT( al.Id )
FROM ALBUMS al
GROUP BY IsWishListItem;

-- Album counts by catalogue
SELECT COUNT( Id ) FROM ALBUMS WHERE IFNULL( IsWishListItem, 0 ) = 0;
SELECT COUNT( Id ) FROM ALBUMS WHERE IFNULL( IsWishListItem, 0 ) = 1;

-- Artist and genre counts by catalogue
SELECT COUNT( DISTINCT ArtistId ) FROM ALBUMS WHERE IFNULL( IsWishListItem, 0 ) = 0;
SELECT COUNT( DISTINCT ArtistId ) FROM ALBUMS WHERE IFNULL( IsWishListItem, 0 ) = 1;

SELECT COUNT( DISTINCT GenreId ) FROM ALBUMS WHERE IFNULL( IsWishListItem, 0 ) = 0;
SELECT COUNT( DISTINCT GenreId ) FROM ALBUMS WHERE IFNULL( IsWishListItem, 0 ) = 1;