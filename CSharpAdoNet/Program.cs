using System;
using System.Data.SqlClient;
using static System.Console;
namespace CSharpAdoNet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WriteLine("================================== CONTROLE DE CLIENTES ==================================");
            WriteLine("Selecione uma opção");
            WriteLine("1 - Listar");
            WriteLine("2 - Cadastrar");
            WriteLine("3 - Editar");
            WriteLine("4 - Excluir");
            WriteLine("5 - Visualizar");
            Write("Opção: ");
            int opc = Convert.ToInt32(ReadLine());
            Clear();

            switch (opc)
            {
                case 1:
                    Title = "Listagem de Clientes";
                    WriteLine("================================== CONTROLE DE CLIENTES ==================================");
                    ListarClientes();//Metódo Lista informações do Cliente
                    break;
                case 2:
                    Title = "Novo Cliente";
                    WriteLine("==================================  CADASTRO DE NOVO CLIENTES ==================================");
                    Write("Informe um nome: ");
                    string nome = ReadLine();
                    Write("Informe um e-mail: ");
                    string email = ReadLine();
                    SalvarCliente(nome, email);//Metodo envia informações para o BD
                    break;
                case 3:
                    Title = "Alteração Cliente";
                    WriteLine("==================================  ALTERAÇÃO CLIENTES ==================================");
                    
                    ListarClientes();
                   
                    Write("Selecione um cliente pelo ID: ");
                    int idOp = Convert.ToInt32(ReadLine());
                    //Trazendo dados para a Tupla
                    (int _id, string _nome, string _email) = SelecionarCliente(idOp);
                    Clear();

                    Title = "Alteração Cliente - "+ _nome;
                    WriteLine($"================================== ALTERAÇÃO CLIENTES - {_nome} ==================================");
                    
                    Write("Informe um nome: ");
                    nome = ReadLine();
                   
                    Write("Informe um e-mail: ");
                    email = ReadLine();

                    nome = nome.Equals("") ? _nome : nome;
                    email = email.Equals("") ? _email : email;
                    SalvarCliente(nome, email, idOp);

                    break;
                case 4:
                    Title = "Exclusão de Cliente";
                    WriteLine("================================== EXCLUSÃO DE CLIENTE ==================================");
                    ListarClientes();

                    Write("Selecione um cliente pelo ID: ");
                     idOp = Convert.ToInt32(ReadLine());

                    //Trazendo dados para a Tupla
                    ( _id,  _nome,  _email) = SelecionarCliente(idOp);
                    Clear();

                    Title = "Exclusão Cliente - " + _nome;
                    WriteLine($"================================== EXCLUSÃO DE CLIENTE - {_nome} ==================================");
                    WriteLine("\n************************** ATENÇÃO ***************************\n");
                    WriteLine("Deseja continuar? (S PARA SIM, OU N PARA NÃO)");
                    string confirmar = ReadLine().ToUpper();
                    if (confirmar.Equals("S"))
                    {
                        DeletarCliente(idOp);
                    }
                    //Write("Informe um nome: ");
                    //nome = ReadLine();


                    break;
                case 5:
                    Title = "Visualização Cliente";
                    WriteLine("==================================  VER DETALHER DE CLIENTES ==================================");
                    break;
                default:
                    Title = "Opção invalida";
                    WriteLine("==================================  OPÇÃO INVALIDA DIGITE NOVA OPÇÃO ==================================");
                    break;
            }
            ReadKey();
        }

        //Listar Cliente
        static void ListarClientes()
        {
            string connString = getStringConn();


            using(SqlConnection conn = new SqlConnection(connString))
            {
                
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from clientes order by id";

                using(SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        WriteLine("ID: {0}", dr["id"]);
                        WriteLine("Nome: {0}", dr["nome"]);
                        WriteLine("-------------------------------");
                    }
                }
            }
        }
        //Adicionar
        
        static void SalvarCliente(string nome, string email)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "insert into clientes(nome, email)values(@nome, @email)";
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.ExecuteNonQuery();
            }
        }

        //Editar
        //Sobrecarga
        static void SalvarCliente(string nome, string email,int id)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "update clientes set nome = @nome, email = @email where id = @id";
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
        //Deletar Cliente
        static void DeletarCliente( int id)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "delete from clientes where id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

            }
        }
        //Tupla p/ selecionar cliente no banco de dados
        static (int, string, string) SelecionarCliente(int id)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from clientes where id = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();
                    return (Convert.ToInt32(dr["id"].ToString()), dr["nome"].ToString(), dr["email"].ToString());
                }
            }
        }
        //Fazer string de conexão
        static string getStringConn()
        {
            string connString = "Server=ADAM\\SQLEXPRESS;DataBase=CSharpAdoNET;User Id=sa;Password=dev123";
            return connString;
        }
    }
}

