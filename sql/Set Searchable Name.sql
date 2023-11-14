UPDATE ARTISTS
SET SearchableName = SUBSTR( Name, 4 - LENGTH( Name ) )
WHERE Name LIKE 'The %';
