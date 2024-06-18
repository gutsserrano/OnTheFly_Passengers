# Documentação de Rotas - OnTheFly

## Endereços

### 1. Index

- **Endpoint**: `GET /api/addresses`
- **Descrição**: Retorna os endereços cadastrados.
- **Parâmetros**: Nenhum.
- **Exemplo de Resposta**:
  ```json
    {
        "take": 10,
        "page": 1,
        "total": 1,
        "pages": 1,
        "nextPage": 1,
        "previousPage": 1,
        "hasNextPage": false,
        "hasPreviousPage": false,
        "data": [
            {
                "id": 1,
                "name": "Luis",
                "surname": "Brandino",
                "identification_number": "000.000.000-01",
                "city": "Mat├úo",
                "phone": "(16) 99700-2606",
                "cep": "12345678",
                "address": "R. Das Casas",
                "building_number": "1",
                "email": "luis@brasilux.com.br",
                "reference": "Brasilux"
            }
        ]
    }
  ```
  
### 2. Criar um novo cliente

- **Endpoint**: `POST /client`
- **Descrição**: Cria um novo cliente no sistema.
- **Corpo da requisição**:
    ```json
    {
        "name": "Lúcia Rosa Eduarda"
        "surname": "Araújo"
        "identification_number": "773.998.379-96" // CPF ou CNPJ
        "city": "São José dos Campos"
        "cep": "12228-500"
        "address": "Rua H19B"
        "building_number": "292"
        "email": "lucia-araujo86@regional.com.br"
        "phone": "12986818734"
        "reference": "Próximo ao cemitério" // Opcional
    }
    ```
      
### 3. Alterar um cliente

- **Endpoint**: `POST /client/:id/edit`
- **Descrição**: Atualiza um cliente existente com base no ID fornecido.
- **Parâmetros**: ID do cliente a ser atualizado na URL.
- **Corpo da requisição**:
    ```json
    {
        "surname": "Araújo dos Santos" // Enviado somente os campos a serem modificados
    }
    ```
