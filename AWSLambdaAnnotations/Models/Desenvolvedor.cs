using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSLambdaAnnotations.Models
{
    [DynamoDBTable("desenvolvedores")]
    public class Desenvolvedor
    {
        [DynamoDBHashKey("id")]
        public int Id { get; set; }
        [DynamoDBProperty("nome")]
        public string Nome { get; set; }
        [DynamoDBProperty("email")]
        public string Email { get; set; }
    }
}
