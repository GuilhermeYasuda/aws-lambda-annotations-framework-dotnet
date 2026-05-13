using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.DynamoDBv2.DataModel;
using AWSLambdaAnnotations.Models;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaAnnotations;

public class Function
{
    private static readonly HttpClient _client = new HttpClient();
    private readonly IDynamoDBContext _context;

    public Function(IDynamoDBContext context)
    {
        _context = context;
    }

    private static async Task<string> GetCallingIP()
    {
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Add("User-Agent", "AWS Lambda .Net Client");

        var msg = await _client.GetStringAsync("http://checkip.amazonaws.com/").ConfigureAwait(continueOnCapturedContext: false);

        return msg.Replace("\n", "");
    }

    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Get, template: "/hello/{name}")]
    public async Task<Dictionary<string, string>> GetCallingIpFunction(string name)
    {
        var location = await GetCallingIP();
        return new Dictionary<string, string> {
            { "message", $"Hello {name}!" },
            { "location", location }
        };
    }

    [LambdaFunction(MemorySize = 1024)]
    [HttpApi(LambdaHttpMethod.Post, template: "/desenvolvedores")]
    public async Task<int> CriarDesenvolvedor([FromBody] Desenvolvedor request)
    {
        await _context.SaveAsync(request);
        return request.Id;
    }

    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Get, template: "/desenvolvedores/{id}")]
    public async Task<Desenvolvedor> ObterDesenvolvedor(int id)
    {
        return await _context.LoadAsync<Desenvolvedor>(id);
    }
}