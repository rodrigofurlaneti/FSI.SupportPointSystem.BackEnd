CREATE PROCEDURE SpCreateSeller
    @Cpf VARCHAR(11),
    @PasswordHash VARCHAR(255),
    @Name VARCHAR(100),
    @Phone VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @v_UserId UNIQUEIDENTIFIER = NEWID();

    BEGIN TRANSACTION;
    BEGIN TRY
        -- Cria as credenciais na tabela Users
        INSERT INTO Users (Id, Cpf, PasswordHash, [Role])
        VALUES (@v_UserId, @Cpf, @PasswordHash, 'SELLER');

        -- Cria o perfil do vendedor vinculado
        INSERT INTO Sellers (UserId, Name, Phone, Active)
        VALUES (@v_UserId, @Name, @Phone, 1);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

CREATE PROCEDURE SpUpsertCustomer
    @CompanyName VARCHAR(150),
    @Cnpj VARCHAR(14),
    @LatTarget DECIMAL(12, 9),
    @LogTarget DECIMAL(12, 9)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM Customers WHERE Cnpj = @Cnpj)
    BEGIN
        UPDATE Customers SET 
            CompanyName = @CompanyName,
            LatitudeTarget = @LatTarget,
            LongitudeTarget = @LogTarget
        WHERE Cnpj = @Cnpj;
    END
    ELSE
    BEGIN
        INSERT INTO Customers (CompanyName, Cnpj, LatitudeTarget, LongitudeTarget)
        VALUES (@CompanyName, @Cnpj, @LatTarget, @LogTarget);
    END
END;
GO

CREATE PROCEDURE SpRecordCheckin
    @SellerId UNIQUEIDENTIFIER,
    @CustomerId UNIQUEIDENTIFIER,
    @LatCaptured DECIMAL(12, 9),
    @LogCaptured DECIMAL(12, 9),
    @Distance FLOAT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Checkins WHERE SellerId = @SellerId AND CheckoutTimestamp IS NULL)
    BEGIN
        RAISERROR ('Vendedor já possui um check-in ativo em andamento.', 16, 1);
        RETURN;
    END

    INSERT INTO Checkins (SellerId, CustomerId, LatitudeCaptured, LongitudeCaptured, DistanceMeters, CheckinTimestamp)
    VALUES (@SellerId, @CustomerId, @LatCaptured, @LogCaptured, @Distance, GETDATE());
END;
GO
CREATE PROCEDURE SpRecordCheckout
    @SellerId UNIQUEIDENTIFIER,
    @CustomerId UNIQUEIDENTIFIER,
    @LatCaptured DECIMAL(12, 9),
    @LogCaptured DECIMAL(12, 9),
    @Distance FLOAT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @v_CheckinId UNIQUEIDENTIFIER;
    DECLARE @v_CheckinTime DATETIME2;

    -- Localiza a última visita aberta
    SELECT TOP 1 @v_CheckinId = Id, @v_CheckinTime = CheckinTimestamp
    FROM Checkins 
    WHERE SellerId = @SellerId 
      AND CustomerId = @CustomerId 
      AND CheckoutTimestamp IS NULL
    ORDER BY CheckinTimestamp DESC;

    IF @v_CheckinId IS NULL
    BEGIN
        RAISERROR ('Nenhuma visita aberta encontrada para este vendedor neste cliente.', 16, 1);
        RETURN;
    END

    -- Atualiza e calcula duração em minutos
    UPDATE Checkins SET 
        CheckoutLatitude = @LatCaptured,
        CheckoutLongitude = @LogCaptured,
        CheckoutDistanceMeters = @Distance,
        CheckoutTimestamp = GETDATE(),
        DurationMinutes = DATEDIFF(MINUTE, @v_CheckinTime, GETDATE())
    WHERE Id = @v_CheckinId;
END;
GO