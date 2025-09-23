##  📘 API SAP Business One Integration



### 🧩 Sobre o Projeto

Esta API foi desenvolvida em ASP.NET Core para integrar com o SAP Business One Service Layer, permitindo operações como:


- 🔍 Listar itens (Items)

- ➕ Criar, atualizar e deletar itens

- 👥 Gerenciar parceiros de negócios (BusinessPartners)


------------

### 🚀 Como rodar o projeto

#### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/aplication_sl.git
cd aplication_sl
```

#### 2. Configure o appsettings.json

Crie um arquivo appsettings.json na raiz do projeto com base no appsettings.example.json:
```json
{
  "SAP": {
    "BaseUrl": "https://seu-servidor:50000/b1s/v2/",
    "Url": "https://seu-servidor:50000/b1s/v1/Login",
    "CompanyDB": "DATA_BASE",
    "UserName": "<seu-usuário>",
    "Password": "<sua-senha>",
    "Language": 9
  }
}
```
⚠️ Nunca versionar esse arquivo — ele está protegido no .gitignore.

#### 3. Restaure os pacotes e compile
```bash
dotnet restore
dotnet build
```

#### 4. Execute a aplicação
```bash
dotnet run
```


------------

### 📦 Endpoints disponíveis

#### 🔹 Items
| Método | Rota                     | Descrição                     |
|--------|--------------------------|-------------------------------|
| GET    | /api/items               | Lista os itens do SAP         |
| POST   | /api/items               | Cria um novo item             |
| PATCH  | /api/items/{itemCode}    | Atualiza um item existente    |
| DELETE | /api/items/{itemCode}    | Remove um item  (hard delete)              |


#### 🔹 Business Partners
| Método | Rota                                         | Descrição                                 |
|--------|----------------------------------------------|-------------------------------------------|
| GET    | /api/businesspartners                        | Lista parceiros ativos                    |
| POST   | /api/businesspartners                        | Cria novo parceiro                        |
| PATCH  | /api/businesspartners/{cardCode}             | Atualiza parceiro existente               |
| DELETE | /api/businesspartners/{cardCode}             | Remove parceiro (hard delete)             |


------------

### 🛡️ Segurança e boas práticas

 ✅ appsettings.json está no .gitignore

 ✅ Use appsettings.example.json para compartilhar estrutura

 ✅ Session ID do SAP é gerenciado automaticamente via SapService
 

------------

### 🧪 Testes com Postman

Para testar os endpoints:
1. Autentique-se com o SAP Service Layer

2. Use o B1SESSION no header:

```
Cookie: B1SESSION=SEU_SESSION_ID
```

3. Envie requisições para os endpoints da API

------------






