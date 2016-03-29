CREATE PROCEDURE [dbo].[Agents_Add]
(
          @agentId  INT,
          @name  INT,
          @description NVARCHAR,
          @email NVARCHAR,
          @password NVARCHAR,
          @logo_photoId  INT
)
AS
    INSERT INTO[Agents]
 (
          agentId,
          name,
          description,
          email,
          password,
          logo_photoId
)
    VALUES
 (
           @agentId,
           @name,
           @description,
           @email,
           @password,
           @logo_photoId
) 
     SET @agentId = SCOPE_IDENTITY()
  GO
CREATE PROCEDURE [dbo].[Agents_Update]
(
          @agentId  INT,
          @name  INT,
          @description NVARCHAR,
          @email NVARCHAR,
          @password NVARCHAR,
          @logo_photoId  INT
AS
     UPDATE  [Agents]
     SET 
       agentId = @agentId,
       name = @name,
       description = @description,
       email = @email,
       password = @password,
       logo_photoId = @logo_photoId
     WHERE
       agentId = @agentId
GO
