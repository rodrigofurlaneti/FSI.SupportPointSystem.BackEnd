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
    -- Cria o Usuário primeiro (Perfil SELLER)
    INSERT INTO Users (Cpf, PasswordHash, Role)
    VALUES (p_Cpf, p_PasswordHash, 'SELLER')
    RETURNING Id INTO v_UserId;

    -- Cria o Vendedor vinculado ao Usuário
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
    INSERT INTO Checkins (SellerId, CustomerId, LatitudeCaptured, LongitudeCaptured, DistanceMeters)
    VALUES (p_SellerId, p_CustomerId, p_LatCaptured, p_LogCaptured, p_Distance);
END;
$$;
