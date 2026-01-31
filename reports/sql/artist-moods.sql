SELECT  a.Id AS "Artist_Id",
        m.Name AS "Mood_Name",
        m.MorningWeight,
        m.AfternoonWeight,
        m.EveningWeight,
        m.LateWeight
FROM    ARTISTS a
JOIN    ARTIST_MOODS am ON am.ArtistId = a.Id
JOIN    MOODS m ON m.Id = am.MoodId;
