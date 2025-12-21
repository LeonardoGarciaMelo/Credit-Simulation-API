# Credit Simulation API

> **Uma API REST robusta e containerizada para simulação de crédito financeiro, implementando arquitetura em camadas, logs estruturados e testes automatizados.**

## 📋 Sobre o Projeto

Este projeto é uma modernização e refatoração completa de um desafio de um Hackathon. Foi reescrito utilizando as melhores práticas do ecossistema **.NET 8**.

O objetivo é fornecer um motor de cálculo financeiro (SAC e Price) resiliente, observável e pronto para ambientes de nuvem (Cloud-Native), mas mantendo requisitos de auditoria bancária tradicional.

### 🚀 Principais Features

* **Cálculos Financeiros de Precisão:** Algoritmos validados para sistemas de amortização SAC e Price.
* **Persistência Híbrida:**
    * **Oracle Database:** Leitura de parâmetros de produtos (Simulação de Legado/Mainframe).
    * **SQLite:** Gravação transacional de histórico de simulações.
* **Relatórios Agregados:** Endpoints analíticos com agregação de dados e enriquecimento (Enrichment) entre bancos distintos.
* **Paginação Server-Side:** Otimização de performance para listagem de grandes volumes de dados.
* **Health Checks:** Monitoramento ativo de dependências (Liveness/Readiness) para orquestradores (K8s).

---

## 🏗️ Arquitetura e Padrões

O projeto segue uma arquitetura limpa focada em manutenibilidade e escalabilidade:

1.  **Observabilidade Híbrida (Diferencial):**
    * **Logs de Auditoria (.txt):** Gravação em disco com rotação diária para conformidade bancária.
    * **Logs de Telemetria (Seq):** Envio de logs estruturados para servidor Seq via Docker para análise em tempo real.
    * **Middleware Customizado:** Interceptação de requisições para cronometragem e registro de falhas globais.
2.  **Qualidade de Código:**
    * **Testes Unitários (xUnit):** Cobertura de lógica de negócio e serviços.
    * **Mocking (Moq) & In-Memory DB:** Testes isolados sem dependência de infraestrutura externa.
3.  **DevOps:**
    * **Docker Multi-Stage Build:** Imagens otimizadas para produção.
    * **Docker Compose:** Orquestração completa do ambiente (API + Logs).

---

## 🛠️ Tecnologias Utilizadas

* **Core:** C# .NET 8 (Web API)
* **ORM:** Entity Framework Core
* **Bancos de Dados:** Oracle (Provedor ODP.NET) e SQLite
* **Logging:** Serilog (Sinks para File, Console e Seq)
* **Documentação:** Swagger / OpenAPI
* **Testes:** xUnit, Moq, FluentAssertions
* **Containerização:** Docker e Docker Compose

---

## 🚀 Como Rodar o Projeto

### Pré-requisitos

* [Docker](https://www.docker.com/) e Docker Compose instalados.
* (Opcional) .NET 8 SDK para rodar localmente sem Docker.

### 🔒 Configuração de Ambiente (.env)

Por segurança, as credenciais não estão versionadas. Crie um arquivo `.env` na raiz do projeto seguindo o modelo abaixo:

```env
# Configurações da Aplicação
ASPNETCORE_ENVIRONMENT=Production
SEQ_URL=http://seq:5341

# Senha de Administração do Seq (Defina sua senha forte)
SEQ_ADMIN_PASSWORD=SuaSenha

# Credenciais do Banco Oracle (Exemplo)
ORACLE_USER=usuario
ORACLE_PASSWORD=Senha
ORACLE_HOST=host
ORACLE_PORT=porta
ORACLE_SID=SID
