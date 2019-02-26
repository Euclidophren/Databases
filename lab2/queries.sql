SELECT FullName,Age
FROM dbo.Authors
WHERE Age > 25 AND Sex = 'male'
ORDER BY Age DESC;


SELECT DISTINCT PackageName,QuestionAmount
FROM Packages
WHERE QuestionAmount BETWEEN 10 AND 30
ORDER BY PackageName;


SELECT DISTINCT Content,Answer,Source
FROM Questions
WHERE Content LIKE '%USSR%';


Select Author,PackageID
FROM PacktoAuthors
WHERE PackageID IN(
    SELECT PackageID
    FROM Packages
    WHERE Theme = "ABBA"
);


SELECT *
FROM Authors
WHERE EXISTS (
    SELECT Questions.Fk_Author_ID
    FROM Questions LEFT OUTER JOIN Authors
    ON Question.Fk_Author_ID = Authors.Pk_Author_ID
    WHERE Question.Type = 'C'
);


SELECT PackageName,Theme
FROM Packages
WHERE Packages.QuestionAmount > ALL(
            SELECT Packages.QuestionAmount
            FROM Packages
            WHERE Theme = 'ABBA'
            );


SELECT COUNT(Pk_Question_ID) As 'Questions (CHGK)'
FROM Questions
WHERE Type = 'C'
UNION
SELECT COUNT(Pk_Question_ID) As 'Questions (Svoya Igra)'
FROM Questions
WHERE Type = 'S'
UNION
SELECT COUNT(Pk_Question_ID) As 'Questions (Beskrilka)'
FROM Questions
WHERE Type = 'B';


SELECT  PassengerId,
    (
    SELECT AVG(T.Fare/T.Persons)
    FROM T
    WHERE T.TicketId = PTS.TicketId
    ) AS AVGFare,
PTS.Passenger
FROM PTS JOIN T ON  T.TicketId = PTS.TicketId
WHERE Survival = 0
ORDER BY AVGFare DESC;



SELECT Content, Category =
      CASE Type
         WHEN 'C' THEN 'What? Where? When? question'
         WHEN 'B' THEN 'Wingless(poetic type)'
         WHEN 'S' THEN 'Jeopardy question'
         ELSE 'Other'
      END,
FROM Questions
ORDER BY Content;


SELECT PackageID, Theme, QuestionAmount
  CASE
    WHEN QuestionAmount < 10 THEN 'Small'
    WHEN QuestionAmount > 10 AND QuestionAmount < 25 THEN 'Medium'
    ELSE 'Large'
FROM Packages
ORDER BY PackageID;


SELECT COUNT(Pk_Question_ID) As 'Questions (CHGK)'
FROM Questions
WHERE Type = 'C'
UNION
SELECT COUNT(Pk_Question_ID) As 'Questions (Svoya Igra)'
FROM Questions
WHERE Type = 'S'
UNION
SELECT COUNT(Pk_Question_ID) As 'Questions (Beskrilka)'
FROM Questions
WHERE Type = 'B';
INTO #QuestionsPerType
FROM Questions


SELECT Theme,
    AVG(Packages.QuestionAmount) AS AVGQuestionAmount,
    MAX(Packages.QuestionAmount) AS MAXQuestionAmount,
    --MAX(T.Persons) AS MAXPerson
FROM dbo.Packages
GROUP BY Packages.Theme


SELECT AuthorID,FullName
FROM dbo.Authors
WHERE Sex = 'female' AND Age IS NOT NULL
GROUP BY AuthorID
HAVING AVG(Age) >
(
    SELECT AVG(Age)
    FROM Authors
  )


  INSERT dbo.Authors (Pk_Author_ID,FullName,Age,Sex)
  VALUES (1001,'Boris the Blade', 65, 'male')


  INSERT Question (QuestionID, Content, Answer, Source, Fk_Author_ID, Type, Fk_Package_ID)
  SELECT APQ.


  UPDATE dbo.Packages
  SET Theme = 'Other'
  WHERE Theme IS NULL


  DELETE Author
  WHERE Authors.FullName = 'Boris the Blade'

WITH USSRMen (AuthorID, FullName, Age)
AS
(
    SELECT APQ.AuthorID, APQ.Author, Authors.Age, APQ.Used
    FROM APQ JOIN Authors ON APQ.AuthorID = Authors.Pk_Author_ID
    WHERE Authors.Sex = 'male' AND Authors.Age IS NOT NULL
)
SELECT FullName, Age, Used
FROM USSRMen
WHERE Used = 1 AND Age > 18
