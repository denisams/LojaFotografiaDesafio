# LojaFotografiaDesafio

Bem-vindo ao repositório **LojaFotografiaDesafio**! Este projeto é uma aplicação UWP (Universal Windows Platform) para gerenciar uma loja de fotografia, incluindo câmeras e acessórios. Abaixo está um guia detalhado sobre como configurar e utilizar o projeto. O Projeto tem 2 aplicações de Autenticação e Catálogo, ambas com SWAGGER para teste do back-end, tem cache dos produtos em REDIS e armazenamento no MongoDB, conforme pedido de banco NOSql. Esses itens, MongoDB e e REDIS estão em container e podem ser ativados com o docker-compose, sendo necessário apenas a máquina que vai rodar a aplicação ter o docker-desktop.

### * OBS: Feito por Back-end. A parte de front, não é minha especialidade, mas foi um desafio interessante pra primeira vez utilizando UWP.

## Sumário

1. [Requisitos](#requisitos)
2. [Instalação](#instalação)
3. [Configuração](#configuração)
4. [Uso](#uso)
   - [Login](#login)
   - [Gerenciar Câmeras](#gerenciar-câmeras)
   - [Gerenciar Acessórios](#gerenciar-acessórios)
5. [Ambiente de Desenvolvimento](#ambiente-de-desenvolvimento)
   - [Docker Compose](#docker-compose)


## Requisitos

Antes de começar, certifique-se de ter os seguintes requisitos atendidos:

- **Visual Studio 2019 ou 2022** com as ferramentas UWP instaladas.
- **Windows 10** ou superior.
- **SDK Windows 10** versão 10.0.18362.0 ou superior.
- Docker-Desktop(Para os containers de MongoDB e REDIS)

## Instalação

Para clonar e configurar o projeto localmente, siga os passos abaixo:

1. Clone o repositório:
   ```sh
   git clone https://github.com/denisams/LojaFotografiaDesafio.git
   ```
2. Abra o projeto no **Visual Studio**:
   - Navegue até o diretório onde o repositório foi clonado e abra o arquivo `.sln`.

## Configuração

Antes de executar a aplicação, algumas configurações precisam ser feitas:

1. Configure a URL base da API:
   - Abra o arquivo `App.config` ou `appsettings.json` (dependendo da configuração do projeto).
   - Atualize a chave `BaseUrl` com a URL da sua API.

2. Verifique as dependências:
   - Certifique-se de que todas as dependências do projeto estão corretamente instaladas através do NuGet.

## Uso

### Login 
## (Será necessário para Adicionar, Atualizar ou Apagar Câmeras e Acessórios.  A Listagem dos mesmos não tem necessidade de Login)

Para acessar as funcionalidades do sistema, você precisa realizar o login: 


1. Execute a aplicação.
2. Navegue até a página de login.
3. Insira suas credenciais (usuário e senha) e clique no botão "Login".
   - Quando terminar de criar o banco, não terá usuário.  Pelo Swagger do Auth, eu deixei o endpoint para que possa fazer o cadastro de usuários.    
5. Caso as credenciais estejam corretas, você será redirecionado para a página principal com uma mensagem de boas-vindas.

### Gerenciar Câmeras

Para gerenciar câmeras:

1. Navegue até a seção "Gerenciar Câmeras".
2. Você verá uma lista de câmeras cadastradas.
3. As ações disponíveis incluem: (Necessário Login para os itens abaixo)
   - **Adicionar**: Clique no botão "Adicionar Câmera" e preencha o formulário.
   - **Editar**: Selecione uma câmera e clique no ícone de edição. Faça as alterações necessárias e salve.
   - **Excluir**: Selecione uma câmera e clique no ícone de exclusão.

### Gerenciar Acessórios

Para gerenciar acessórios:

1. Navegue até a seção "Gerenciar Acessórios".
2. Você verá uma lista de acessórios cadastrados.
3. As ações disponíveis incluem: (Necessário Login para os itens abaixo)
   - **Adicionar**: Clique no botão "Adicionar Acessório" e preencha o formulário.
   - **Editar**: Selecione um acessório e clique no ícone de edição. Faça as alterações necessárias e salve.
   - **Excluir**: Selecione um acessório e clique no ícone de exclusão.

## Ambiente de Desenvolvimento

Para facilitar o gerenciamento dos serviços de banco de dados e cache, utilizamos o Docker Compose para configurar e iniciar o MongoDB e o Redis. Siga os passos abaixo para configurar o ambiente:

1. Certifique-se de que o Docker está instalado e em execução no seu sistema.
2. Navegue até o diretório onde o arquivo `docker-compose.yml` está localizado.
3. Execute o comando `docker-compose up -d` para iniciar os serviços.
4. Verifique se os containers estão em execução com o comando `docker-compose ps`.

Para parar e remover os containers, use o comando:
```sh
docker-compose down
```

---

Obrigado pelo desafio e utilizar **LojaFotografiaDesafio**! Se tiver dúvidas ou encontrar problemas, sinta-se à vontade para abrir uma issue no repositório.
