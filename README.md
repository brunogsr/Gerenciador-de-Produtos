# Gerenciador de Produtos

Este projeto é uma API para gerenciar produtos, categorias e usuários. A API permite a criação, leitura, atualização e exclusão de produtos e categorias, com autenticação baseada em token para garantir a segurança das rotas.

## Requisitos

- .NET 6.0 ou superior
- SQL Server
- Visual Studio
- Swagger para testar a API

## Conectando ao Banco de Dados

O banco de dados SQL Server está localizado na raiz do projeto com o nome `DB_ProdutosGostaria.mdf`. Siga as instruções abaixo para configurar a conexão com o banco de dados:

1. Abra o Visual Studio e conecte-se ao banco de dados com o SQL Server, utilizando a **autenticação do Windows**.
2. Após conectar no banco de dados, clique com o botão direito sobre o banco de dados `DB_ProdutosGostaria.mdf` e selecione **Propriedades**.
3. Copie a **cadeia de conexão** (Connection String) exibida nas propriedades do banco de dados.
4. Dentro do projeto, abra o arquivo `Program.cs` e localize a linha 72.
5. Substitua a cadeia de conexão de exemplo pela cadeia de conexão que você copiou do SQL Server.

Exemplo de configuração no `Program.cs`:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer("Data Source=INSIRA_AQUI_A_CADEIA_DE_CONEXÃO_AO_BANCO_SQL;Integrated Security=True;Connect Timeout=30;Integrated Security=True;Connect Timeout=30");
});
```
## Compilando e Executando o Projeto
Abra o projeto no Visual Studio ou Visual Studio Code.
Compile o projeto para garantir que não há erros.
Execute o projeto. Após iniciar o servidor, você será redirecionado automaticamente para o Swagger, onde poderá testar todas as rotas da API.
Autenticação
A API usa autenticação baseada em token JWT. Para acessar as rotas protegidas, siga os passos abaixo:

No Swagger, acesse a rota POST /User/Login.
No corpo da requisição, envie as credenciais de login:
```csharp
{
  "email": "email@email.com",
  "password": "123456"
}
```
Se as credenciais forem válidas, você receberá um token JWT com o cargo de Gerente, permitindo o acesso a todas as rotas da API.
