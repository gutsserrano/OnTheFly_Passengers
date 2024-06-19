# OnTheFly

Neste repositório encontra-se duas APIs que fazem parte do Ecossistema do Aeroporto OnTheFly. Cada API possui os métodos CRUD necessários para o funcionamento deste módulo,
que visa controlar os passageiros e endereços cadastrados.

### Regras de Negócio

**API Passageiros**<br>
O aeroporto atende apenas pessoas físicas, sendo assim, não temos que nos preocupar em vender para pessoas jurídicas. Os passageiros menores de 18 anos podem ser cadastrados, mas não podem comprar nenhuma passagem. Os registros de restrição devem ter apenas as funções de incluir e remover o cadastro.

**API Endereço**<br>
Consulta a API do VIACEP, a partir do CEP, informando apenas o número e complemento.

# Documentação de Rotas

## Passageiros

### 1. Listar todos

- **Endpoint**: `GET /api/Passengers`
- **Descrição**: Retorna os passageiros cadastrados.
- **Parâmetros**: Nenhum.
- **Exemplo de Resposta**:
  ```json
  [
    {
        "cpf": "285.905.590-84",
        "name": "José",
        "gender": "M",
        "phone": "16123456789",
        "dtBirth": "2006-02-01T00:00:00",
        "dtRegister": "2024-06-19T14:23:18.4113539",
        "restricted": false,
        "address": {
            "zipCode": "12223-690",
            "number": "10",
            "street": "Rua Cidade de Assunção",
            "complement": "Casa",
            "city": "São José dos Campos",
            "state": "SP"
        }
    },
    {
        "cpf": "293.821.810-91",
        "name": "Luisa",
        "gender": "F",
        "phone": "169876543210",
        "dtBirth": "2002-02-01T00:00:00",
        "dtRegister": "2024-06-19T14:24:16.6002874",
        "restricted": false,
        "address": {
            "zipCode": "02310-190",
            "number": "15",
            "street": "Rua Francisco Thomaz Carbonelli",
            "complement": "Apto 10",
            "city": "São Paulo",
            "state": "SP"
        }
    },
    {
        "cpf": "750.088.170-35",
        "name": "Adalberto",
        "gender": "M",
        "phone": "1409876",
        "dtBirth": "2022-02-03T00:00:00",
        "dtRegister": "2024-06-18T15:53:17.8785236",
        "restricted": true,
        "address": {
            "zipCode": "14801-719",
            "number": "65",
            "street": "Rua Mahiba Barcha",
            "complement": "Casa A",
            "city": "Araraquara",
            "state": "SP"
        }
    }
  ]
  ```

### 2. Encontrar um passageiro

- **Endpoint**: `GET /api/Passengers/:cpf?deleted=:bool`
- **Descrição**: Retorna um passageiro com base no cpf. Na consulta deverá ser informado se deseja buscar dentre os excluidos com *deleted=true*.
- **Parâmetros**:
    - **cpf**: string
    - **deleted**: bool
- **Exemplo de Resposta**:
    ```json
  {
    "cpf": "293.821.810-91",
    "name": "Luisa",
    "gender": "F",
    "phone": "169876543210",
    "dtBirth": "2002-02-01T00:00:00",
    "dtRegister": "2024-06-19T14:24:16.6002874",
    "restricted": false,
    "address": {
        "zipCode": "02310-190",
        "number": "15",
        "street": "Rua Francisco Thomaz Carbonelli",
        "complement": "Apto 10",
        "city": "São Paulo",
        "state": "SP"
      }
    }
    ```

### 3. Cadastrar um passageiro

- **Endpoint**: `POST /api/Passengers`
- **Descrição**: Cadastra um novo passageiro.
- **Corpo da requisição**:
    ```json
    {
    "Cpf": "293.821.810-91",
    "Name": "Luisa",
    "Gender": "F",
    "Phone": "169876543210",
    "DtBirth": "2002-02-01",
    "AddressDTO": {
        "zipcode": "02310-190",
        "number": "15",
        "complement": "Apto 10"
      }
    }
    ```
- **Exemplo de Resposta**:
    ```json
    {
    "cpf": "293.821.810-91",
    "name": "Luisa",
    "gender": "F",
    "phone": "169876543210",
    "dtBirth": "2002-02-01T00:00:00",
    "dtRegister": "2024-06-19T14:24:16.6002874-03:00",
    "restricted": false,
    "address": {
        "zipCode": "02310-190",
        "number": "15",
        "street": "Rua Francisco Thomaz Carbonelli",
        "complement": "Apto 10",
        "city": "São Paulo",
        "state": "SP"
      }
    }
    ```

### 4. Editar um passageiro

- **Endpoint**: `PUT /api/Passengers/:cpf`
- **Descrição**: Edita um passageiro.
- **Parâmetros**:
    - **cpf**: string
- **Corpo da requisição**:
    ```json
    {
    "cpf": "293.821.810-91",
    "name": "Luisa",
    "gender": "F",
    "phone": "169876543210",
    "dtBirth": "2002-02-01T00:00:00",
    "restricted": true
    }
    ```
- **Exemplo de Resposta**:
    ```json
    {
    "cpf": "293.821.810-91",
    "name": "Luisa",
    "gender": "F",
    "phone": "169876543210",
    "dtBirth": "2002-02-01T00:00:00",
    "dtRegister": "2024-06-19T14:24:16.6002874",
    "restricted": true,
    "address": {
        "zipCode": "02310-190",
        "number": "15",
        "street": "Rua Francisco Thomaz Carbonelli",
        "complement": "Apto 10",
        "city": "São Paulo",
        "state": "SP"
      }
    }
    ```
    
### 4. Remover um passageiro

- **Endpoint**: `DELETE /api/Passengers/:cpf`
- **Descrição**: Transfere passageiro para a tabela de passageiros removidos.
- **Parâmetros**:
    - **cpf**: string
- **Exemplo de Resposta**:
    ```json
    sem respoosta
    ```

## Endereços

### 1. Listar todos

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
