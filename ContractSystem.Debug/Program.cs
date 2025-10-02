using ContractSystem.Core.DTO;
using ContractSystem.Repository;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ContractSystem.Debug
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //    Console.WriteLine(UserRepository.GetAllUsers().Count);
            //    UserIn u = UserRepository.AddUser("Иванов", "Иван");
            //    print(u);
            //    string temp = u.Firstname;
            //    u.Firstname = u.Lastname;
            //    u.Lastname = temp;
            //    UserRepository.UpdateUser(u);
            //    print(UserRepository.GetAllUsers());
            //    Console.WriteLine(UserRepository.GetAllUsers().Count);
            //    UserRepository.DeleteUserById(u.Id);
            //    Console.WriteLine(UserRepository.GetAllUsers().Count);

            //DocumentIn document = DocumentRepository.AddDocument("Doc-2025-2", "Some data");
            //print(document);

            print(ApprovalRepository.GetApprovalsByUser(1));
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