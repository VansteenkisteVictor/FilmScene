using Microsoft.Extensions.Configuration;
using MovieApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Repositories
{
    public class Movies_SQL : IMovies_SQL
    {
        private readonly string connectionString;

        public Movies_SQL(IConfiguration config)
        {
            var connectionConfig = config.GetSection("Configurations")["OtherConnection"];
            connectionString = ConfigurationExtensions.GetConnectionString(config, "DefaultConnection");
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MovieSearch> Update(MovieSearch student)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MovieSearch>> GetAllMoviesAsync()
        {
            List<MovieSearch> lst = new List<MovieSearch>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                //1. SQL query
                string sql = "SELECT * FROM MovieSearch";
                SqlCommand cmd = new SqlCommand(sql, con);
                //2. Data ophalen
                con.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                lst = await GetData(reader);
                con.Close();
            }
            return lst;

        }

        public async Task<MovieDetail> GetMovieDetailAsync(string id)
        {
            MovieDetail detail = new MovieDetail();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                //1. SQL query
                string sql = "SELECT * FROM Movies WHERE imdbID = @Id";
                SqlCommand cmd = new SqlCommand(sql, con)
                {
                    CommandType = System.Data.CommandType.Text,
                };
                cmd.Parameters.AddWithValue("@Id", id);
                //2. Data ophalen
                con.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                detail = (await GetData2(reader))[0];
                con.Close();
            }
            return detail;

        }

        //SQL helpers --------------------------------------------------
        private async Task<List<MovieSearch>> GetData(SqlDataReader reader)
        {
            List<MovieSearch> lst = new List<MovieSearch>();
            //1. try catch verhindert applicatie crash
            try
            {
                while (await reader.ReadAsync())
                {
                    MovieSearch s = new MovieSearch();
                    
                    s.Title = !Convert.IsDBNull(reader["Search_Title"]) ? (string)reader["Search_Title"] : "";
                    s.Year = Convert.ToInt32(reader["Search_Year"]);
                    s.imdbID = !Convert.IsDBNull(reader["Search_imdbID"]) ? (string)reader["Search_imdbID"] : "";
                    s.Type = !Convert.IsDBNull(reader["Search_Type"]) ? (string)reader["Search_Type"] : "";
                    s.Poster = !Convert.IsDBNull(reader["Search_Poster"]) ? (string)reader["Search_Poster"] : "";
                    lst.Add(s);
                }
            }
            catch (Exception exc)
            {
                Console.Write(exc.Message); //later loggen
            }
            finally
            {
                reader.Close();  //Niet vergeten. Beperkt aantal verbindingen (of kosten)
            }
            return lst;
        }

        private async Task<List<MovieDetail>> GetData2(SqlDataReader reader)
        {
            List<MovieDetail> lst = new List<MovieDetail>();
            //1. try catch verhindert applicatie crash
            try
            {
                while (await reader.ReadAsync())
                {
                    MovieDetail s = new MovieDetail();

                    s.Title = !Convert.IsDBNull(reader["Title"]) ? (string)reader["Title"] : "";
                    s.Year = Convert.ToInt32(reader["Year"]);
                    s.Rated = !Convert.IsDBNull(reader["Rated"]) ? (string)reader["Rated"] : "";
                    s.Poster = !Convert.IsDBNull(reader["Poster"]) ? (string)reader["Poster"] : "";
                    s.Runtime = !Convert.IsDBNull(reader["Runtime"]) ? (string)reader["Runtime"] : "";
                    s.Genre = !Convert.IsDBNull(reader["Genre"]) ? (string)reader["Genre"] : "";
                    s.Writer = !Convert.IsDBNull(reader["Writer"]) ? (string)reader["Writer"] : "";
                    s.Actors = !Convert.IsDBNull(reader["Director"]) ? (string)reader["Director"] : "";
                    s.Plot = !Convert.IsDBNull(reader["Plot"]) ? (string)reader["Plot"] : "";


                }
            }
            catch (Exception exc)
            {
                Console.Write(exc.Message); //later loggen
            }
            finally
            {
                reader.Close();  //Niet vergeten. Beperkt aantal verbindingen (of kosten)
            }
            return lst;
        }

    }
}
