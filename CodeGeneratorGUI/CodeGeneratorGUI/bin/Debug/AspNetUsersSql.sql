CREATE PROCEDURE [dbo].[AspNetUsers_Add]
(
          @Id NVARCHAR,
          @website NVARCHAR,
          @aboutMe NVARCHAR,
          @firstName NVARCHAR,
          @lastName NVARCHAR,
          @contactNumber NVARCHAR,
          @country NVARCHAR,
          @city NVARCHAR,
          @language NVARCHAR,
          @Email NVARCHAR,
          @EmailConfirmed  BIT,
          @PasswordHash NVARCHAR,
          @SecurityStamp NVARCHAR,
          @PhoneNumber NVARCHAR,
          @PhoneNumberConfirmed  BIT,
          @TwoFactorEnabled  BIT,
          @LockoutEndDateUtc DATETIME,
          @LockoutEnabled  BIT,
          @AccessFailedCount  INT,
          @UserName NVARCHAR
)
AS
    INSERT INTO[AspNetUsers]
 (
          Id,
          website,
          aboutMe,
          firstName,
          lastName,
          contactNumber,
          country,
          city,
          language,
          Email,
          EmailConfirmed,
          PasswordHash,
          SecurityStamp,
          PhoneNumber,
          PhoneNumberConfirmed,
          TwoFactorEnabled,
          LockoutEndDateUtc,
          LockoutEnabled,
          AccessFailedCount,
          UserName
)
    VALUES
 (
           @Id,
           @website,
           @aboutMe,
           @firstName,
           @lastName,
           @contactNumber,
           @country,
           @city,
           @language,
           @Email,
           @EmailConfirmed,
           @PasswordHash,
           @SecurityStamp,
           @PhoneNumber,
           @PhoneNumberConfirmed,
           @TwoFactorEnabled,
           @LockoutEndDateUtc,
           @LockoutEnabled,
           @AccessFailedCount,
           @UserName
) 
     SET @Id = SCOPE_IDENTITY()
  GO
CREATE PROCEDURE [dbo].[AspNetUsers_Update]
(
          @Id NVARCHAR,
          @website NVARCHAR,
          @aboutMe NVARCHAR,
          @firstName NVARCHAR,
          @lastName NVARCHAR,
          @contactNumber NVARCHAR,
          @country NVARCHAR,
          @city NVARCHAR,
          @language NVARCHAR,
          @Email NVARCHAR,
          @EmailConfirmed  BIT,
          @PasswordHash NVARCHAR,
          @SecurityStamp NVARCHAR,
          @PhoneNumber NVARCHAR,
          @PhoneNumberConfirmed  BIT,
          @TwoFactorEnabled  BIT,
          @LockoutEndDateUtc DATETIME,
          @LockoutEnabled  BIT,
          @AccessFailedCount  INT,
          @UserName NVARCHAR
AS
     UPDATE  [AspNetUsers]
     SET 
       Id = @Id,
       website = @website,
       aboutMe = @aboutMe,
       firstName = @firstName,
       lastName = @lastName,
       contactNumber = @contactNumber,
       country = @country,
       city = @city,
       language = @language,
       Email = @Email,
       EmailConfirmed = @EmailConfirmed,
       PasswordHash = @PasswordHash,
       SecurityStamp = @SecurityStamp,
       PhoneNumber = @PhoneNumber,
       PhoneNumberConfirmed = @PhoneNumberConfirmed,
       TwoFactorEnabled = @TwoFactorEnabled,
       LockoutEndDateUtc = @LockoutEndDateUtc,
       LockoutEnabled = @LockoutEnabled,
       AccessFailedCount = @AccessFailedCount,
       UserName = @UserName
     WHERE
       Id = @Id
GO
