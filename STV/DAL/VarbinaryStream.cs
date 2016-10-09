using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace STV.DAL
{
    public class VarbinaryStream : Stream
    {
        private STVDbContext _db;
        private List<SqlParameter> parameters;

        private SqlConnection _Connection;

        private string _TableName;
        private string _BinaryColumn;
        private string _KeyColumn;
        private int _KeyValue;
        private long _Tamanho;

        private long _Offset;

        private SqlDataReader _SQLReader;
        private long _SQLReadPosition;

        private bool _AllowedToRead = false;

        public VarbinaryStream(
        string ConnectionString,
        string TableName,
        string BinaryColumn,
        string KeyColumn,
        int KeyValue,
        STVDbContext db,
        long Tamanho,
        bool AllowRead = false)
        {
            // create own connection with the connection string.
            _Connection = new SqlConnection(ConnectionString);

            if (db == null)
                _db = new STVDbContext();
            else
                _db = db;

            _TableName = TableName;
            _BinaryColumn = BinaryColumn;
            _KeyColumn = KeyColumn;
            _KeyValue = KeyValue;
            _Tamanho = Tamanho;

            // only query the database for a result if we are going to be reading, otherwise skip.
            _AllowedToRead = AllowRead;
            if (_AllowedToRead == true)
            {
                try
                {
                    if (_Connection.State != ConnectionState.Open)
                        _Connection.Open();

                    SqlCommand cmd = new SqlCommand(
                    @"SELECT TOP 1 [" + _BinaryColumn + @"]
                                    FROM [dbo].[" + _TableName + @"]
                                    WHERE [" + _KeyColumn + "] = @id",
                    _Connection);

                    cmd.Parameters.Add(new SqlParameter("@id", _KeyValue));

                    _SQLReader = cmd.ExecuteReader(
                    CommandBehavior.SequentialAccess |
                    CommandBehavior.SingleResult |
                    CommandBehavior.SingleRow |
                    CommandBehavior.CloseConnection);

                    _SQLReader.Read();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message + " - CS=" + _Connection.ConnectionString);
                }
            }
        }

        // this method will be called as part of the Stream ímplementation when we try to write to our VarbinaryStream class.
        public override void Write(byte[] buffer, int index, int count)
        {
            try
            {
                //if (_Connection.State != ConnectionState.Open)
                //    _Connection.Open();

                parameters = new List<SqlParameter>();

                if (_Offset == 0)
                {
                    // for the first write we just send the bytes to the Column
                    //SqlCommand cmd = new SqlCommand(
                    //                        @"UPDATE [dbo].[" + _TableName + @"]
                    //                            SET [" + _BinaryColumn + @"] = @firstchunk 
                    //                                WHERE [" + _KeyColumn + "] = @id",
                    //                                    _Connection);

                    //cmd.Parameters.Add(new SqlParameter("@firstchunk", buffer));
                    //cmd.Parameters.Add(new SqlParameter("@id", _KeyValue));

                    //cmd.ExecuteNonQuery();

                    parameters.Add(new SqlParameter("@firstchunk", buffer));
                    parameters.Add(new SqlParameter("@id", _KeyValue));

                    _db.Database.ExecuteSqlCommand(
                                            @"UPDATE [dbo].[" + _TableName + @"]
                                                SET [" + _BinaryColumn + @"] = @firstchunk 
                                                    WHERE [" + _KeyColumn + "] = @id",
                                                        parameters.ToArray());

                    _Offset = count;
                }
                else
                {
                    // for all updates after the first one we use the TSQL command .WRITE() to append the data in the database
                    //SqlCommand cmd = new SqlCommand(
                    //                        @"UPDATE [dbo].[" + _TableName + @"]
                    //                            SET [" + _BinaryColumn + @"].WRITE(@chunk, NULL, @length)
                    //                                WHERE [" + _KeyColumn + "] = @id",
                    //                                    _Connection);

                    //cmd.Parameters.Add(new SqlParameter("@chunk", buffer));
                    //cmd.Parameters.Add(new SqlParameter("@length", count));
                    //cmd.Parameters.Add(new SqlParameter("@id", _KeyValue));

                    //cmd.ExecuteNonQuery();

                    parameters.Add(new SqlParameter("@chunk", buffer));
                    parameters.Add(new SqlParameter("@length", count));
                    parameters.Add(new SqlParameter("@id", _KeyValue));

                    _db.Database.ExecuteSqlCommand(
                                            @"UPDATE [dbo].[" + _TableName + @"]
                                                SET [" + _BinaryColumn + @"].WRITE(@chunk, NULL, @length)
                                                    WHERE [" + _KeyColumn + "] = @id",
                                                        parameters.ToArray());

                    _Offset += count;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " - CS=" + _Connection.ConnectionString);
                // log errors here
            }
        }

        // this method will be called as part of the Stream ímplementation when we try to read from our VarbinaryStream class.
        public override int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                long bytesRead = _SQLReader.GetBytes(0, _SQLReadPosition, buffer, offset, count);
                _SQLReadPosition += bytesRead;
                return (int)bytesRead;
            }
            catch (Exception)
            {
                // log errors here
            }
            return -1;
        }
        public override bool CanRead
        {
            get { return _AllowedToRead; }
        }

        #region unimplemented methods
        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            //throw new NotImplementedException();
        }

        public override long Length
        {
            get { return _Tamanho; }
            //get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get
            {
                return _SQLReadPosition;
                //throw new NotImplementedException();
            }
            set
            {
                _SQLReadPosition = value;
            }
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
        #endregion unimplemented methods
    }
}


