using ContractSystem.Core.DTO;
using ContractSystem.Core;
using Npgsql;

namespace ContractSystem.RepositoryOld
{
    public class UserRepository
    {
        private const string QueryGetUserByID = "SELECT id, firstname , lastname FROM users WHERE id = @id;";
        private const string QueryGetFirstUserByFirstname = "SELECT id, firstname , lastname FROM users WHERE firstname = @firstname;";
        private const string QueryGetAllUsers = "SELECT id, firstname , lastname FROM users";
        private const string QueryAddUser = "INSERT INTO USERs(firstname , lastname) VALUES (@firstname,@lastname) RETURNING id;";
        private const string QueryDeleteUserByID = "DELETE FROM users WHERE id=@id;";
        private const string QueryUpdateUser = "UPDATE users SET firstname=@firstname,lastname=@lastname WHERE id=@id RETURNING id;";

        public static UserDTO GetUserById(int id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryGetUserByID, connection);
                command.Parameters.AddWithValue("id", id);

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    return new UserDTO()
                    {
                        Id = reader.GetInt32(0),
                        Firstname = reader.GetString(1),
                        Lastname = reader.GetString(2)
                    };
                }
                else
                {
                    throw new Exception("UserIn not found");
                }
            }
        }

        public static UserDTO? GetFirstUserByFirstname(string firstname) 
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryGetFirstUserByFirstname, connection);
                command.Parameters.AddWithValue("firstname", firstname);

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    return new UserDTO()
                    {
                        Id = reader.GetInt32(0),
                        Firstname = reader.GetString(1),
                        Lastname = reader.GetString(2)
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        public static UserDTO AddUser(string firstname, string lastname)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryAddUser, connection);
                command.Parameters.AddWithValue("firstname", firstname);
                command.Parameters.AddWithValue("lastname", lastname);

                int id = (int)command.ExecuteScalar()!;
                return new UserDTO()
                {
                    Id = id,
                    Firstname = firstname,
                    Lastname = lastname
                };
            }
        }

        public static bool UpdateUser(UserDTO user)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryUpdateUser, connection);
                command.Parameters.AddWithValue("id", user.Id);
                command.Parameters.AddWithValue("firstname", user.Firstname);
                command.Parameters.AddWithValue("lastname", user.Lastname);

                int? result = (int?)command.ExecuteScalar();
                if (result.HasValue)
                {
                    return true;
                }
                else
                {
                    throw new Exception("UserIn not found");
                }
            }
        }

        public static List<UserDTO> GetAllUsers()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryGetAllUsers, connection);

                List<UserDTO> users = new List<UserDTO>();

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        users.Add(new UserDTO()
                        {
                            Id = reader.GetInt32(0),
                            Firstname = reader.GetString(1),
                            Lastname = reader.GetString(2)
                        });
                    }
                    return users;
                }
                else
                {
                    throw new Exception("UserIn not found");
                }
            }
        }

        public static void DeleteUserById(int id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryDeleteUserByID, connection);
                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}