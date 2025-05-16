# Gerenciador de Produtos

Este projeto é uma API para gerenciar produtos, categorias e usuários. A API permite a criação, leitura, atualização e exclusão de produtos, categorias e usuários do Banco de Dados, com autenticação baseada em token para garantir a segurança das rotas.

## Requisitos

- .NET 8.0 ou superior
- SQL Server
- Visual Studio
- Swagger para testar a API

## Compilando e Executando o Projeto

1. Abra o projeto no **Visual Studio**.
2. Clique em Recompilar com o botão direito no arquivo GerenciadorProdutos.csproj.
3. Após a compilação bem-sucedida, execute o projeto. O servidor será iniciado e você será redirecionado automaticamente para a interface do **Swagger**, onde poderá testar todas as rotas da API.

## Autenticação

A API utiliza **autenticação baseada em token JWT** para acessar as rotas protegidas. Para obter o token, siga os seguintes passos:

1. Acesse a rota `POST /User/Login` no Swagger.
2. Envie as credenciais de login no corpo da requisição:

    ```json
    {
      "email": "email@email.com",
      "password": "123456"
    }
    ```

3. Se as credenciais forem válidas, a API retornará um **token JWT** com o cargo de **Gerente**, permitindo o acesso a todas as rotas protegidas da API.
4. Para acessar as rotas que exigem autenticação, inclua o token JWT no cabeçalho da requisição, no campo `Authorization` com o valor `Bearer {token}`.

    Exemplo de cabeçalho com o token JWT:

    ```
    Authorization: Bearer {seu_token_aqui}
    ```

Esse token será necessário para interagir com as rotas que exigem permissões de **Gerente**.

## Testando as Rotas

### Category

- **`GET /api/Category`** - Retorna todas as categorias.
- **`POST /api/Category`** - Cria uma nova categoria. Requer autenticação.

  Body válido:
      ```json
      {
      "categoria": "Material escolar"
      }
      ```
- **`GET /api/Category/{Id}`** - Retorna uma categoria específica.

### Product 

- **`GET /api/Product`** - Retorna todos os produtos.
- **`POST /api/Product`** - Cria um novo produto. Requer autenticação.

  Produto novo válido:
     ```json
      {
        "nome": "Seiya de Pegaso action figure",
        "descricao": "Bonecos colecionáveis",
        "status": "Em estoque",
        "preco": 50,
        "quantidadeEstoque": 10,
        "categoryId": 4
      }
     ```     
- **`GET /api/Product/{Id}`** - Retorna um produto específico.
- **`PUT /api/Product/{Id}`** - Atualiza um produto existente. Requer autenticação.

  Id = 14 Válido para o teste:
    ```json
    {
      "nome": "Vegeta action figure",
      "descricao": "Bonecos colecionáveis",
      "status": "Em estoque",
      "preco": 50,
      "quantidadeEstoque": 3,
      "categoryId": 4
    }
    ```

- **`PATCH /api/Product/{Id}`** - Atualiza parcialmente um produto. Aceita apenas status "Em estoque" ou "Indisponível". Requer autenticação.

  Body válido:
      ```json
      {
        "status": "Em estoque",
        "quantidadeEstoque": 99
      }
      ```
- **`DELETE /api/Product/{Id}`** - Deleta um produto. Requer autenticação.
- **`GET /api/Product/GetProductsByCategory`** - Retorna produtos por categoria.
- **`GET /api/Product/GetStock`** - Retorna produtos em estoque.

### User

- **`GET /api/User`** - Retorna todos os usuários. Requer autenticação.
- **`GET /api/User/{Id}`** - Retorna detalhes de um usuário. Requer autenticação.
- **`DELETE /api/User/{Id}`** - Deleta um usuário. Requer autenticação.
- **`POST /api/User/Login`** - Realiza login e retorna um token JWT.
  
  Conta com autenticação:
    ```json
    {
      "email": "email@email.com",
      "password": "123456"
    }
    ```
    
- **`POST /api/User/SignUp`** - Cria um novo usuário com cargo de Cliente de forma padrão.

  Conta sem falhas de autenticação:
  ```json
    {
      "name": "Andréa",
      "email": "andrea@email.com",
      "password": "123456"
    }
    ```
- **`PATCH /api/User/{Id}/Role`** - Atualiza o cargo de um usuário. Aceita apenas "Cliente", "Funcionário" ou "Gerente". Requer autenticação.
  
  Body válido:
    ```json
    {
      "role": " Gerente"
    }
    ```
