SELECT      a.Name AS "Artist",
            m.Name AS "Mood"
FROM        ARTISTS a
INNER JOIN  ARTIST_MOODS am ON am.ArtistId = a.Id
INNER JOIN  MOODS m ON m.Id = am.MoodId
ORDER BY    a.Name, m.Name;
