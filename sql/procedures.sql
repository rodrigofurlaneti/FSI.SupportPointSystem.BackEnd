CREATE OR REPLACE PROCEDURE SpCreateSeller(
    p_Cpf VARCHAR,
    p_PasswordHash VARCHAR,
    p_Name VARCHAR,
    p_Phone VARCHAR
)
LANGUAGE plpgsql AS $$
DECLARE
    v_UserId UUID;
BEGIN
    -- Cria as credenciais de acesso
    INSERT INTO Users (Cpf, PasswordHash, Role)
    VALUES (p_Cpf, p_PasswordHash, 'SELLER')
    RETURNING Id INTO v_UserId;

    -- Cria o perfil do vendedor vinculado
    INSERT INTO Sellers (UserId, Name, Phone)
    VALUES (v_UserId, p_Name, p_Phone);
END;
$$;

CREATE OR REPLACE PROCEDURE SpUpsertCustomer(
    p_CompanyName VARCHAR,
    p_Cnpj VARCHAR,
    p_LatTarget DECIMAL,
    p_LogTarget DECIMAL
)
LANGUAGE plpgsql AS $$
BEGIN
    INSERT INTO Customers (CompanyName, Cnpj, LatitudeTarget, LongitudeTarget)
    VALUES (p_CompanyName, p_Cnpj, p_LatTarget, p_LogTarget)
    ON CONFLICT (Cnpj) DO UPDATE SET
        CompanyName = EXCLUDED.CompanyName,
        LatitudeTarget = EXCLUDED.LatitudeTarget,
        LongitudeTarget = EXCLUDED.LongitudeTarget;
END;
$$;

CREATE OR REPLACE PROCEDURE SpRecordCheckin(
    p_SellerId UUID,
    p_CustomerId UUID,
    p_LatCaptured DECIMAL,
    p_LogCaptured DECIMAL,
    p_Distance FLOAT
)
LANGUAGE plpgsql AS $$
BEGIN
    -- Valida se já existe uma visita aberta para este vendedor
    IF EXISTS (SELECT 1 FROM Checkins WHERE SellerId = p_SellerId AND CheckoutTimestamp IS NULL) THEN
        RAISE EXCEPTION 'Vendedor já possui um check-in ativo em andamento.';
    END IF;

    INSERT INTO Checkins (SellerId, CustomerId, LatitudeCaptured, LongitudeCaptured, DistanceMeters)
    VALUES (p_SellerId, p_CustomerId, p_LatCaptured, p_LogCaptured, p_Distance);
END;
$$;

CREATE OR REPLACE PROCEDURE SpRecordCheckout(
    p_SellerId UUID,
    p_CustomerId UUID,
    p_LatCaptured DECIMAL,
    p_LogCaptured DECIMAL,
    p_Distance FLOAT
)
LANGUAGE plpgsql AS $$
DECLARE
    v_CheckinId UUID;
    v_CheckinTime TIMESTAMP;
BEGIN
    -- Localiza a última visita aberta para este par Vendedor/Cliente
    SELECT Id, CheckinTimestamp INTO v_CheckinId, v_CheckinTime
    FROM Checkins 
    WHERE SellerId = p_SellerId 
      AND CustomerId = p_CustomerId 
      AND CheckoutTimestamp IS NULL
    ORDER BY CheckinTimestamp DESC
    LIMIT 1;

    IF v_CheckinId IS NULL THEN
        RAISE EXCEPTION 'Nenhuma visita aberta encontrada para este vendedor neste cliente.';
    END IF;

    -- Atualiza os dados de saída e calcula o tempo de permanência
    UPDATE Checkins SET 
        CheckoutLatitude = p_LatCaptured,
        CheckoutLongitude = p_LogCaptured,
        CheckoutDistanceMeters = p_Distance,
        CheckoutTimestamp = CURRENT_TIMESTAMP,
        DurationMinutes = EXTRACT(EPOCH FROM (CURRENT_TIMESTAMP - v_CheckinTime)) / 60
    WHERE Id = v_CheckinId;
END;
$$;
