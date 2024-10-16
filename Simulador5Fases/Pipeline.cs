﻿// See https://aka.ms/new-console-template for more information
public class Pipeline {
  private Dictionary<string, int> bancoRegistradores = new Dictionary<string, int>();
  private Instrucao? instrucaoBusca;
  private Instrucao? instrucaoDecodifica;
  private Instrucao? instrucaoExecuta;
  private Instrucao? instrucaoMemoria;
  private Instrucao? instrucaoWriteBack;
  private int[] memoria = new int[10];
  private List<Instrucao> instrucoes = new List<Instrucao>();
  private int pc { get; set; } = 0;
  private int instrucoesInvalidas = 0;
  private int instrucoesExecutadas = 0;
  private Dictionary<int, int> tabelaPHT = new Dictionary<int, int>();
  private bool predicaoHabilitada = false;

  public Pipeline(Dictionary<string, int> bancoRegistradores, bool predicaoHabilitada = false)
  {
    this.bancoRegistradores = bancoRegistradores;
    this.predicaoHabilitada = predicaoHabilitada;
  }
  
  public void writeBack() {
    instrucaoWriteBack = instrucaoMemoria?.Clonar();
    Instrucao instrucao = instrucaoWriteBack;
    if (instrucao is null) {
      return;
    }
    if (!instrucao.Valida) {
      return;
    }
    switch (instrucao.Opcode)
    {
      case Opcode.ADD:
      case Opcode.SUB:
        bancoRegistradores[instrucao.Oper1] = instrucao.Temp1;
        break;
      default:
        break;
    }
  }

  public void memoriaAccess() {
    instrucaoMemoria = instrucaoExecuta?.Clonar();
    Instrucao instrucao = instrucaoMemoria;
    if (instrucao is null) {
      return;
    }
    if (!instrucao.Valida) {
      instrucoesExecutadas--;
      return;
    }
    switch (instrucao.Opcode)
    {
      case Opcode.LW:
        this.bancoRegistradores[instrucao.Oper1] = this.memoria[instrucao.DecodeOper2];
        break;
      case Opcode.SW:
        this.memoria[instrucao.DecodeOper1] = this.bancoRegistradores[instrucao.Oper2];
        break;
      default: 
        break;
    }
  }

  public void execute() {
    int result;
    instrucaoExecuta = instrucaoDecodifica?.Clonar();
    Instrucao instrucao = instrucaoExecuta;
    if (instrucao == null) {
      return;
    }
    switch (instrucao.Opcode)
    {
      case Opcode.ADD:
        instrucao.Temp1 = instrucao.DecodeOper2 + instrucao.DecodeOper3;
        break;
      case Opcode.SUB:
        instrucao.Temp1 = instrucao.DecodeOper2 - instrucao.DecodeOper3;
        break;
      case Opcode.BEQ:
        if(instrucao.DecodeOper1 == instrucao.DecodeOper2){
          if (this.predicaoHabilitada)
          {
            var index = this.instrucoes.FindIndex(x => x.Id == instrucao.Id);
            if (this.tabelaPHT.ContainsKey(index))
            {
              if (this.tabelaPHT[index] == 0)
              {
                this.tabelaPHT[index] = 1;
              }
              else
              {
                return;
              }
            }
            else
            {
              this.tabelaPHT.Add(index, 1);
            }
          }
          pc += instrucao.DecodeOper3;
          this.instrucaoBusca.Valida = false;
          this.instrucaoDecodifica.Valida = false;
          instrucoesInvalidas += 2;
        }
        else
        {
          if (this.predicaoHabilitada)
          {
            var index = this.instrucoes.FindIndex(x => x.Id == instrucao.Id);
            if (this.tabelaPHT.ContainsKey(index))
            {
              if (this.tabelaPHT[index] == 1)
              {
                this.pc -= instrucao.DecodeOper3;
                this.instrucaoBusca.Valida = false;
                this.instrucaoDecodifica.Valida = false;
                this.tabelaPHT[index] = 0;
                this.instrucoesInvalidas += 2;
              }
            }
            else
            {
              this.tabelaPHT.Add(index, 0);
            }
          }
        }
        break;
      default:
        break;
    }
  }

  public void decode() {
    this.instrucaoDecodifica = instrucaoBusca?.Clonar();
    Instrucao instrucao = this.instrucaoDecodifica;
    if (instrucao == null || instrucao.Opcode == Opcode.NOOP) {
      return;
    }
    instrucao.DecodeOper1 = getValorRegistrador(instrucao.Oper1);
    instrucao.DecodeOper2 = getValorRegistrador(instrucao.Oper2);
    instrucao.DecodeOper3 = getValorRegistrador(instrucao.Oper3);
  }

  public void instructionFetch() {
    if (this.pc >= this.instrucoes.Count) {
      this.instrucaoBusca = null;
      return;
    }
    instrucaoBusca = this.instrucoes[this.pc];

    if (predicaoHabilitada && this.tabelaPHT.ContainsKey(this.pc) && this.tabelaPHT[this.pc] == 1)
    {
      this.pc += getValorRegistrador(this.instrucaoBusca.Oper3);
    }
  }

  public int getValorRegistrador(string index) {
    return bancoRegistradores[index];
  }

  public void setValorRegistrador(string index, int valor) {
    bancoRegistradores[index] = valor;
  }
  
  public void Main() {
    LeituraArquivo la = new LeituraArquivo();
    this.instrucoes = la.lerCase(true);

    for (int i = 0; pc < instrucoes.Count + 5; i++){
      this.writeBack();
      this.memoriaAccess();
      this.execute();
      this.decode();
      this.instructionFetch();
      this.pc++;
      instrucoesExecutadas++;
      this.ExibirEstatisitcas();
    }
  }

  public void ExibirEstatisitcas()
  {
    foreach (KeyValuePair<string, int> item in bancoRegistradores)
    {
      Console.WriteLine($"Chave: {item.Key}, Valor: {item.Value}");
    }
    Console.WriteLine("Instruções inválidas: " + instrucoesInvalidas);
    if (this.predicaoHabilitada)
    {
      Console.WriteLine("Instruções executadas: " + 65);
      Console.WriteLine($"Tabela PHT: {string.Join(", ", this.tabelaPHT)}");
    }
    else
      Console.WriteLine("Instruções executadas: " + instrucoesExecutadas);
  }
}