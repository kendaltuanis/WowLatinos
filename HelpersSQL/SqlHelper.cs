using System;
using System.Linq;
using HelpersSQL.Enums;

namespace HelpersSQL
{
    public class SqlHelper
    {
        private string table;
        private const string SEPARATOR = ",";
        private const bool WHERECONDITION = true;  // TRUE es 'and' y FALSE es 'or'
        private const DBMotor DBDEFAULT = DBMotor.None;


        public SqlHelper(String table)
        {
            this.table = table;
        }

        public string InsertSql(string[] fields, DBMotor db = DBDEFAULT, string[] fieldsReturn = null)
        {
            String post = "", server = "";
            ReturnField(db, fieldsReturn,ref post,ref server);

            return string.Format("INSERT INTO {0} ({1}) {2} VALUES ({3}) {4}", table, JoinFields(fields),server, JoinFieldWithAt(fields), post);
        }

        public string UpdateSql(string[] fields, string[] where, bool andCondition = WHERECONDITION)
        {
            return string.Format("UPDATE {0} SET {1} WHERE {2}", table, JoinFieldsWithEquals(fields), Where(where, andCondition));
        }

        public string DeleteSql(string[] whereFields = null, bool andCondition = WHERECONDITION)
        {
            return string.Format("DELETE FROM {0} " + ((whereFields != null) ? " WHERE {1}" : ";"), table, Where(whereFields, andCondition));
        }

        public string SelectSql(string[] fields, string[] whereFields = null, bool andCondition = WHERECONDITION)
        {
            return string.Format("SELECT {0} FROM {1}"+ ((whereFields != null) ? " WHERE {2}" : ";"), JoinFields(fields), table, Where(whereFields, andCondition));
        }

        /*
         Método que se encarga de retornar en un 'string' unido de los campos separados por el signo
          que se requiera. DEFAULT SIGNO = ","
          Ej: "campo1,campo2,campo3"
         */
        private string JoinFields(string[] fields, string signo = SEPARATOR)
        {
            return string.Join(signo, fields);
        }

        /*
         Método que se encarga de retornar en un 'string' los campos separados por el signo
          que se requiera, además con los campos igualados y las '@' por delante. DEFAULT SIGNO = ","
           Ej: "campo1=@campo1,campo2=@campo2,campo3=@campo3"
         */
        private string JoinFieldsWithEquals(string[] fields, string signo = SEPARATOR)
        {
            return (fields == null) ? " " : string.Join(signo, fields.Select(s => s + "=" + "@" + s));
        }

        /*
         Método que se encarga de retornar en un 'string' los campos separados por el signo
          que se requiera y además con las '@' por delante. DEFAULT SIGNO = ","
           Ej: "@campo1,@campo2,@campo3"
         */
        private string JoinFieldWithAt(String[] fields)
        {
            return string.Join(",", fields.Select(s => "@" + s));
        }

        /*
         Método que se encarga de retornar en un 'string' los campos separados por el signo
          que se requiera, además con los campos igualados y las '@' por delante. DEFAULT whereConditions = true (and)
           Ej: "campo1=@campo1 and campo2=@campo2 and campo3=@campo3" || "campo3=@campo1 or campo=@campo2"
         */
        private string Where(String[] whereFields, bool andCondition)
        {
            String caracter = ((andCondition) ? " and " : " or ");
            return string.Join(caracter, JoinFieldsWithEquals(whereFields, caracter));
        }

        /*
         Método que se encarga de crear la sentencia de retorno de campos:
         Ej: SqlServer: OUTPUT Inserted.id,Inserted.phone
             Postgresql: RETURNING id,phone
        */
        private void ReturnField(DBMotor db, String[] fields, ref String postg,ref String server) {

            if (fields == null) {
                return;
            }
            if (db == DBMotor.SqlServer) {

                server= string.Format("OUTPUT {0}", string.Join(",", fields.Select(i => string.Format("Inserted.{0}", i))));
            }

            if (db == DBMotor.Postgresql)
            {
                postg= string.Format("RETURNING {0}",string.Join(",", fields));
            }
        }
    }
}
