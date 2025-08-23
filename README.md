# API BootCamp - Guia de Testes

## 📋 Visão Geral

Esta é uma API RESTful desenvolvida com ASP.NET Core 9.0 que gerencia três entidades principais:
- **Accounts** (Contas de usuário)
- **Contacts** (Contatos)
- **Products** (Produtos)

## 🛠️ Tecnologias Utilizadas

- **.NET 9.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 9.0**
- **MySQL 9.4**
- **Docker & Docker Compose**
- **Swagger/OpenAPI**

## 📁 Estrutura do Projeto

```
Application/
├── Backend/
│   ├── Server/
│   │   ├── Config/
│   │   │   ├── Database.cs          # Configuração do DbContext
│   │   │   ├── Pipeline.cs          # Configuração do pipeline de middleware
│   │   │   ├── Service.cs           # Configuração de serviços DI
│   │   │   └── Startup.cs           # Ponto de entrada da aplicação
│   │   └── Routes/                  # Pasta referente a os endpoints
│   │       ├── Account/             # CRUD de contas de usuário
│   │       ├── Contact/             # CRUD de contatos
│   │       └── Product/             # CRUD de produtos
│   ├── Migrations/                  # Migrações do Entity Framework
│   ├── appsettings.json             # Configurações de produção
│   ├── appsettings.Development.json # Configurações de desenvolvimento
│   └── Backend.csproj               # Arquivo de projeto
├── docker-compose.yml               # Configuração do MySQL
└── Application.sln                  # Solução do Visual Studio
```

## 🚀 Como Executar a Aplicação

### Pré-requisitos

1. **.NET 9.0 SDK** instalado
2. **Docker** e **Docker Compose** instalados
3. **PowerShell** (Windows)

### Passo 1: Iniciar o Banco de Dados

1. Abra o PowerShell na pasta raiz do projeto
2. Execute o comando para iniciar o MySQL:

```powershell
docker-compose up -d
```

Este comando irá:
- Baixar a imagem do MySQL 9.4
- Criar um container com banco de dados `test`
- Configurar usuário `test` com senha `test`
- Expor a porta 3306

### Passo 2: Executar as Migrações

1. Navegue até a pasta Backend:

```powershell
cd Backend
```

2. Execute as migrações para criar as tabelas:

```powershell
dotnet ef migrations add init
```

```powershell
dotnet ef database update
```

### Passo 3: Executar a API

1. Execute a aplicação:

```powershell
dotnet run
```

A API estará disponível em:
- **HTTP**: http://localhost:5080
- **HTTPS**: https://localhost:7033

### Passo 4: Acessar a Documentação Swagger

Com a aplicação rodando, acesse:
- **Swagger UI**: http://localhost:5080/swagger

## 🧪 Como Testar a API

### 1. Testando via Swagger UI

A forma mais fácil de testar é através da interface Swagger:

1. Acesse http://localhost:5080/swagger
2. Explore os endpoints disponíveis
3. Clique em "Try it out" para testar qualquer endpoint
4. Preencha os parâmetros necessários
5. Clique em "Execute"

## 🔧 Configurações

### Banco de Dados
- **Host**: localhost
- **Porta**: 3306
- **Database**: test
- **Usuário**: test
- **Senha**: test

### API
- **Ambiente Development**: http://localhost:5080

## 🛠️ Comandos Úteis

## 🧪 Fluxo de Teste Recomendado

1. **Iniciar infraestrutura**: `docker-compose up -d`
2. **Aplicar migrações**: `dotnet ef database update`
3. **Iniciar API**: `dotnet run`
4. **Criar uma conta** via Swagger ou PowerShell
5. **Criar contatos e produtos** associados à conta
6. **Testar operações CRUD** em todas as entidades
7. **Verificar soft delete** nas contas
8. **Testar validações** enviando dados inválidos

## 📝 Notas Importantes

- A API usa **soft delete** para contas (campo `DeletedAt`)
- Todas as datas são armazenadas em **UTC**
- A validação de dados é feita via **Data Annotations**
- O Swagger está disponível apenas em **Development**
- As senhas são armazenadas em **texto plano** (não recomendado para produção)

---