# 📋 Desafio Assino - Gerenciador de Tarefas

## Objetivo do Teste
Avaliar pensamento sistêmico, arquitetura, boas práticas, qualidade de código, experiência com APIs e front-end, e capacidade de comunicação.

## Descrição do Desafio (versão moderna)
Uma startup deseja lançar um sistema de gerenciamento de tarefas com SLA. Seu objetivo é entregar uma solução funcional que permita:

1. Criar uma tarefa com: 
    * Titulo
    * SLA (horas)
    * Upload de arquivo
    * Upload de 1 arquivo
2. Listar tarefas, com filtro por concluídas
3. Notificação automática quando SLA expirar
4. API REST documentada com Swagger - Utilizando .NET
5. Front-end em React ou Angular
6. Layout simples, mas com boa usabilidade

## O que é esperado para construção do teste
**Arquitetura**: Clean Architecture, DDD light, SOLID

**Banco de dados**:
- Queries performáticas
- Se utilização de Banco de Dados Relacional, relacionamentos claros e coesos
- Se utilização de Banco de Dados Não Relacional, separação de responsabilidade 

**Testes**: Unitários e integração

---

## 🏗️ Arquitetura Utilizada

O projeto foi desenvolvido seguindo rigorosamente os princípios da **Clean Architecture** (Arquitetura Limpa) e **DDD Light** (Domain-Driven Design simplificado) no Backend.

### Backend (.NET 10)
A solução foi dividida em camadas concêntricas para garantir a separação de responsabilidades, testabilidade e independência de frameworks:

1.  **Domain (Core)**:
    *   Contém as **Entidades**, **Enums** e **Exceptions** de domínio.
    *   É o núcleo da aplicação, totalmente agnóstico e sem dependências externas.
    *   As entidades são ricas, contendo comportamentos e validações de negócio (não são anêmicas).

2.  **Application**:
    *   Contém os **Serviços de Aplicação**, **Interfaces** e **DTOs**.
    *   Orquestra o fluxo de dados e aplica validações usando **FluentValidation**.
    *   Define contratos que a infraestrutura deve implementar.

3.  **Infrastructure**:
    *   Implementa as interfaces definidas na Application.
    *   Contém o **Entity Framework Core**, **Repositórios** e serviços externos como **FileStorage** e **Notificação**.
    *   Responsável pelo acesso a dados e comunicação com o mundo externo.

4.  **API (Presentation)**:
    *   Camada de entrada (**Controllers**), responsável apenas por receber requisições HTTP, configurar a Injeção de Dependência e expor os endpoints via **Swagger**.
    *   Utiliza **Middlewares** para tratamento global de erros.

### Frontend (Angular 17+)

*   **Modular Architecture**: Optou-se pelo uso de NgModules (em vez de Standalone Components puros) para melhor organização e separação de responsabilidades, facilitando a escalabilidade e a manutenção por desenvolvedores familiarizados com padrões corporativos.
*   **Component-Based**: Separação clara entre lógica (.ts), visual (.html) e estilo (.scss).
---

## 🧩 Design Patterns

Os seguintes padrões de projeto foram aplicados para resolver problemas comuns de forma eficiente e elegante:

*   **Dependency Injection (DI)**:
    *   Fundamental no .NET para desacoplar classes concretas. Permite trocar implementações (ex: trocar SQL Server por SQLite em testes) sem alterar o código da aplicação.

*   **Repository Pattern**:
    *   Abstrai a lógica de acesso a dados. O serviço de aplicação não sabe se os dados vêm do SQL Server ou de um arquivo, ele apenas chama `ITarefaRepository.AddAsync`.

*   **Service Pattern**:
    *   Encapsula a lógica de negócio complexa (como expiração de SLA e orquestração de upload), mantendo os Controllers "magros" e focados apenas em HTTP.

*   **Background Service (Hosted Service)**:
    *   Utilizado para o job de expiração de SLA (`SlaExpirationBackgroundService`). Permite processamento assíncrono em segundo plano sem bloquear a API.

*   **Strategy / Adapter (implícito)**:
    *   Na implementação do `IFileStorageService`, permitindo facilmente trocar o armazenamento local por S3 ou Azure Blob Storage no futuro.
  
* **DTO (Data Transfer Object)**:
    *   Utilizado para separar o modelo de domínio do modelo de apresentação. Exemplo: (`CriarTarefaForm`) para receber dados complexos com arquivos via multipart/form-data e (`TarefaResponse`) para formatar a saída para o cliente.

* **Observer Pattern**:
    *   Utilizado no Frontend através do RxJS. Permite que a interface reaja assincronamente às respostas da API e mudanças de estado sem bloquear a experiência do usuário.

---

## 📦 Bibliotecas e Pacotes

Abaixo, as principais bibliotecas externas e a motivação para sua escolha:

### Backend
*   **Entity Framework Core 10 (SQL Server & SQLite)**:
    *   *Motivação:* ORM robusto que aumenta a produtividade, protege contra SQL Injection e facilita a troca de bancos de dados (usamos SQLite In-Memory para testes de integração).
*   **FluentValidation**:
    *   *Motivação:* Separa as regras de validação das entidades e DTOs, permitindo validações complexas e encadeadas de forma legível e testável.
*   **Swashbuckle (Swagger)**:
    *   *Motivação:* Gera documentação viva e interativa da API, essencial para facilitar o consumo pelo Frontend e testes manuais.
*   **Microsoft.AspNetCore.Mvc.Testing**:
    *   *Motivação:* Permite criar testes de integração end-to-end, subindo a API em memória para validar o fluxo completo (Controller → Service → Banco).
*   **Moq & FluentAssertions**:
    *   *Motivação:* Essenciais para testes unitários legíveis e expressivos, permitindo simular comportamentos e validar resultados de forma fluida.

### Frontend
*   **Angular Material**:
    *   *Motivação:* Consistência visual e UX. Fornece componentes robustos (Table, Card, Toolbar, FormField) prontos e acessíveis, acelerando drasticamente o desenvolvimento da interface seguindo as diretrizes do Material Design.

*   **RxJS**:
    *   *Motivação:* Essencial para lidar com programação reativa e chamadas assíncronas HTTP, permitindo manipular fluxos de dados complexos de forma elegante.

---

## 🚀 Maiores Desafios do Teste

Durante o desenvolvimento, os pontos de maior complexidade e aprendizado foram:

1.  **Testes de Integração com Banco Relacional (SQLite In-Memory)**:
    *   Configurar o ambiente de teste para substituir o SQL Server pelo SQLite In-Memory sem conflitos de injeção de dependência foi desafiador. Foi necessário criar uma `CustomWebApplicationFactory` robusta para gerenciar o ciclo de vida da conexão e garantir o isolamento entre testes.

2.  **Gerenciamento de Upload de Arquivos (Multipart/Form-Data)**:
    *   Integrar o recebimento de arquivos via `IFormFile` na API mantendo a arquitetura limpa (sem sujar a camada de Application com dependências HTTP) exigiu a criação de ViewModels específicos (`CriarTarefaForm`) na camada de API.

3.  **Lógica de Expiração de SLA**:
    *   Implementar um serviço em background que monitora e expira tarefas automaticamente exigiu cuidado com concorrência e precisão nas datas (`DateTime.UtcNow`), além de garantir que a lógica fosse testável por meio de endpoints de simulação ("viagem no tempo").

---

## ▶️ Como Rodar o Projeto

### Pré-requisitos
*   .NET 10 SDK
*   Node.js
*   SQL Server

### Backend
1.  Navegue até a pasta `backend/src/DesafioAssino.Api`.
2.  Configure a string de conexão no `appsettings.json` (ou use o padrão local).
3.  Execute as migrações (se houver) ou deixe o `EnsureCreated` rodar.
4.  Execute:
    ```bash
    dotnet run
    ```
5.  Acesse o Swagger em: `https://localhost:7200/index.html`.

### Frontend
1.  Navegue até a pasta `frontend/`.
2.  Instale as dependências com:
    ```bash
    npm install
    ```
3.  Inicie com:
    ```bash
    npm start
    ```
4.  Acesse `http://localhost:4200/tarefas` no seu navegador

### Testes
Para rodar a suíte completa de testes (Unitários e Integração):
```bash
dotnet test
```