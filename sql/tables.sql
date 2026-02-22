-- Criação do Banco de Dados
CREATE DATABASE SupportPointDB;

-- Tabela de Usuários (Auth e RBAC)
CREATE TABLE Users (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    Cpf VARCHAR(11) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Role VARCHAR(20) CHECK (Role IN ('ADMIN', 'SELLER')) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabela de Vendedores
CREATE TABLE Sellers (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    UserId UUID REFERENCES Users(Id) ON DELETE CASCADE,
    Name VARCHAR(100) NOT NULL,
    Phone VARCHAR(20),
    Active BOOLEAN DEFAULT TRUE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabela de Clientes
CREATE TABLE Customers (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    CompanyName VARCHAR(150) NOT NULL,
    Cnpj VARCHAR(14) UNIQUE NOT NULL,
    LatitudeTarget DECIMAL(10, 8) NOT NULL,
    LongitudeTarget DECIMAL(11, 8) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tabela de Visitas (Unificação de Check-in e Check-out)
CREATE TABLE Checkins (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    SellerId UUID REFERENCES Sellers(Id) NOT NULL,
    CustomerId UUID REFERENCES Customers(Id) NOT NULL,
    
    -- Dados da Entrada (Check-in)
    LatitudeCaptured DECIMAL(10, 8) NOT NULL,
    LongitudeCaptured DECIMAL(11, 8) NOT NULL,
    DistanceMeters FLOAT NOT NULL,
    CheckinTimestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    -- Dados da Saída (Check-out) - Iniciam nulos
    CheckoutLatitude DECIMAL(10, 8),
    CheckoutLongitude DECIMAL(11, 8),
    CheckoutDistanceMeters FLOAT,
    CheckoutTimestamp TIMESTAMP,
    
    -- Resultado (Calculado via Procedure ou Aplicação)
    DurationMinutes INT
);
