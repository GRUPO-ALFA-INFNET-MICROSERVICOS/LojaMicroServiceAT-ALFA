<h1 align="center">Subscription service </h1>
<p align="center">Serviço que tem como responsabilidade toda a parte de assinatura da Pet Friends.
 </p>

<p align="center">
 <a href="#contexto-geral">Contexto geral</a> •
 <a href="#arvore-de-pastas">Árvore de pastas</a> •
 <a href="#objetivo">Objetivo</a> •
 <a href="#pre-requisitos">Pré-requisitos</a> •
 <a href="#rodando-a-api">Rodando a aplicação</a> •
 <a href="#logica-do-schedule">Lógica do schedule</a> 
</p>

## 📝 [Contexto geral](#contexto-geral)
O Subscription service nasceu como uma necessidade da Pet Friends com a intenção de permitir um serviço 
de assinaturas no negócio. Com base nisso, a Pet Friends seguiu com a seguinte estrutura:

<img width="663" alt="image" src="https://user-images.githubusercontent.com/43764175/207766798-04b4a2ad-4e63-4443-b092-714e3ba80caa.png">


Tendo os serviços de:
- Assinatura
- Pedido
- Pagamento
- Loja
- Produto
- Cliente

Cada serviço ficou com um desenvolvedor(a) responsável e também a comunicação entre os serviços foi gerenciada
pelas pessoas responsáveis pelo mesmo.

O serviço de assinatura "conversa" apenas com o serviço de pedidos a partir de um serviço de mensageria, onde 
a assinatura posta uma mensagem nessa fila e o pedido consome essa mensagem e cria de fato um pedido na Pet
Friends.

**Nesta documentação focaremos exclusivamente no serviço de assinatura.**

## 🌳 [Árvore de pastas](#arvore-de-pastas)
```
├── README.md
├── app
│   ├── api
│   │   └── __init__.py
│   │   └── request_model.py
│   │   └── routes.py
│   ├── domain
│   │   ├── entities
│   │       ├── __init__.py
│   │       ├── subscription.py
│   │       └── subscription_item.py
│   │   ├── enum
│   │       ├── __init__.py
│   │       ├── renew_type.py
│   │       └── status_type.py
│   │   ├── value_objects
│   │   └── __init__.py
│   ├── infrastructure
│   │   ├── mapping
│   │       ├── __init__.py
│   │       └── subscription_mapping.py
│   │   ├── models
│   │       ├── __init__.py
│   │       └── subscription_model.py
│   │   ├── repositories
│   │       ├── __init__.py
│   │       └── subscription_repository.py
│   │   └── __init__.py
│   ├── worker
│   │   ├── __init__.py
│   │   └── schedule.py
│   ├── config.py
│   ├── main.py
│   └── producer.py
├── mongodb
├── rabbitmq
├── .gitignore
├── Dockerfile
├── README.md
└── requirements.txt
    
```

## 📎 [Objetivo](#objetivo)
Esta aplicação tem como principal objetivo disponibilizar todo o contexto referente a assinatura de um produto. 
Além disso, também conta com uma rotina responsável por verificar se o prazo de uma assinatura já expirou, para 
poder renovar automaticamente.

Responsabilidades do serviço de assinatura:
- Criar uma assinatura a partir de um id do usuário;
- Editar status da assinatura;
- Listar todas as assinaturas do usuário;

Além disso, o serviço de assinatura tem um schedule, que tem como principal função verificar se uma assinatura
precisa ou não ser renovada.

## ✅ [Pré-requisitos](#pre-requisitos)
Nessa aplicação foram utilizadas algumas tecnologias, cada uma delas teve uma importância significativa
no projeto, sendo elas:

- [**Python**](https://www.python.org/downloads/): linguagem de programação utilizada para escrever toda a aplicação. Esta linguagem foi escolhida
principalmente pelo fato de ser uma linguagem que tenho bastante familiaridade;


- [**FastAPI**](https://fastapi.tiangolo.com/): framework Python que tem como intuito melhorar a perfomance da aplicação, obviamente o Python 
de longe não é uma das linguagens mais performáticas, mas fazendo o benchmark do FastAPI com outros 
frameworks de Python é notável a sua velocidade;


- [**Docker**](https://www.docker.com/): utilizado para colocar a aplicação em um container e conseguir subir pra nuvem;


- [**MongoDB**](https://www.mongodb.com/home): banco de dados escolhido para a aplicação. O MongoDB é um banco que nunca trabalhei, então tive
que aprender como funcionava e todas as suas particularidades. Acredito que tenha atendido super bem o desafio
que essa aplicação traz.


- [**RabbitMQ**](https://www.rabbitmq.com/): serviço de mensageria utilizado pela aplicação, foi utilizada
pois o professor Felipe ensinou sobre o seu funcionamento e facilidade de implementação.

Logo, para conseguir rodar com êxito essa aplicação, é necessário ter todas as ferramentas.

## 🎲 [Rodando a aplicação](#rodando-a-api)

### Rodando localmente
```bash
# Primeiramente é necessário clonar a aplicação no github:
$ git clone git@github.com:GRUPO-ALFA-INFNET-MICROSERVICOS/subscription_service.git

# Acesse a pasta do projeto no terminal
$ cd subscription_service

# Após isso, é importante ter uma venv dentro da sua pasta da aplicação
$ python -m venv venv 

# Ativar a venv
$ source venv/bin/activate

# Instalar todas as dependências da aplicação
$ pip install -r requirements.txt

# Rode a aplicação com o seguinte comando
$ uvicorn app.main:app --reload
```


### Docker
```bash
# Para rodar a API usando Docker é necessário primeiro realizar o build da imagem com o seguinte comando:
$ docker build -t subscription_service .

# Após realizado o build, para executar a imagem basta rodar o seguinte comando:
$ docker run -d --name subs_service -p 8000:8000 subscription_service
```

**Por conta de ter um workflow e uma pipeline sendo rodada pelo GitHub Actions, quando essa aplicação é 
atualizada uma imagem do docker é criada no Docker Hub. Você consegue ver as versões da imagem 
[aqui](https://hub.docker.com/repository/docker/mayaralima22/subscription_service).**


## 🕗 [Lógica do Schedule](#logica-do-schedule)

O serviço de assinatura tem um schedule, que tem como principal função verificar se uma assinatura
precisa ser renovada ou não. Para isso foi utilizada o método weekday da lib de datetime do Python, onde 
o número inteiro representa o seguinte dia da semana:
- 0: segunda
- 1: terça
- 2: quarta
- 3: quinta
- 4: sexta
- 5: sábado
- 6: domingo

Com isso, verificamos a partir do `renew` se o tipo de assinatura é mensal ou semanal.

Para `renew` do tipo semanal, utilizamos a seguinte lógica:

Utilizamos o campo `renew_at`, que é um inteiro, para verificar se o dia da renovação da assinatura é o dia
de hoje, para isso utilizamos o método de weekday().
- Caso seja o dia de hoje: ele gera uma mensagem pra fila de subscription_created e o serviço de pedido 
consome essa mensagem, gerando um pedido em si;
- Caso não seja, ele apenas continua verificando se as outras assinaturas precisam ser renovadas.

Já para `renew` do tipo monthly o schedule apenas pega o dia de hoje e verifica se a data corresponde, como o
schedule só roda uma vez ao dia, não há problema de duplicação de pedidos.

**O schedule roda apenas uma vez ao dia, fazendo as verificações necessárias.**

