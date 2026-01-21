# Control Expenses API

API para controle de gastos residenciais, desenvolvida como teste técnico.

O sistema permite o cadastro de pessoas, categorias e transações financeiras,
além da consulta de totais por pessoa, respeitando as regras de negócio definidas
no enunciado.

---

## Tecnologias Utilizadas

- ASP.NET Core (.NET)
- C#
- Entity Framework Core
- SQLite
- Swagger / OpenAPI

---

## Estrutura do Projeto

- **Models**: Entidades de domínio (Pessoa, Categoria, Transacao)
- **Enums**: Estados fixos do domínio (TipoTransacao, FinalidadeCategoria)
- **Data**: Configuração do Entity Framework (DbContext)
- **Dtos**: Objetos de entrada e saída da API
- **Controllers**: Endpoints da aplicação

---

## Regras de Negócio Implementadas

- Pessoas possuem nome obrigatório e idade maior que zero
- Categorias possuem descrição obrigatória e finalidade definida
- Transações:
  - Valor deve ser positivo
  - Menores de idade só podem registrar despesas
  - Categorias só podem ser usadas conforme sua finalidade
- Ao excluir uma pessoa, todas as suas transações são removidas automaticamente
- Consulta de totais por pessoa com cálculo de receitas, despesas e saldo

---

## Banco de Dados

O projeto utiliza **SQLite** para persistência.

Para facilitar a avaliação do teste técnico, o arquivo do banco de dados já está
versionado no repositório, permitindo executar a aplicação sem a necessidade de
rodar migrations ou configurações adicionais.

Em um cenário de produção, o banco não seria versionado e seria criado via
migrations do Entity Framework Core.

---

## Como Executar o Projeto

1. Clonar o repositório
2. Abrir a solução no Visual Studio
3. Executar o projeto (`F5` ou botão Run)
4. Acessar o Swagger para testar os endpoints

---

## Observações

- O projeto foi estruturado com foco em clareza, organização e aderência às
  regras de negócio.
- Recursos adicionais não foram implementados para manter fidelidade ao escopo
  do teste técnico.
