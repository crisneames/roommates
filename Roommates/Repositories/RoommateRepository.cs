using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
	public class RoommateRepository : BaseRepository
	{
        public RoommateRepository(string connectionString) : base(connectionString) { }


        public List<Roommate> GetAll()
        {
            
            using (SqlConnection conn = Connection)
            {
               
                conn.Open();

              
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = @"SELECT rm.Id, rm.FirstName, rm.LastName, rm.RentPortion, rm.MoveInDate, rm.RoomId, r.name, r.MaxOccupancy from Roommate rm
                      join Room r on rm.RoomId = r.Id";

                    
                    SqlDataReader reader = cmd.ExecuteReader();

                   
                    List<Roommate> roommates = new List<Roommate>();

                    
                    while (reader.Read())
                    {
                        
                        int idColumnPosition = reader.GetOrdinal("Id");
                         int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("FirstName");
                        string FirstNameValue = reader.GetString(nameColumnPosition);

                        int LastNameColumPosition = reader.GetOrdinal("LastName");
                        string LastNameValue = reader.GetString(LastNameColumPosition);

                        int RentPortionColumnPosition = reader.GetOrdinal("RentPortion");
                        int RentPortionValue = reader.GetInt32(RentPortionColumnPosition);

                        int MoveInDateColumnPosition = reader.GetOrdinal("MoveInDate");
                        DateTime MoveInDateValue = reader.GetDateTime(MoveInDateColumnPosition);

                        int RoomIdColumnPosition = reader.GetOrdinal("RoomId");
                        int RoomIdValue = reader.GetInt32(RoomIdColumnPosition);

                        int roomnameColumnPosition = reader.GetOrdinal("Name");
                        string RoomName = reader.GetString(roomnameColumnPosition);

                        int OccupancyColumnPosition = reader.GetOrdinal("MaxOccupancy");
                        int OccupancyValue = reader.GetInt32(OccupancyColumnPosition);

                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            FirstName = FirstNameValue,
                            LastName = LastNameValue,
                            RentPortion = RentPortionValue,
                            MovedInDate = MoveInDateValue,
                            Room = new Room
                            {
                                Id = RoomIdValue,
                                Name =  RoomName,
                                MaxOccupancy = OccupancyValue
                            }
                            
                           
                        };

                        
                        roommates.Add(roommate);
                    }

                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of rooms who whomever called this method.
                    return roommates;
                }
            }
        }


        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT rm.FirstName, rm.RentPortion, r.Name as RoomName FROM Roommate rm join Room r on rm.RoomId = r.id where rm.id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    // If we only expect a single row back from the database, we don't need a while loop.
                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),

                            Room =new Room

                            {
                                Name = reader.GetString(reader.GetOrdinal("RoomName"))
                            }
                        };
                    }

                    reader.Close();

                    return roommate;
                }
            }
        }




    }
}

