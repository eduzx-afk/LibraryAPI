# LibraryAPI

Projeto acadêmico desenvolvido para a disciplina de **Algoritmos e Estrutura de Dados II**, da Faculdade UniSapiens.

## Sobre o projeto

A LibraryAPI é uma API REST desenvolvida para simular o gerenciamento de uma biblioteca, permitindo o cadastro e gerenciamento de livros, autores, categorias, usuários e empréstimos.

O sistema também implementa algoritmos de busca, ordenação e seleção aplicados ao gerenciamento do acervo, permitindo localizar e organizar livros por diferentes critérios.

## Objetivo

Aplicar na prática os conceitos de algoritmos e estruturas de dados estudados na disciplina, especialmente os algoritmos de busca, ordenação e seleção, integrando-os a uma aplicação funcional.

Além disso, o projeto busca explorar conceitos de desenvolvimento de APIs, persistência de dados, organização em camadas e programação orientada a objetos.

## Algoritmos implementados

- **Busca Linear (Linear Search):** percorre os elementos até encontrar o livro desejado.
- **Busca Binária (Binary Search):** realiza buscas em uma coleção previamente ordenada.
- **Merge Sort:** organiza os livros de acordo com critérios definidos.
- **QuickSelect:** encontra o elemento que ocupa determinada posição em uma coleção, sem precisar ordená-la por completo.

## Tecnologias e ferramentas

- **C#** — linguagem de programação.
- **.NET / ASP.NET Core** — desenvolvimento da API REST.
- **Entity Framework Core** — mapeamento objeto-relacional (ORM).
- **SQLite** — banco de dados.
- **Swagger / OpenAPI** — documentação e testes dos endpoints.
- **Git e GitHub** — versionamento e hospedagem do código.

## Como executar o projeto

Certifique-se de ter instalado:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/downloads)

```bash
git clone https://github.com/eduzx-afk/LibraryAPI.git
cd LibraryAPI
dotnet restore
dotnet tool install --global dotnet-ef
dotnet ef database update
dotnet run
```

## Critérios de ordenação e seleção

Os algoritmos de ordenação e seleção utilizam o enum `BookSortCriteria` para determinar o critério aplicado aos livros.

Os valores numéricos correspondentes são:

| Valor | Critério | Descrição |
|---:|---|---|
| `0` | `Title` | Título do livro |
| `1` | `PublicationYear` | Ano de publicação |
| `2` | `ISBN` | ISBN do livro |

Esses valores podem ser utilizados nos parâmetros dos endpoints que recebem o critério de ordenação ou seleção.

Por exemplo:

```http
GET /api/algorithm/sort?criteria=0
```

O exemplo acima solicita a ordenação dos livros pelo título.

Para o QuickSelect, também é necessário informar a posição `k` desejada:

```http
GET /api/algorithm/selection/quickselect?k=2&criteria=1
```

Nesse caso, o algoritmo retorna o terceiro livro considerando a ordem crescente do ano de publicação. **A posição `k` começa em zero.**

## Integrantes

- Eduardo Andrade
- Gabriel Galvão
- Kassiane

**Professora:** Rosângela Cunha

**Instituição:** Faculdade UniSapiens

**Disciplina:** Algoritmos e Estrutura de Dados II
