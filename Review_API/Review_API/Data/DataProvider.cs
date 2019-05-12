using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Models;
using Review_API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Review_API.Data
{
   
    public class DataProvider : IDataProvider
    {
        private readonly string connectionString;
        public DataProvider(IConfiguration config)
        {
            var connectionConfig = config.GetSection("Configurations")["OtherConnection"];
            connectionString = ConfigurationExtensions.GetConnectionString(config, "DefaultConnection");
        }

        public async Task<IEnumerable<ReviewTask_RA>> GetAllReviewsASync(string MovieId)
        {
            List<ReviewTask_RA> lst = new List<ReviewTask_RA>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    //1. SQL query
                    string sql = "SELECT * FROM Review WHERE Review.MovieId = @Id";
                    SqlCommand cmd = new SqlCommand(sql, con)
                    {
                        CommandType = System.Data.CommandType.Text,
                    };
                    cmd.Parameters.AddWithValue("@Id", MovieId);
                    //2. Data ophalen
                    con.Open();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    lst = await GetData(reader);
                    con.Close();
                }
                catch(Exception ex)
                {
                    throw ex;
                }

            }
            return lst;

        }

        public async Task<ReviewTask_RA> GetReview(string Id)
        {
            List<ReviewTask_RA> lst = new List<ReviewTask_RA>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    //1. SQL query
                    string sql = "SELECT * FROM Review WHERE Review.Id = @Id";
                    SqlCommand cmd = new SqlCommand(sql, con)
                    {
                        CommandType = System.Data.CommandType.Text,
                    };
                    cmd.Parameters.AddWithValue("@Id", Id);
                    //2. Data ophalen
                    con.Open();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    lst = await GetData(reader);
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return lst.ElementAt(0);

        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsASync()
        {
            List<Reservation> lst = new List<Reservation>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    //1. SQL query
                    string sql = "SELECT * FROM Reservation";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    //2. Data ophalen
                    con.Open();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    lst = await ReservationData(reader);
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return lst;

        }

        public async Task<IEnumerable<Reservation>> GetUserReservationsASync(string id)
        {
            List<Reservation> lst = new List<Reservation>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    //1. SQL query
                    string sql = "SELECT * FROM Reservation WHERE Reservation.UserId = @Id";
                    SqlCommand cmd = new SqlCommand(sql, con)
                    {
                        CommandType = System.Data.CommandType.Text,
                    };
                    cmd.Parameters.AddWithValue("@Id", id);
                    //2. Data ophalen
                    con.Open();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    lst = await ReservationData(reader);
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return lst;

        }

        public async Task<Reservation> GetDetailReservation(string id)
        {
            List<Reservation> reservation = new List<Reservation>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    //1. SQL query
                    string sql = "SELECT * FROM Reservation WHERE Reservation.Id = @Id";
                    SqlCommand cmd = new SqlCommand(sql, con)
                    {
                        CommandType = System.Data.CommandType.Text,
                    };
                    cmd.Parameters.AddWithValue("@Id", id);
                    //2. Data ophalen
                    con.Open();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    reservation = await ReservationData(reader);
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return reservation.ElementAt(0);

        }

        public async Task<IEnumerable<ReviewTask_RA>> GetAllReviewsASyncByUser(string UserId)
        {
            List<ReviewTask_RA> lst = new List<ReviewTask_RA>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    //1. SQL query
                    string sql = "SELECT * FROM Review WHERE Review.UserId = @Id";
                    SqlCommand cmd = new SqlCommand(sql, con)
                    {
                        CommandType = System.Data.CommandType.Text,
                    };
                    cmd.Parameters.AddWithValue("@Id", UserId);
                    //2. Data ophalen
                    con.Open();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    lst = await GetData(reader);
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return lst;

        }


        public async Task Delete(string id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                //1. SQL query
                string sql = "DELETE FROM Review WHERE Review.Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, con)
                {
                    CommandType = System.Data.CommandType.Text,
                };
                cmd.Parameters.AddWithValue("@Id", id);
                //2. Data ophalen
                con.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                con.Close();
            }
        }


        public async Task DeleteReservation(string id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                //1. SQL query
                string sql = "DELETE FROM Reservation WHERE Reservation.Id = @Id";
                SqlCommand cmd = new SqlCommand(sql, con)
                {
                    CommandType = System.Data.CommandType.Text,
                };
                cmd.Parameters.AddWithValue("@Id", id);
                //2. Data ophalen
                con.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                con.Close();
            }
        }

        public async Task<ReviewTask_RA> Add(ReviewTask_RA review)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            { //TO DO: Gender, DateOfBirth, Password
                string SQL = "Insert into Review(Id, Name, Comment, Score, MovieId, UserId)"; SQL += " Values(@Id, @Name, @Comment, @Score, @MovieId, @UserId)";
                SqlCommand cmd = new SqlCommand(SQL, con);
                cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@Name", review.Name);
                cmd.Parameters.AddWithValue("@Comment", review.Comment ?? ""); //EducationId is NULLABLE! int?
                cmd.Parameters.AddWithValue("@Score", review.Score);
                cmd.Parameters.AddWithValue("@MovieId", review.MovieId);
                cmd.Parameters.AddWithValue("@UserId", review.UserId);
                con.Open();
                await cmd.ExecuteNonQueryAsync(); //enkel uitvoeren, geen reader
                con.Close();
                return review;
            }
        }

        public async Task<Reservation> AddReservation(Reservation reservation)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            { //TO DO: Gender, DateOfBirth, Password
                string SQL = "Insert into Reservation(Id, Email, Location, Date, Amount, MovieId, UserId)"; SQL += " Values(@Id, @Email, @Location, @Date, @Amount, @MovieId, @UserId)";
                SqlCommand cmd = new SqlCommand(SQL, con);
                cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@Email", reservation.Email);
                cmd.Parameters.AddWithValue("@Location", reservation.Location);
                cmd.Parameters.AddWithValue("@Date", reservation.Date);
                cmd.Parameters.AddWithValue("@Amount", reservation.Amount);
                cmd.Parameters.AddWithValue("@MovieId", reservation.MovieId);
                cmd.Parameters.AddWithValue("@UserId", reservation.UserId);
                con.Open();
                await cmd.ExecuteNonQueryAsync(); //enkel uitvoeren, geen reader
                con.Close();
                return reservation;
            }
        }

        public async Task AddLogin(Login login)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string SQL = "Insert into Login(Id, Username, Password, Salt, Admin)"; SQL += " Values(@Id, @Username, @Password, @Salt, @Admin)";
                SqlCommand cmd = new SqlCommand(SQL, con);
                cmd.Parameters.AddWithValue("@Id", Guid.NewGuid());
                cmd.Parameters.AddWithValue("@Username", login.Username);
                cmd.Parameters.AddWithValue("@Password", login.Password);
                cmd.Parameters.AddWithValue("@Salt", login.Salt);
                cmd.Parameters.AddWithValue("@Admin", login.Admin);
                con.Open();
                await cmd.ExecuteNonQueryAsync(); //enkel uitvoeren, geen reader
                con.Close();
            }
        }

        public async Task UpdateReview(ReviewTask_RA review)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            { //TO DO: Gender, DateOfBirth, Password
                try
                {
                    string SQL = "UPDATE Review SET Comment = @Comment, Score = @Score WHERE UserId = @UserId AND Id = @Id";
                    SqlCommand cmd = new SqlCommand(SQL, con);
                    cmd.Parameters.AddWithValue("@Comment", review.Comment);
                    cmd.Parameters.AddWithValue("@Score", review.Score);
                    cmd.Parameters.AddWithValue("@UserId", review.UserId);
                    cmd.Parameters.AddWithValue("@Id", review.Id);
                    con.Open();
                    await cmd.ExecuteNonQueryAsync(); //enkel uitvoeren, geen reader
                    con.Close();
                }
                catch(Exception ex)
                {
                    throw ex;
                }

            }
        }

        public async Task UpdateReservation(Reservation reservation)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            { //TO DO: Gender, DateOfBirth, Password
                try
                {
                    string SQL = "UPDATE Reservation SET Location = @Location, Date = @Date, Amount = @Amount WHERE UserId = @UserId AND Id = @Id";
                    SqlCommand cmd = new SqlCommand(SQL, con);
                    cmd.Parameters.AddWithValue("@Location", reservation.Location);
                    cmd.Parameters.AddWithValue("@Date", reservation.Date);
                    cmd.Parameters.AddWithValue("@Amount", reservation.Amount);
                    cmd.Parameters.AddWithValue("@UserId", reservation.UserId);
                    cmd.Parameters.AddWithValue("@Id", reservation.Id);
                    con.Open();
                    await cmd.ExecuteNonQueryAsync(); //enkel uitvoeren, geen reader
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public async Task<MovieDetail> GetMovieDetailAsync(string id)
        {
            List<MovieDetail> lst = new List<MovieDetail>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                //1. SQL query
                string sql = "SELECT * FROM Movies WHERE Movies.imdbID = @Id";
                SqlCommand cmd = new SqlCommand(sql, con)
                {
                    CommandType = System.Data.CommandType.Text,
                };
                cmd.Parameters.AddWithValue("@Id", id);
                //2. Data ophalen
                con.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                lst = await GetDataMovieDetail(reader);
                con.Close();
            }
            return lst[0];

        }

        public async Task<Login> GetLogin(string Username)
        {
            Login login = new Login();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                //1. SQL query
                string sql = "SELECT * FROM Login WHERE Login.Username = @Username";
                SqlCommand cmd = new SqlCommand(sql, con)
                {
                    CommandType = System.Data.CommandType.Text,
                };
                cmd.Parameters.AddWithValue("@Username", Username);
                //2. Data ophalen
                con.Open();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                login = await GetLoginData(reader);
                con.Close();
            }
            return login;

        }


        private async Task<List<ReviewTask_RA>> GetData(SqlDataReader reader)
        {
            List<ReviewTask_RA> lst = new List<ReviewTask_RA>();
            //1. try catch verhindert applicatie crash
            try
            {
                while (await reader.ReadAsync())
                {
                    ReviewTask_RA s = new ReviewTask_RA();

                    s.Id = (Guid)reader["Id"];
                    s.Name = !Convert.IsDBNull(reader["Name"]) ? (string)reader["Name"] : "";
                    s.Comment = !Convert.IsDBNull(reader["Comment"]) ? (string)reader["Comment"] : "";
                    s.Score = Convert.ToInt32(reader["Score"]);
                    s.MovieId = !Convert.IsDBNull(reader["MovieId"]) ? (string)reader["MovieId"] : "";
                    s.UserId = !Convert.IsDBNull(reader["UserId"]) ? (string)reader["UserId"] : "";
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

        private async Task<List<Reservation>> ReservationData(SqlDataReader reader)
        {
            List<Reservation> lst = new List<Reservation>();
            //1. try catch verhindert applicatie crash
            try
            {
                while (await reader.ReadAsync())
                {
                    Reservation s = new Reservation();

                    s.Id = (Guid)reader["Id"];
                    s.Email = !Convert.IsDBNull(reader["Email"]) ? (string)reader["Email"] : "";
                    s.Location = !Convert.IsDBNull(reader["Location"]) ? (string)reader["Location"] : "";
                    s.Date = !Convert.IsDBNull(reader["Date"]) ? (string)reader["Date"] : "";
                    s.Amount = Convert.ToInt32(reader["Amount"]);
                    s.MovieId = !Convert.IsDBNull(reader["MovieId"]) ? (string)reader["MovieId"] : "";
                    s.UserId = !Convert.IsDBNull(reader["UserId"]) ? (string)reader["UserId"] : "";
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


        private async Task<Login> GetLoginData(SqlDataReader reader)
        {
            Login s = new Login();
            //1. try catch verhindert applicatie crash
            try
            {
                while (await reader.ReadAsync())
                {
                    s.Id = (Guid)reader["Id"];
                    s.Username = !Convert.IsDBNull(reader["Username"]) ? (string)reader["Username"] : "";
                    s.Password = !Convert.IsDBNull(reader["Password"]) ? (string)reader["Password"] : "";
                    s.Salt = (byte[])reader["Salt"];
                    s.Admin = !Convert.IsDBNull(reader["Admin"]) ? (string)reader["Admin"] : "";
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
            return s;
        }



        

        private async Task<List<MovieDetail>> GetDataMovieDetail(SqlDataReader reader)
        {
            List<MovieDetail> lst = new List<MovieDetail>();
            //1. try catch verhindert applicatie crash
            try
            {
                while (await reader.ReadAsync())
                {
                    MovieDetail s = new MovieDetail();

                    s.Title = !Convert.IsDBNull(reader["Title"]) ? (string)reader["Title"] : "";
                    s.Year = !Convert.IsDBNull(reader["Year"]) ? (string)reader["Year"] : "";
                    s.Rated = !Convert.IsDBNull(reader["Rated"]) ? (string)reader["Rated"] : "";
                    s.Poster = !Convert.IsDBNull(reader["Poster"]) ? (string)reader["Poster"] : "";
                    s.Runtime = !Convert.IsDBNull(reader["Runtime"]) ? (string)reader["Runtime"] : "";
                    s.Genre = !Convert.IsDBNull(reader["Genre"]) ? (string)reader["Genre"] : "";
                    s.Writer = !Convert.IsDBNull(reader["Writer"]) ? (string)reader["Writer"] : "";
                    s.Actors = !Convert.IsDBNull(reader["Director"]) ? (string)reader["Director"] : "";
                    s.Plot = !Convert.IsDBNull(reader["Plot"]) ? (string)reader["Plot"] : "";
                    s.imdbID = !Convert.IsDBNull(reader["imdbID"]) ? (string)reader["imdbID"] : "";
                    s.imdbRating = Convert.ToDecimal(reader["imdbRating"]);
                    s.Metascore = !Convert.IsDBNull(reader["Metascore"]) ? (string)reader["Metascore"] : "";
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

    }
}
