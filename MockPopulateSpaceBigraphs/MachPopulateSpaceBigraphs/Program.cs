using System;
using System.Data.SqlClient;

class PopulateBigraphsProgram
{
    static void Main()
    {
        string connectionString = //YOUR CONNECTION STRING HERE

        // List of letters 'a' to 'z'
        char[] letters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Insert space-related bigraphs
            foreach (char letter in letters)
            {
                // Insert leading space bigraph ('1' + letter)
                InsertBigraph(connection, $"1{letter}", null); // '1' represents space

                // Insert trailing space bigraph (letter + '1')
                InsertBigraph(connection, $"{letter}1", null);
            }

            Console.WriteLine("52 possible space bigraphs inserted successfully!");
        }
    }

    static void InsertBigraph(SqlConnection connection, string bigraph, int? wordId)
    {
        string insertBigraphQuery = "INSERT INTO Bigraphs (Bigraph, WordId) VALUES (@Bigraph, @WordId)";
        using (SqlCommand insertCommand = new SqlCommand(insertBigraphQuery, connection))
        {
            insertCommand.Parameters.AddWithValue("@Bigraph", bigraph);
            insertCommand.Parameters.AddWithValue("@WordId", DBNull.Value); // WordId is null for space bigraphs
            insertCommand.ExecuteNonQuery();
        }
    }
}
