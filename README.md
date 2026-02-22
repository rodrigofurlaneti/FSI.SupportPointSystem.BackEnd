# 📍 Support Point API

### **Sistema de Ponto de Apoio e Check-in Geolocalizado**

Este projeto é uma API robusta desenvolvida em **.NET 8.0** focada no rastreio e validação de visitas de vendedores externos. A solução utiliza geolocalização para garantir que o vendedor esteja em um raio de 100 metros do cliente para realizar check-in e check-out.

---

## 🏗️ Arquitetura e Tecnologias

O projeto segue os princípios de **Domain-Driven Design (DDD)** e **Clean Architecture**:

* **Core:** .NET 8.0 (C#)
* **Banco de Dados:** PostgreSQL / SQL Server (Padrão `SupportPointDB`)
* **Persistência:** Dapper (Execução de Stored Procedures)
* **Testes:** xUnit & Moq (Foco em TDD)
* **Especificação:** Gherkin / BDD

---

## 📄 Especificação de Negócio (BDD)

As regras de negócio foram definidas utilizando a linguagem **Gherkin**. O arquivo de especificação completo encontra-se em `/docs/features/ponto_apoio.feature`.

### **Principais Regras:**

* **Margem de Erro:** O sistema aceita uma tolerância de até **100 metros** de distância do ponto oficial do cliente.
* **Estado da Visita:** Um vendedor não pode iniciar um novo check-in sem ter finalizado (check-out) o anterior.
* **Segurança:** Autenticação via **CPF** com perfis de `ADMIN` (gestão) e `SELLER` (operação).

---

## 🗄️ Estrutura do Banco de Dados

O sistema utiliza o banco de dados `SupportPointDB`. A lógica de persistência é centralizada em **Stored Procedures** para garantir integridade e performance.

### **Principais Tabelas:**

* **Users:** Credenciais e Roles (ADMIN/SELLER).
* **Sellers:** Dados dos representantes comerciais.
* **Customers:** Cadastro de clientes com `LatitudeTarget` e `LongitudeTarget`.
* **Checkins:** Registro de visitas, distâncias capturadas, timestamps e duração.

---

## 🚀 Como Executar o Projeto

### **1. Requisitos**
* SDK .NET 8.0
* Instância de banco de dados compatível (PostgreSQL/SQL Server)
* IDE (VS Code ou Visual Studio 2022)

### **2. Configuração do Banco**
Execute os scripts contidos em `/database/init.sql` para criar as tabelas e as seguintes procedures:
* `SpCreateSeller`
* `SpUpsertCustomer`
* `SpRecordCheckin`
* `SpRecordCheckout`

### **3. Rodando a API**
```bash
# Restaurar dependências
dotnet restore

# Executar o projeto
dotnet run --project SupportPoint.Api

# 📂 Estrutura de Pastas (DDD)

/src
├── SupportPoint.Domain         # Entidades, Value Objects e Interfaces
├── SupportPoint.Application    # Use Cases (Commands) e DTOs
├── SupportPoint.Infrastructure # Implementação de Repositórios (Dapper)
└── SupportPoint.Api            # Controllers e Injeção de Dependência
/tests
└── SupportPoint.Tests          # Testes de Unidade e Integração (TDD)

