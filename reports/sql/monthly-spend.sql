SELECT		RANK() OVER ( ORDER BY al.Purchased ) AS "Id",
			STRFTIME( '%Y', al.Purchased ) AS "Year",
			STRFTIME( '%m', al.Purchased ) AS "Month",
			SUM( al.Price ) AS "Spend"
FROM		ALBUMS al
WHERE		IFNULL( al.IsWishListItem, 0 ) = 0
AND			al.Price IS NOT NULL
GROUP BY	STRFTIME( '%Y', al.Purchased ),
			STRFTIME( '%m', al.Purchased );
