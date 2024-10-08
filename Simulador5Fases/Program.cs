// See https://aka.ms/new-console-template for more information
public class Program {
  private Dictionary<string, int> bancoRegistradores = new Dictionary<string, int>();
  private int[] memoria = new int[10];
  private Instrucao[] instrucoes;
  private int pc { get; set; } = 0;
  
  public void writeBack() {
    Instrucao instrucao = instrucoes[pc];
    switch (instrucao.Opcode)
    {
      case Opcode.ADD:
        bancoRegistradores[instrucao.Oper1] = instrucao.Temp1;
        break;
      case Opcode.SUB:
        bancoRegistradores[instrucao.Oper1] = instrucao.Temp1;
        break;
      default:
        break;
    }
  }

  public void execute() {
    int result;
    Instrucao instrucao = instrucoes[pc];
    switch (instrucao.Opcode)
    {
      case Opcode.ADD:
        instrucao.Temp1 = instrucao.Temp2 + instrucao.Temp3;
        break;
      case Opcode.SUB:
        instrucao.Temp1 = instrucao.Temp2 - instrucao.Temp3;
        break;
      default:
        break;
    }
  }

  public void decode() {
    Instrucao instrucao = instrucoes[pc];
    instrucao.Temp2 = getValorRegistrador(instrucao.Oper2);
    instrucao.Temp3 = getValorRegistrador(instrucao.Oper3);
  }

  public void instructionFetch() {
    Instrucao instrucao = instrucoes[pc];
    pc++;
  }

  public int getValorRegistrador(string index) {
    return bancoRegistradores[index];
  }

  public void setValorRegistrador(string index, int valor) {
    bancoRegistradores[index] = valor;
  }

  public void main() {
  }
}