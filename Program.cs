using Players;
using Associacao;
using GerenciadorJogos;

        
Console.WriteLine("Teste 1: Adicionar jogador");
Jogador.AdicionarJogador(new Jogador("Philipe", 25, Posicao.AtacanteDireito));
Jogador.AdicionarJogador(new Jogador("Alisson", 22, Posicao.Goleiro));
Console.WriteLine("Jogadores adicionados com sucesso!");

Console.WriteLine("\nTeste 2: Listar jogadores");
var jogadores = Jogador.ListarJogadores();
foreach (var jogador in jogadores)
{
    Console.WriteLine(jogador.ToString());
}

Console.WriteLine("\nTeste 3: Atualizar jogador");
if (jogadores.Count > 0)        
{
    var primeiroJogador = jogadores[0];
    bool atualizado = Jogador.AtualizarJogador(primeiroJogador.codigo, "Carlos Silva", 26, Posicao.Goleiro);
    Console.WriteLine(atualizado ? "Jogador atualizado com sucesso!" : "Falha ao atualizar jogador.");
}

Console.WriteLine("\nTeste 4: Remover jogador");
if (jogadores.Count > 1)
{
    var segundoJogador = jogadores[1];
    bool removido = Jogador.RemoverJogador(segundoJogador.codigo);
    Console.WriteLine(removido ? "Jogador removido com sucesso!" : "Falha ao remover jogador.");
}

Console.WriteLine("\nTeste 5: Listar jogadores após remoção");
jogadores = Jogador.ListarJogadores();
foreach (var jogador in jogadores)
{
    Console.WriteLine(jogador.ToString());
}

// Teste 5: Listar jogadores após remoção
jogadores = Jogador.ListarJogadores();
Console.WriteLine("\nJogadores após remoção:");
foreach (var jogador in jogadores)
{
    Console.WriteLine(jogador.ToString());
}

// Teste 6: CRUD de Jogos
var dataJogo = DateTime.Now.AddDays(2);
var jogo = new GerenciadorDeJogos(dataJogo, "Quadra Central", "Society", 5, 2, 10);
Database.SalvarJogo(jogo);
Console.WriteLine("\nJogo criado e salvo!");

// Teste 7: Listar jogos
var jogos = Database.ListarJogos();
Console.WriteLine("\nJogos cadastrados:");
foreach (var j in jogos)
{
    Console.WriteLine($"Código: {j.Codigo}, Data: {j.Data}, Local: {j.Local}, Tipo: {j.TipoCampo}, Jogadores/time: {j.JogadoresPorTime}");
}