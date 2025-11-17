# Music Streaming API - Backend em .NET 8

Este é o projeto de backend para a aplicação de streaming de música, desenvolvido em **C# com .NET 8 e ASP.NET Core**. Ele fornece uma API RESTful completa com CRUD funcional, autenticação JWT e integração com banco de dados MySQL, pronta para ser consumida pelo frontend em Angular.

---

## 🚀 Funcionalidades

- ✅ **API RESTful Completa**: Endpoints para Músicas, Planos e Autenticação.
- ✅ **CRUD de Músicas**: Criar, Ler, Atualizar e Deletar músicas.
- ✅ **Autenticação JWT**: Sistema de login e cadastro com tokens JWT seguros.
- ✅ **Banco de Dados MySQL**: Persistência de dados com Entity Framework Core e MySQL.
- ✅ **Seed de Dados**: O banco de dados é populado com planos e músicas de exemplo na inicialização.
- ✅ **Documentação Swagger**: Interface interativa para testar todos os endpoints.
- ✅ **CORS Pré-configurado**: Pronto para aceitar requisições do frontend Angular em `http://localhost:4200`.
- ✅ **Docker Ready**: Inclui `docker-compose.yml` para subir uma instância do MySQL com um único comando.

---

## 🛠️ Tecnologias Utilizadas

- **Backend**: C# 12, .NET 8, ASP.NET Core
- **Banco de Dados**: MySQL 8.0
- **ORM**: Entity Framework Core 8
- **Autenticação**: ASP.NET Core Identity, JWT (JSON Web Tokens)
- **Container**: Docker
- **IDE**: Visual Studio Code

---

## 📋 Pré-requisitos

Antes de começar, garanta que você tenha os seguintes softwares instalados:

1.  **[.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)**
2.  **[Docker Desktop](https://www.docker.com/products/docker-desktop/)**
3.  **[Visual Studio Code](https://code.visualstudio.com/)**
    - Extensão recomendada: [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)

---

## ⚙️ Guia de Instalação e Execução (Passo a Passo)

Siga estes passos para rodar o backend localmente.

### Passo 1: Obter o Projeto

Descompacte o arquivo `.zip` fornecido em uma pasta de sua escolha.

### Passo 2: Iniciar o Banco de Dados MySQL com Docker

Abra um terminal na raiz do projeto (na pasta `MusicStreamingAPI`) e execute o seguinte comando:

```bash
docker-compose up -d
```

**O que este comando faz?**
- Baixa a imagem do MySQL 8.0 (se ainda não tiver).
- Cria um container chamado `music-streaming-mysql`.
- Expõe a porta `3306` para a sua máquina local.
- Cria um banco de dados chamado `MusicStreamingDB`.
- Define a senha do usuário `root` como `root`.
- Cria um volume para persistir os dados do banco.

Para verificar se o container está rodando, use `docker ps`.

### Passo 3: Abrir o Projeto no VS Code

Abra a pasta `MusicStreamingAPI/MusicStreamingAPI` no Visual Studio Code.

```bash
code MusicStreamingAPI/MusicStreamingAPI
```

### Passo 4: Restaurar Dependências

O VS Code deve restaurar os pacotes NuGet automaticamente. Caso não aconteça, abra o terminal integrado (`Ctrl + '`) e execute:

```bash
dotnet restore
```

### Passo 5: Executar a API

Agora, basta executar o projeto. Pressione `F5` no VS Code ou use o comando no terminal:

```bash
dotnet run
```

**O que acontece ao executar?**
1.  A API será compilada e iniciada.
2.  O Entity Framework Core irá se conectar ao banco de dados MySQL.
3.  O método `context.Database.EnsureCreated()` irá **criar o banco de dados e as tabelas** automaticamente.
4.  Os dados de **seed** (planos e músicas de exemplo) serão inseridos no banco.

O terminal exibirá mensagens de sucesso:

```
✅ Banco de dados inicializado com sucesso!
🚀 Music Streaming API iniciada!
📖 Documentação Swagger: http://localhost:5000
🔗 API Base URL: http://localhost:5000/api
```

---

## 🧪 Testando a API com Swagger

Após executar a API, abra seu navegador e acesse:

**[http://localhost:5000](http://localhost:5000)**

Você verá a interface do Swagger, que documenta e permite testar todos os endpoints.

### Fluxo de Teste Recomendado:

1.  **Cadastro**: Vá em `POST /api/Auth/cadastro`, clique em "Try it out" e preencha os dados para criar um usuário.
    - **Resultado**: Você receberá um `token` JWT.

2.  **Autorização**: Clique no botão **"Authorize"** no topo da página. Na janela que abrir, cole o token JWT no formato `Bearer {seu_token}` e clique em "Authorize".

3.  **Acessar Endpoints Protegidos**: Agora você está autenticado!
    - Vá em `GET /api/Musicas` e clique em "Execute". Você receberá a lista de músicas.
    - Teste outros endpoints como `POST /api/Musicas` para criar uma nova música.

4.  **Acessar Endpoints Públicos**: Vá em `GET /api/Planos`. Ele funciona mesmo sem autenticação.

---

## 🔗 Integração com o Frontend Angular

O backend já está pronto para se comunicar com o frontend.

1.  **Verifique o CORS**: O `Program.cs` já permite requisições de `http://localhost:4200`.

2.  **URL da API no Angular**: No projeto Angular, atualize o arquivo `src/environments/environment.ts`:

    ```typescript
    export const environment = {
      production: false,
      apiUrl: 'http://localhost:5000/api' // URL do seu backend
    };
    ```

3.  **Execute os dois projetos**: Mantenha o backend rodando e inicie o frontend Angular (`npm start`). A aplicação Angular fará as chamadas para a API .NET e tudo funcionará de forma integrada.

---

## 🐛 Solução de Problemas (Troubleshooting)

- **Erro de conexão com o banco**: Verifique se o container Docker do MySQL está rodando (`docker ps`). Confira se a `ConnectionString` em `appsettings.json` está correta.

- **Porta 5000 já em uso**: Altere a porta no arquivo `Properties/launchSettings.json`.

- **Erro de CORS no navegador**: Verifique se a URL do frontend em `Program.cs` na política de CORS está correta. Limpe o cache do navegador.

- **Erro 401 Unauthorized**: Certifique-se de que você copiou o token JWT corretamente e o inseriu no Swagger com o prefixo `Bearer `.

---

## 📂 Estrutura do Projeto

```
/MusicStreamingAPI
├── docker-compose.yml       # Arquivo para iniciar o MySQL
├── README.md                # Este guia
└── /MusicStreamingAPI       # Pasta do projeto .NET
    ├── Controllers/         # Controladores da API (Musicas, Auth, Planos)
    ├── Data/                # DbContext para acesso ao banco
    ├── DTOs/                # Data Transfer Objects para API
    ├── Models/              # Entidades do banco de dados (Musica, Usuario, Plano)
    ├── Properties/          # Configurações de inicialização
    ├── appsettings.json     # Configurações da aplicação (JWT, Connection String)
    └── Program.cs           # Arquivo principal de configuração e inicialização
```
