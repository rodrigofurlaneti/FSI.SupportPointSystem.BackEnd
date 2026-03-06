# 📍 Support Point API

### **Sistema de Ponto de Apoio e Check-in Geolocalizado**

Este projeto é uma solução robusta focada no rastreio e validação de visitas de vendedores externos. A aplicação utiliza geolocalização para garantir que o vendedor esteja em um raio de 100 metros do cliente para realizar check-in e check-out.

---

## 🏗️ Arquitetura e Tecnologias

O projeto segue os princípios de **Domain-Driven Design (DDD)** e **Clean Architecture**, operando em um modelo **Multi-Cloud Ativo-Ativo/Failover**:

* **Runtime:** .NET 8.0 (C#)
* **Frontend:** React / Angular (Hospedado em ambas as nuvens)
* **Multi-Cloud Hosting:** * **Azure:** App Services & Static Web Apps
    * **AWS:** Elastic Beanstalk / ECS & S3 CloudFront
* **Database (Single Source of Truth):** Amazon RDS (MySQL / SQL Server) na AWS
* **Persistência:** Dapper (Execução de Stored Procedures)
* **Testes:** xUnit & Moq (Foco em TDD)

---

## 🌐 Estratégia Multi-Cloud (Distributed Hosting)

Diferente de arquiteturas convencionais, este projeto distribui suas camadas de front-end e API em dois provedores globais, mantendo a persistência centralizada:

1.  **Redundância de Aplicação (Azure & AWS):** A API e o Front-end estão implantados simultaneamente no **Azure App Service** e no **AWS Elastic Beanstalk**. Isso permite que, em caso de instabilidade em uma das regiões ou provedores, o serviço permaneça disponível.
2.  **Banco de Dados Centralizado (AWS RDS):** Para evitar inconsistência de dados (split-brain), o banco de dados reside exclusivamente no **Amazon RDS**. Ambas as instâncias da API (Azure e AWS) conectam-se a este endpoint central.
3.  **Tráfego e DNS:** O roteamento é gerenciado para direcionar o tráfego de forma inteligente entre as duas nuvens, garantindo baixa latência para o usuário final.



---

## 📄 Especificação de Negócio (BDD)

As regras de negócio foram definidas utilizando a linguagem **Gherkin**. O arquivo completo está em `/docs/features/ponto_apoio.feature`.

* **Tolerância de Localização:** Raio de **100 metros** do ponto oficial do cliente.
* **Fluxo de Visita:** Check-out obrigatório antes de um novo Check-in.
* **Segurança:** Autenticação via **CPF** com perfis `ADMIN` e `SELLER`.

---

## 🗄️ Estrutura de Dados (AWS RDS)

A lógica de persistência é centralizada em **Stored Procedures** para garantir que, independentemente de qual cloud a requisição venha (Azure ou AWS), a performance e a integridade sejam idênticas.

### **Procedures Principais:**
* `SpRecordCheckin`: Valida geolocalização e inicia visita.
* `SpRecordCheckout`: Finaliza visita e calcula métricas.

---

## 🚀 Como Executar o Projeto

### **1. Requisitos**
* SDK .NET 8.0
* Instância ativa no **Amazon RDS**
* Configuração de permissões de rede (Security Groups)

### **2. Configuração de Redes Multi-Cloud**
Para que a API no Azure acesse o RDS na AWS:
1.  Obtenha os **Outbound IP Addresses** do seu App Service no Azure.
2.  Adicione estes IPs nas **Inbound Rules** do Security Group do seu RDS na AWS (Porta 5432/1433).

### **3. Variáveis de Ambiente**
```json
{
  "ConnectionStrings": {
    "SupportPointDB": "Endpoint-RDS-AWS;Database=SupportPointDB;User=xxx;Password=xxx;"
  }
}
```
## 📂 Estrutura de Pastas (DDD)
/src

├── SupportPoint.Domain         # Regras de Negócio e Entidades

├── SupportPoint.Application    # Casos de Uso e DTOs

├── SupportPoint.Infrastructure # Repositórios Dapper (Acesso ao RDS)

├── SupportPoint.Api            # API .NET 8.0 (Azure/AWS)

└── SupportPoint.Web            # Front-end (Azure/AWS)

/tests

└── SupportPoint.Tests          # TDD com xUnit

---


