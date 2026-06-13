namespace MilhasAPI.Models;

// DTOs de saída do dashboard. Nomes de propriedade são serializados em camelCase
// (web defaults), mantendo o contrato que o frontend já consome.

public class DashboardResumoDto
{
    public int TotalMiles { get; set; }
    public int Crescimento { get; set; }
    public int AVencer { get; set; }
    public List<ProgramaResumoDto> Programas { get; set; } = new();
    public List<TransacaoDto> Transacoes { get; set; } = new();
    public List<AlertDto> Alerts { get; set; } = new();
}

public class ProgramaResumoDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int Miles { get; set; }
    public string Color { get; set; } = null!;
    public string Logo { get; set; } = null!;
}

public class TransacaoDto
{
    public string Id { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Program { get; set; } = null!;
    public int Amount { get; set; }
    public string Date { get; set; } = null!;
    public string Description { get; set; } = null!;
}

public class AlertDto
{
    public string Id { get; set; } = null!;
    public string Text { get; set; } = null!;
    public string Type { get; set; } = null!;
}

public class DashboardAnalyticsDto
{
    public List<ProgramaAnalyticsDto> Programas { get; set; } = new();
    public List<ProgramaAnalyticsDto> Distribuicao { get; set; } = new();
    public List<MonthMilesDto> Evolucao { get; set; } = new();
    public List<MonthMilesDto> Vencimento { get; set; } = new();
    public string Crescimento { get; set; } = null!;
    public string AVencer { get; set; } = null!;
    public string TotalVencimento { get; set; } = null!;
}

public class ProgramaAnalyticsDto
{
    public string Name { get; set; } = null!;
    public int Miles { get; set; }
    public string Fill { get; set; } = null!;
}

public class MonthMilesDto
{
    public string Month { get; set; } = null!;
    public int Miles { get; set; }
}
