using System;
using System.Collections.Generic;
using System.Linq;

namespace Projeto_futebol.Util
{
    public static class Utils
    {
        // Exibe uma janela padronizada com título e opções (com bordas, cor e alinhamento centralizado)
        public static void ExibirJanela(string titulo, string[] linhas, ConsoleColor corTitulo = ConsoleColor.Cyan, int larguraMinima = 70)
        {
            // Corrigir: considerar que uma linha pode conter '\n' e dividir em várias linhas visuais
            var todasLinhas = linhas.SelectMany(l => l.Split('\n')).ToArray();
            int maxLinha = todasLinhas.Length > 0 ? todasLinhas.Max(l => l.Length) : 0;
            int largura = Math.Max(larguraMinima, Math.Max(titulo.Length + 8, maxLinha + 4));
            string borda = "+" + new string('-', largura) + "+";
            Console.ForegroundColor = corTitulo;
            Console.WriteLine(borda);
            // Centraliza o título
            Console.WriteLine($"|{titulo.PadLeft((largura + titulo.Length) / 2).PadRight(largura)}|");
            Console.ResetColor();
            Console.WriteLine(borda);
            foreach (var linha in todasLinhas)
            {
                int espacos = largura - linha.Length;
                int esquerda = espacos / 2;
                int direita = espacos - esquerda;
                string linhaCentralizada = new string(' ', esquerda) + linha + new string(' ', direita);
                Console.WriteLine($"|{linhaCentralizada}|");
            }
            Console.WriteLine(borda);
        }

        // Exibe uma lista genérica com título em janela padronizada
        public static void ExibirLista<T>(IEnumerable<T> lista, string titulo = "Itens", ConsoleColor cor = ConsoleColor.Yellow, int larguraMinima = 70)
        {
            var itens = new List<string>();
            int count = 0;
            foreach (var item in lista)
            {
                itens.Add(item?.ToString() ?? "");
                count++;
            }
            if (count == 0)
                itens.Add("Nenhum item encontrado.");
            ExibirJanela(titulo, itens.ToArray(), cor, larguraMinima);
        }

        // Valida se o valor é um inteiro positivo
        public static bool ValidarId(string input, out int id)
        {
            if (int.TryParse(input, out id) && id > 0)
                return true;
            MensagemErro("ID inválido, voltando ao menu.");
            return false;
        }

        // Mensagem padrão de erro em janela
        public static void MensagemErro(string mensagem = "Ocorreu um erro. Voltando ao menu...", int larguraMinima = 70)
        {
            ExibirJanela("ERRO", new[] { mensagem }, ConsoleColor.Red, larguraMinima);
        }

        // Mensagem padrão de sucesso em janela
        public static void MensagemSucesso(string mensagem = "Operação realizada com sucesso!", int larguraMinima = 70)
        {
            ExibirJanela("SUCESSO", new[] { mensagem }, ConsoleColor.Green, larguraMinima);
        }

        // Mensagem de retorno ao menu em janela
        public static void MensagemRetornoMenu(string mensagem = "Voltando ao menu...", int larguraMinima = 70)
        {
            ExibirJanela("RETORNO", new[] { mensagem }, ConsoleColor.DarkGray, larguraMinima);
        }
    }
}
