SELECT  a.Id AS "ArtistId",
        a.Name AS "Artist",
        m.Name AS "Mood"
FROM    ARTISTS a
JOIN    ARTIST_MOODS am ON am.ArtistId = a.Id
JOIN    MOODS m ON m.Id = am.MoodId;
