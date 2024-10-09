// See https://aka.ms/new-console-template for more information
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

  public Pipeline (Dictionary<string, int> bancoRegistradores) {
    this.bancoRegistradores = bancoRegistradores;
  }
  
  public void writeBack() {
    instrucaoWriteBack = instrucaoMemoria?.Clonar();
    Instrucao instrucao = instrucaoWriteBack;
    if (instrucao == null) {
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
    if (instrucao == null) {
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
        if(instrucao.DecodeOper1 != instrucao.DecodeOper2){
          pc += instrucao.DecodeOper3;
        }
        break;
      default:
        break;
    }
  }

  public void decode() {
    this.instrucaoDecodifica = instrucaoBusca?.Clonar();
    Instrucao instrucao = this.instrucaoDecodifica;
    if (instrucao == null) {
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

    for (int i = 0; i < instrucoes.Count + 5; i++){
      this.writeBack();
      this.memoriaAccess();
      this.execute();
      this.decode();
      this.instructionFetch();
      this.pc++;
    }

    foreach (KeyValuePair<string, int> item in bancoRegistradores)
    {
        Console.WriteLine($"Chave: {item.Key}, Valor: {item.Value}");
    }
  }
}