# Gerenciador de Produtos

Este projeto é uma API para gerenciar produtos, categorias e usuários. A API permite a criação, leitura, atualização e exclusão de produtos e categorias, com autenticação baseada em token para garantir a segurança das rotas.

## Requisitos

- .NET 6.0 ou superior
- SQL Server
- Visual Studio
- Swagger para testar a API

## Compilando e Executando o Projeto

1. Abra o projeto no **Visual Studio** ou **Visual Studio Code**.
2. Compile o projeto para garantir que não há erros de construção.
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
- **`GET /api/Category/{Id}`** - Retorna uma categoria específica.

### Product 

- **`GET /api/Product`** - Retorna todos os produtos.
- **`POST /api/Product`** - Cria um novo produto. Requer autenticação.
- **`GET /api/Product/{Id}`** - Retorna um produto específico.
- **`PUT /api/Product/{Id}`** - Atualiza um produto existente. Requer autenticação.
- **`PATCH /api/Product/{Id}`** - Atualiza parcialmente um produto. Requer autenticação.
- **`DELETE /api/Product/{Id}`** - Deleta um produto. Requer autenticação.
- **`GET /api/Product/GetProductsByCategory`** - Retorna produtos por categoria.
- **`GET /api/Product/GetStock`** - Retorna produtos em estoque.

### User

- **`GET /api/User`** - Retorna todos os usuários. Requer autenticação.
- **`GET /api/User/{Id}`** - Retorna detalhes de um usuário. Requer autenticação.
- **`DELETE /api/User/{Id}`** - Deleta um usuário. Requer autenticação.
- **`POST /api/User/Login`** - Realiza login e retorna um token JWT.
- **`POST /api/User/SignUp`** - Cria um novo usuário.
- **`PATCH /api/User/{Id}/Role`** - Atualiza o cargo de um usuário. Requer autenticação.

## Observações

- O projeto é configurado para usar **SQL Server** e **Windows Authentication** para conectar ao banco de dados.
- Certifique-se de que o arquivo de banco de dados **DB_ProdutosGostaria.mdf** está acessível e corretamente conectado ao projeto.
- Caso o banco de dados não exista, ele será criado automaticamente durante a execução.

## Contribuindo

Contribuições são bem-vindas! Se você tiver sugestões ou melhorias para o projeto, sinta-se à vontade para abrir um **pull request** ou **issue**.

## Licença

Este projeto está licenciado sob a **MIT License** - veja o arquivo [LICENSE](LICENSE) para mais detalhes.
