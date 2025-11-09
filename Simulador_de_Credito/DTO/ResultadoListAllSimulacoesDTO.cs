using System.Text.Json.Serialization;

namespace Simulador_de_Credito.DTO
{
    public record ResultadoListAllSimulacoesDTO
    {
        [JsonPropertyName("pagina")]
        public int Pagina { get; init; }

        [JsonPropertyName("qtdRegistros")]
        public int QtdRegistros { get; init; }

        [JsonPropertyName("qtdRegistrosPagina")]
        public int QtdRegistrosPagina { get; init; }

        [JsonPropertyName("registros")]
        public List<RegistrosDTO> Registros { get; init; }

        public ResultadoListAllSimulacoesDTO(int Pagina, int QtdRegistros, int QtdRegistrosPagina, List<RegistrosDTO> Registros)
        {
            this.Pagina = Pagina;
            this.QtdRegistros = QtdRegistros;
            this.QtdRegistrosPagina = QtdRegistrosPagina;
            this.Registros = Registros;
        }
    }
}
