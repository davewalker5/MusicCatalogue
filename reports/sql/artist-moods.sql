SELECT  a.Id AS "Artist_Id",
        m.Name AS "Mood_Name"
FROM    ARTISTS a
JOIN    ARTIST_MOODS am ON am.ArtistId = a.Id
JOIN    MOODS m ON m.Id = am.MoodId;
