namespace IPartidaGenerica;

// usei o T para um tipo de parametro que não foi pre-determiado, podendo usar o generico para salvar algo durante o projeto
public interface ICaixaObjeto<T>{
    void Guardar(T a);
}