using ContractSystem.Core.DTO;
using Core;
using Npgsql;

namespace ContractSystem.Repository
{
    public class DocumentRepository
    {
        private const string QueryGetDocumentByID = """
            SELECT id, index, content, 
            (
               	(SELECT COUNT(id) FROM approvals WHERE document_id=doc.id AND approved=FALSE) = 0
               	AND
               	(SELECT COUNT(id) FROM approvals WHERE document_id=doc.id AND approved=TRUE) > 0
            ) as approved 
            FROM documents AS doc
            WHERE id=@id;
            """;
        private const string QueryGetAllDocuments = """
            SELECT id, index, content, 
            (
            	(SELECT COUNT(id) FROM approvals WHERE document_id=doc.id AND approved=FALSE) = 0
            	AND
            	(SELECT COUNT(id) FROM approvals WHERE document_id=doc.id AND approved=TRUE) > 0
            ) as approved 
            FROM documents AS doc;
            """;
        private const string QueryGetAllDocumentsByUser = """
            SELECT doc.id, doc."index", doc.content,
            (
            	(SELECT COUNT(id) FROM approvals WHERE document_id=doc.id AND approved=FALSE) = 0
            	AND
            	(SELECT COUNT(id) FROM approvals WHERE document_id=doc.id AND approved=TRUE) > 0
            ) as approved 
            FROM document_owners AS owners
            JOIN documents AS doc ON owners.document_id = doc.id
            WHERE user_id = @user_id;
            """;
        private const string QueryMakeOwnedDocumentToUser = "INSERT INTO document_owners(user_id, document_id) VALUES (@user_id, @document_id);";
        private const string QueryMakeUnownedDocumentToUser = "DELETE * FROM document_owners WHERE user_id = @user_id AND  document_id = @document_id;";
         private const string QueryAddDocument = "INSERT INTO Documents(index , content) VALUES (@index,@content) RETURNING id;";
        private const string QueryDeleteDocumentByID = "DELETE FROM Documents WHERE id=@id;";
        private const string QueryUpdateDocument = "UPDATE Documents SET index=@index,content=@content WHERE id=@id RETURNING id;";

        public static Document GetDocumentById(int id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryGetDocumentByID, connection);
                command.Parameters.AddWithValue("id", id);

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    return new Document()
                    {
                        Id = reader.GetInt32(0),
                        Index = reader.GetString(1),
                        Content = reader.GetString(2)
                    };
                }
                else
                {
                    throw new Exception("Document not found");
                }
            }
        }

        public static Document AddDocument(string index, string content)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryAddDocument, connection);
                command.Parameters.AddWithValue("index", index);
                command.Parameters.AddWithValue("content", content);

                int id = (int)command.ExecuteScalar()!;
                return new Document()
                {
                    Id = id,
                    Index = index,
                    Content = content
                };
            }
        }

        public static void MakeOwnedDocumentToUser(int user_id, int document_id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryMakeOwnedDocumentToUser, connection);
                command.Parameters.AddWithValue("user_id", user_id);
                command.Parameters.AddWithValue("document_id", document_id);

                command.ExecuteNonQuery();
            }
        }

        public static void MakeUnownedDocumentToUser(int user_id, int document_id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryMakeUnownedDocumentToUser, connection);
                command.Parameters.AddWithValue("user_id", user_id);
                command.Parameters.AddWithValue("document_id", document_id);

                command.ExecuteNonQuery();
            }
        }

        public static bool UpdateDocument(Document Document)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryUpdateDocument, connection);
                command.Parameters.AddWithValue("id", Document.Id);
                command.Parameters.AddWithValue("index", Document.Index);
                command.Parameters.AddWithValue("content", Document.Content);

                int? result = (int?)command.ExecuteScalar();
                if (result.HasValue)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Document not found");
                }
            }
        }

        public static List<Document> GetAllDocuments()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryGetAllDocuments, connection);

                List<Document> Documents = new List<Document>();

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Documents.Add(new Document()
                        {
                            Id = reader.GetInt32(0),
                            Index = reader.GetString(1),
                            Content = reader.GetString(2),
                            IsApproved = reader.GetBoolean(3),
                        });
                    }
                    return Documents;
                }
                else
                {
                    throw new Exception("Document not found");
                }
            }
        }

        public static List<Document> GetAllDocumentsByUser(int user_id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryGetAllDocumentsByUser, connection);
                command.Parameters.AddWithValue("user_id", user_id);

                List<Document> Documents = new List<Document>();

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Documents.Add(new Document()
                        {
                            Id = reader.GetInt32(0),
                            Index = reader.GetString(1),
                            Content = reader.GetString(2),
                            IsApproved = reader.GetBoolean(3),
                        });
                    }
                    return Documents;
                }
                else
                {
                    throw new Exception("Document not found");
                }
            }
        }

        public static void DeleteDocumentById(int id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Options.ConnectionString))
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand(QueryDeleteDocumentByID, connection);
                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
