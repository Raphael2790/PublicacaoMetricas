namespace PublicacaoMetricas.Common;

public class CriarChecklist
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    
    public CriarChecklist(string nome, string descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }
}