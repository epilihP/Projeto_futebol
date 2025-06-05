using Players;
using Associacao;
using GerenciadorJogos;
using Partidas;
using Associacao;
using Players;
using Time;


Console.WriteLine("Teste 1: Adicionar jogador");
Jogador.AdicionarJogador(new Jogador("Philipe", 19, Posicao.Atacante));
Jogador.AdicionarJogador(new Jogador("Luisa", 18, Posicao.Goleiro));
Jogador.AdicionarJogador(new Jogador("Luisa", 18, Posicao.Goleiro));
Jogador.AdicionarJogador(new Jogador("Leo", 20, Posicao.Defesa));
Jogador.AdicionarJogador(new Jogador("Gigi", 19, Posicao.Atacante));
Jogador.AdicionarJogador(new Jogador("Lucas", 19, Posicao.Goleiro));
Jogador.AdicionarJogador(new Jogador("Pedro", 18, Posicao.Atacante));
Jogador.AdicionarJogador(new Jogador("Duda", 40, Posicao.Atacante));
Jogador.AdicionarJogador(new Jogador("Ilicio", 42, Posicao.Defesa));
Jogador.AdicionarJogador(new Jogador("Renan", 20, Posicao.Defesa));
Jogador.AdicionarJogador(new Jogador("Alisson", 22, Posicao.Defesa));
Jogador.AdicionarJogador(new Jogador("Pedro", 18, Posicao.Atacante));
Jogador.AdicionarJogador(new Jogador("Duda", 40, Posicao.Atacante));
Jogador.AdicionarJogador(new Jogador("Ilicio", 42, Posicao.Defesa));
Jogador.AdicionarJogador(new Jogador("Renan", 20, Posicao.Defesa));
Jogador.AdicionarJogador(new Jogador("Alisson", 22, Posicao.Defesa));
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
    bool atualizado = Jogador.AtualizarJogador(primeiroJogador.RA, "Carlos Silva", 26, Posicao.Goleiro);
    Console.WriteLine(atualizado ? "Jogador atualizado com sucesso!" : "Falha ao atualizar jogador.");
}

Console.WriteLine("\nTeste 4: Remover jogador");
if (jogadores.Count > 1)
{
    var segundoJogador = jogadores[1];
    bool removido = Jogador.RemoverJogador(segundoJogador.RA);
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

// // Teste 6: CRUD de Jogos
// var dataJogo = DateTime.Now.AddDays(2);
// var jogo = new GerenciadorDeJogos(dataJogo, "Quadra Central", "Society", 5, 2, 10);
// Database.SalvarJogo(jogo);
// Console.WriteLine("\nJogo criado e salvo!");

// // Teste 7: Listar jogos
// var jogos = Database.ListarJogos();
// Console.WriteLine("\nJogos cadastrados:");
// foreach (var j in jogos)
// {
//     Console.WriteLine($"Código: {j.Codigo}, Data: {j.Data}, Local: {j.Local}, Tipo: {j.TipoCampo}, Jogadores/time: {j.JogadoresPorTime}");
// }

// // Exemplo de times (códigos dos times)
// Partida.contagemTimes = new List<int> { 101, 202, 303, 404 };

// // Cria e executa a partida
// Partida partida = new Partida();
// partida.ComecoFim = true;
// partida.GerenciarPartidas();

// // Mostra o histórico gerado
// Console.WriteLine("Histórico das rodadas:");
// foreach (var rodada in partida.historico)
// {
//     Console.WriteLine($"Rodada {rodada.rodada}: Rei {rodada.reiAntes} x Desafiante {rodada.desafiante} => Vencedor: {rodada.vencedor}");
// }

// // Salva o histórico em JSON
// Database.SalvarHistorico(partida.historico);
// Console.WriteLine("\nHistórico salvo em JSON!");

// // Lê o histórico salvo e mostra novamente
// var historicoSalvo = Database.LerHistorico();
// Console.WriteLine("\nHistórico lido do JSON:");
// foreach (var rodada in historicoSalvo)
// {
//     Console.WriteLine($"Rodada {rodada.rodada}: Rei {rodada.reiAntes} x Desafiante {rodada.desafiante} => Vencedor: {rodada.vencedor}");
// }

// Lê os jogadores do "banco de dados" (arquivo JSON)
var jogadoresBanco = Jogador.ListarJogadores();

// Cria o objeto Times
var times = new Times(DateTime.Now, "Futsal", "Quadra 1", 1, null, null);

// Preenche a lista de interessados com os RAs gerados
times.Interessados = jogadores.Select(j => j.RA).ToList();

// Monta os times
times.StartToPlay(jogadores);

// Exibe os times
Console.WriteLine("Primeiro Time:");
Times.PrimeiroTime.ForEach(nome => Console.WriteLine(nome));

Console.WriteLine("\nSegundo Time:");
Times.SegundoTime.ForEach(nome => Console.WriteLine(nome));