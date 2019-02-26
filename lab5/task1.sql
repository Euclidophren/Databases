SELECT Content, Answer, Source
FROM Questions
WHERE Type = 'C'
FOR XML RAW ROOT('CHGK questions')
--FOR XML AUTO ROOT('CHGK questions')
--FOR XML EXPLICIT('CHGK questions')
