using ContractSystem.Core.Models.DTO;
using Core;
using Npgsql;

namespace ContractSystem.Repository
{
    public class ApprovalRepository
    {
        private const string QueryAddApproval = "INSERT INTO approvals(user_id, document_id) VALUES (@user_id, @document_id);";
        private const string QueryRemoveApproval = "DELETE * FROM approvals WHERE user_id = @user_id AND  document_id = @document_id;";
        private const string QueryUpdateApproval = "UPDATE approvals SET approved = @approved , approval_date=CASE WHEN @approved THEN NOW() ELSE NULL END WHERE user_id = @user_id AND  document_id = @document_id;";
        private const string QueryGetApprovalsByUser = """
            SELECT u.id AS user_id, u.firstname, u.lastname, docs.id AS document_id, docs."index", docs.content, a.id as approval_id, a.approved, a.approval_date
                FROM approvals AS a 
                JOIN users AS u
                    ON u.id = a.user_id 
                JOIN documents AS docs
                    ON docs.id = a.document_id 
                WHERE a.user_id = @user_id;
            """;
        private const string QueryGetApprovalsByDocument = """
            SELECT u.id AS user_id, u.firstname, u.lastname, docs.id AS document_id, docs."index", docs.content, a.id as approval_id, a.approved, a.approval_date
                FROM approvals AS a 
                JOIN users AS u
                    ON u.id = a.user_id 
                JOIN documents AS docs
                    ON docs.id = a.document_id 
                WHERE a.document_id = @document_id;
            """;

        public static List<ApprovalDTO> GetApprovalsByDocument(int document_id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryGetApprovalsByDocument, connection);
                command.Parameters.AddWithValue("document_id", document_id);

                List<ApprovalDTO> Approvals = new List<ApprovalDTO>();

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Approvals.Add(new ApprovalDTO()
                        {
                            Id = reader.GetInt32(6),
                            IsApproved = reader.GetBoolean(7),
                            ApprovalDate = (reader.IsDBNull(8)) ? null : reader.GetDateTime(8),
                            User = new()
                            {
                                Id = reader.GetInt32(0),
                                Firstname = reader.GetString(1),
                                Lastname = reader.GetString(2),
                            },
                            Document = new()
                            {
                                Id = reader.GetInt32(3),
                                Index = reader.GetString(4),
                                Content = reader.GetString(5)
                            }
                        });
                    }
                }

                return Approvals;
            }
        }

        public static List<ApprovalDTO> GetApprovalsByUser(int user_id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryGetApprovalsByUser, connection);
                command.Parameters.AddWithValue("document_id", user_id);

                List<ApprovalDTO> Approvals = new List<ApprovalDTO>();

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Approvals.Add(new ApprovalDTO()
                        {
                            Id = reader.GetInt32(6),
                            IsApproved = reader.GetBoolean(7),
                            ApprovalDate = (reader.IsDBNull(8)) ? null : reader.GetDateTime(8),
                            User = new()
                            {
                                Id = reader.GetInt32(0),
                                Firstname = reader.GetString(1),
                                Lastname = reader.GetString(2),
                            },
                            Document = new()
                            {
                                Id = reader.GetInt32(3),
                                Index = reader.GetString(4),
                                Content = reader.GetString(5),
                                IsApproved = reader.GetBoolean(7),
                            }
                        });
                    }
                }

                return Approvals;
            }
        }
        
        public static void AddApproval(int user_id, int document_id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryAddApproval, connection);
                command.Parameters.AddWithValue("user_id", user_id);
                command.Parameters.AddWithValue("document_id", document_id);

                command.ExecuteNonQuery();
            }
        }

        public static void RemoveApproval(int user_id, int document_id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryRemoveApproval, connection);
                command.Parameters.AddWithValue("user_id", user_id);
                command.Parameters.AddWithValue("document_id", document_id);

                command.ExecuteNonQuery();
            }
        }

        public static bool UpdateApproval(int user_id, int document_id, bool approved)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryUpdateApproval, connection);
                command.Parameters.AddWithValue("user_id", user_id);
                command.Parameters.AddWithValue("document_id", document_id);
                command.Parameters.AddWithValue("approved", approved);

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
