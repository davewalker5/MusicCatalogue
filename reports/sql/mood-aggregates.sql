SELECT      m.Name AS Mood,
            COUNT(DISTINCT a.Id) AS "ArtistCount",
            AVG(a.Energy) AS "AvgEnergy",
            AVG(a.Intimacy) AS "AvgIntimacy",
            AVG(a.Warmth) AS "AvgWarmth"
FROM        MOODS m
JOIN        ARTIST_MOODS am ON am.MoodId = m.Id
JOIN        ARTISTS a ON a.Id = am.ArtistId
GROUP BY    m.Id, m.Name
ORDER BY    ArtistCount DESC;
