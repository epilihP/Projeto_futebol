using System;
using System.Collections.Generic;

namespace Projeto_futebol.Util
{
    public static class Utils
    {
        // Exibe uma lista genérica com título
        public static void ExibirLista<T>(IEnumerable<T> lista, string titulo = "Itens")
        {
            Console.WriteLine($"\n--- {titulo} ---");
            int count = 0;
            foreach (var item in lista)
            {
                Console.WriteLine(item);
                count++;
            }
            if (count == 0)
                Console.WriteLine("Nenhum item encontrado.");
        }

        // Valida se o valor é um inteiro positivo
        public static bool ValidarId(string input, out int id)
        {
            if (int.TryParse(input, out id) && id > 0)
                return true;
            Console.WriteLine("ID inválido, voltando ao menu.");
            return false;
        }

        // Mensagem padrão de erro
        public static void MensagemErro(string mensagem = "Ocorreu um erro. Voltando ao menu.")
        {
            Console.WriteLine($"[ERRO] {mensagem}");
        }

        // Mensagem padrão de sucesso
        public static void MensagemSucesso(string mensagem = "Operação realizada com sucesso!")
        {
            Console.WriteLine($"[OK] {mensagem}");
        }

        // Mensagem de retorno ao menu
        public static void MensagemRetornoMenu(string mensagem = "Voltando ao menu...")
        {
            Console.WriteLine(mensagem);
        }
    }
}
