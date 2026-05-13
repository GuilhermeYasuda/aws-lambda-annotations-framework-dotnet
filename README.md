# AWS Lambda Annotations Framework com .NET

Este repositório demonstra a aplicação prática do framework oficial de anotações da AWS ([Amazon.Lambda.Annotations](https://www.nuget.org/packages/Amazon.Lambda.Annotations)) para o desenvolvimento serverless utilizando **C#** e **.NET**. 

O objetivo principal é criar funções Lambda mais limpas, eliminando códigos repetitivos (*boilerplate*) através de geradores de código em tempo de compilação (*Source Generators*), aproximando a experiência de desenvolvimento ao modelo tradicional do ASP.NET Core.

---

## 🚀 Funcionalidades Demonstradas

*   **Anotações Nativas (`[LambdaFunction]`):** Simplifica a declaração de handlers sem poluição visual no código.
*   **Injeção de Dependência Nativa (`[LambdaStartup]`):** Configuração de serviços de forma idêntica ao ecossistema do ASP.NET Core utilizando `IServiceCollection`.
*   **Sincronização Automática com CloudFormation:** Geração automática e atualização do arquivo `template.yaml` durante o build do projeto.
*   **Integração com DynamoDB:** Exemplos de operações de persistência utilizando o SDK da AWS para .NET de forma desacoplada.
*   **Roteamento para API Gateway:** Vinculação direta de rotas HTTP e REST HTTP (`[HttpApi]` e `[RestApi]`) aos parâmetros dos métodos.

---

## 🛠️ Tecnologias Utilizadas

*   **Linguagem:** C# (.NET Core)
*   **Framework AWS:** Amazon.Lambda.Annotations
*   **Banco de Dados:** Amazon DynamoDB
*   **Infraestrutura:** AWS CloudFormation / SAM (Serverless Application Model)

---

## 💻 Estrutura do Código

Abaixo está um exemplo conceitual de como as funções estão estruturadas utilizando o framework:

### 1. Configuração da Injeção de Dependência (Startup)
```csharp
using Microsoft.Extensions.DependencyInjection;
using Amazon.Lambda.Annotations;

[LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Registro de serviços e clientes AWS (ex: DynamoDB)
        services.AddAWSService<Amazon.DynamoDBv2.IAmazonDynamoDB>();
        services.AddSingleton<IOrderRepository, OrderRepository>();
    }
}
```

### 2. Definição da Função Lambda (Handler)
```csharp
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;

public class OrderFunctions
{
    private readonly IOrderRepository _repository;

    public OrderFunctions(IOrderRepository repository)
    {
        _repository = repository;
    }

    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Post, "/orders")]
    public async Task<IHttpResult> CreateOrder([FromBody] OrderRequest request)
    {
        await _repository.SaveAsync(request);
        return HttpResults.Ok("Pedido criado com sucesso!");
    }
}
```

---

## 📦 Como Executar e Clonar o Projeto

### Pré-requisitos
*   [.NET SDK](https://microsoft.com) instalado (versão 6.0 ou superior).
*   [AWS SAM CLI](https://amazon.com) instalado para deploy local/nuvem.
*   Ferramenta de testes [AWS Mock Lambda Test Tool](https://github.com) (opcional).

## 🚀 Como Utilizar
1.  **Pré-requisitos:** .NET SDK e AWS SAM CLI instalados.
2.  **Build:** Execute `sam build` para gerar o template de infraestrutura automaticamente.
3.  **Deploy:** Utilize `sam deploy --guided`.
4.  **Limpeza:** Remova os recursos com `sam delete`.

### Passo a Passo

1. Clonar o repositório:
   ```bash
   git clone https://github.com/GuilhermeYasuda/aws-lambda-annotations-framework-dotnet.git
   cd aws-lambda-annotations-framework-dotnet
   ```

2. Restaurar as dependências do NuGet:
   ```bash
   dotnet restore
   ```

3. Compilar a aplicação (isso irá gerar/atualizar o arquivo `serverless.template` automaticamente):
   ```bash
   sam build
   ```

4. Gerar o deploy da aplicação na AWS:
   ```bash
   sam deploy --guided
   ```

5. Deletar o deploy da aplicação na AWS após utilização:
   ```bash
   sam delete --stack-name {StackName}
   ```

---

## 📚 Créditos e Referências

Este projeto foi desenvolvido tendo como base prática e guia de estudos o excelente tutorial do portal **Code With Mukesh**:

*   **Artigo Completo:** [.NET Lambda Annotations Framework for Simplified Development | .NET on AWS](https://codewithmukesh.com/blog/dotnet-lambda-annotations-framework-aws/)
*   **Autor:** Mukesh Murugan

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.
