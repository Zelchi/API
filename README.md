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

### 2. Testando via PowerShell (Invoke-RestMethod)

#### Testando Accounts (Contas)

**Criar uma conta:**
```powershell
$body = @{
    username = "testuser"
    email = "test@example.com"
    password = "password123"
    role = "admin"
} | ConvertTo-Json

$headers = @{
    "Content-Type" = "application/json"
}

Invoke-RestMethod -Uri "http://localhost:5080/api/v1/Account" -Method Post -Body $body -Headers $headers
```

**Listar todas as contas:**
```powershell
Invoke-RestMethod -Uri "http://localhost:5080/api/v1/Account" -Method Get
```

**Buscar conta por ID:**
```powershell
Invoke-RestMethod -Uri "http://localhost:5080/api/v1/Account/1" -Method Get
```

**Atualizar uma conta:**
```powershell
$body = @{
    username = "updateduser"
    role = "user"
} | ConvertTo-Json

$headers = @{
    "Content-Type" = "application/json"
}

Invoke-RestMethod -Uri "http://localhost:5080/api/v1/Account/1" -Method Put -Body $body -Headers $headers
```

**Deletar uma conta:**
```powershell
Invoke-RestMethod -Uri "http://localhost:5080/api/v1/Account/1" -Method Delete
```

#### Testando Contacts (Contatos)

**Criar um contato:**
```powershell
$body = @{
    name = "João Silva"
    email = "joao@example.com"
    phone = "11999999999"
    accountId = 1
} | ConvertTo-Json

$headers = @{
    "Content-Type" = "application/json"
}

Invoke-RestMethod -Uri "http://localhost:5080/api/v1/Contact" -Method Post -Body $body -Headers $headers
```

**Listar todos os contatos:**
```powershell
Invoke-RestMethod -Uri "http://localhost:5080/api/v1/Contact" -Method Get
```

#### Testando Products (Produtos)

**Criar um produto:**
```powershell
$body = @{
    name = "Notebook Dell"
    description = "Notebook para desenvolvimento"
    price = 2500.00
    accountId = 1
} | ConvertTo-Json

$headers = @{
    "Content-Type" = "application/json"
}

Invoke-RestMethod -Uri "http://localhost:5080/api/v1/Product" -Method Post -Body $body -Headers $headers
```

**Listar todos os produtos:**
```powershell
Invoke-RestMethod -Uri "http://localhost:5080/api/v1/Product" -Method Get
```

### 3. Testando via cURL (Alternativa)

Se preferir usar cURL:

**Criar uma conta:**
```bash
curl -X POST "http://localhost:5080/api/v1/Account" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com", 
    "password": "password123",
    "role": "admin"
  }'
```

## 📊 Endpoints Disponíveis

### Accounts
- `GET /api/v1/Account` - Listar todas as contas
- `GET /api/v1/Account/{id}` - Buscar conta por ID
- `POST /api/v1/Account` - Criar nova conta
- `PUT /api/v1/Account/{id}` - Atualizar conta
- `DELETE /api/v1/Account/{id}` - Deletar conta (soft delete)

### Contacts  
- `GET /api/v1/Contact` - Listar todos os contatos
- `GET /api/v1/Contact/{id}` - Buscar contato por ID
- `POST /api/v1/Contact` - Criar novo contato
- `PUT /api/v1/Contact/{id}` - Atualizar contato
- `DELETE /api/v1/Contact/{id}` - Deletar contato

### Products
- `GET /api/v1/Product` - Listar todos os produtos
- `GET /api/v1/Product/{id}` - Buscar produto por ID
- `POST /api/v1/Product` - Criar novo produto
- `PUT /api/v1/Product/{id}` - Atualizar produto
- `DELETE /api/v1/Product/{id}` - Deletar produto

## 🗃️ Modelos de Dados

### Account (Conta)
```json
{
  "id": 1,
  "username": "string",
  "email": "string",
  "password": "string",
  "role": "string",
  "createdAt": "2025-08-23T10:30:00Z",
  "updatedAt": "2025-08-23T10:30:00Z",
  "deletedAt": "0001-01-01T00:00:00"
}
```

### Contact (Contato)
```json
{
  "id": 1,
  "name": "string",
  "email": "string", 
  "phone": "string",
  "accountId": 1,
  "createdAt": "2025-08-23T10:30:00Z",
  "updatedAt": "2025-08-23T10:30:00Z"
}
```

### Product (Produto)
```json
{
  "id": 1,
  "name": "string",
  "description": "string",
  "price": 0.0,
  "accountId": 1,
  "createdAt": "2025-08-23T10:30:00Z", 
  "updatedAt": "2025-08-23T10:30:00Z"
}
```

## 🔧 Configurações

### Banco de Dados
- **Host**: localhost
- **Porta**: 3306
- **Database**: test
- **Usuário**: test
- **Senha**: test

### API
- **Ambiente Development**: http://localhost:5080
- **Ambiente Production**: https://localhost:7033

## 🛠️ Comandos Úteis

### Docker
```powershell
# Iniciar containers
docker-compose up -d

# Parar containers
docker-compose down

# Ver logs do MySQL
docker-compose logs mysql-db

# Resetar volumes (apaga dados)
docker-compose down -v
```

### Entity Framework
```powershell
# Criar nova migração
dotnet ef migrations add NomeDaMigracao

# Aplicar migrações
dotnet ef database update

# Remover última migração (não aplicada)
dotnet ef migrations remove

# Ver status das migrações
dotnet ef migrations list
```

### .NET
```powershell
# Executar aplicação
dotnet run

# Executar com hot reload
dotnet watch run

# Compilar projeto
dotnet build

# Limpar compilação
dotnet clean
```

## 🐛 Troubleshooting

### Problema: "Connection refused" ao acessar MySQL
**Solução**: Verifique se o Docker está rodando e execute:
```powershell
docker-compose up -d
```

### Problema: Porta já está em uso
**Solução**: Mate o processo na porta ou altere a porta no `launchSettings.json`

### Problema: Migrações não aplicadas
**Solução**: Execute:
```powershell
dotnet ef database update
```

### Problema: Swagger não carrega
**Solução**: Certifique-se de estar no ambiente Development e acesse http://localhost:5080/swagger

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