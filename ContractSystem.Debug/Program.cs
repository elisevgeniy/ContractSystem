using ContractSystem.Core;
using ContractSystem.Core.DTO;
using ContractSystem.Repositories;
using Mapster;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ContractSystem.Debug
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DataContext dataContext = new DataContext();
            UserRepository userRepository = new UserRepository(dataContext);
            DocumentRepository documentRepository = new DocumentRepository(dataContext);
            ApprovalRepository approvalRepository = new ApprovalRepository(dataContext);

            var user = userRepository.Add(new UserDTO()
            {
                Firstname = "FTest",
                Lastname = "LTest"
            });

            var doc = documentRepository.Add(new DocumentDTO()
            {
                Index = "Ind",
                Content = "Doc Content",
                Owner = user,
            });

        }

        private static void print(Object obj)
        {
            Console.WriteLine(
                    JsonSerializer.Serialize(
                        obj,
                    new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    })
                );
        }
    }
}