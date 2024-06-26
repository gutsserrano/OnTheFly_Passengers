# Documentação de Rotas - OnTheFly

## Endereços

### 1. Index

- **Endpoint**: `GET /api/addresses`
- **Descrição**: Retorna os endereços cadastrados.
- **Parâmetros**: Nenhum.
- **Exemplo de Resposta**:
  ```json
    [
        {
            "zipcode": "15990-540",
            "number": "5",
            "street": "Avenida Trolesi",
            "complement": "Casa A",
            "city": "Matão",
            "state": "SP"
        },
        { 
            "zipcode": "15990-640",
            "number": "5",
            "street": "Avenida Siqueira Campos",
            "complement": "Casa A",
            "city": "Matão",
            "state": "SP"
        },
        {
            "zipcode": "15990-740",
            "number": "5",
            "street": "Avenida José Gonçalves",
            "complement": "Casa A",
            "city": "Matão",
            "state": "SP"
        }
    ]
  ```
  
### 2. Encontrar um endereço

- **Endpoint**: `GET /api/addresses/zipcode/:zipcode/number/:number`
- **Descrição**: Retorna o endereço com base no código postal e número residencial, se cadastrado.
- **Parâmetros**:
    - **zipcode**: string
    - **number**: string
- **Exemplo de Resposta**:
    ```json
    {
        "zipcode": "15990-540",
        "number": "5",
        "street": "Avenida Trolesi",
        "complement": "Casa A",
        "city": "Matão",
        "state": "SP"
    }
    ```
      
### 3. Cadastrar um endereço

- **Endpoint**: `POST /api/addresses`
- **Descrição**: Cadastra um novo endereço com base no código postal e número.
- **Corpo da requisição**:
    ```json
    {
        "zipcode": "15990-820",
        "number": "156",
        "complement": "Casa B" // Opcional
    }
    ```
- **Exemplo de Resposta**:
    ```json
    {
        "zipcode": "15990-820",
        "number": "156",
        "street": "Avenida Jornalista José da Costa Filho",
        "complement": "Casa B",
        "city": "Matão",
        "state": "SP"
    }
    ```
